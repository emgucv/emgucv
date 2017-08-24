cd ../..
rm -rf tmp
git archive --format=tar --prefix=tmp/ HEAD | tar xf -
cd platforms
cd xamarin
cd emgucv_v3
cd component
c:\cygwin64\bin\mkdir.exe -p lib/ios
c:\cygwin64\bin\mkdir.exe -p lib/android
REM rm -rf samples
REM mkdir samples
cd samples
REM mkdir ios
cd ios
REM mkdir Emgu.CV.Example
cd Emgu.CV.Example
REM mkdir iOS
cd iOS
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/iOS/* .
cp -rf ../../../../../../../../opencv/data/haarcascades/haarcascade_eye.xml .
cp -rf ../../../../../../../../opencv/data/haarcascades/haarcascade_frontalface_default.xml .
cp -rf ../../../../../../../../opencv/LICENSE .
cp -rf ../../../../../../../../CommonAssemblyInfo.cs .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/FeatureMatching/box.png .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/FeatureMatching/box_in_scene.png .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/FeatureMatching/DrawMatches.cs .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/PedestrianDetection/pedestrian.png .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/PedestrianDetection/FindPedestrian.cs .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/PlanarSubdivision/DrawSubdivision.cs .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/TrafficSignRecognition/StopSignDetector.cs .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/TrafficSignRecognition/stop-sign.jpg .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/TrafficSignRecognition/stop-sign-model.png .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/FaceDetection/DetectFace.cs .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/FaceDetection/lena.jpg .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/LicensePlateRecognition/license-plate.jpg .
cp -rf ../../../../../../../../tmp/Emgu.CV.Example/LicensePlateRecognition/LicensePlateDetector.cs .

git checkout Emgu.CV.Example.iOS.csproj

cd ..
cd ..
cd ..

REM mkdir android
cd android
cp -rf ../../../../../../tmp/components.config .
cp -rf ../../../../../../tmp/nuget.config .
cp -rf ../../../../../../tmp/Emgu.CV.Example/Android/* .
cp -rf ../../../../../../opencv/data/haarcascades/haarcascade_eye.xml Assets/
cp -rf ../../../../../../opencv/data/haarcascades/haarcascade_frontalface_default.xml Assets/
c:\cygwin64\bin\mkdir.exe -p Resources/Raw
cp -rf ../../../../../../opencv/LICENSE Resources/Raw/
cp -rf ../../../../../../CommonAssemblyInfo.cs .
cp -rf ../../../../../../tmp/Emgu.CV.Example/FeatureMatching/box.png Assets/
cp -rf ../../../../../../tmp/Emgu.CV.Example/FeatureMatching/box_in_scene.png Assets/
cp -rf ../../../../../../tmp/Emgu.CV.Example/FeatureMatching/DrawMatches.cs .
cp -rf ../../../../../../tmp/Emgu.CV.Example/PedestrianDetection/pedestrian.png Assets/
cp -rf ../../../../../../tmp/Emgu.CV.Example/PedestrianDetection/FindPedestrian.cs .
cp -rf ../../../../../../tmp/Emgu.CV.Example/PlanarSubdivision/DrawSubdivision.cs .
cp -rf ../../../../../../tmp/Emgu.CV.Example/TrafficSignRecognition/StopSignDetector.cs .
cp -rf ../../../../../../tmp/Emgu.CV.Example/TrafficSignRecognition/stop-sign.jpg Assets/
cp -rf ../../../../../../tmp/Emgu.CV.Example/TrafficSignRecognition/stop-sign-model.png Assets/
cp -rf ../../../../../../tmp/Emgu.CV.Example/FaceDetection/DetectFace.cs .
cp -rf ../../../../../../tmp/Emgu.CV.Example/FaceDetection/lena.jpg Assets/
cp -rf ../../../../../../tmp/Emgu.CV.Example/LicensePlateRecognition/license-plate.jpg Assets/
cp -rf ../../../../../../tmp/Emgu.CV.Example/LicensePlateRecognition/LicensePlateDetector.cs .

git checkout AndroidExamples.csproj

cd ..


REM cp ../../../Emgu.CV.iOS.Example.sln ios/
REM cp ../../../Emgu.CV.Example.iOS.csproj ios/Emgu.CV.Example/iOS/
REM cp ../../../Emgu.CV.Android.Example.sln android/
REM cp ../../../AndroidExamples.csproj android/

cd ..
cd ..
cd ..

REM xamarin-component  package emgucv_v3\component
