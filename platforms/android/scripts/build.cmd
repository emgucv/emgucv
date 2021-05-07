REM POSSIBLE OPTIONS: 
REM %1%: "x86", "x86_64", "arm64-v8a" or "armeabi-v7a"
REM %2%: "core", if set to "core", will not build the contrib module
REM %3%: optional android-toolchain

pushd %~p0

cd ..\..\..

@ECHO OFF

:: enable command extensions
VERIFY BADVALUE 2>NUL
SETLOCAL ENABLEEXTENSIONS || (ECHO Unable to enable command extensions. & EXIT \B)

:: build environment
SET SOURCE_DIR=%cd%
echo SOURCE_DIR=%SOURCE_DIR%
SET ANDROID_NATIVE_API_LEVEL=24

:: load configuration
PUSHD %~dp0
SET SCRIPTS_DIR=%cd%
IF EXIST .\wincfg.cmd CALL .\wincfg.cmd %1
POPD

IF EXIST .\jni\nul (SET BUILD_JAVA_PART=1) ELSE (SET BUILD_JAVA_PART=0)

SET BUILD_OPENCV=1

:: Check optional android-toolchain
IF ("%3"=="") (SET ANDROID_TOOLCHAIN_CMAKE="") else (SET ANDROID_TOOLCHAIN_CMAKE=-DANDROID_TOOLCHAIN_NAME=%3)

:: inherit old names
IF NOT DEFINED CMAKE SET CMAKE=%CMAKE_EXE%
IF NOT DEFINED MAKE SET MAKE=%MAKE_EXE%

:: defaults
IF NOT DEFINED BUILD_DIR SET BUILD_DIR=build_%1
IF NOT DEFINED ANDROID_ABI SET ANDROID_ABI=%1
SET OPENCV_BUILD_DIR=%SCRIPTS_DIR%\..\%BUILD_DIR%
SET INSTALL_FOLDER=%cd%\%BUILD_DIR%\install

:: check that all required variables defined
PUSHD .
IF NOT DEFINED ANDROID_NDK (ECHO. & ECHO You should set an environment variable ANDROID_NDK to the full path to your copy of Android NDK & GOTO end)
(CD "%ANDROID_NDK%") || (ECHO. & ECHO Directory "%ANDROID_NDK%" specified by ANDROID_NDK variable does not exist & GOTO end)

IF NOT EXIST "%CMAKE%" (ECHO. & ECHO You should set an environment variable CMAKE to the full path to cmake executable & GOTO end)
IF NOT EXIST "%MAKE%" (ECHO. & ECHO You should set an environment variable MAKE to the full path to native port of make executable & GOTO end)

IF NOT %BUILD_JAVA_PART%==1 GOTO required_variables_checked

IF NOT DEFINED ANDROID_SDK (ECHO. & ECHO You should set an environment variable ANDROID_SDK to the full path to your copy of Android SDK & GOTO end)
(CD "%ANDROID_SDK%" 2>NUL) || (ECHO. & ECHO Directory "%ANDROID_SDK%" specified by ANDROID_SDK variable does not exist & GOTO end)

IF NOT DEFINED ANT_DIR (ECHO. & ECHO You should set an environment variable ANT_DIR to the full path to Apache Ant root & GOTO end)
(CD "%ANT_DIR%" 2>NUL) || (ECHO. & ECHO Directory "%ANT_DIR%" specified by ANT_DIR variable does not exist & GOTO end)

IF NOT DEFINED JAVA_HOME (ECHO. & ECHO You should set an environment variable JAVA_HOME to the full path to JDK & GOTO end)
(CD "%JAVA_HOME%" 2>NUL) || (ECHO. & ECHO Directory "%JAVA_HOME%" specified by JAVA_HOME variable does not exist & GOTO end)

:required_variables_checked
POPD

:: check for ninja
echo "%MAKE%"|findstr /i ninja >nul:
IF %errorlevel%==1 (SET BUILD_WITH_NINJA=0) ELSE (SET BUILD_WITH_NINJA=1)
IF %BUILD_WITH_NINJA%==1 (SET CMAKE_GENERATOR=Ninja) ELSE (SET CMAKE_GENERATOR=MinGW Makefiles)

