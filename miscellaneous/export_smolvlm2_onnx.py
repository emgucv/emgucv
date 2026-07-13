# Export HuggingFaceTB/SmolVLM2-256M-Video-Instruct to ONNX files usable by
# the OpenCV dnn module (Emgu.CV.Models.SmolVlm2).
#
# The official ONNX exports of SmolVLM2 cannot be used: the decoder contains
# onnxruntime contrib operations (SimplifiedLayerNormalization,
# MultiHeadAttention) and the vision encoder contains data dependent mask
# handling (NonZero / GatherND over booleans) that the dnn module does not
# support. This script produces raw exports instead:
#
#  - vision_raw.onnx     the vision tower + connector, composed from the clean
#                        submodules with the attention mask machinery bypassed
#                        (the masks are always all-ones for the 512x512 tiles
#                        the processor produces)
#  - decoder_raw.onnx    the Llama (SmolLM2-135M) text backbone, exported with
#  + decoder_raw.onnx_data  past key values through the supported optimum
#                        text path, with the input_ids embedding lookup
#                        replaced by an inputs_embeds graph input so image
#                        features can be spliced in
#
# The official onnx/embed_tokens.onnx of the huggingface repository is clean
# and used as-is.
#
# Requirements:
#   pip install "optimum[exporters]" "optimum-onnx[onnxruntime]" torch transformers onnx
#
# Usage:
#   python export_smolvlm2_onnx.py <output_folder>

import os
import subprocess
import sys
import tempfile

import onnx
import torch
from onnx import TensorProto, helper
from transformers import AutoModelForImageTextToText, AutoTokenizer, LlamaForCausalLM

MODEL_ID = "HuggingFaceTB/SmolVLM2-256M-Video-Instruct"
HIDDEN = 576


def export_vision(model, out_path):
    class VisionWrapper(torch.nn.Module):
        def __init__(self, model):
            super().__init__()
            vm = model.model.vision_model
            self.patch_embedding = vm.embeddings.patch_embedding
            self.position_embedding = vm.embeddings.position_embedding
            self.encoder = vm.encoder
            self.post_layernorm = vm.post_layernorm
            self.connector = model.model.connector
            self.register_buffer("position_ids", torch.arange(1024).unsqueeze(0), persistent=False)

        def forward(self, pixel_values):
            x = self.patch_embedding(pixel_values)  # (n, 768, 32, 32)
            x = x.flatten(2).transpose(1, 2)        # (n, 1024, 768)
            x = x + self.position_embedding(self.position_ids)
            x = self.encoder(inputs_embeds=x).last_hidden_state
            x = self.post_layernorm(x)
            return self.connector(x)                # (n, 64, 576)

    wrapper = VisionWrapper(model)
    dummy = torch.randn(2, 3, 512, 512)
    torch.onnx.export(
        wrapper, (dummy,), out_path,
        input_names=["pixel_values"], output_names=["image_features"],
        dynamic_axes={"pixel_values": {0: "num_images"}, "image_features": {0: "num_images"}},
        opset_version=17)
    print("exported", out_path)


def export_decoder(model, out_folder):
    # Extract the Llama text backbone into a plain LlamaForCausalLM, which is
    # supported by the optimum with-past export.
    lm = LlamaForCausalLM(model.config.text_config)
    lm.model.load_state_dict(model.model.text_model.state_dict())
    lm.lm_head.load_state_dict(model.lm_head.state_dict())

    with tempfile.TemporaryDirectory() as tmp:
        lm.save_pretrained(tmp)
        AutoTokenizer.from_pretrained(MODEL_ID).save_pretrained(tmp)
        export_dir = os.path.join(tmp, "onnx")
        subprocess.check_call([
            sys.executable, "-m", "optimum.exporters.onnx",
            "--model", tmp, "--task", "causal-lm-with-past", export_dir])

        # Replace the internal input_ids -> Gather embedding lookup with an
        # inputs_embeds graph input, so the image features can be spliced into
        # the token embeddings.
        m = onnx.load(os.path.join(export_dir, "model.onnx"))
        g = m.graph
        gathers = [n for n in g.node if n.op_type == "Gather" and "input_ids" in n.input]
        assert len(gathers) == 1
        gather = gathers[0]
        embed_out = gather.output[0]
        g.node.remove(gather)
        for n in g.node:
            for i, name in enumerate(n.input):
                if name == embed_out:
                    n.input[i] = "inputs_embeds"
        inputs_embeds = helper.make_tensor_value_info(
            "inputs_embeds", TensorProto.FLOAT, ["batch_size", "sequence_length", HIDDEN])
        old_inputs = list(g.input)
        del g.input[:]
        for i in old_inputs:
            g.input.append(inputs_embeds if i.name == "input_ids" else i)
        onnx.checker.check_model(m, full_check=False)
        onnx.save(m, os.path.join(out_folder, "decoder_raw.onnx"),
                  save_as_external_data=True, location="decoder_raw.onnx_data")
    print("exported", os.path.join(out_folder, "decoder_raw.onnx"))


if __name__ == "__main__":
    out = sys.argv[1] if len(sys.argv) > 1 else "smolvlm2_onnx"
    os.makedirs(out, exist_ok=True)
    model = AutoModelForImageTextToText.from_pretrained(
        MODEL_ID, dtype=torch.float32, attn_implementation="eager")
    model.eval()
    export_vision(model, os.path.join(out, "vision_raw.onnx"))
    export_decoder(model, out)
    print("Done. Also download onnx/embed_tokens.onnx and tokenizer.json from")
    print("https://huggingface.co/" + MODEL_ID)
