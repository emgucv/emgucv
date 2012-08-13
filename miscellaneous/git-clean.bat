REM go to the folder of the current script
pushd %~p0

cd ..
REM emgu cv folder
cd opencv_attic
git clean -d -fx "" 
cd ..
REM emgu cv folder
cd Emgu.CV.Extern

cd cvblob
cd libcvblob
git clean -d -fx "" 
cd ..
cd ..

cd tesseract
cd libtesseract
cd tesseract-ocr
git clean -d -fx "" 
cd ..
cd .. 
cd ..

cd ..

git clean -d -fx "" 

popd
