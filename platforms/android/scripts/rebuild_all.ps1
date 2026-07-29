<#
.SYNOPSIS
    Cleans and rebuilds all four Android ABIs, then merges the per-ABI
    packages into a single libemgucv-android.zip.

.DESCRIPTION
    PowerShell port of rebuild_all.bat. This is legacy tooling that predates
    the current MAUI-based Emgu.CV.runtime.maui.android packaging flow (it
    targets the older jni/ant Java project layout and a hand-merged zip of
    all four ABIs' "sdk/native/libs" trees) -- kept as a faithful,
    structurally-equivalent conversion rather than rewritten against the
    current packaging, since its output format has not been re-verified
    against what "make package"/cpack actually produces today.

    Per-ABI build directories are android_<abi> (not build_<abi>, which is
    what rebuild_all.bat's own clean list used) to match the android_<abi>
    convention the rest of the (already-converted) scripts use -- see
    wincfg.ps1's header comment.

.PARAMETER NoClean
    Skip deleting the existing build/ and android_<abi>/ directories before
    rebuilding. Was checking %1%=="noclean" in the .bat.
#>
[CmdletBinding()]
param(
    [switch]$NoClean
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

$scriptsDir = $PSScriptRoot
$sourceDir = (Resolve-Path (Join-Path $scriptsDir '..\..\..')).Path

# --- Strip stale PATH entries that can shadow the intended toolchain -------
$removeFromPathScript = Join-Path $scriptsDir 'remove_from_path.ps1'
& $removeFromPathScript -VariableName PATH -Value 'C:\Program Files (x86)\Git\bin;'
& $removeFromPathScript -VariableName PATH -Value 'C:\Anaconda2;'
& $removeFromPathScript -VariableName PATH -Value 'C:\Anaconda2\MinGW\bin;'
& $removeFromPathScript -VariableName PATH -Value 'C:\Anaconda2\Scripts;'
& $removeFromPathScript -VariableName PATH -Value 'C:\Anaconda2\Library\bin;'

Push-Location $sourceDir
try {
    Copy-Item -Path (Join-Path $sourceDir 'opencv\platforms\android\android.toolchain.cmake') -Destination (Join-Path $sourceDir 'android.toolchain.cmake') -Force

    $abis = @('armeabi-v7a', 'arm64-v8a', 'x86', 'x86_64')

    if (-not $NoClean) {
        Remove-Item -Recurse -Force (Join-Path $sourceDir 'build') -ErrorAction SilentlyContinue
        foreach ($abi in $abis) {
            Remove-Item -Recurse -Force (Join-Path $sourceDir "android_$abi") -ErrorAction SilentlyContinue
        }
    }

    foreach ($abi in $abis) {
        Invoke-Native -FilePath (Join-Path $scriptsDir 'build.ps1') -ArgumentList @($abi)
    }

    # --- Merge the four per-ABI packages into build/libemgucv-android -------
    $mergedDir = Join-Path $sourceDir 'build'
    New-Item -ItemType Directory -Force -Path $mergedDir | Out-Null

    foreach ($abi in $abis) {
        $zipPath = Join-Path $sourceDir "android_$abi\libemgucv-android-$abi.zip"
        Expand-Archive -Path $zipPath -DestinationPath $mergedDir -Force
    }

    Push-Location $mergedDir
    try {
        $libemgucvAndroidDir = Join-Path $mergedDir 'libemgucv-android'
        New-Item -ItemType Directory -Force -Path $libemgucvAndroidDir | Out-Null

        foreach ($abi in $abis) {
            $extractedDir = Join-Path $mergedDir "libemgucv-android-$abi"
            Copy-Item -Path (Join-Path $extractedDir '*') -Destination $libemgucvAndroidDir -Recurse -Force
        }

        Push-Location $libemgucvAndroidDir
        try {
            Move-Item -Path 'bin' -Destination 'libs' -Force
            Copy-Item -Path 'sdk\native\libs\*' -Destination 'libs' -Recurse -Force

            Remove-Item -Recurse -Force 'sdk\native' -ErrorAction SilentlyContinue
            Remove-Item -Force 'libs\android\armeabi\libopencv_androidcamera.a' -ErrorAction SilentlyContinue
            Remove-Item -Force 'libs\android\armeabi-v7a\libopencv_androidcamera.a' -ErrorAction SilentlyContinue
            Remove-Item -Force 'libs\android\x86\libopencv_androidcamera.a' -ErrorAction SilentlyContinue
        }
        finally {
            Pop-Location
        }

        Remove-Item -Force 'libemgucv-android.zip' -ErrorAction SilentlyContinue
        Compress-Archive -Path $libemgucvAndroidDir -DestinationPath 'libemgucv-android.zip' -Force
    }
    finally {
        Pop-Location
    }
}
finally {
    Pop-Location
}
