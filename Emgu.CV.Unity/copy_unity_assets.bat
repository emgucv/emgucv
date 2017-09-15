cd Assets
cd Emgu.CV
rm -rf Emgu.Util
mkdir Emgu.Util
rm -rf Emgu.CV
mkdir Emgu.CV
rm -rf Emgu.CV.Contrib
mkdir Emgu.CV.Contrib
rm -rf Emgu.CV.OCR
mkdir Emgu.CV.OCR
cd ..
cd ..

cp ../opencv/LICENSE Assets/Emgu.CV/opencv.license.txt
cp ../Emgu.CV.Extern/tesseract/libtesseract/tesseract-ocr.git/COPYING Assets/Emgu.CV/tesseract.ocr.license.txt
cp unityStoreIcons/README.txt Assets/Emgu.CV/README.txt

cp ../Emgu.Util/*.cs Assets/Emgu.CV/Emgu.Util/

cp ../Emgu.CV/*.cs Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/CameraCalibration Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Capture Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Color Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Cvb Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Features2D Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Flann Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Geodetic Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Ml Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Ocl Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Optflow Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Photo Assets/Emgu.CV/Emgu.CV/
mkdir Assets\Emgu.CV\Emgu.CV\PInvoke
xcopy ..\Emgu.CV\PInvoke Assets\Emgu.CV\Emgu.CV\PInvoke\ /s /e /Y
rm -rf Assets/Emgu.CV/Emgu.CV/PInvoke/iOS
rm -rf Assets/Emgu.CV/Emgu.CV/PInvoke/Android
cp -r ../Emgu.CV/PointAndLine Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Reflection Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Shape Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Stitching Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Superres Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Video Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Videostab Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/VideoSurveillance Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Dnn Assets/Emgu.CV/Emgu.CV/

REM rm -rf Assets/Emgu.CV/Emgu.CV/PInvoke/Windows.Store
REM cp -r ../Emgu.CV/Tiff Assets/Emgu.CV/Emgu.CV/
cp -r ../Emgu.CV/Util Assets/Emgu.CV/Emgu.CV/

REM cp -r ../Emgu.CV/Viz Assets/Emgu.CV/Emgu.CV/

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

mkdir Assets\Emgu.CV\Emgu.CV.Contrib\Aruco
cp -r ../Emgu.CV.Contrib/Aruco/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/Aruco
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\BgSegm
cp -r ../Emgu.CV.Contrib/BgSegm/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/BgSegm

mkdir Assets\Emgu.CV\Emgu.CV.Contrib\Dpm
cp -r ../Emgu.CV.Contrib/Dpm/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/Dpm
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\Face
cp -r ../Emgu.CV.Contrib/Face/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/Face
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\Fuzzy
cp -r ../Emgu.CV.Contrib/Fuzzy/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/Fuzzy
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\LineDescriptor
cp -r ../Emgu.CV.Contrib/LineDescriptor/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/LineDescriptor
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\Plot
cp -r ../Emgu.CV.Contrib/Plot/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/Plot
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\Text
cp -r ../Emgu.CV.Contrib/Text/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/Text
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\Tracking
cp -r ../Emgu.CV.Contrib/Tracking/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/Tracking 
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\XFeatures2D
cp -r ../Emgu.CV.Contrib/XFeatures2D/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/XFeatures2D
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\XImgproc
cp -r ../Emgu.CV.Contrib/XImgproc/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/XImgproc
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\XObjdetect
cp -r ../Emgu.CV.Contrib/XObjdetect/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/XObjdetect
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\XPhoto
cp -r ../Emgu.CV.Contrib/XPhoto/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/XPhoto
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\Bioinspired
cp -r ../Emgu.CV.Contrib/Bioinspired/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/Bioinspired
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\DnnModern
cp -r ../Emgu.CV.Contrib/DnnModern/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/DnnModern
mkdir Assets\Emgu.CV\Emgu.CV.Contrib\ImgHash
cp -r ../Emgu.CV.Contrib/ImgHash/*.cs Assets/Emgu.CV/Emgu.CV.Contrib/ImgHash


cp -r ../Emgu.CV.OCR/*.cs Assets/Emgu.CV/Emgu.CV.OCR/