IF EXIST "%ANDROID_NDK%\build\cmake\android.toolchain.cmake" SET CMAKE_TOOLCHAIN_FILE=%ANDROID_NDK%\build\cmake\android.toolchain.cmake
REM IF EXIST "%SCRIPTS_DIR%\..\..\..\opencv\platforms\android\android.toolchain.cmake" SET CMAKE_TOOLCHAIN_FILE=%SCRIPTS_DIR%\..\..\..\opencv\platforms\android\android.toolchain.cmake

::Setup the contrib modules
IF "%2"=="core" GOTO CORE_BUILD_CONFIG

:FULL_BUILD_CONFIG
SET CMAKE_CXX_FLAGS_RELEASE_OPTION=-DCMAKE_CXX_FLAGS_RELEASE:STRING="-g0 -O3" 
SET CMAKE_C_FLAGS_RELEASE_OPTION=-DCMAKE_C_FLAGS_RELEASE:STRING="-g0 -O3" 
SET TESSERACT_OPTION=-DEMGU_CV_WITH_TESSERACT:BOOL=ON 
GOTO END_CONFIG

:CORE_BUILD_CONFIG
SET CMAKE_CXX_FLAGS_RELEASE_OPTION=-DCMAKE_CXX_FLAGS_RELEASE:STRING="-g0 -O3" 
SET CMAKE_C_FLAGS_RELEASE_OPTION=-DCMAKE_C_FLAGS_RELEASE:STRING="-g0 -O3"
SET TESSERACT_OPTION=-DEMGU_CV_WITH_TESSERACT:BOOL=OFF
:END_CONFIG

SET CMAKE_CONF_FLAGS= -G "%CMAKE_GENERATOR%" ^
-DANDROID_ABI="%ANDROID_ABI%" ^
-DANDROID_PLATFORM=%ANDROID_NATIVE_API_LEVEL% ^
-DCMAKE_TOOLCHAIN_FILE="%CMAKE_TOOLCHAIN_FILE%" ^
%ANDROID_TOOLCHAIN_CMAKE% ^
-DCMAKE_MAKE_PROGRAM="%MAKE%" ^
-DCMAKE_C_FLAGS:STRING=-std=c11 ^
%CMAKE_CXX_FLAGS_RELEASE_OPTION% ^
%CMAKE_C_FLAGS_RELEASE_OPTION% ^
-DCMAKE_SHARED_LINKER_FLAGS:STRING="-Wl,--gc-sections, -Wl,--exclude-libs,All" ^
-DCMAKE_POLICY_DEFAULT_CMP0069=NEW ^
-DCMAKE_INTERPROCEDURAL_OPTIMIZATION:BOOL=ON ^
-DCMAKE_INSTALL_PREFIX:STRING="%INSTALL_FOLDER:\=/%" ^
-DCMAKE_BUILD_TYPE:STRING=Release

REM -DCMAKE_SHARED_LINKER_FLAGS:STRING="-Wl,--gc-sections" ^

REM cd openvino 
REM IF NOT EXIST "%BUILD_DIR%" mkdir "%BUILD_DIR%"
REM cd "%BUILD_DIR%"
REM "%CMAKE%" %CMAKE_CONF_FLAGS% %* -DENABLE_OPENCV:BOOL=OFF -DENABLE_MKL_DNN:BOOL=OFF -DANDROID_LD=deprecated .. 
REM SET OPENVINO_DIR=%cd%
REM echo on
REM "%CMAKE%" --build . --config Release --target install
REM cd .. 
REM cd ..

cd eigen 
IF NOT EXIST "%BUILD_DIR%" mkdir "%BUILD_DIR%"
cd "%BUILD_DIR%"
"%CMAKE%" %CMAKE_CONF_FLAGS% %* .. 
SET EIGEN_DIR=%cd%
echo on
"%CMAKE%" --build . --config Release --target install
cd .. 
cd ..


