REM go to the folder of the current script
pushd %~p0

cd ..

cd vtk
git clean -d -f -x "."
cd ..

cd freetype2
git clean -d -f -x "."
cd ..

cd harfbuzz 
git clean -d -f -x "."
cd ..

cd opencv
git clean -d -f -x "." 
cd ..

cd opencv_contrib
git clean -d -f -x "." 
cd ..

cd opencv_extra
git clean -d -f -x "." 
cd ..

cd Emgu.CV.Extern

cd tesseract
cd libtesseract

cd tesseract-ocr.git
git clean -d -f -x "." 
cd ..

cd leptonica
cd leptonica.git
git clean -d -f -x "."
cd ..
cd ..

cd ..
 
cd ..

cd ..

IF NOT "%1%"=="--keep_binary" GOTO CLEAN_ALL

:KEEP_BINARY
move libs opencv\
git clean -d -f -x "." 
move opencv\libs .\
GOTO END_OF_CLEAN

:CLEAN_ALL
git clean -d -f -x "." 

:END_OF_CLEAN


popd
