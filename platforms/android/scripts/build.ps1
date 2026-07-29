<#
.SYNOPSIS
    Configures and builds the Emgu CV native C++ layer for a single Android
    ABI on Windows.

.DESCRIPTION
    PowerShell port of build.cmd. Brought to parity with the more actively
    maintained build.sh (macOS/Linux), which build.cmd had drifted from:

      - Adds -DWITH_KLEIDICV=OFF. Without it, arm64 builds can crash with
        SIGILL on real devices/emulators: KleidiCV (ARM's optimized HAL, on
        by default for arm64 in OpenCV 5.0) compiles objects with
        -march=armv8-a+sve2, and under the whole-program (LTO) build that
        SVE2 codegen leaks into libwebp's WebPGetColorPalette. SVE is absent
        on the emulator and most real devices.
      - Adds -DCMAKE_POLICY_VERSION_MINIMUM=3.5 to the freetype2/harfbuzz
        sub-builds, needed for modern CMake to configure those submodules'
        old cmake_minimum_required.
      - Adds -DCMAKE_CXX_FLAGS=-Wno-cast-function-type-strict for harfbuzz.
      - Uses BUILD_DIR=android_<abi> (via the also-updated wincfg.ps1),
        matching build.sh and CLAUDE.md, instead of wincfg.cmd's stray
        build_<abi>.

    The dead "BUILD_OPENCV=0" / other-cmake code path in build.cmd is
    dropped: BUILD_OPENCV was hardcoded to 1 and never read anywhere else,
    so that branch was unreachable. Likewise the commented-out
    OpenVINO/VTK experiments are dropped rather than carried over as dead
    comments.

.PARAMETER Abi
    Target ABI: "x86", "x86_64", "arm64-v8a", or "armeabi-v7a". Was %1 in
    the .cmd.

.PARAMETER Variant
    "core" -- no Tesseract, no Freetype, no contrib modules.
    "mini" -- same as core, plus strips calib/dnn/photo/features/video.
    Anything else (including omitted) -- full build. Was %2 in the .cmd.

.PARAMETER AndroidToolchain
    Optional value for -DANDROID_TOOLCHAIN_NAME. Was %3 in the .cmd.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateSet('x86', 'x86_64', 'arm64-v8a', 'armeabi-v7a')]
    [string]$Abi,

    [Parameter(Position = 1)]
    [string]$Variant = '',

    [Parameter(Position = 2)]
    [string]$AndroidToolchain = ''
)

$ErrorActionPreference = 'Stop'

function Invoke-Native {
    param(
        [Parameter(Mandatory = $true)][string]$FilePath,
        [Parameter(ValueFromRemainingArguments = $true)][string[]]$ArgumentList
    )
    Write-Host "> $FilePath $($ArgumentList -join ' ')"
    & $FilePath @ArgumentList | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "Command failed with exit code $($LASTEXITCODE): $FilePath $($ArgumentList -join ' ')"
    }
}

function ConvertTo-ForwardSlash {
    param([Parameter(Mandatory = $true)][string]$Path)
    return $Path -replace '\\', '/'
}

$scriptsDir = $PSScriptRoot
$sourceDir = (Resolve-Path (Join-Path $scriptsDir '..\..\..')).Path
Write-Host "SOURCE_DIR=$sourceDir"

$androidNativeApiLevel = 24

# --- Load configuration (NDK/SDK/CMake/Make/Ant/JDK/BUILD_DIR) -------------
$wincfgScript = Join-Path $scriptsDir 'wincfg.ps1'
if (Test-Path $wincfgScript) {
    . $wincfgScript -Abi $Abi
}

$buildJavaPart = Test-Path (Join-Path $sourceDir 'jni')

# --- Optional android-toolchain --------------------------------------------
$androidToolchainCmakeOption = @()
if ($AndroidToolchain) { $androidToolchainCmakeOption = @("-DANDROID_TOOLCHAIN_NAME=$AndroidToolchain") }

