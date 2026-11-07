# ONNX Runtime CUDA execution provider does not load in the Windows CUDA build

Status: **known limitation, accepted for now** (decision 2026-11-07: keep running ONNX Runtime on the CPU).
Revisit when a CUDA 13 build of ONNX Runtime is available for Windows, or when the build is moved to a CUDA 12
toolkit.

## Symptom
In the Windows CUDA build (`Build_Binary.ps1 -Arch x86_64 -Cuda -OnnxRuntime ...`), a net created with
`EngineType.Ort` and `SetPreferableTarget(Target.Cuda)` runs on the **CPU**. There is no managed exception.
The only sign is one line on the native stderr:

```
[ WARN:0@4.084] global net_impl_backend.cpp:113 finalizeOrt DNN/ONNX/ORT: CUDA EP unavailable
(OrtSessionOptionsAppendExecutionProvider_Cuda: Failed to load shared library), using CPU
```

`opencv/modules/dnn/src/net_impl_backend.cpp` (around line 106) appends ORT's CUDA provider and, on any failure,
logs that warning and falls back to the CPU provider.

## Measurement (2026-11-07)
`yolo11m.onnx` (640x640, random image), average of a few iterations after one warm-up, CUDA build, RTX 3070,
no CPU dispatch list:

| Engine | ms per inference |
|---|---|
| `EngineType.OpenCV`, CPU | 481 |
| `EngineType.Ort`, default target | 211 |
| `EngineType.Ort`, `Target.Cuda` | 199 |

The last two are the same, because the CUDA provider never loaded. Only this one model was timed, with 3 to 10
iterations, so treat the numbers as indicative.

## Cause
ORT's own log gives the reason:

```
Error loading ...\runtimes\win-x64\native\onnxruntime_providers_cuda.dll which depends on
"cublasLt64_12.dll" which is missing. (Error 126: "The specified module could not be found.")
```

The ONNX Runtime GPU package that the OpenCV build downloads (`DOWNLOAD_ONNXRUNTIME_GPU`, version
`ONNXRUNTIME_VERSION` 1.25.1, file `onnxruntime-win-x64-gpu-1.25.1.zip`) is built against **CUDA 12**. The Emgu
CUDA build uses the **CUDA 13.2** toolkit, so `libs/runtimes/win-x64/native` holds the CUDA 13 libraries
(`cublas64_13.dll`, `cublasLt64_13.dll`, `cudart64_13.dll`, `cufft64_12.dll`, `curand64_10.dll`, NVRTC 13...) and
not the CUDA 12 ones the provider needs. I did not find an official Windows CUDA 13 build of ONNX Runtime 1.25
(web search was inconclusive; CUDA 13 builds seen were Linux only).

## Where this matters in the code
- `Emgu.CV.Models/Dnn/Yolo.cs` (around line 325): `useOrtCuda = CvInvoke.HaveOnnxRuntime && CudaInvoke.HasCuda &&
  !IsV10(version)` selects the ORT engine plus `Target.Cuda`. With this build it silently runs ORT on the CPU.
  Still correct and about twice as fast as the OpenCV engine on that model, just not on the GPU.
- `Emgu.CV.Models/Dnn/Qwen25.cs` and `Qwen3.cs` deliberately do not use ORT (OpenCV's ORT engine has no KV-cache
  support), so they are unaffected.
- `platforms/windows/Build_Binary.ps1`: `-Cuda` together with `-OnnxRuntime` sets `DOWNLOAD_ONNXRUNTIME_GPU=ON`.
- `Emgu.CV.Extern/cmake/DependencyDlls.cmake`: copies `onnxruntime*.dll` (including
  `onnxruntime_providers_cuda.dll`, about 294 MB, and the TensorRT provider) into the native folder. These large
  provider DLLs are shipped but unusable until the CUDA 12 dependencies are present.

## Options considered
1. **Build against a CUDA 12 toolkit.** Then ORT's provider finds `cublasLt64_12.dll` and the rest. Not possible on
   the current dev machine as set up: `v12.9` does not resolve through `CUDA_PATH_V12_9`, and the cuDNN 9.20
   install only has a `13.2` bin folder. It would also move the whole OpenCV CUDA build to CUDA 12.
2. **Ship the CUDA 12 runtime libraries next to the provider** (`cublas64_12`, `cublasLt64_12`, `cudart64_12`, and
   probably cuDNN and cuFFT for 12), alongside the CUDA 13 ones. Several hundred MB extra.
3. **Keep ORT on the CPU. (chosen)** No change needed.

## Follow-ups to consider (not done)
- Option 3 still downloads and ships the GPU ORT package (large provider DLLs) that cannot be used. If the
  situation does not change, consider `-OnnxRuntime` without `DOWNLOAD_ONNXRUNTIME_GPU` for the CUDA build to
  shrink the package, or skip the provider DLLs in `DependencyDlls.cmake`.
- A one-time check at model load that warns managed-side when the CUDA provider did not load, instead of relying
  on a native stderr line.

## How to re-check later
Call `ReadNetFromONNX(model, EngineType.Ort)` then `SetPreferableTarget(Target.Cuda)` and time a few forward passes
against `EngineType.Ort` with no target set. A real GPU run is clearly faster, and the stderr line above is absent.
`dotnet test` hides the native stderr, so redirect it in the test: P/Invoke `_open_osfhandle` and `_dup2(fd, 2)` from
`ucrtbase.dll` onto a file opened with `FileShare.ReadWrite`. ORT writes its error as UTF-16, so decode the file
with `utf-16le`.

Related notes in this folder: `Qwen25_KnownIssues.md`, `TODO_VlmOcrModels.md`.
