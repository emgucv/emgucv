pushd %~p0

call remove_from_path PATH "C:\Program Files (x86)\Git\bin;"

cd ..\..\..
cp -f opencv\platforms\android\android.toolchain.cmake android.toolchain.cmake
if "%1%"=="noclean" GOTO END_CLEAN

:CLEAN
rm -rf build_armeabi
rm -rf build_armeabi-v7a
rm -rf build_x86
rm -rf build

:END_CLEAN
call platforms\android\scripts\build armeabi
call platforms\android\scripts\build armeabi-v7a
call platforms\android\scripts\build x86
unzip build_armeabi\libemgucv-android-armeabi -d build
unzip build_armeabi-v7a\libemgucv-android-armeabi-v7a -d build
unzip build_x86\libemgucv-android-x86 -d build
cd build
mkdir libemgucv-android 
xcopy libemgucv-android-x86 libemgucv-android /E /Y
xcopy libemgucv-android-armeabi libemgucv-android /E /Y
xcopy libemgucv-android-armeabi-v7a libemgucv-android /E /Y
cd libemgucv-android
mv bin libs
xcopy sdk\native\libs libs /E /Y
rm -rf sdk\native libs\armeabi\libopencv_androidcamera.a libs\armeabi-v7a\libopencv_androidcamera.a libs\x86\libopencv_androidcamera.a 
cd ..
rm -rf libemgucv-android.zip
zip -r libemgucv-android libemgucv-android
popd