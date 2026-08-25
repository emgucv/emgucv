Creating Unity package:

* Copy "x86" folder for Windows to "Assets\Emgu.CV\Plugins\x86"
* Copy "x64" folder for Windows "Assets\Emgu.CV\Plugins\x86_64"
* Copy "android" folder for Android to "Assets\Emgu.CV\Plugins\Android\libs", only "armeabi-v7a", "x86" and "arm64-v8a" is needed. 
* Copy "UWP\x86" folder for UWP to "Assets\Emgu.CV\Plugins\WSA\SDK100\x86"
* Copy "UWP\x64" folder for UWP to "Assets\Emgu.CV\Plugins\WSA\SDK100\x86_64"
* Copy "UWP\arm" folder for UWP to "Assets\Emgu.CV\Plugins\WSA\SDK100\ARM"
* Copy "emgucv.bundle" for macOS to "Assets\Emgu.CV\Plugins\emgucv.bundle"
* Copy "iOS\libemgucv.a" for iOS to "Assets\Emgu.CV\Plugins\iOS\libemgucv.a"
* Copy "cvextern.a" for WebGL to "Assets\Emgu.CV\Plugins\WebGL\cvextern.a" -- built with
  `platforms/unity/webgl/cmake_configure_unity.sh` against the target Unity Editor's OWN
  bundled Emscripten toolchain (found under
  `<Editor>/PlaybackEngines/WebGLSupport/BuildTools/Emscripten`), NOT the .NET/Blazor
  Emscripten build in `platforms/emscripten/`. The two toolchains are different pinned
  versions and are not ABI compatible; rebuild this file whenever the target Unity
  version's bundled Emscripten version changes. This build also disables PNG
  (`WITH_PNG:BOOL=FALSE`) -- Unity's own WebGL runtime statically links its own libpng
  for `Texture2D.LoadImage`, and a second copy from cvextern.a causes a wasm-ld
  "duplicate symbol" failure at Unity's final link step -- and disables
  `-fwasm-exceptions` (`EMGU_CV_EMSCRIPTEN_WASM_EXCEPTIONS:BOOL=FALSE`) -- Unity's own
  final link uses the legacy Emscripten JS-exception model, and mixing the two ABIs
  crashes Unity's bundled wasm-ld with a SIGSEGV. Decode images via `Texture2D` +
  `TextureConvert` rather than `CvInvoke.Imread`/`Imwrite` on this platform.
  The default build (no second argument to `cmake_configure_unity.sh`) is the
  **full** variant (opencv_contrib + full module set, ~131 MB) -- run
  `cmake_configure_unity.sh [editor-version] mini` instead for the smaller
  ~29 MB variant (no opencv_contrib/dnn/calib/photo/features/video) if download
  size matters more than module coverage. Like every other binary under
  `Plugins/`, only `cvextern.a.meta` is checked into git -- the `.a` itself is
  built locally and copied in by hand, or produced by CI. The full variant
  needs `PlayerSettings.WebGL.initialMemorySize >= 256MB`, which
  `Assets/Emgu.CV/Editor/WebGLBuildSettings.cs` applies automatically for any
  WebGL build in a project that includes this package.

**Verifying a new `cvextern.a` build for WebGL:** open `HelloWorldScene.unity` (uses
`HelloTexture.cs`, which already exercises `Mat`, `CvInvoke.PutText`, and
`Mat.ToTexture2D()`), switch the build target to WebGL, and build+run in a browser.
A clean browser console with the rendered text confirms the native plugin linked and
executes correctly -- this is the fastest regression check after rebuilding the
native library or updating the target Unity Editor version.