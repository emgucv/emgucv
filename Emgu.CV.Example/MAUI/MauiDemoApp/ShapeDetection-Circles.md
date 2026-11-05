# Shape Detection: circles are under-detected

Status: open, not yet investigated. Observed 2026-11-05.

## Symptom

On the Shape Detection page, the **3. Circles** result often misses circles that are
clearly present in the sample image. Triangles, rectangles and lines are detected as
expected on the same images, and the pipeline itself runs without errors.

| Scene | Circles in the sample image | Circles detected |
|---|---|---|
| 1 | 1 (filled pink, large) | 0 (blank result image) |
| 2 | 3 (purple outline, filled pink, small filled green) | 2: purple outline and filled pink; the small filled green circle was missed |

Detection time was 19–27 ms per image, so this is not a timeout or a crash. The page
generates a new random scene each time it opens and on **New Image**, so results vary
from run to run.

## Where it was seen

- Device: iPhone X (`iPhone10,6`), iOS 16.7.16, physical device over USB.
- Build: Debug, `net10.0-ios26.0`, `ios-arm64`, native `libs/iOS/libcvextern_ios.xcframework`
  built with `platforms/ios/build.sh device_arm64` (full variant).
- Not yet compared against another platform, so it is **unconfirmed** whether this is
  iOS-specific. The detection code is shared by every platform, so the expectation is
  that it is not.

## Relevant code

- `ShapeDetector.cs`, `ComputeShapes()`: circle detection is
  ```csharp
  double cannyThreshold = 180.0;
  double circleAccumulatorThreshold = 120;
  circles = CvInvoke.HoughCircles(_gray, HoughModes.Gradient, 2.0, 20.0, cannyThreshold,
      circleAccumulatorThreshold, 5);
  ```
  `dp = 2.0`, `minDist = 20`, `param1` (Canny high threshold) = 180, `param2`
  (accumulator threshold) = 120, `minRadius = 5`, `maxRadius` defaulted.
  The input is `_gray` after a 3x3 Gaussian blur.
- `ShapeDetectionPage.cs`, `GenerateRandomShapes()` / `DrawShape()`: builds the
  800x560 random scene: 5–8 non-overlapping shapes, circle radius 34–74 px, randomly
  filled or outlined, random colors on white.

## Hypotheses, none verified yet

1. `param2 = 120` is too strict for this image size and these radii. With
   `HoughModes.Gradient`, a lower accumulator threshold finds more circles, with more
   false positives.
2. Color-dependent contrast: after `Bgr2Gray`, some fill colors may have weak edges
   against white. The missed green and pink fills are candidates. The Canny threshold of
   180 inside `HoughCircles` may drop those edges.
3. Possibly `HoughModes.GradientAlt` would be more robust for these clean synthetic
   images (it uses a different `param2` scale, roughly 0–1).

## Suggested next steps

- Reproduce on desktop (Windows, macOS or Ubuntu) with the same generated scene to
  confirm it is platform-independent.
- Make the scene reproducible for testing, for example with a fixed seed for `_rng`, and
  compare the count of drawn circles with the count detected over many seeds.
- Try lowering `circleAccumulatorThreshold`, or switch to `GradientAlt`, and check for
  false positives on the triangles and rectangles.

## Reproducing on the iPhone X (as done on 2026-11-05)

This Intel Mac mini runs at most Xcode 26.3, so the default `net10.0-ios` target (iOS
26.5 bindings from workload set 10.0.401) does not build. The demo also sets an iOS
minimum of 17.2 in `Directory.Build.props.in`. Both were overridden on the command line
only:

```bash
dotnet build Emgu.CV.Example/MAUI/MauiDemoApp/MauiDemoApp.csproj \
  -p:TargetFrameworks=net10.0-ios26.0 -f net10.0-ios26.0 -r ios-arm64 \
  -p:SupportedOSPlatformVersion=16.0 -p:ValidateXcodeVersion=false \
  -p:CodesignKey="Apple Development: Canming Huang (424YYX4M5U)"

/usr/local/share/dotnet/packs/Microsoft.iOS.Sdk.net10.0_26.0/26.0.11017/tools/bin/mlaunch \
  --installdev Emgu.CV.Example/MAUI/MauiDemoApp/bin/Debug/net10.0-ios26.0/ios-arm64/MauiDemoApp.app \
  --devname 5c134567325d1fc18d3a669ea08c6c4f4bdd23c4
```

The page was driven with Appium 3.9.0 and the XCUITest driver, using a WebDriverAgent
already running on the device (`appium:webDriverAgentUrl` = `http://localhost:8100`,
`appium:bundleId` = `com.emgu.cv.maui.demo`). The steps were: tap **Shape Detection**,
scroll to **Results**, tap **3. Circles** to view the result full screen, then use
**New Image** for another random scene. The full-screen viewer's close button has no
accessibility label, so it was tapped by screen position.
