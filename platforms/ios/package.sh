#!/usr/bin/env bash
set -e

cd ../..
rm -rf tmp
git archive --format=tar --prefix=tmp/ HEAD | tar xf -
cd platforms/ios

rm -rf ios-package

mkdir -p ios-package/libs/iOS
cp -f ../../libs/iOS/libcvextern.a ios-package/libs/iOS

cp -r ../../tmp/Emgu.CV ios-package/Emgu.CV
cp -f ../../Emgu.CV/*.cs ios-package/Emgu.CV/
cp -f ../../Emgu.CV/Capture/*.cs ios-package/Emgu.CV/Capture
cp -f ../../Emgu.CV/Features2D/*.cs ios-package/Emgu.CV/Features2D
cp -f ../../Emgu.CV/Ocl/*.cs ios-package/Emgu.CV/Ocl
cp -f ../../Emgu.CV/Ml/*.cs ios-package/Emgu.CV/Ml
cp -f ../../Emgu.CV/Optflow/*.cs ios-package/Emgu.CV/Optflow
cp -f ../../Emgu.CV/Shape/*.cs ios-package/Emgu.CV/Shape
cp -f ../../Emgu.CV/Stitching/*.cs ios-package/Emgu.CV/Stitching
cp -f ../../Emgu.CV/Video/*.cs ios-package/Emgu.CV/Video
cp -f ../../Emgu.CV/PInvoke/CvInvokeEntryPoints.cs ios-package/Emgu.CV/PInvoke
cp -f ../../Emgu.CV/Util/*.cs ios-package/Emgu.CV/Util
rm -rf ios-package/Emgu.CV/PInvoke/Android
rm -rf ios-package/Emgu.CV/PInvoke/System.Drawing
rm -rf ios-package/Emgu.CV/PInvoke/Windows.Store
rm -rf ios-package/Emgu.CV/PInvoke/Unity

cp -r ../../tmp/Emgu.CV.OCR ios-package/Emgu.CV.OCR
cp -f ../../Emgu.CV.OCR/*.cs ios-package/Emgu.CV.OCR

cp -r ../../tmp/Emgu.Util ios-package/Emgu.Util

cp -r ../../tmp/Emgu.CV.Cuda ios-package/Emgu.CV.Cuda
cp -f ../../Emgu.CV.Cuda/*.cs ios-package/Emgu.CV.Cuda/

cp -r ../../tmp/Emgu.CV.Contrib ios-package/Emgu.CV.Contrib
cp -f ../../Emgu.CV.Contrib/Plot/*.cs ios-package/Emgu.CV.Contrib/Plot
cp -f ../../Emgu.CV.Contrib/LineDescriptor/VectorOf*.cs ios-package/Emgu.CV.Contrib/LineDescriptor
cp -f ../../Emgu.CV.Contrib/Text/VectorOf*.cs ios-package/Emgu.CV.Contrib/Text
cp -f ../../Emgu.CV.Contrib/XPhoto/*.cs ios-package/Emgu.CV.Contrib/XPhoto
cp -f ../../Emgu.CV.Contrib/XFeatures2D/*.cs ios-package/Emgu.CV.Contrib/XFeatures2D

cp -r ../../tmp/Emgu.CV.World ios-package/Emgu.CV.World

mkdir -p ios-package/Solution/iOS
cp ../../tmp/Solution/iOS/Emgu.CV.iOS.sln ios-package/Solution/iOS/
cp ../../tmp/Solution/iOS/Emgu.CV.iOS.Example.sln ios-package/Solution/iOS/

mkdir -p ios-package/Emgu.CV.Example/FeatureMatching
mkdir -p ios-package/Emgu.CV.Example/PlanarSubdivision
mkdir -p ios-package/Emgu.CV.Example/LicensePlateRecognition
mkdir -p ios-package/Emgu.CV.Example/PedestrianDetection
mkdir -p ios-package/Emgu.CV.Example/TrafficSignRecognition
mkdir -p ios-package/Emgu.CV.Example/FaceDetection
mkdir -p ios-package/Emgu.CV.Example/XamarinForms/Core
mkdir -p ios-package/Emgu.CV.Example/XamarinForms/iOS

mkdir -p ios-package/opencv/data/haarcascades
cp -r ../../tmp/Emgu.CV.Example/iOS ios-package/Emgu.CV.Example/iOS
cp ../../tmp/Emgu.CV.Example/FeatureMatching/box.png ios-package/Emgu.CV.Example/FeatureMatching/box.png
cp ../../tmp/Emgu.CV.Example/FeatureMatching/box_in_scene.png ios-package/Emgu.CV.Example/FeatureMatching/box_in_scene.png
cp ../../tmp/Emgu.CV.Example/FeatureMatching/DrawMatches.cs ios-package/Emgu.CV.Example/FeatureMatching/DrawMatches.cs
cp ../../tmp/Emgu.CV.Example/PlanarSubdivision/DrawSubdivision.cs ios-package/Emgu.CV.Example/PlanarSubdivision/DrawSubdivision.cs
cp ../../tmp/Emgu.CV.Example/LicensePlateRecognition/license-plate.jpg ios-package/Emgu.CV.Example/LicensePlateRecognition/license-plate.jpg
cp ../../tmp/Emgu.CV.Example/LicensePlateRecognition/LicensePlateDetector.cs ios-package/Emgu.CV.Example/LicensePlateRecognition/LicensePlateDetector.cs
cp ../../tmp/Emgu.CV.Example/PedestrianDetection/pedestrian.png ios-package/Emgu.CV.Example/PedestrianDetection/pedestrian.png
cp ../../tmp/Emgu.CV.Example/PedestrianDetection/FindPedestrian.cs ios-package/Emgu.CV.Example/PedestrianDetection/FindPedestrian.cs
cp ../../tmp/Emgu.CV.Example/TrafficSignRecognition/stop-sign.jpg ios-package/Emgu.CV.Example/TrafficSignRecognition/stop-sign.jpg
cp ../../tmp/Emgu.CV.Example/TrafficSignRecognition/stop-sign-model.png ios-package/Emgu.CV.Example/TrafficSignRecognition/stop-sign-model.png
cp ../../tmp/Emgu.CV.Example/TrafficSignRecognition/StopSignDetector.cs ios-package/Emgu.CV.Example/TrafficSignRecognition/StopSignDetector.cs
cp ../../tmp/Emgu.CV.Example/FaceDetection/lena.jpg ios-package/Emgu.CV.Example/FaceDetection/lena.jpg
cp ../../tmp/Emgu.CV.Example/FaceDetection/DetectFace.cs ios-package/Emgu.CV.Example/FaceDetection/DetectFace.cs
cp -r ../../tmp/Emgu.CV.Example/XamarinForms/Core/* ios-package/Emgu.CV.Example/XamarinForms/Core/
cp -r ../../tmp/Emgu.CV.Example/XamarinForms/iOS/* ios-package/Emgu.CV.Example/XamarinForms/iOS/
cp ../../opencv/data/haarcascades/haarcascade_eye.xml ios-package/opencv/data/haarcascades/haarcascade_eye.xml
cp ../../opencv/data/haarcascades/haarcascade_frontalface_default.xml ios-package/opencv/data/haarcascades/haarcascade_frontalface_default.xml
cp ../../CommonAssemblyInfo.cs ios-package
cp ../../tmp/components.config ios-package
cp ../../tmp/nuget.config ios-package
find ./ios-package -type f -name CMakeList* -exec rm '{}' \;
#find ./ios-package -type f -name *Android* -exec rm '{}' \;
#find ./ios-package -type f -name *Windows.Store* -exec rm '{}' \;
#find ./ios-package -type f -name *Windows.Phone* -exec rm '{}' \;
cd ios-package
#rm Emgu.CV/Emgu.CV.csproj Emgu.CV.ML/Emgu.CV.ML.csproj Emgu.CV.Cuda/Emgu.CV.Cuda.csproj Emgu.CV.OCR/Emgu.CV.OCR.csproj Emgu.Util/Emgu.Util.csproj Emgu.CV.Stitching/Emgu.CV.Stitching.csproj Emgu.CV.Contrib/Emgu.CV.Contrib.csproj Emgu.CV.Shape/Emgu.CV.Shape.csproj
#find . -regex  '.*[^S].csproj' -exec rm '{}' \;
cd ..

/Library/Frameworks/Mono.framework/Commands/xbuild /p:Configuration=Release ../../Emgu.CV.World/Emgu.CV.World.IOS.csproj
#mkdir -p ios-package/bin
cp ../../libs/Emgu.CV.World.IOS.dll ios-package/libs
cp ../../libs/Emgu.CV.World.IOS.XML ios-package/libs
cp ../../Emgu.CV.License.txt ios-package
#gitversion=$(git log --oneline | wc -l | tr -d " ")
emgucvversion=$(cat ../../emgucv.version.txt)
cd ios-package
zip -r libemgucv-ios-unified-$emgucvversion.zip *
mv *.zip ../
cd ..
