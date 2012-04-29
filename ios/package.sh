rm -rf ios-package
mkdir ios-package
svn export ../Emgu.CV ios-package/Emgu.CV
rm -rf ios-package/Emgu.CV/Android 
cp -rf ../Emgu.CV/PInvoke/MonoTouch/libemgucv.a ios-package/Emgu.CV/PInvoke/MonoTouch

svn export ../Emgu.CV.ML ios-package/Emgu.CV.ML 
svn export ../Emgu.CV.OCR ios-package/Emgu.CV.OCR 
svn export ../Emgu.CV.Stitching ios-package/Emgu.CV.Stitching
svn export ../Emgu.Util ios-package/Emgu.Util
svn export ../Emgu.CV.GPU ios-package/Emgu.CV.GPU

mkdir -p ios-package/Solution/MonoTouch
svn export ../Solution/MonoTouch/Emgu.CV.MonoTouch.sln ios-package/Solution/MonoTouch/Emgu.CV.MonoTouch.sln

mkdir -p ios-package/Emgu.CV.Example/SURFFeature
mkdir -p ios-package/Emgu.CV.Example/PlanarSubdivision
mkdir -p ios-package/Emgu.CV.Example/LicensePlateRecognition
mkdir -p ios-package/Emgu.CV.Example/PedestrianDetection
mkdir -p ios-package/Emgu.CV.Example/TrafficSignRecognition
mkdir -p ios-package/Emgu.CV.Example/FaceDetection
mkdir -p ios-package/opencv/data/haarcascades
svn export ../Emgu.CV.Example/MonoTouch ios-package/Emgu.CV.Example/MonoTouch
svn export ../Emgu.CV.Example/SURFFeature/box.png ios-package/Emgu.CV.Example/SURFFeature/box.png
svn export ../Emgu.CV.Example/SURFFeature/box_in_scene.png ios-package/Emgu.CV.Example/SURFFeature/box_in_scene.png
svn export ../Emgu.CV.Example/SURFFeature/DrawMatches.cs ios-package/Emgu.CV.Example/SURFFeature/DrawMatches.cs
svn export ../Emgu.CV.Example/PlanarSubdivision/DrawSubdivision.cs ios-package/Emgu.CV.Example/PlanarSubdivision/DrawSubdivision.cs
svn export ../Emgu.CV.Example/LicensePlateRecognition/license-plate.jpg ios-package/Emgu.CV.Example/LicensePlateRecognition/license-plate.jpg
svn export ../Emgu.CV.Example/LicensePlateRecognition/LicensePlateDetector.cs ios-package/Emgu.CV.Example/LicensePlateRecognition/LicensePlateDetector.cs
svn export ../Emgu.CV.Example/PedestrianDetection/pedestrian.png ios-package/Emgu.CV.Example/PedestrianDetection/pedestrian.png
svn export ../Emgu.CV.Example/PedestrianDetection/FindPedestrian.cs ios-package/Emgu.CV.Example/PedestrianDetection/FindPedestrian.cs
svn export ../Emgu.CV.Example/TrafficSignRecognition/stop-sign.jpg ios-package/Emgu.CV.Example/TrafficSignRecognition/stop-sign.jpg
svn export ../Emgu.CV.Example/TrafficSignRecognition/stop-sign-model.png ios-package/Emgu.CV.Example/TrafficSignRecognition/stop-sign-model.png
svn export ../Emgu.CV.Example/TrafficSignRecognition/StopSignDetector.cs ios-package/Emgu.CV.Example/TrafficSignRecognition/StopSignDetector.cs
svn export ../Emgu.CV.Example/FaceDetection/lena.jpg ios-package/Emgu.CV.Example/FaceDetection/lena.jpg
svn export ../opencv/data/haarcascades/haarcascade_eye.xml ios-package/opencv/data/haarcascades/haarcascade_eye.xml
svn export ../opencv/data/haarcascades/haarcascade_frontalface_default.xml ios-package/opencv/data/haarcascades/haarcascade_frontalface_default.xml
cp ../CommonAssemblyInfo.cs ios-package
find ./ios-package -type f -name CMakeList* -exec rm '{}' \;
find ./ios-package -type f -name *Android* -exec rm '{}' \;
cd ios-package
rm Emgu.CV/Emgu.CV.csproj Emgu.CV.ML/Emgu.CV.ML.csproj Emgu.CV.GPU/Emgu.CV.GPU.csproj Emgu.CV.OCR/Emgu.CV.OCR.csproj Emgu.Util/Emgu.Util.csproj Emgu.CV.Stitching/Emgu.CV.Stitching.csproj
cd ..

