# Qwen25 / KV-cache: open investigation notes (OpenCV 5.1)

Status: **unresolved, to be revisited.** Written 2026-11-07 while checking whether the OpenCV 5.1
`Net::reserveKVCache()` helps the MAUI LLM demos. These notes apply to `Qwen25.cs`, and most likely to
`Qwen3.cs` and `SmolVlm2.cs`, which use the same `EnableKVCache` / `ResetKVCache` / one-token decode pattern.
Only `Qwen25` was tested.

## 1. Native crash when a later prefill is longer than any earlier prefill

### Symptom
The test host process dies with no managed exception, nothing on stderr (also with `OPENCV_LOG_LEVEL=DEBUG`)
and no Windows Application Error event. It happens inside the **prefill forward pass** of a later
`Qwen25.Generate()` call on a model that has already been used.

### Observations
Each row is a sequence of `Generate(prompt, maxNewTokens)` calls on the **same loaded `Qwen25`**.
"Prefill" is the token count fed to the first forward pass (ChatML template plus prompt), printed by
temporary tracing in `ForwardChunk`.

| Prefill lengths (in order) | Result |
|---|---|
| 11 -> 9 | OK |
| 11 -> 11 | OK |
| 13 -> 11 -> 13 | OK |
| 11 -> 13 (maxNewTokens 1, 2 or 8) | **crash** in the 13-token prefill |
| 11 -> 13 -> 11, and 11 -> 13 -> 11 -> longer | crash already at the first 13 |
| long prompt first (about 20 tokens), then shorter prompts | OK |

Concrete prompts: `"Warm up."` gives an 11-token prefill, `"Warm up again please."` gives 13 and `"Hi"` gives 9.

Pattern that fits these runs: **a prefill longer than any prefill seen so far on this `Net` crashes.**
It does not explain everything: `TestQwen25` (a Generate, then a 2-turn Chat whose second turn re-prefills a
longer transcript) passes, so the rule is incomplete. Both the full model suite (311 passed) and
`TestQwen25` pass on this build.

### Why it may matter
`Qwen25.Chat()` re-prefills the **whole transcript every turn** (`EnableKVCache(); ResetKVCache();` then one
prefill over `_transcript`). Every turn therefore has a longer prefill than the one before. A short first
turn followed by a longer second turn could hit this in the MAUI chat demo. Not verified in the demo itself.

### Ruled out
- **Paged KV-cache on the second run.** Hypothesis: the cache manager is built before the attention layers are
  fused (fusion runs lazily in `Net::Impl::prepareForInference()` at the first forward), so a later
  `ResetKVCache` would switch the model to the paged path. Disproved: after fusion, `reserveKVCache()` still
  warns that the model has no paged attention layers. Paged attention is never used for this export.
- **Managed pointer lifetime.** `SetInt64Input` unpins its array before `Forward`, but OpenCV's
  `setInput` copies the data into its own tensor (`setGraphInput`: `fit` + `convertTo`), so this is safe.
- **`ReserveKVCache`.** The crash occurs without it.

### Not yet known
- Whether it also crashes on the **Commercial** build (`CPU_DISPATCH=ALL`) or a non-CUDA build. Only the CUDA
  build described below was tested.
- Whether it is in OpenCV (dynamic-shape / buffer handling when the sequence length grows between runs
  after `resetKVCache()`, the `present.*` -> `past_key_values.*` routing in `kv_cache_manager.cpp`, or the
  graph's buffer planning) or in how Emgu drives the net.
- The exact rule, given the `TestQwen25` counter-example above.

### Suggested next steps
1. Reproduce outside Emgu with a small C++ or Python OpenCV program: load `model.onnx`, then loop
   `enableKVCache()`, `resetKVCache()`, prefill of N tokens, with N = 11 then 13. Run on both the CUDA (no
   dispatch) and the Commercial build.
2. If it reproduces in OpenCV, file it upstream with that repro and the Qwen2.5 `causal-lm-with-past` export.
3. If not, look at how `Qwen25` sets `attention_mask` / `position_ids` against the cached length.

### Test environment
- Emgu CV `EMGUCV_5_x`, OpenCV 5.1.0 (plus the local UMat/OpenCL patch in the `opencv` submodule).
- Windows x64 CUDA build (`Build_Binary.ps1 -Arch x86_64 -Cuda -OnnxRuntime -ExtraModules NonFree
  -CudaArchBin '8.6'`), CUDA 13.2, RTX 3070, **no CPU dispatch list** (non-Commercial build by design).
- DNN engine `EngineType.OpenCV`, default backend and target (CPU).
- Model: `emgu-public/qwen2.5_0.5b_instruct_onnx/model.onnx` (optimum `causal-lm-with-past` export).

## 2. `ReserveKVCache` does not help the current models

`Net.ReserveKVCache(int)` (wraps `cv::dnn::Net::reserveKVCache`) only has an effect for models whose attention
imports as a single fused op (ONNX `Attention`, `com.microsoft` MultiHeadAttention / GroupQueryAttention).
Otherwise OpenCV logs `reserveKVCache() has no effect, the model has no paged attention layers ...` and does
nothing.

Measured: no effect for any of these (OpenCV warning every time):
- `Qwen25`: 128 new tokens took about 97 s with and without the call (the build has no CPU dispatch, which
  is why it is slow).
- Granite-Docling-258M `decoder_model_merged.onnx` and PaddleOCR-VL-1.5 `decoder.onnx` (onnx-community
  exports): warning both before the first forward and after forward + `ResetKVCache`.

To get a paged cache, the model has to be exported with the PyTorch dynamo exporter at **opset 23** and
**without `past_key_values`** (position ids passed explicitly); see the docstring of
`opencv/samples/dnn/qwen_inference.py` for the recipe. OpenCV samples that call `reserveKVCache`:
`qwen_inference.py`, `gemma3_inference.py`, `granite_docling_inference.py`, `paddleocr_vl_inference.py`.
Using it here would mean re-exporting the models and changing the input signature in `Qwen25.cs` /
`Qwen3.cs`; call it after `ResetKVCache()` (the reset drops the reservation) and before prefill.

## 3. Related notes
- `Net.ReserveKVCache` wrapper: `Emgu.CV/Dnn/Net.cs` (commit "Wrap OpenCV 5.1 FontFace.setBuiltinFont,
  alphaComposite, reserveKVCache, blobFromImage interpolation").
- No test covers `ReserveKVCache` yet; it needs a fused-attention model.
- 5.1's tokenizer applies the `post_processor` template, so `Tokenizer.Encode` may already append `<eos>`
  (see the `Siglip2.Tokenize` fix). Check the `Qwen25` / `Qwen3` tokenizer config if their output changes.
