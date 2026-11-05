# Shape Detection: circles are under-detected

Status: **fixed** on 2026-11-05 by lowering the `HoughCircles` accumulator threshold from 120 to
90 in `ShapeDetector.cs`. First observed on 2026-11-05.

## Symptom

On the Shape Detection page, the **3. Circles** result often missed circles that were clearly in
the sample image. Triangles, rectangles and lines were detected as expected on the same images.

| Scene | Circles in the sample image | Circles detected |
|---|---|---|
| 1 | 1 (filled pink, large) | 0 (blank result image) |
| 2 | 3 (purple outline, filled pink, small filled green) | 2: the small filled green circle was missed |

Seen on an iPhone X (iOS 16.7.16). The detection code is shared by every platform, and the
benchmark below reproduces the same behavior on macOS, so it is not iOS-specific.

## Cause

The circle step in `ShapeDetector.ComputeShapes()` is
`HoughCircles(_gray, HoughModes.Gradient, dp 2.0, minDist 20, param1 180, param2, minRadius 5)`.
With `param2` (the accumulator threshold) at 120, outlined circles were almost always found but
filled circles often weren't. A thick outline has two edges, inner and outer, so its center
collects about twice the votes of a filled circle, which has a single edge. 120 was above what
many filled circles of the demo's 34–74 px radii can reach.

Fill color made no difference: the misses were spread evenly over all nine palette colors, so
the earlier color-contrast hypothesis was wrong.

## Benchmark

A scratch console app recreated `ShapeDetectionPage.GenerateRandomShapes()` / `DrawShape()` with
a fixed seed per scene, recorded every circle drawn, and scored the same preprocessing
(`Bgr2Gray`, 3x3 Gaussian blur) plus `HoughCircles` against that ground truth. A detection
counts as found if its center is within 25% of the radius and its radius within 30%. Run on
macOS x64 against `libs/runtimes/osx-x64/native/libcvextern.dylib`, 1000 scenes (seeds 0–999),
1359 circles:

| `param2` | Circles found | False detections | Perfect scenes |
|---|---|---|---|
| 120 (before) | 54.7% (577 of 616 misses were filled) | 0 | 530 / 1000 |
| 85 | 97.1% | 42 | 935 / 1000 |
| **90 (chosen)** | **94.8%** | **17** | **920 / 1000** |
| 95 | 91.3% | 10 | 881 / 1000 |

At 80 and below, rectangles, triangles and line ends start producing false circles (46 at 80
and 971 at 60, over 300 scenes). Raising `minDist` to 50 removed the duplicate circles stacked
on a real one but not those false circles. `HoughModes.GradientAlt` found about 97% but always
left about 26 duplicate circles per 300 scenes, so the plain `Gradient` mode with a lower
threshold is the simpler fix. 90 was picked over 85 for about 40% of the false detections, at
the cost of 15 fewer perfect scenes per 1000.

## Remaining limits

About 5% of circles are still missed. At 90, 68 of the 70 misses over 1000 scenes are filled
circles with a radius under 60 px (36 at 34–44 px, 32 at 45–59 px); none of 60 px or more
were missed. A finer accumulator (`dp` 1.5) didn't help: at `param2` 60 it matched the
perfect-scene count with 3x the false detections, and at 70 it found only 74.6%. On the iPhone
X after the change, three of four random scenes had all their circles found; the fourth
missed one small filled circle, which is consistent with this limit. Higher recall would need a
different approach, such as detecting circles from contours (`FindContours` + circularity),
rather than more `HoughCircles` tuning.
