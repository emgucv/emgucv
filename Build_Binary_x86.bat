@echo off
IF "%1%"=="64" ECHO "BUILDING 64bit solution" 
IF NOT "%1%"=="64" ECHO "BUILDING 32bit solution"

SET OS_MODE=
IF "%1%"=="64" SET OS_MODE= Win64
  
SET PROGRAMFILES_DIR_X86=%programfiles(x86)%
if NOT EXIST "%PROGRAMFILES_DIR_X86%" SET PROGRAMFILES_DIR_X86=%programfiles%
SET PROGRAMFILES_DIR=%programfiles%

REM Find CMake  
SET CMAKE="cmake.exe"
IF EXIST "%PROGRAMFILES_DIR_X86%\CMake 2.8\bin\cmake.exe" SET CMAKE="%PROGRAMFILES_DIR_X86%\CMake 2.8\bin\cmake.exe"

IF EXIST "CMakeCache.txt" del CMakeCache.txt

REM Find Visual Studio or Msbuild
SET VS2005="%VS80COMNTOOLS%..\IDE\devenv.exe"
SET VS2008="%VS90COMNTOOLS%..\IDE\devenv.exe"
REM SET VS2010="%VS100COMNTOOLS%\..\IDE\devenv.exe"
SET MSBUILD35="%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe"

IF EXIST %MSBUILD35% SET DEVENV=%MSBUILD35%
IF EXIST %VS2005% SET DEVENV=%VS2005% 
IF EXIST %VS2008% SET DEVENV=%VS2008%
REM IF EXIST %VS2010% SET DEVENV=%VS2010%

IF %DEVENV%==%MSBUILD35% SET BUILD_TYPE=/property:Configuration=Release
IF %DEVENV%==%VS2005% SET BUILD_TYPE=/Build Release
IF %DEVENV%==%VS2008% SET BUILD_TYPE=/Build Release
REM IF %DEVENV%==%VS2010% SET BUILD_TYPE=/Build Release

IF %DEVENV%==%MSBUILD35% SET CMAKE_CONF="Visual Studio 8 2005%OS_MODE%"
IF %DEVENV%==%VS2005% SET CMAKE_CONF="Visual Studio 8 2005%OS_MODE%"
IF %DEVENV%==%VS2008% SET CMAKE_CONF="Visual Studio 9 2008%OS_MODE%"
REM IF %DEVENV%==%VS2010% SET CMAKE_CONF="Visual Studio 10%OS_MODE%"

SET CMAKE_CONF_FLAGS= -G %CMAKE_CONF% ^
-DBUILD_DOXYGEN_DOCS:BOOL=FALSE ^
-DBUILD_TESTS:BOOL=FALSE ^
-DBUILD_NEW_PYTHON_SUPPORT:BOOL=FALSE ^
-DEMGU_ENABLE_SSE:BOOL=TRUE ^
-DCMAKE_INSTALL_PREFIX="%TEMP%" 

IF "%4%"=="doc" ^
SET CMAKE_CONF_FLAGS=%CMAKE_CONF_FLAGS% -DEMGU_CV_DOCUMENTATION_BUILD:BOOL=TRUE 

IF NOT "%2%"=="gpu" GOTO END_OF_GPU

:WITH_GPU
REM Find cuda
SET CUDA_SDK_DIR=%CUDA_PATH%.
IF "%OS_MODE%"==" Win64" SET CUDA_SDK_DIR=%CUDA_PATH_V3_2%.
SET NPP_SDK_DIR=%CUDA_SDK_DIR%\npp_3.2.16_win_32\SDK
IF "%OS_MODE%"==" Win64" SET NPP_SDK_DIR=%CUDA_SDK_DIR%\npp_3.2.16_win_64\SDK
echo %NPP_SDK_DIR%

IF EXIST "%NPP_SDK_DIR%" SET CMAKE_CONF_FLAGS=%CMAKE_CONF_FLAGS% ^
-DWITH_CUDA:BOOL=TRUE ^
-DCUDA_VERBOSE_BUILD:BOOL=TRUE ^
-DCUDA_TOOLKIT_ROOT_DIR="%CUDA_SDK_DIR%" ^
-DCUDA_SDK_ROOT_DIR="%CUDA_SDK_DIR%" ^
-DCUDA_NPP_LIBRARY_ROOT_DIR="%NPP_SDK_DIR%" 

:END_OF_GPU

IF "%3%"=="intel" GOTO INTEL_COMPILER
IF NOT "%3%"=="intel" GOTO VISUAL_STUDIO

:INTEL_COMPILER
REM Find Intel Compiler 
SET INTEL_DIR=%ICPP_COMPILER12%\bin
SET INTEL_ENV=%ICPP_COMPILER12%\bin\iclvars.bat
SET INTEL_ICL=%ICPP_COMPILER12%\bin\ia32\icl.exe
IF "%OS_MODE%"==" Win64" SET INTEL_ICL=%ICPP_COMPILER12%\bin\intel64\icl.exe
SET INTEL_TBB=%TBB30_INSTALL_DIR%\include
IF "%OS_MODE%"==" Win64" SET INTEL_IPP=%ICPP_COMPILER12%\redist\intel64\ipp
SET ICPROJCONVERT=%PROGRAMFILES_DIR_X86%\Common Files\Intel\shared files\ia32\Bin\ICProjConvert120.exe

REM initiate the compiler enviroment
@echo on
REM IF "%OS_MODE%"=="" CALL "%INTEL_ENV%" ia32
REM IF "%OS_MODE%"==" WIN64" CALL "%INTEL_ENV%" intel64

REM SET INTEL_ICL_CMAKE=%INTEL_ICL:\=/%
SET INTEL_TBB_CMAKE=%INTEL_TBB:\=/%

SET CMAKE_CONF_FLAGS=^
-DWITH_TBB:BOOL=TRUE ^
-DTBB_INCLUDE_DIR="%INTEL_TBB_CMAKE%" ^
-DWITH_IPP:BOOL=TRUE ^
-DCV_ICC:BOOL=TRUE ^
%CMAKE_CONF_FLAGS%

REM create visual studio project
%CMAKE% %CMAKE_CONF_FLAGS%

REM convert the project to use intel compiler 
"%ICPROJCONVERT%" emgucv.sln /IC
REM exclude tesseract_wordrec, tesseract_ccstruct, tesseract_ccmain and libjpeg
REM these projects create problems for intel compiler
"%ICPROJCONVERT%" emgucv.sln ^
Emgu.CV.Extern\tesseract\libtesseract\tesseract-ocr\wordrec\tesseract_wordrec.icproj ^
Emgu.CV.Extern\tesseract\libtesseract\tesseract-ocr\ccstruct\tesseract_ccstruct.icproj ^
Emgu.CV.Extern\tesseract\libtesseract\tesseract-ocr\ccmain\tesseract_ccmain.icproj ^
opencv\3rdparty\libjpeg\libjpeg.icproj ^
/VC

GOTO BUILD

:VISUAL_STUDIO
@echo on
%CMAKE% %CMAKE_CONF_FLAGS% -DWITH_IPP:BOOL=FALSE.  

:BUILD

SET BUILD_PROJECT=
IF "%5%"=="package" SET BUILD_PROJECT= /project PACKAGE 

%DEVENV% %BUILD_TYPE% emgucv.sln %BUILD_PROJECT% /out "%CD%\build.log"