# --- Defaults ----------------------------------------------------------------
$buildDir = if ($env:BUILD_DIR) { $env:BUILD_DIR } else { "android_$Abi" }
$androidAbi = if ($env:ANDROID_ABI) { $env:ANDROID_ABI } else { $Abi }
$installFolder = Join-Path $sourceDir "$buildDir\install"

# --- Check required variables -----------------------------------------------
if (-not $env:ANDROID_NDK) { throw 'You should set an environment variable ANDROID_NDK to the full path to your copy of Android NDK' }
if (-not (Test-Path $env:ANDROID_NDK)) { throw "Directory `"$env:ANDROID_NDK`" specified by ANDROID_NDK variable does not exist" }

if (-not ($env:CMAKE -and (Test-Path $env:CMAKE))) { throw "You should set an environment variable CMAKE to the full path to cmake executable. CMAKE=`"$env:CMAKE`"" }
if (-not ($env:MAKE -and (Test-Path $env:MAKE))) { throw "You should set an environment variable MAKE to the full path to native port of make executable. MAKE=`"$env:MAKE`"" }

if ($buildJavaPart) {
    if (-not $env:ANDROID_SDK) { throw 'You should set an environment variable ANDROID_SDK to the full path to your copy of Android SDK' }
    if (-not (Test-Path $env:ANDROID_SDK)) { throw "Directory `"$env:ANDROID_SDK`" specified by ANDROID_SDK variable does not exist" }

    if (-not $env:ANT_DIR) { throw 'You should set an environment variable ANT_DIR to the full path to Apache Ant root' }
    if (-not (Test-Path $env:ANT_DIR)) { throw "Directory `"$env:ANT_DIR`" specified by ANT_DIR variable does not exist" }

    if (-not $env:JAVA_HOME) { throw 'You should set an environment variable JAVA_HOME to the full path to JDK' }
    if (-not (Test-Path $env:JAVA_HOME)) { throw "Directory `"$env:JAVA_HOME`" specified by JAVA_HOME variable does not exist" }
}

# --- Ninja vs. MinGW Makefiles ----------------------------------------------
$buildWithNinja = $env:MAKE -match 'ninja'
$cmakeGenerator = if ($buildWithNinja) { 'Ninja' } else { 'MinGW Makefiles' }

$cmakeToolchainFile = Join-Path $env:ANDROID_NDK 'build\cmake\android.toolchain.cmake'
if (-not (Test-Path $cmakeToolchainFile)) { $cmakeToolchainFile = '' }

# --- Variant configuration ---------------------------------------------------
$tesseractOption = '-DEMGU_CV_WITH_TESSERACT:BOOL=ON'
$extraFlags = @()
$buildContrib = $true

switch ($Variant) {
    'core' {
        $tesseractOption = '-DEMGU_CV_WITH_TESSERACT:BOOL=OFF'
        $buildContrib = $false
    }
    'mini' {
        $tesseractOption = '-DEMGU_CV_WITH_TESSERACT:BOOL=OFF'
        $buildContrib = $false
        # flann must stay enabled: in OpenCV 5 imgproc depends on geometry, which depends on flann.
        $extraFlags = @(
            '-DBUILD_opencv_calib:BOOL=FALSE',
            '-DBUILD_opencv_dnn:BOOL=FALSE',
            '-DBUILD_opencv_photo:BOOL=FALSE',
            '-DBUILD_opencv_features:BOOL=FALSE',
            '-DBUILD_opencv_video:BOOL=FALSE'
        )
    }
}

# --- Base CMake flags shared by all sub-builds (eigen, freetype, harfbuzz, main) ---
$baseCmakeFlags = @(
    '-G', $cmakeGenerator,
    "-DANDROID_ABI=$androidAbi",
    "-DANDROID_PLATFORM=$androidNativeApiLevel",
    "-DCMAKE_TOOLCHAIN_FILE=$cmakeToolchainFile"
) + $androidToolchainCmakeOption + @(
    "-DCMAKE_MAKE_PROGRAM=$($env:MAKE)",
    '-DCMAKE_C_FLAGS:STRING=-std=c11',
    '-DCMAKE_CXX_FLAGS_RELEASE:STRING=-g0 -O3',
    '-DCMAKE_C_FLAGS_RELEASE:STRING=-g0 -O3',
    '-DWITH_KLEIDICV=OFF',
    '-DCMAKE_SHARED_LINKER_FLAGS:STRING=-Wl,--gc-sections, -Wl,--exclude-libs,All',
    '-DCMAKE_POLICY_DEFAULT_CMP0069=NEW',
    '-DCMAKE_INTERPROCEDURAL_OPTIMIZATION:BOOL=ON',
    "-DCMAKE_INSTALL_PREFIX:STRING=$(ConvertTo-ForwardSlash $installFolder)",
    '-DCMAKE_BUILD_TYPE:STRING=Release'
)

# --- Build Eigen (unconditionally) ------------------------------------------
Write-Host ''
Write-Host '=== Building Eigen ==='
Push-Location (Join-Path $sourceDir 'eigen')
try {
    New-Item -ItemType Directory -Force -Path $buildDir | Out-Null
    Push-Location $buildDir
    try {
        Invoke-Native -FilePath $env:CMAKE -ArgumentList ($baseCmakeFlags + @('..'))
        $eigenDir = (Get-Location).Path
        Invoke-Native -FilePath $env:CMAKE -ArgumentList @('--build', '.', '--config', 'Release', '--target', 'install')
    }
    finally {
        Pop-Location
    }
}
finally {
    Pop-Location
}

# --- Build freetype2 + harfbuzz (full build only) ---------------------------
$contribCmakeFlags = @()
if ($buildContrib) {
    Write-Host ''
    Write-Host '=== Building freetype2 ==='
    Push-Location (Join-Path $sourceDir '3rdParty\freetype2')
    try {
        New-Item -ItemType Directory -Force -Path $buildDir | Out-Null
        Push-Location $buildDir
        try {
            Invoke-Native -FilePath $env:CMAKE -ArgumentList ($baseCmakeFlags + @('-DCMAKE_POLICY_VERSION_MINIMUM=3.5', '..'))
            Invoke-Native -FilePath $env:CMAKE -ArgumentList @('--build', '.', '--config', 'Release', '--parallel', '--target', 'install')
        }
        finally {
            Pop-Location
        }
    }
    finally {
        Pop-Location
    }

    Write-Host ''
    Write-Host '=== Building harfbuzz ==='
    Push-Location (Join-Path $sourceDir 'harfbuzz')
    try {
        New-Item -ItemType Directory -Force -Path $buildDir | Out-Null
        Push-Location $buildDir
        try {
            Invoke-Native -FilePath $env:CMAKE -ArgumentList ($baseCmakeFlags + @(
                    '-DCMAKE_POLICY_VERSION_MINIMUM=3.5',
                    "-DCMAKE_FIND_ROOT_PATH:STRING=$(ConvertTo-ForwardSlash $installFolder)",
                    '-DHB_HAVE_FREETYPE:BOOL=TRUE',
                    '-DCMAKE_CXX_FLAGS=-Wno-cast-function-type-strict',
                    '..'
                ))
            Invoke-Native -FilePath $env:CMAKE -ArgumentList @('--build', '.', '--config', 'Release', '--parallel', '--target', 'install')
        }
        finally {
            Pop-Location
        }
    }
    finally {
        Pop-Location
    }

    $openCvExtraModulesDir = Join-Path $sourceDir 'opencv_contrib\modules'
    $contribCmakeFlags = @("-DOPENCV_EXTRA_MODULES_PATH:String=$(ConvertTo-ForwardSlash $openCvExtraModulesDir)")
}
else {
    $contribCmakeFlags = @('-DEMGU_CV_WITH_FREETYPE:BOOL=OFF')
}

# --- Create/enter the build dir ---------------------------------------------
Write-Host ''
Write-Host "=== Building Emgu CV (ABI=$androidAbi, variant=$(if ($Variant) { $Variant } else { 'full' })) ==="
if ($env:REBUILD) { Remove-Item -Recurse -Force (Join-Path $sourceDir $buildDir) -ErrorAction SilentlyContinue }
New-Item -ItemType Directory -Force -Path (Join-Path $sourceDir $buildDir) | Out-Null
Push-Location (Join-Path $sourceDir $buildDir)
try {
    # --- Run cmake -----------------------------------------------------------
    Write-Host ''
    Write-Host 'Running cmake...'
    Write-Host "ANDROID_ABI=$androidAbi"
    Write-Host ''

    $fullCmakeFlags = $baseCmakeFlags + @($tesseractOption) + $extraFlags + $contribCmakeFlags + @(
        '-DBUILD_SHARED_LIBS:BOOL=OFF',
        '-DBUILD_ANDROID_EXAMPLES:BOOL=OFF',
        '-DBUILD_PERF_TESTS:BOOL=OFF',
        '-DPARALLEL_ENABLE_PLUGINS:BOOL=OFF',
        '-DVIDEOIO_ENABLE_PLUGINS:BOOL=OFF',
        '-DHIGHGUI_ENABLE_PLUGINS:BOOL=OFF',
        '-DWITH_IPP:BOOL=OFF',
        '-DBUILD_DOCS:BOOL=OFF',
        '-DBUILD_TESTS:BOOL=OFF',
        '-DBUILD_WITH_DEBUG_INFO:BOOL=OFF',
        '-DBUILD_opencv_java:BOOL=OFF',
        '-DBUILD_opencv_java_bindings_generator:BOOL=OFF',
        '-DBUILD_opencv_ts:BOOL=OFF',
        '-DWITH_ITT:BOOL=OFF',
        '-DWITH_OPENCL:BOOL=ON',
        '-DWITH_CUDA:BOOL=OFF',
        '-DBUILD_ANDROID_PROJECTS=OFF',
        '-DWITH_EIGEN:BOOL=ON',
        '-DBUILD_FAT_JAVA_LIB:BOOL=FALSE',
        '-DBUILD_JAVA:BOOL=FALSE',
        '-DEMGU_CV_WITH_DEPTHAI:BOOL=FALSE',
        "-DCMAKE_FIND_ROOT_PATH:STRING=$(ConvertTo-ForwardSlash $installFolder)",
        "-DEigen3_DIR:STRING=$(ConvertTo-ForwardSlash $eigenDir)"
    )

    Invoke-Native -FilePath $env:CMAKE -ArgumentList ($fullCmakeFlags + @($sourceDir))

    # --- Build -----------------------------------------------------------------
    Write-Host ''
    Write-Host 'Building native libs...'
    if ($buildWithNinja) {
        Invoke-Native -FilePath $env:MAKE -ArgumentList @('-j', $env:NUMBER_OF_PROCESSORS)
    }
    else {
        Invoke-Native -FilePath $env:MAKE -ArgumentList @('-j', $env:NUMBER_OF_PROCESSORS, 'VERBOSE=1', 'package')
    }
}
finally {
    Pop-Location
}

if ($buildJavaPart) {
    Push-Location $sourceDir
    try {
        Write-Host ''
        Write-Host 'Updating Android project...'
        Invoke-Native -FilePath (Join-Path $env:ANDROID_SDK 'tools\android') -ArgumentList @('update', 'project', '--name', $env:PROJECT_NAME, '--path', '.')

        Write-Host ''
        Write-Host 'Compiling Android project...'
        Invoke-Native -FilePath (Join-Path $env:ANT_DIR 'bin\ant') -ArgumentList @('debug')
    }
    finally {
        Pop-Location
    }
}

Write-Host ''
Write-Host "=== Build complete: $buildDir ==="
