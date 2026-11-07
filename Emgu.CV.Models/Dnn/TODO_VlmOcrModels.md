# TODO: wrap Granite-Docling-258M and PaddleOCR-VL-1.5 as Emgu.CV.Models

Status: **not started.** Written 2026-11-07 after reading the OpenCV 5.1 samples
`opencv/samples/dnn/granite_docling_inference.py` and `paddleocr_vl_inference.py` (added upstream in PR #29618).
Both are vision-language models that turn a page image into text. Neither has been run end to end yet.
The two decoder networks were only loaded and given a 4-token forward pass in a scratch test.

Related: `Qwen25_KnownIssues.md` in this folder (KV-cache findings and an open native crash that affects
any model driven with this decode loop).

## Pipeline (identical for both models)
1. **Vision encoder** (ONNX): page image -> image-feature tokens, already projected to the decoder hidden size.
2. **Embedding network** (ONNX): token ids -> text embeddings. The prompt holds one placeholder token per image
   feature; overwrite the embedding at each placeholder, in order, with the next image-feature row
   (`merge_image_features` in the samples).
3. **Text decoder** (ONNX, `EngineType.OpenCV`): prefill once over the merged embeddings, then greedy decoding,
   one token per step (embed the new id, feed `inputs_embeds` and `attention_mask`), until EOS or `maxNewTokens`.
4. `Tokenizer.Decode` turns the ids into the output text.

`SmolVlm2.cs` in this folder already has this structure (vision, embed_tokens, decoder, own preprocessing), so use
it as the template for the class layout, `Init()` download logic and test style.

## Model details

| | Granite-Docling-258M | PaddleOCR-VL-1.5 |
|---|---|---|
| Output | DocTags markup (text, tables, headers...) | text and layout |
| Default prompt | `Convert this page to docling.` | `OCR:` |
| Hugging Face | `ibm-granite/granite-docling-258M`, ONNX: `onnx-community/granite-docling-258M-ONNX` | `PaddlePaddle/PaddleOCR-VL-1.5`, ONNX: `onnx-community/PaddleOCR-VL-1.5-ONNX` |
| Networks | `onnx/vision_encoder.onnx` (+`_data` 374 MB), `onnx/embed_tokens.onnx` (+`_data` 231 MB), `onnx/decoder_model_merged.onnx` (+`_data` 658 MB) | `onnx/vision_encoder.onnx` (+`.data` 1757 MB), `onnx/embedding.onnx` (+`.data` 424 MB), `onnx/decoder.onnx` (+`.data` 1443 MB) |
| Decoder precision | fp32 `decoder_model_merged.onnx` | fp32 `decoder.onnx` only (the int4/int8 variants need `MatMulNBits` asymmetric / `MatMulInteger`, which OpenCV's importer does not support) |
| Hidden size | 576 | 1024 |
| Image preprocessing | Idefics3: round the page up to a multiple of `max_image_size.longest_edge` (512), cut into 512x512 tiles, append the whole page resized to 512x512 as a global frame. Normalize to [-1,1] (mean=std=0.5), CHW. Inputs `pixel_values (1, nFrames, 3, 512, 512)` and bool `pixel_attention_mask`. A 4x4 page gives 17 x 64 = 1088 image tokens. | `smart_resize` to a multiple of `patch_size * merge_size` within `min_pixels`/`max_pixels`; normalize mean=std=0.5; pack into `pixel_values (1, nPatches, 3, 14, 14)` and `image_grid_thw (1, 3)`. One image token per `merge_size^2` patches. |
| Prompt | ChatML-like with `<\|start_of_role\|>`, per-tile `<fake_token_around_image><row_R_col_C>` blocks, `<global-img>` (see `build_prompt`) | `<\|begin_of_sentence\|>User: <\|IMAGE_START\|>` + placeholders + `<\|IMAGE_END\|>` + prompt + `\nAssistant:\n` |
| Extra inputs | optional `position_ids` (the community decoder had none) | none |
| Config files | `config.json`, `tokenizer.json` (OpenCV format, see below), `preprocessor_config.json`, `processor_config.json` (`image_seq_len`) | `config.json`, `tokenizer.json`, `processor_config.json` (`image_processor`: `patch_size`, `merge_size`, `min_pixels`, `max_pixels`) |

Tokenizer: the samples need an **OpenCV-format** `config.json` (not Hugging Face's `tokenizer_config.json`; it
needs `model_type`/`method` plus `image_token_id` and `eos_token_id`). `opencv_extra` already has them at
`testdata/dnn/llm/granite/{config.json,tokenizer.json}` and `testdata/dnn/llm/paddleocr_vl/{config.json,tokenizer.json}`,
the same place `Qwen25.cs` fetches `tokenizer.json` from (`raw.githubusercontent.com/opencv/opencv_extra/5.x/...`).
The processor/preprocessor JSON files come from the `onnx-community` repos.

## Checklist
- [ ] Decide hosting: download from Hugging Face directly (large files, pinned SHA-256 via
      `FileDownloadManager.AddFile`) or mirror to the `emgu-public` bucket like the other models. Compute SHA-256 for
      every file (including the external `.onnx_data` / `.onnx.data` files, which must sit next to their `.onnx`).
- [ ] `GraniteDoclingModel.cs` and `PaddleOcrVlModel.cs` in this folder: `Init()` (download, build the three nets),
      `Generate(IInputArray image, string prompt, int maxNewTokens)`, `Clear()`, `DisposeObject()`.
      Follow the `#if UNITY_...` `IEnumerator Init` vs `async Task Init` split used by the other models.
- [ ] Port the image preprocessing with `CvInvoke` / `Mat` (tile split, `smart_resize`, patch packing, normalization)
      and the prompt builders. Read the processor JSON values with a small parser. Do not use `System.Text.Json`
      unguarded: it is not available under Unity (see how `Siglip2.cs` / `Clip.cs` guard it).
- [ ] Pass int64 inputs the way `Qwen25.SetInt64Input` does (pinned array, `Mat` with `DepthType.Cv64S`);
      OpenCV copies the data in `setInput`, so unpinning afterwards is safe.
- [ ] Tests in `Emgu.CV.Test/AutoTestModels.cs` (ignored unless `-p:TestModels=true`): a page image from
      `opencv_extra` or a rendered text image; check that the recognized text contains the expected words.
      For Granite, assert DocTags structure rather than exact text.
- [ ] Decide whether to expose them in the MAUI demo (see `QwenChatPage.cs`, `SmolVlm2Page.cs`) and add the Unity
      copy script entries if needed.
- [ ] Measure speed on the Commercial (`CPU_DISPATCH=ALL`) build; the open-source Windows build has no dispatch
      list and runs these models very slowly (Qwen2.5 managed about 1.3 tokens per second there).

## Known caveats
- **`ReserveKVCache` is a no-op for both decoders.** Checked on 2026-11-07: OpenCV warns "no paged attention
  layers", both before the first forward pass and after one forward + `ResetKVCache`. The onnx-community exports
  do not import as a single fused Attention op. Do not rely on `ReserveKVCache` for these models; a paged cache
  would need an opset-23 dynamo re-export (recipe in `opencv/samples/dnn/qwen_inference.py`).
- **Possible native crash in the decode loop** when a later prefill is longer than any earlier prefill on the same
  loaded net (seen with `Qwen25`; see `Qwen25_KnownIssues.md`). Granite/PaddleOCR page prompts are long (over
  1000 image tokens), and every `Generate` is a fresh prefill. Test two different page sizes in one process
  before shipping.
- Image-token count must equal the number of placeholders in the prompt, otherwise `merge_image_features`
  fails; the sample raises `ValueError` in that case.
- Gemma 4 E2B was considered as an alternative OCR model; OpenCV 5.1 has no Gemma 4 support (only the Gemma
  tokenizer and a text-only Gemma 3 sample), so it is out of scope for now.
