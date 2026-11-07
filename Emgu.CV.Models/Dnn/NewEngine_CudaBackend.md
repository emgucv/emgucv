# CUDA backend of the new DNN engine (OpenCV 5.1)

Status: findings from 2026-11-07, one dev machine, one build. Not a complete survey of which models run on the GPU.

OpenCV 5.1 added a CUDA backend to the new graph engine (the only engine left; the classic engine was removed,
and `EngineType.OpenCV` selects it). It is separate from the ONNX Runtime CUDA provider, see
`OnnxRuntime_CudaKnownIssue.md`.

## How it works (from `opencv/modules/dnn/src/net_impl2.cpp`)
- Each operation gets its own CUDA executor, registered per op type
  (`CV_DNN_REGISTER_EXEC_CLASS(<op>, DNN_BACKEND_CUDA, ...)`, e.g. in `layers/conv2_layer.cpp` and `op_cuda.cpp`).
- `Net::Impl::finalizeGraph` has a **whole-graph gate**: the graph runs on CUDA only if every op has a CUDA
  executor. If one op has none, or the graph has control-flow subgraphs, or an op has dynamic output shapes that
  cannot be sized on the host, the **entire graph runs on the CPU**. The source comment says this avoids partial
  CPU/GPU execution and its host/device coherence problems for now.
- The fallback is only reported through `CV_LOG_INFO` ("running the whole graph on CPU"). Info logging is compiled
  out of release builds (`CV_LOG_STRIP_LEVEL` defaults to DEBUG), so there is **no error and no visible warning**.
- `WITH_CUDNNJIT` (cuDNN graph API, runtime-compiled engines) applies to this engine; the Emgu CUDA build uses
  legacy cuDNN (`WITH_CUDNN=ON`). Several layers (LSTM, LRN, deconvolution) report no CUDA support in a JIT-only
  build, so under that setting those models fall back too.
- Upstream commits: `72ffdfc170` (CUDA support, layer split and per-op executors), `38a2f1556a` (UMat device
  buffers), `5750b9d74c` (ResNet layer coverage and cuDNN JIT conv/pooling).

## Usage
```csharp
Net net = DnnInvoke.ReadNetFromONNX(path, EngineType.OpenCV);
net.SetPreferableBackend(Emgu.CV.Dnn.Backend.Cuda);   // not Emgu.CV.Backend, that is the videoio one
net.SetPreferableTarget(Target.Cuda);
```

## Measurements (Windows CUDA build, RTX 3070, CUDA 13.2, no CPU dispatch list)
| Model | CPU | CUDA | Output difference | Result |
|---|---|---|---|---|
| `opencv_extra/testdata/dnn/onnx/models/mobilenetv4.onnx`, 224x224 | 2.2 ms | 0.6 ms | relative L2 error 7e-8 | ran on the GPU, about 3.7x faster |
| `yolo11m.onnx`, 640x640 | about 500 ms | about 500 ms | exactly 0 | **fell back to the CPU** |

A bit-identical result and an unchanged time are the signs of a silent CPU fallback; a real GPU run differs by
float rounding. The op that blocks YOLO11 was not identified. Not tested: YuNet (it ran but was not checked),
Qwen and SmolVLM decoders, other models.

## How to check whether a model really runs on the GPU
Run the net on the CPU and with `Backend.Cuda` / `Target.Cuda` on the same input, and compare: a relative L2 error
of exactly 0 together with the same time means it fell back. To find the blocking op, build OpenCV with
`-DCV_LOG_STRIP_LEVEL=CV_LOG_LEVEL_VERBOSE` (or a debug build) and set `OPENCV_LOG_LEVEL=INFO`; look for
`DNN/NewEngine: graph '...': op '...' (...) has no CUDA exec; running the whole graph on CPU`.
`dotnet test` hides the native stderr; redirect fd 2 to a file (`_open_osfhandle` and `_dup2` from `ucrtbase.dll`).

## Implications for Emgu
- `Emgu.CV.Models/Dnn/Yolo.cs` uses ORT with CUDA, which falls back to the CPU in this build. The new engine's CUDA
  backend is not an alternative for YOLO11 yet (it also falls back).
- A model class can try `Backend.Cuda` on the OpenCV engine and keep the CPU result if nothing changes, but it cannot
  detect the fallback from the API. Only enable it per model after confirming a speed-up as above.
- `EngineType.OpenCV` XML doc in `Emgu.CV/Dnn/DnnInvoke.cs` was updated to describe the CUDA support and the
  all-or-nothing rule.

Related notes in this folder: `OnnxRuntime_CudaKnownIssue.md`, `Qwen25_KnownIssues.md`, `TODO_VlmOcrModels.md`.
