rm -rf Assets/Emgu.CV
mkdir Assets\Emgu.CV
cd Assets
cd Emgu.CV
mkdir Emgu.Util
mkdir Emgu.CV
mkdir Emgu.CV.ML
mkdir Emgu.CV.Cuda
mkdir Emgu.CV.Nonfree
mkdir Emgu.CV.OCR
mkdir Emgu.CV.Stitching
mkdir Emgu.CV.Shape
cd ..
cd ..

cp -r ../Emgu.Util/*.cs Assets/Emgu.CV/Emgu.Util/

cp -r ../Emgu.CV/*.cs Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/CameraCalibration Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Color Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Cvb Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Features2D Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Flann Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Geodetic Assets/Emgu.CV/Emgu.CV/
mkdir Assets\Emgu.CV\Emgu.CV\PInvoke
xcopy ..\Emgu.CV\PInvoke Assets\Emgu.CV\Emgu.CV\PInvoke\ /s /e
cp -r ../Emgu.CV/Reflection Assets/Emgu.CV/Emgu.CV/
rm -rf Assets/Emgu.CV/Emgu.CV/PInvoke/iOS
rm -rf Assets/Emgu.CV/Emgu.CV/PInvoke/Windows.Store
rm -rf Assets/Emgu.CV/Emgu.CV/PInvoke/Android
cp -r ../Emgu.CV/PointAndLine Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Shape Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Tiff Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Util Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/VideoSurveillance Assets/Emgu.CV/Emgu.CV/

cp -r ../Emgu.CV.ML/*.cs Assets/Emgu.CV/Emgu.CV.ML/
mkdir Assets\Emgu.CV\Emgu.CV.ML\PInvoke 
xcopy ..\Emgu.CV.ML\PInvoke Assets\Emgu.CV\Emgu.CV.ML\ /s /e

cp -r ../Emgu.CV.Cuda/*.cs Assets/Emgu.CV/Emgu.CV.Cuda/
mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Bgsegm
xcopy ..\Emgu.CV.Cuda\Bgsegm Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e
mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Features2d
xcopy ..\Emgu.CV.Cuda\Features2d Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e
mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Features2d
xcopy ..\Emgu.CV.Cuda\Filters Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e
mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Imgproc
xcopy ..\Emgu.CV.Cuda\Imgproc Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e
mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Optflow
xcopy ..\Emgu.CV.Cuda\Optflow Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e
mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Stereo
xcopy ..\Emgu.CV.Cuda\Stereo Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e

cp -r ../Emgu.CV.Nonfree/*.cs Assets/Emgu.CV.Nonfree/

cp -r ../Emgu.CV.OCR/*.cs Assets/Emgu.CV.OCR/

cp -r ../Emgu.CV.Stitching/*.cs Assets/Emgu.CV.Stitching/

cp -r ../Emgu.CV.Shape/*.cs Assets/Emgu.CV.Shape/

cd Assets
mkdir Plugins
cd Plugins
mkdir Android
cd Android
mkdir libs
xcopy ..\..\..\..\libs libs\ /s /e
cd ..
cd ..
cd ..