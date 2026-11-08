# ONNX Runtime CUDA execution provider in the Windows CUDA build

Status: **resolved for the Windows CUDA 13 build (2026-11-08) by moving to ONNX Runtime 1.31.1.** The text below keeps
the history, because the same failure comes back if the ORT package and the CUDA toolkit are ever out of step again.

## Resolution
`Build_Binary.ps1 -Cuda -OnnxRuntime` now passes `-DONNXRUNTIME_VERSION=1.31.1`, and the OpenCV dnn module downloads
`onnxruntime-win-x64-gpu_cuda13-1.31.1.zip`. That provider is built against CUDA 13, like the Emgu CUDA build (CUDA
13.2 toolkit), so it loads without any extra libraries. ORT 1.27 and later publish separate `gpu_cuda12` and
`gpu_cuda13` packages; the old `gpu` name used by ORT 1.25 no longer exists.

- OpenCV change (opencv fork, commit `036bf26b07`, `modules/dnn/CMakeLists.txt`): new cache option
  `ONNXRUNTIME_CUDA_MAJOR` (12 or 13, default 13) selects the package name for ORT 1.27 and later. OpenCV's own default
  `ONNXRUNTIME_VERSION` is still 1.25.1, so other platforms are unchanged (the 1.31.1 release has no macOS x86_64
  package, for example).
- The provider needs `cublas64_13`, `cublasLt64_13`, `cudart64_13`, `cufft64_12`, `cudnn64_9` and NVRTC, all of which
  the Emgu CUDA build already ships.
- GPU coverage of the 1.31.1 CUDA 13 provider: compiled code for `sm_75`, `sm_80`, `sm_86`, `sm_89`, `sm_90a`,
  `sm_120a`, plus PTX for `sm_120`. The 1.25.1 CUDA 12 provider only had `sm_75`, `sm_86`, `sm_89` and PTX for
  `sm_90`, so A100 and V100 class GPUs had no kernels (ORT issue 28762).
- `onnxruntime_providers_cuda.dll` is 197 MB, down from 294 MB.
- `cvextern` built against the 1.31.1 headers needs the 1.31 `onnxruntime.dll`: running it with the 1.25.1 DLL crashed
  the test host.

Measurement, `yolo11m.onnx` 640x640, RTX 3070, 10 iterations after a warm-up:

| Engine | ms per inference |
|---|---|
| `EngineType.Ort`, default target (CPU) | 158.3 |
| `EngineType.Ort`, `Target.Cuda` | **20.5** |

CPU and CUDA outputs of one forward pass agree: relative L2 error 1.2E-4, max absolute difference 0.55 on box values up
to 637 (one model, one input).

Unit test: `AutoTestModels.TestYoloOrtCuda` (runs with `-p:TestModels=true`, Windows only). It runs yolo11n on both ORT
targets with native stderr captured, fails if the "CUDA EP unavailable" line appears, and compares the two outputs. It
fails as expected when `onnxruntime_providers_cuda.dll` is hidden.

Not verified: Linux (the same package naming applies to `onnxruntime-linux-x64-gpu_cuda13`, but it was not built or
tested), and models other than YOLO.

## History: ORT 1.25.1 with the CUDA 13 build
### Symptom
In the Windows CUDA build, a net created with `EngineType.Ort` and `SetPreferableTarget(Target.Cuda)` ran on the
**CPU**. There is no managed exception. The only sign is one line on the native stderr:

```
[ WARN:0@4.084] global net_impl_backend.cpp:113 finalizeOrt DNN/ONNX/ORT: CUDA EP unavailable
(OrtSessionOptionsAppendExecutionProvider_Cuda: Failed to load shared library), using CPU
```

`opencv/modules/dnn/src/net_impl_backend.cpp` (around line 106) appends ORT's CUDA provider and, on any failure,
logs that warning and falls back to the CPU provider.

### Measurement (2026-11-07, ORT 1.25.1)
`yolo11m.onnx` (640x640, random image), CUDA build, RTX 3070, no CPU dispatch list:

| Engine | ms per inference |
|---|---|
| `EngineType.OpenCV`, CPU | 481 |
| `EngineType.Ort`, default target | 211 |
| `EngineType.Ort`, `Target.Cuda` | 199 |

The last two were the same, because the CUDA provider never loaded.

### Cause
ORT's own log gave the reason:

```
Error loading ...\runtimes\win-x64\native\onnxruntime_providers_cuda.dll which depends on
"cublasLt64_12.dll" which is missing. (Error 126: "The specified module could not be found.")
```

The ORT 1.25.1 GPU package (`onnxruntime-win-x64-gpu-1.25.1.zip`) is built against **CUDA 12**, while the Emgu CUDA
build uses the **CUDA 13.2** toolkit, so `libs/runtimes/win-x64/native` held the CUDA 13 libraries and not the CUDA 12
ones the provider needs (`cudart64_12`, `cublas64_12`, `cublasLt64_12`, `cufft64_11`, `cudnn64_9`).

### Workaround that was verified with 1.25.1
Copying the CUDA 12.6.54 DLLs from the Emgu runtime packages (kept in `D:\release\nuget_repo`) next to the provider made
it load: 19.3 ms per inference with `Target.Cuda`, about 10x faster. It was not adopted because of packaging problems:
- The CUDA 12 and CUDA 13 packages use the same NuGet ids (`Emgu.runtime.windows.cuda.rt`, `.blas`, ...), and a project
  can resolve only one version of an id.
- `cublasLt64_12.dll` is 487 MB (the `.blas.lt` 12.6.54 nupkg is 335 MB), over nuget.org's 250 MB package limit.
- The four extra libraries add about 850 MB unpacked.

Moving to the CUDA 13 package of ORT 1.31.1 made this workaround unnecessary.

## Where this matters in the code
- `Emgu.CV.Models/Dnn/Yolo.cs` (around line 325): `useOrtCuda = CvInvoke.HaveOnnxRuntime && CudaInvoke.HasCuda &&
  !IsV10(version)` selects the ORT engine plus `Target.Cuda`.
- `Emgu.CV.Models/Dnn/Qwen25.cs` and `Qwen3.cs` deliberately do not use ORT (OpenCV's ORT engine has no KV-cache
  support), so they are unaffected.
- `platforms/windows/Build_Binary.ps1`: `-Cuda` together with `-OnnxRuntime` sets `DOWNLOAD_ONNXRUNTIME_GPU=ON` and the
  ORT version.
- `Emgu.CV.Extern/cmake/DependencyDlls.cmake`: copies `onnxruntime*.dll` (including `onnxruntime_providers_cuda.dll`
  and the TensorRT provider) into the native folder.

## Follow-ups to consider (not done)
- A one-time check at model load that warns managed-side when the CUDA provider did not load, instead of relying on a
  native stderr line.
- Linux: confirm the `gpu_cuda13` package works in the Ubuntu CUDA build.

## How to re-check later
Run `AutoTestModels.TestYoloOrtCuda`. By hand: call `ReadNetFromONNX(model, EngineType.Ort)` then
`SetPreferableTarget(Target.Cuda)` and time a few forward passes against `EngineType.Ort` with no target set. A real GPU
run is clearly faster, and the stderr line above is absent. `dotnet test` hides the native stderr, so redirect it in
the test: P/Invoke `_open_osfhandle` and `_dup2(fd, 2)` from `ucrtbase.dll` onto a file opened with
`FileShare.ReadWrite`. ORT writes its error as UTF-16, so decode the file with `utf-16le`.

Related notes in this folder: `Qwen25_KnownIssues.md`, `TODO_VlmOcrModels.md`.
