pushd %~p0
rm -rf test_x64
mkdir test_x64
cp -rf ../bin/x64/* ../bin/Release/
cp ../CMakeCache.txt test_x64/
cp -r ../opencv_extra/testdata/* test_x64/
cd test_x64
c:\Python27\python.exe ..\..\opencv\modules\ts\misc\run.py
c:\Python27\python.exe ..\..\opencv\modules\ts\misc\report.py *.xml -c median > out.html
popd