::cd ..\vtk 
::mkdir install
::"%CMAKE%" -G"%CMAKE_GENERATOR%" -DCMAKE_MAKE_PROGRAM="c:\MinGW\bin\mingw32-make.exe" -DANDROID_NDK="%ANDROID_NDK%" -DANDROID_ARCH_NAME=x86 -DANDROID_NATIVE_API_LEVEL=15 -DVTK_ANDROID_BUILD:BOOL=ON -DANDROID_EXECUTABLE="C:\Program Files (x86)\Android\android-sdk\tools\android.bat" -DANT_EXECUTABLE="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Apps\apache-ant-1.9.3\bin\ant.cmd"  -DCMAKE_INSTALL_PREFIX:PATH="%cd%"
::c:\MinGW\bin\mingw32-make.exe
::cd ..\build

::Setup the contrib modules
IF "%2"=="core" GOTO CORE_BUILD_EMGU

:FULL_BUILD_EMGU
cd 3rdParty
cd freetype2
IF NOT EXIST "%BUILD_DIR%" mkdir "%BUILD_DIR%"
cd "%BUILD_DIR%"
"%CMAKE%" %CMAKE_CONF_FLAGS% %* .. 
"%CMAKE%" --build . --config Release --parallel --target install
cd .. 
cd ..
cd ..


cd harfbuzz
IF NOT EXIST "%BUILD_DIR%" mkdir "%BUILD_DIR%"
cd "%BUILD_DIR%"
"%CMAKE%" %CMAKE_CONF_FLAGS% -DCMAKE_FIND_ROOT_PATH:STRING=%INSTALL_FOLDER:\=/% -DHB_HAVE_FREETYPE:BOOL=TRUE ..
"%CMAKE%" --build . --config Release --parallel --target install
cd ..
cd ..


SET OPENCV_EXTRA_MODULES_DIR=%SOURCE_DIR%\opencv_contrib\modules
SET CMAKE_CONF_FLAGS=%CMAKE_CONF_FLAGS% -DOPENCV_EXTRA_MODULES_PATH:String="%OPENCV_EXTRA_MODULES_DIR:\=/%" 
GOTO END_CONTRIB_EMGU

:CORE_BUILD_EMGU
SET CMAKE_CONF_FLAGS=%CMAKE_CONF_FLAGS% -DEMGU_CV_WITH_FREETYPE:BOOL=OFF 

:END_CONTRIB_EMGU


:: create build dir
IF DEFINED REBUILD rmdir /S /Q "%BUILD_DIR%" 2>NUL
MKDIR "%BUILD_DIR%" 2>NUL
PUSHD "%BUILD_DIR%" || (ECHO. & ECHO Directory "%BUILD_DIR%" is not found & GOTO end)

:: run cmake
ECHO. & ECHO Runnning cmake...
ECHO ANDROID_ABI=%ANDROID_ABI%
ECHO.
IF NOT %BUILD_OPENCV%==1 GOTO other-cmake
:opencv-cmake
echo on



SET CMAKE_CONF_FLAGS=%CMAKE_CONF_FLAGS% ^
%TESSERACT_OPTION% ^
-DBUILD_SHARED_LIBS:BOOL=OFF ^
-DBUILD_ANDROID_EXAMPLES:BOOL=OFF ^
-DBUILD_PERF_TESTS:BOOL=OFF ^
-DPARALLEL_ENABLE_PLUGINS:BOOL=OFF ^
-DWITH_IPP:BOOL=OFF ^
-DBUILD_DOCS:BOOL=OFF ^
-DBUILD_TESTS:BOOL=OFF ^
-DBUILD_WITH_DEBUG_INFO:BOOL=OFF ^
-DBUILD_opencv_java:BOOL=OFF ^
-DBUILD_opencv_java_bindings_generator:BOOL=OFF ^
-DBUILD_opencv_ts:BOOL=OFF ^
-DWITH_ITT:BOOL=OFF ^
-DWITH_OPENCL:BOOL=ON ^
-DWITH_CUDA:BOOL=OFF ^
-DBUILD_ANDROID_PROJECTS=OFF ^
-DWITH_EIGEN:BOOL=ON ^
-DBUILD_FAT_JAVA_LIB:BOOL=FALSE ^
-DBUILD_JAVA:BOOL=FALSE ^
-DEMGU_CV_WITH_DEPTHAI:BOOL=FALSE ^
-DCMAKE_FIND_ROOT_PATH:STRING=%INSTALL_FOLDER:\=/% ^
-DEigen3_DIR:STRING=%EIGEN_DIR:\=/% ^
-DANDROID_LD=deprecated

