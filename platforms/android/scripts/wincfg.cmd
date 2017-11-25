:: variables required for OpenCV build ::
:: Note: all pathes should be specified without tailing slashes!
IF EXIST "C:\android-ndk-r10e" SET ANDROID_NDK=C:\android-ndk-r10e
IF EXIST "C:\android-ndk-r11" SET ANDROID_NDK=C:\android-ndk-r11
IF EXIST "C:\android-ndk-r11c" SET ANDROID_NDK=C:\android-ndk-r11c
IF EXIST "C:\android-ndk-r12b" SET ANDROID_NDK=C:\android-ndk-r12b
IF EXIST "C:\android-ndk-r13" SET ANDROID_NDK=C:\android-ndk-r13
IF EXIST "C:\android-ndk-r13b" SET ANDROID_NDK=C:\android-ndk-r13b
IF EXIST "C:\android-ndk-r14" SET ANDROID_NDK=C:\android-ndk-r14
IF EXIST "C:\android-ndk-r14b" SET ANDROID_NDK=C:\android-ndk-r14b
IF EXIST "C:\android-ndk-r15" SET ANDROID_NDK=C:\android-ndk-r15
IF EXIST "C:\android-ndk-r15b" SET ANDROID_NDK=C:\android-ndk-r15b
IF EXIST "C:\android-ndk-r15c" SET ANDROID_NDK=C:\android-ndk-r15c


IF EXIST "%programfiles(x86)%\CMake 2.8\bin\cmake.exe" SET CMAKE_EXE=%programfiles(x86)%\CMake 2.8\bin\cmake.exe
IF EXIST "%programfiles(x86)%\CMake\bin\cmake.exe" SET CMAKE_EXE=%programfiles(x86)%\CMake\bin\cmake.exe
IF EXIST "%programfiles%\CMake\bin\cmake.exe" SET CMAKE_EXE=%programfiles%\CMake\bin\cmake.exe
IF EXIST "%programw6432%\CMake\bin\cmake.exe" SET CMAKE_EXE=%programw6432%\CMake\bin\cmake.exe


SET MAKE_EXE=%ANDROID_NDK%\prebuilt\windows-x86_64\bin\make.exe

:: variables required for android-opencv build ::
IF EXIST "C:\Program Files (x86)\Android\android-sdk" SET ANDROID_SDK=C:\Program Files (x86)\Android\android-sdk
IF EXIST "C:\Android\android-sdk" SET ANDROID_SDK=C:\Android\android-sdk

SET ANT_DIR="%VS140COMNTOOLS%..\..\Apps\apache-ant-1.9.3"

@ECHO OFF &SETLOCAL 
FOR /F "tokens=2*" %%a IN ('REG QUERY "HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Development Kit\1.7" /v JavaHome') DO set "JavaHome17=%%b"
FOR /F "tokens=2*" %%a IN ('REG QUERY "HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Development Kit\1.8" /v JavaHome') DO set "JavaHome18=%%b"
::ECHO Java Home 17: %JavaHome17%

IF NOT "" == "%JavaHome17%" SET JAVA_HOME=%JavaHome17%
IF NOT "" == "%JavaHome18%" set JAVA_HOME=%JavaHome18%
ECHO Java Home: %JAVA_HOME%
:: configuration options ::
:::: general ARM-V7 settings
:: SET ANDROID_ABI=armeabi-v7a
:: SET BUILD_DIR=build_armeabi-v7a

:::: uncomment following lines to compile for old emulator or old device
SET BUILD_DIR=build_%1

:::: uncomment following lines to compile for ARM-V7 with NEON support
::SET ANDROID_ABI=armeabi-v7a with NEON
::SET BUILD_DIR=build_neon

:::: uncomment following lines to compile for x86
::SET ANDROID_ABI=x86
::SET BUILD_DIR=build_x86

:::: other options
::SET ANDROID_NATIVE_API_LEVEL=8   &:: android-3 is enough for native part of OpenCV but android-8 is required for Java API

ECHO CMAKE_EXE: %CMAKE_EXE%
ECHO MAKE_EXE: %MAKE_EXE%
ECHO ANDROID_SDK: %ANDROID_SDK%
ECHO ANDROID_NDK: %ANDROID_NDK%