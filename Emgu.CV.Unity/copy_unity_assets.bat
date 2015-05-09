cd Assets
cd Emgu.CV
rm -rf Emgu.Util
mkdir Emgu.Util
rm -rf Emgu.CV
mkdir Emgu.CV
rm -rf Emgu.CV.ML
mkdir Emgu.CV.ML
rm -rf Emgu.CV.Contrib
mkdir Emgu.CV.Contrib
rm -rf Emgu.CV.OCR
mkdir Emgu.CV.OCR
rm -rf Emgu.CV.Stitching
mkdir Emgu.CV.Stitching
rm -rf Emgu.CV.Shape
mkdir Emgu.CV.Shape
cd ..
cd ..

cp ../opencv/LICENSE Assets/Emgu.CV/opencv.license.txt
cp ../Emgu.CV.Extern/tesseract/libtesseract/tesseract-ocr.git/COPYING Assets/Emgu.CV/tesseract.ocr.license.txt
cp unityStoreIcons/README.txt Assets/Emgu.CV/README.txt

cp -r ../Emgu.Util/*.cs Assets/Emgu.CV/Emgu.Util/

cp -r ../Emgu.CV/*.cs Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/CameraCalibration Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Color Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Cvb Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Features2D Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Flann Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Geodetic Assets/Emgu.CV/Emgu.CV/
mkdir Assets\Emgu.CV\Emgu.CV\PInvoke
xcopy ..\Emgu.CV\PInvoke Assets\Emgu.CV\Emgu.CV\PInvoke\ /s /e /Y
cp -r ../Emgu.CV/Reflection Assets/Emgu.CV/Emgu.CV/
rm -rf Assets/Emgu.CV/Emgu.CV/PInvoke/iOS
REM rm -rf Assets/Emgu.CV/Emgu.CV/PInvoke/Windows.Store
rm -rf Assets/Emgu.CV/Emgu.CV/PInvoke/Android
cp -r ../Emgu.CV/PointAndLine Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Shape Assets/Emgu.CV/Emgu.CV/
REM cp -r ../Emgu.CV/Tiff Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Util Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/VideoSurveillance Assets/Emgu.CV/Emgu.CV/

cp -r ../Emgu.CV.ML/*.cs Assets/Emgu.CV/Emgu.CV.ML/
mkdir Assets\Emgu.CV\Emgu.CV.ML\PInvoke 
xcopy ..\Emgu.CV.ML\PInvoke Assets\Emgu.CV\Emgu.CV.ML\ /s /e /Y

REM cp -r ../Emgu.CV.Cuda/*.cs Assets/Emgu.CV/Emgu.CV.Cuda/
REM mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Bgsegm
REM xcopy ..\Emgu.CV.Cuda\Bgsegm Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e /Y
REM mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Features2d
REM xcopy ..\Emgu.CV.Cuda\Features2d Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e /Y
REM mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Features2d
REM xcopy ..\Emgu.CV.Cuda\Filters Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e /Y
REM mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Imgproc
REM xcopy ..\Emgu.CV.Cuda\Imgproc Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e /Y
REM mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Optflow
REM xcopy ..\Emgu.CV.Cuda\Optflow Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e /Y
REM mkdir Assets\Emgu.CV\Emgu.CV.Cuda\Stereo
REM xcopy ..\Emgu.CV.Cuda\Stereo Assets\Emgu.CV\Emgu.CV.Cuda\ /s /e /Y

cp -r ../Emgu.CV.Contrib/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\BgSegm
cp -r ../Emgu.CV.Contrib/BgSegm/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/BgSegm
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\Face
cp -r ../Emgu.CV.Contrib/Face/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/Face
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\Text
cp -r ../Emgu.CV.Contrib/Text/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/Text 
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\XFeatures2D
cp -r ../Emgu.CV.Contrib/XFeatures2D/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/XFeatures2D


cp -r ../Emgu.CV.OCR/*.cs Assets/Emgu.CV/Emgu.CV.OCR/

cp -r ../Emgu.CV.Stitching/*.cs Assets/Emgu.CV/Emgu.CV.Stitching/

cp -r ../Emgu.CV.Shape/*.cs Assets/Emgu.CV/Emgu.CV.Shape/
