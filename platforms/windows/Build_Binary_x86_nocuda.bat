pushd %~dp0
call Build_Binary_x86.bat x86 nogpu vc no-openni
popd
