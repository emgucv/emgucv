pushd %~p0
cd ..
cp -f ..\opencv\platforms\android\android.toolchain.cmake android.toolchain.cmake
rm -rf build_armeabi
rm -rf build_armeabi-v7a
rm -rf build_x86
rm -rf build
call scripts\build armeabi
call scripts\build armeabi-v7a
call scripts\build x86
unzip build_armeabi\libemgucv-android-armeabi-cuda -d build
unzip build_armeabi-v7a\libemgucv-android-armeabi-v7a-cuda -d build
unzip build_x86\libemgucv-android-x86-cuda -d build
cd build
mkdir libemgucv-android 
xcopy libemgucv-android-x86-cuda libemgucv-android /E /Y
xcopy libemgucv-android-armeabi-cuda libemgucv-android /E /Y
xcopy libemgucv-android-armeabi-v7a-cuda libemgucv-android /E /Y
cd libemgucv-android
mv bin libs
xcopy sdk\native\libs libs /E /Y
rm -rf sdk\native libs\armeabi\libopencv_androidcamera.a libs\armeabi-v7a\libopencv_androidcamera.a libs\x86\libopencv_androidcamera.a 
cd ..
zip -r libemgucv-android libemgucv-android
popd