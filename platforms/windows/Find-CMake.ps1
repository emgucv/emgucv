<#
.SYNOPSIS
    Locates cmake.exe on Windows. Dot-source this file, then call Find-CMakeExe.

.DESCRIPTION
    Shared by windows/Build_Binary.ps1 and android/scripts/wincfg.ps1 so both
    pick the same CMake. Candidates are checked from lowest to highest priority
    and the last one that exists wins:

        1. cmake.exe on PATH (returned as the bare name 'cmake.exe')
        2. a standalone install: Program Files (x86), Program Files, %ProgramW6432%
        3. the CMake bundled with Visual Studio 2022
        4. the CMake bundled with Visual Studio 2026 (skipped when -Toolchain is VS2022)

    The Visual Studio copy is preferred over a standalone install because
    Visual Studio keeps it updated.

.PARAMETER RepoRoot
    Repository root, used to find miscellaneous\vswhere.exe.

.PARAMETER Toolchain
    Optional toolchain pin. 'VS2022' ignores the Visual Studio 2026 CMake.
#>
function Find-CMakeExe {
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [string]$Toolchain = ''
    )

    $vswhere = Join-Path $RepoRoot 'miscellaneous\vswhere.exe'
    function Get-VsInstallPath([string]$VersionRange) {
        if (-not (Test-Path $vswhere)) { return '' }
        $result = & $vswhere -version $VersionRange -property installationPath 2>$null
        if ($LASTEXITCODE -ne 0) { return '' }
        return ($result | Select-Object -First 1)
    }
    $vsBundledCMake = 'Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe'

    $programFilesX86 = ${env:ProgramFiles(x86)}
    if (-not $programFilesX86 -or -not (Test-Path $programFilesX86)) { $programFilesX86 = $env:ProgramFiles }

    $cmakeExe = 'cmake.exe'
    foreach ($root in @($programFilesX86, $env:ProgramFiles, $env:ProgramW6432)) {
        if ($root -and (Test-Path (Join-Path $root 'CMake\bin\cmake.exe'))) {
            $cmakeExe = Join-Path $root 'CMake\bin\cmake.exe'
        }
    }

    $vs2022Dir = Get-VsInstallPath '[17.0,18.0)'
    if ($vs2022Dir -and (Test-Path (Join-Path $vs2022Dir $vsBundledCMake))) {
        $cmakeExe = Join-Path $vs2022Dir $vsBundledCMake
    }
    if ($Toolchain -ne 'VS2022') {
        $vs2026Dir = Get-VsInstallPath '[18.0,19.0)'
        if ($vs2026Dir -and (Test-Path (Join-Path $vs2026Dir $vsBundledCMake))) {
            $cmakeExe = Join-Path $vs2026Dir $vsBundledCMake
        }
    }
    return $cmakeExe
}
