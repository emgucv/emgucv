@ECHO OFF

PUSHD %~dp0..
SET ANDROID_ABI=armeabi
CALL .\scripts\build.cmd %*

SET ANDROID_ABI=armeabi-v7a
CALL .\scripts\build.cmd %*
POPD