REM For reason on adding "-DANDROID_LD=deprecated" flag, see https://github.com/android/ndk/issues/1426

REM -DENABLE_LTO:BOOL=ON ^
REM -DENABLE_THIN_LTO:BOOL=OFF ^

::("%CMAKE%" -G"%CMAKE_GENERATOR%" -DANDROID_ABI="%ANDROID_ABI%" -DCMAKE_TOOLCHAIN_FILE="%SOURCE_DIR%\android.toolchain.cmake" %ANDROID_TOOLCHAIN_CMAKE% -DCMAKE_MAKE_PROGRAM="%MAKE%" %* "%SOURCE_DIR%" -DOPENCV_EXTRA_MODULES_PATH="%SOURCE_DIR%\opencv_contrib\modules" -DANDROID_EXTRA_NDK_VERSIONS="-r15c" -DBUILD_SHARED_LIBS:BOOL=OFF -DBUILD_ANDROID_EXAMPLES:BOOL=OFF -DBUILD_PERF_TESTS:BOOL=OFF -DWITH_IPP:BOOL=OFF -DBUILD_DOCS:BOOL=OFF -DBUILD_TESTS:BOOL=OFF -DBUILD_WITH_DEBUG_INFO:BOOL=OFF -DBUILD_opencv_java:BOOL=OFF -DBUILD_opencv_ts:BOOL=OFF -DBUILD_opencv_androidcamera:BOOL=OFF -DBUILD_opencv_adas:BOOL=OFF -DBUILD_opencv_hdf:BOOL=OFF -DBUILD_opencv_saliency:BOOL=OFF -DBUILD_TBB:BOOL=OFF -DWITH_OPENMP:BOOL=ON -DOpenMP_CXX_FLAGS="-fopenmp" -DOpenMP_C_FLAGS="-fopenmp" -DWITH_OPENCL:BOOL=ON -DWITH_CUDA:BOOL=OFF -DANDROID_NATIVE_API_LEVEL:STRING=21 -DCMAKE_CONFIGURATION_TYPES:STRING=Release) && GOTO cmakefin
("%CMAKE%" %CMAKE_CONF_FLAGS% %* "%SOURCE_DIR%" ) ^
&& GOTO cmakefin
REM -DCMAKE_BUILD_TYPE:STRING=MINSIZEREL) ^
REM -DANDROID_STL:STRING=c++_static ^
REM -DCMAKE_CXX_FLAGS:STRING="-D__cplusplus=201103L" ^

ECHO. & ECHO cmake failed &	GOTO end
:other-cmake
("%CMAKE%" -G"%CMAKE_GENERATOR%" -DANDROID_ABI="%ANDROID_ABI%" -DOpenCV_DIR="%OPENCV_BUILD_DIR%" -DCMAKE_TOOLCHAIN_FILE="%OPENCV_BUILD_DIR%\..\android.toolchain.cmake" -DCMAKE_MAKE_PROGRAM="%MAKE%" %* "%SOURCE_DIR%") && GOTO cmakefin
ECHO. & ECHO cmake failed &	GOTO end
:cmakefin

:: run make
ECHO. & ECHO Building native libs...
SET VERBOSE=1
IF %BUILD_WITH_NINJA%==0 ("%MAKE%" -j %NUMBER_OF_PROCESSORS% VERBOSE=%VERBOSE% package) || (ECHO. & ECHO make failed & GOTO end)
IF %BUILD_WITH_NINJA%==1 ("%MAKE%") || (ECHO. & ECHO ninja failed & GOTO end)

IF NOT %BUILD_JAVA_PART%==1 GOTO end
POPD && PUSHD %SOURCE_DIR%

:: configure java part
ECHO. & ECHO Updating Android project...
(CALL "%ANDROID_SDK%\tools\android" update project --name %PROJECT_NAME% --path .) || (ECHO. & ECHO failed to update android project & GOTO end)

:: compile java part
ECHO. & ECHO Compiling Android project...
(CALL "%ANT_DIR%\bin\ant" debug) || (ECHO. & ECHO failed to compile android project & GOTO end)

:end
POPD
ENDLOCAL

popd
