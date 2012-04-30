rm -rf ..\build_armeabi
rm -rf 
cd ..
rm -rf build_armeabi
rm -rf build_armeabi-v7a
rm -rf build_x86
call scripts\build armeabi
call scripts\build armeabi-v7a
call scripts\build x86
cd scripts