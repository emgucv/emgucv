rm -rf Assets
mkdir Assets
cd Assets
mkdir Emgu.Util
mkdir Emgu.CV
cd ..

cp -r ../Emgu.Util/*.cs Assets/Emgu.Util/

cp -r ../Emgu.CV/*.cs Assets/Emgu.CV/
cp -r ../Emgu.CV/CameraCalibration Assets/Emgu.CV/

cp -r ../Emgu.CV/Color Assets/Emgu.CV/
cp -r ../Emgu.CV/Cvb Assets/Emgu.CV/
cp -r ../Emgu.CV/Features2D Assets/Emgu.CV/
cp -r ../Emgu.CV/Flann Assets/Emgu.CV/
cp -r ../Emgu.CV/Geodetic Assets/Emgu.CV/
cp -r ../Emgu.CV/PInvoke Assets/Emgu.CV/
cp -r ../Emgu.CV/Reflection Assets/Emgu.CV/
rm -rf Assets/Emgu.CV/PInvoke/iOS
rm -rf Assets/Emgu.CV/PInvoke/Windows.Store
rm -rf Assets/Emgu.CV/PInvoke/Android
cp -r ../Emgu.CV/PointAndLine Assets/Emgu.CV/
cp -r ../Emgu.CV/Shape Assets/Emgu.CV/
cp -r ../Emgu.CV/Tiff Assets/Emgu.CV/
cp -r ../Emgu.CV/Util Assets/Emgu.CV/
cp -r ../Emgu.CV/VideoSurveillance Assets/Emgu.CV/