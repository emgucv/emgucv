cd ..
rm -rf tmp
git archive --format=tar --prefix=tmp/ HEAD | tar xf -
cd ios

rm -rf ios-package
mkdir ios-package
cp -r ../tmp/Emgu.CV ios-package/Emgu.CV
rm -rf ios-package/Emgu.CV/Android 
cp -rf ../Emgu.CV/PInvoke/MonoTouch/libemgucv.a ios-package/Emgu.CV/PInvoke/MonoTouch

cp -r ../tmp/Emgu.CV.ML ios-package/Emgu.CV.ML
cp -r ../tmp/Emgu.CV.OCR ios-package/Emgu.CV.OCR
cp -r ../tmp/Emgu.CV.Stitching ios-package/Emgu.CV.Stitching
cp -r ../tmp/Emgu.Util ios-package/Emgu.Util
cp -r ../tmp/Emgu.CV.GPU ios-package/Emgu.CV.GPU

mkdir -p ios-package/Solution/MonoTouch
cp ../tmp/Solution/MonoTouch/Emgu.CV.MonoTouch.sln ios-package/Solution/MonoTouch/Emgu.CV.MonoTouch.sln

mkdir -p ios-package/Emgu.CV.Example/SURFFeature
mkdir -p ios-package/Emgu.CV.Example/PlanarSubdivision
mkdir -p ios-package/Emgu.CV.Example/LicensePlateRecognition
mkdir -p ios-package/Emgu.CV.Example/PedestrianDetection
mkdir -p ios-package/Emgu.CV.Example/TrafficSignRecognition
mkdir -p ios-package/Emgu.CV.Example/FaceDetection
mkdir -p ios-package/opencv/data/haarcascades
cp -r ../tmp/Emgu.CV.Example/MonoTouch ios-package/Emgu.CV.Example/MonoTouch
cp ../tmp/Emgu.CV.Example/SURFFeature/box.png ios-package/Emgu.CV.Example/SURFFeature/box.png
cp ../tmp/Emgu.CV.Example/SURFFeature/box_in_scene.png ios-package/Emgu.CV.Example/SURFFeature/box_in_scene.png
cp ../tmp/Emgu.CV.Example/SURFFeature/DrawMatches.cs ios-package/Emgu.CV.Example/SURFFeature/DrawMatches.cs
cp ../tmp/Emgu.CV.Example/PlanarSubdivision/DrawSubdivision.cs ios-package/Emgu.CV.Example/PlanarSubdivision/DrawSubdivision.cs
cp ../tmp/Emgu.CV.Example/LicensePlateRecognition/license-plate.jpg ios-package/Emgu.CV.Example/LicensePlateRecognition/license-plate.jpg
cp ../tmp/Emgu.CV.Example/LicensePlateRecognition/LicensePlateDetector.cs ios-package/Emgu.CV.Example/LicensePlateRecognition/LicensePlateDetector.cs
cp ../tmp/Emgu.CV.Example/PedestrianDetection/pedestrian.png ios-package/Emgu.CV.Example/PedestrianDetection/pedestrian.png
cp ../tmp/Emgu.CV.Example/PedestrianDetection/FindPedestrian.cs ios-package/Emgu.CV.Example/PedestrianDetection/FindPedestrian.cs
cp ../tmp/Emgu.CV.Example/TrafficSignRecognition/stop-sign.jpg ios-package/Emgu.CV.Example/TrafficSignRecognition/stop-sign.jpg
cp ../tmp/Emgu.CV.Example/TrafficSignRecognition/stop-sign-model.png ios-package/Emgu.CV.Example/TrafficSignRecognition/stop-sign-model.png
cp ../tmp/Emgu.CV.Example/TrafficSignRecognition/StopSignDetector.cs ios-package/Emgu.CV.Example/TrafficSignRecognition/StopSignDetector.cs
cp ../tmp/Emgu.CV.Example/FaceDetection/lena.jpg ios-package/Emgu.CV.Example/FaceDetection/lena.jpg
cp ../tmp/Emgu.CV.Example/FaceDetection/DetectFace.cs ios-package/Emgu.CV.Example/FaceDetection/DetectFace.cs
cp ../opencv/data/haarcascades/haarcascade_eye.xml ios-package/opencv/data/haarcascades/haarcascade_eye.xml
cp ../opencv/data/haarcascades/haarcascade_frontalface_default.xml ios-package/opencv/data/haarcascades/haarcascade_frontalface_default.xml
cp ../CommonAssemblyInfo.cs ios-package
find ./ios-package -type f -name CMakeList* -exec rm '{}' \;
find ./ios-package -type f -name *Android* -exec rm '{}' \;
cd ios-package
rm Emgu.CV/Emgu.CV.csproj Emgu.CV.ML/Emgu.CV.ML.csproj Emgu.CV.GPU/Emgu.CV.GPU.csproj Emgu.CV.OCR/Emgu.CV.OCR.csproj Emgu.Util/Emgu.Util.csproj Emgu.CV.Stitching/Emgu.CV.Stitching.csproj
cd ..

gitversion=$(git log --oneline | wc -l | tr -d " ")
zip -r libemgucv-ios-$gitversion ios-package