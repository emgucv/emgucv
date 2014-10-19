REM go to the folder of the current script
pushd %~p0

cd ..
cd opencv
git clean -d -fx "" 
cd ..

cd opencv_contrib
git clean -d -fx "" 
cd ..

cd opencv_extra
git clean -d -fx "" 
cd ..

cd Emgu.CV.Extern

cd cvblob
cd libcvblob
git clean -d -fx "" 
cd ..
cd ..

cd tesseract
cd libtesseract
cd tesseract-ocr.git
git clean -d -fx "" 
cd ..
cd .. 
cd ..

cd ..

git clean -d -fx "" 

popd
