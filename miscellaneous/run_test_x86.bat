pushd %~p0
rm -rf test_x86
mkdir test_x86
cp -rf ../bin/x86/* ../bin/Release/
cp ../CMakeCache.txt test_x86/
cp -r ../opencv_extra/testdata/* test_x86/
cd test_x86
c:\Python27\python.exe ..\..\opencv\modules\ts\misc\run.py
c:\Python27\python.exe ..\..\opencv\modules\ts\misc\report.py *.xml -c median > out.html
popd