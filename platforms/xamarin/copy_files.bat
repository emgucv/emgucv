cd ../..
rm -rf tmp
git archive --format=tar --prefix=tmp/ HEAD | tar xf -
cd platforms
cd xamarin
cd emgucv_v3
cd component
c:\cygwin64\bin\mkdir.exe -p lib/ios
c:\cygwin64\bin\mkdir.exe -p lib/android
rm -rf samples
mkdir samples
cd samples


mkdir Emgu.CV.World
cp -rf ../../../../../tmp/Emgu.CV.World/tessdata Emgu.CV.World/
mkdir opencv
cd opencv
mkdir data
cd data
mkdir haarcascades
cd ../..
cp -rf ../../../../../opencv/data/haarcascades/haarcascade_eye.xml opencv/data/haarcascades/
cp -rf ../../../../../opencv/data/haarcascades/haarcascade_frontalface_default.xml opencv/data/haarcascades/
cp -rf ../../../../../opencv/LICENSE opencv/
cp -rf ../../../../../CommonAssemblyInfo.cs .
cp -rf ../../../../../components.config .
cp -rf ../../../../../nuget.config .
mkdir Emgu.CV.Example
cd Emgu.CV.Example
mkdir iOS
cd iOS
cp -rf ../../../../../../../tmp/Emgu.CV.Example/iOS/* .
cd ..
mkdir Android
cd Android
cp -rf ../../../../../../../tmp/Emgu.CV.Example/Android/* .
cd ..

mkdir FeatureMatching
mkdir PedestrianDetection
mkdir PlanarSubdivision
mkdir LicensePlateRecognition
mkdir TrafficSignRecognition
mkdir FaceDetection

cp -rf ../../../../../../tmp/Emgu.CV.Example/FeatureMatching/box.png FeatureMatching/
cp -rf ../../../../../../tmp/Emgu.CV.Example/FeatureMatching/box_in_scene.png FeatureMatching/
cp -rf ../../../../../../tmp/Emgu.CV.Example/FeatureMatching/DrawMatches.cs FeatureMatching/
cp -rf ../../../../../../tmp/Emgu.CV.Example/PedestrianDetection/pedestrian.png PedestrianDetection/
cp -rf ../../../../../../tmp/Emgu.CV.Example/PedestrianDetection/FindPedestrian.cs PedestrianDetection/
cp -rf ../../../../../../tmp/Emgu.CV.Example/PlanarSubdivision/DrawSubdivision.cs PlanarSubdivision/
cp -rf ../../../../../../tmp/Emgu.CV.Example/TrafficSignRecognition/StopSignDetector.cs TrafficSignRecognition/
cp -rf ../../../../../../tmp/Emgu.CV.Example/TrafficSignRecognition/stop-sign.jpg TrafficSignRecognition/
cp -rf ../../../../../../tmp/Emgu.CV.Example/TrafficSignRecognition/stop-sign-model.png TrafficSignRecognition/
cp -rf ../../../../../../tmp/Emgu.CV.Example/FaceDetection/DetectFace.cs FaceDetection/
cp -rf ../../../../../../tmp/Emgu.CV.Example/FaceDetection/lena.jpg FaceDetection/
cp -rf ../../../../../../tmp/Emgu.CV.Example/LicensePlateRecognition/license-plate.jpg LicensePlateRecognition/
cp -rf ../../../../../../tmp/Emgu.CV.Example/LicensePlateRecognition/LicensePlateDetector.cs LicensePlateRecognition/
cd ..
rm -rf Solution
mkdir Solution
cd Solution
mkdir iOS
mkdir Android
cd ..
cp ../../../Emgu.CV.iOS.Example.sln Solution/iOS/
cp ../../../Emgu.CV.Example.iOS.csproj Emgu.CV.Example/iOS/
cp ../../../Emgu.CV.Android.Example.sln Solution/Android/
cp ../../../AndroidExamples.csproj Emgu.CV.Example/Android/

cd ..
cd ..
cd ..

REM xamarin-component  package emgucv_v3\component
