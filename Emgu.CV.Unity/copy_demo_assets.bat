rm -rf Assets/Emgu.CV/Emgu.CV.Demo
REM mkdir Assets\Emgu.CV\Emgu.CV.Demo
mkdir -p Assets\Emgu.CV\Emgu.CV.Demo\Resources
REM mkdir Assets\Emgu.CV\Emgu.CV.Demo\Resources\tessdata

cp ../opencv/samples/data/lena.jpg Assets/Emgu.CV/Emgu.CV.Demo/Resources
cp ../Emgu.CV.Test/stitch*.jpg Assets/Emgu.CV/Emgu.CV.Demo/Resources/

cp ../opencv/data/haarcascades/haarcascade_frontalface_alt2.xml Assets/Emgu.CV/Emgu.CV.Demo/Resources

REM cd ../Emgu.CV.World/tessdata/
REM c:\cygwin64\bin\find . -type f -exec cp "{}" "../../Emgu.CV.Unity/Assets/Emgu.CV/Emgu.CV.Demo/Resources/tessdata/{}.bytes" ";"
REM cd ../../Emgu.CV.Unity

REM cp haarcascade_frontalface_alt2.xml.meta Assets/Emgu.CV/Emgu.CV.Demo/Resources
cp -rf demo/* Assets/Emgu.CV/Emgu.CV.Demo
cp unityStoreIcons/EmguCVLogo_128x128.png Assets/Emgu.CV/Emgu.CV.Demo/EmguCVLogo.png