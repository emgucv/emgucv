<#
.SYNOPSIS
    Detects the Android NDK/SDK, CMake, Make/Ninja, Ant, and JDK locations on
    Windows and populates the environment variables build.ps1 expects.

.DESCRIPTION
    PowerShell port of wincfg.cmd. Dot-source this from build.ps1 (or a
    PowerShell prompt) so the environment variables it sets persist in the
    caller:

        . .\wincfg.ps1 -Abi x86_64

    Sets: $env:ANDROID_SDK, $env:ANDROID_NDK, $env:MAKE, $env:CMAKE,
    $env:ANT_DIR, $env:JAVA_HOME, $env:BUILD_DIR.

    Note: unlike wincfg.cmd (whose BUILD_DIR=build_%1 line was silently
    overridden by a SETLOCAL scoping quirk in some invocations, and in any
    case doesn't match the android_<abi> layout the rest of the tooling
    uses), this sets BUILD_DIR=android_<abi> to match build.sh and
    CLAUDE.md's documented android_<abi>/ layout.

.PARAMETER Abi
    Target ABI, e.g. "x86_64". Only used to compute BUILD_DIR.
#>
param(
    [Parameter(Mandatory = $true)]
    [string]$Abi
)

$AndroidNdkParentDir = 'C:\android'

# ---------------------------------------------------------------------------
# Android SDK
# ---------------------------------------------------------------------------
if (Test-Path 'C:\Program Files (x86)\Android\android-sdk') { $env:ANDROID_SDK = 'C:\Program Files (x86)\Android\android-sdk' }
if (Test-Path (Join-Path $AndroidNdkParentDir 'android-sdk')) { $env:ANDROID_SDK = Join-Path $AndroidNdkParentDir 'android-sdk' }

