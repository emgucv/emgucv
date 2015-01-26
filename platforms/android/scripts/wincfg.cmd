:: variables required for OpenCV build ::
:: Note: all pathes should be specified without tailing slashes!
SET ANDROID_NDK=C:\android-ndk-r10d
SET CMAKE_EXE=C:\Program Files (x86)\CMake\bin\cmake.exe
SET MAKE_EXE=%ANDROID_NDK%\prebuilt\windows-x86_64\bin\make.exe

:: variables required for android-opencv build ::
SET ANDROID_SDK=C:\Program Files (x86)\Android\android-sdk
SET ANT_DIR=C:\apache-ant-1.9.2
SET JAVA_HOME=C:\Program Files (x86)\Java\jdk1.6.0_27

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