# ---------------------------------------------------------------------------
# Android NDK -- last matching candidate wins, oldest to newest, mirroring
# wincfg.cmd's cascade exactly.
# ---------------------------------------------------------------------------
$ndkCandidates = @(
    (Join-Path $AndroidNdkParentDir 'android-ndk-r10e'),
    "$env:PROGRAMDATA\Microsoft\AndroidNDK64\android-ndk-r10e",
    (Join-Path $AndroidNdkParentDir 'android-ndk-r11'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r11c'),
    "$env:PROGRAMDATA\Microsoft\AndroidNDK64\android-ndk-r11c",
    (Join-Path $AndroidNdkParentDir 'android-ndk-r12b'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r13'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r13b'),
    'C:\Microsoft\AndroidNDK64\android-ndk-r13b',
    "$env:PROGRAMDATA\Microsoft\AndroidNDK64\android-ndk-r13b",
    (Join-Path $AndroidNdkParentDir 'android-ndk-r14'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r14b'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r15'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r15b'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r15c'),
    'C:\Microsoft\AndroidNDK64\android-ndk-r15c',
    "$env:PROGRAMDATA\Microsoft\AndroidNDK64\android-ndk-r15c",
    (Join-Path $AndroidNdkParentDir 'android-ndk-r16'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r16b'),
    'C:\Microsoft\AndroidNDK64\android-ndk-r16b',
    (Join-Path $AndroidNdkParentDir 'android-ndk-r17c'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r18b'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r19c'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r20'),
    (Join-Path $AndroidNdkParentDir 'android-ndk-r21')
)
foreach ($candidate in $ndkCandidates) {
    if (Test-Path $candidate) { $env:ANDROID_NDK = $candidate }
}

if ($env:ProgramFiles -and (Test-Path "${env:ProgramFiles(x86)}\Android\android-sdk\ndk-bundle")) { $env:ANDROID_NDK = "${env:ProgramFiles(x86)}\Android\android-sdk\ndk-bundle" }
if ($env:ANDROID_SDK -and (Test-Path (Join-Path $env:ANDROID_SDK 'ndk-bundle'))) { $env:ANDROID_NDK = Join-Path $env:ANDROID_SDK 'ndk-bundle' }
if ($env:ANDROID_SDK -and (Test-Path (Join-Path $env:ANDROID_SDK 'ndk\28.0.12916984'))) { $env:ANDROID_NDK = Join-Path $env:ANDROID_SDK 'ndk\28.0.12916984' }

# ---------------------------------------------------------------------------
# Make (prefer the NDK's bundled make.exe)
# ---------------------------------------------------------------------------
if ($env:ANDROID_NDK -and (Test-Path $env:ANDROID_NDK)) {
    $env:MAKE = Join-Path $env:ANDROID_NDK 'prebuilt\windows-x86_64\bin\make.exe'
}

# ---------------------------------------------------------------------------
# CMake -- last matching candidate wins, same order as wincfg.cmd.
# ---------------------------------------------------------------------------
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..\..\..')
$vswhere = Join-Path $repoRoot 'miscellaneous\vswhere.exe'
$vs2022Dir = ''
if (Test-Path $vswhere) {
    $vs2022Result = & $vswhere -version '[17.0,18.0)' -property installationPath 2>$null
    if ($vs2022Result) { $vs2022Dir = ($vs2022Result | Select-Object -First 1) }
}

if (Test-Path "${env:ProgramFiles(x86)}\CMake 2.8\bin\cmake.exe") { $env:CMAKE = "${env:ProgramFiles(x86)}\CMake 2.8\bin\cmake.exe" }
if (Test-Path "${env:ProgramFiles(x86)}\CMake\bin\cmake.exe") { $env:CMAKE = "${env:ProgramFiles(x86)}\CMake\bin\cmake.exe" }
if (Test-Path "$env:ProgramFiles\CMake\bin\cmake.exe") { $env:CMAKE = "$env:ProgramFiles\CMake\bin\cmake.exe" }
if ($vs2022Dir -and (Test-Path (Join-Path $vs2022Dir 'Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe'))) {
    $env:CMAKE = Join-Path $vs2022Dir 'Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe'
}
if ($env:ProgramW6432 -and (Test-Path "$env:ProgramW6432\CMake\bin\cmake.exe")) { $env:CMAKE = "$env:ProgramW6432\CMake\bin\cmake.exe" }

# ---------------------------------------------------------------------------
# Ant (legacy, only used by the old jni/ant Java project flow)
# ---------------------------------------------------------------------------
$env:ANT_DIR = "$env:VS140COMNTOOLS..\..\Apps\apache-ant-1.9.3"

# ---------------------------------------------------------------------------
# JDK -- registry lookups (last found wins), then explicit path overrides.
# ---------------------------------------------------------------------------
function Get-RegistryJavaHome {
    param([Parameter(Mandatory = $true)][string]$KeyPath)
    try {
        $prop = Get-ItemProperty -Path "HKLM:\SOFTWARE\$KeyPath" -Name JavaHome -ErrorAction Stop
        return $prop.JavaHome
    }
    catch {
        return $null
    }
}

$javaHome17 = Get-RegistryJavaHome 'JavaSoft\Java Development Kit\1.7'
$javaHome18 = Get-RegistryJavaHome 'JavaSoft\Java Development Kit\1.8'
$javaHome25 = Get-RegistryJavaHome 'JavaSoft\JDK\25'

if ($javaHome17) { $env:JAVA_HOME = $javaHome17 }
if ($javaHome18) { $env:JAVA_HOME = $javaHome18 }
if ($javaHome25) { $env:JAVA_HOME = $javaHome25 }
if (Test-Path 'C:\Program Files\Android\jdk\microsoft_dist_openjdk_1.8.0.25') { $env:JAVA_HOME = 'C:\Program Files\Android\jdk\microsoft_dist_openjdk_1.8.0.25' }
if (Test-Path 'C:\Program Files (x86)\Android\openjdk\jdk-17.0.14') { $env:JAVA_HOME = 'C:\Program Files (x86)\Android\openjdk\jdk-17.0.14' }

Write-Host "Java Home: $env:JAVA_HOME"

# ---------------------------------------------------------------------------
# Build directory -- android_<abi>, matching build.sh and CLAUDE.md (NOT
# build_<abi>, which is what wincfg.cmd's own SET BUILD_DIR=build_%1 said).
# ---------------------------------------------------------------------------
$env:BUILD_DIR = "android_$Abi"

Write-Host "CMAKE: $env:CMAKE"
Write-Host "MAKE: $env:MAKE"
Write-Host "ANDROID_SDK: $env:ANDROID_SDK"
Write-Host "ANDROID_NDK: $env:ANDROID_NDK"
