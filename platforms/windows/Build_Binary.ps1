<#
.SYNOPSIS
    Configures (and optionally builds) the Emgu CV native C++ layer on Windows.

.DESCRIPTION
    PowerShell port of Build_Binary_x86.bat. Build_Binary_x86.bat is left
    completely untouched -- this script is being validated in parallel
    before any wrapper scripts, CI, or Docker builds are repointed to it.
    See the migration plan for the full inventory of what still calls the
    .bat and is deliberately not touched by this file.

    Targets Windows PowerShell 5.1 syntax (no ternary/null-coalescing/etc.)
    so it runs without any extra install on a stock Windows machine.

    Unlike the .bat, every external command's exit code is checked and a
    failure throws immediately instead of silently continuing into a broken
    partial build.

.PARAMETER Arch
    Target architecture. Was positional %1 in the .bat.

.PARAMETER ComponentSet
    Which OpenCV/Emgu CV component set to build. Was folded into %2 in the
    .bat (which used the same slot for this AND the -Cuda flag below --
    split into two parameters here since nothing in the current wrapper
    scripts ever combines Core/Mini with CUDA anyway).

.PARAMETER Cuda
    Build with CUDA support. Was the other meaning of %2 ("gpu") in the .bat.

.PARAMETER CudaArchBin
    Manually specify CUDA_ARCH_BIN_OPTION, e.g. "8.6". Was %9 in the .bat.

.PARAMETER Toolchain
    Compiler / VS-version / UWP selection. Was %3 in the .bat.

.PARAMETER ExtraModules
    Optional extra module to enable. Was %4 in the .bat.

.PARAMETER Documentation
    Build the documentation target. Was %5=="doc" in the .bat.

.PARAMETER Package
    Build the .zip/.exe package target. Was %6=="package" in the .bat.

.PARAMETER Build
    Actually build (not just configure) after running CMake. Was
    %7=="build" in the .bat.

.PARAMETER Nuget
    Build the NuGet package target. Was %8=="nuget" in the .bat.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('x86_64', 'x86', 'arm', 'arm64')]
    [string]$Arch,

    [ValidateSet('Full', 'Core', 'Mini')]
    [string]$ComponentSet = 'Full',

    [switch]$Cuda,

    [string]$CudaArchBin = '',

    [ValidateSet('None', 'Intel', 'IntelOpenVino', 'OpenVino', 'WindowsStore10',
        'WindowsPhone81', 'WindowsStore81', 'VS2015', 'VS2022', 'Commercial')]
    [string]$Toolchain = 'None',

    [ValidateSet('None', 'NonFree', 'OpenNI', 'DepthAI')]
    [string]$ExtraModules = 'None',

    [switch]$Documentation,
    [switch]$Package,
    [switch]$Build,
    [switch]$Nuget
)

$ErrorActionPreference = 'Stop'

# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------

function Invoke-Native {
    <#
    Runs an external command and throws if it exits non-zero. The .bat this
    replaces never checked exit codes and would silently continue on
    failure; this is the one deliberate, happy-path-preserving behavior
    change (see script synopsis).

    Piped through Out-Host rather than left as bare `& $FilePath @Args`:
    PowerShell treats an external command's stdout as pipeline output, so
    an unredirected call inside a function would otherwise get silently
    merged into that function's own return value (a real bug this script
    hit: Build-Vtk's `return $buildDir` came back as cmake's entire stdout
    plus $buildDir, since its two Invoke-Native calls were unredirected).
    Out-Host still streams every line live; it just keeps it out of the
    success-object pipeline.
    #>
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

# ---------------------------------------------------------------------------
# Visual Studio / CMake toolchain detection
#
# Ports the vswhere-based cascade from Build_Binary_x86.bat, including this
# session's fix: derive tool paths from the already-resolved DevEnvPath via
# ordinary string replacement, not by re-splicing a raw, unquoted
# installationPath (the source of the "Program Files (x86)" parsing bugs
# that motivated the .bat fix in the first place). PowerShell's -replace
# doesn't have the .bat's %-expansion-before-parsing hazard, so this is
# purely for behavioral fidelity, not because the hazard exists here too.
# ---------------------------------------------------------------------------

function Resolve-VisualStudioEnvironment {
    param(
        [Parameter(Mandatory = $true)][string]$Arch,
        [Parameter(Mandatory = $true)][string]$Toolchain,
        [Parameter(Mandatory = $true)][string]$ExtraModules
    )

    $buildToolsFolder = 'C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools'
    $programFilesX86 = ${env:ProgramFiles(x86)}
    if (-not (Test-Path $programFilesX86)) { $programFilesX86 = $env:ProgramFiles }
    $programFiles = $env:ProgramFiles

    $repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..\..')
    $vswhere = Join-Path $repoRoot 'miscellaneous\vswhere.exe'

    function Get-VsWhereInstallPath([string]$VersionRange) {
        $result = & $vswhere -version $VersionRange -property installationPath 2>$null
        if ($LASTEXITCODE -ne 0) { return '' }
        return ($result | Select-Object -First 1)
    }
    $vs2017Dir = Get-VsWhereInstallPath '[15.0,16.0)'
    $vs2019Dir = Get-VsWhereInstallPath '[16.0,17.0)'
    $vs2022Dir = Get-VsWhereInstallPath '[17.0,18.0)'
    $vs2026Dir = Get-VsWhereInstallPath '[18.0,19.0)'
    $vsBuildToolsResult = & $vswhere -products * -property installationPath 2>$null
    $vsBuildToolsDir = if ($vsBuildToolsResult) { ($vsBuildToolsResult | Select-Object -Last 1) } else { '' }

    function DevEnvPath([string]$VsDir) {
        if ([string]::IsNullOrEmpty($VsDir)) { return '' }
        return (Join-Path $VsDir 'Common7\IDE\devenv.com')
    }
    $vs2017 = DevEnvPath $vs2017Dir
    $vs2019 = DevEnvPath $vs2019Dir
    $vs2022 = DevEnvPath $vs2022Dir
    $vs2026 = DevEnvPath $vs2026Dir

    # Legacy VS2005-2015 (COMNTOOLS environment variables). Dead on any
    # modern machine, but ported for fidelity with the .bat.
    function LegacyDevEnvPath([string]$EnvVarName) {
        $toolsDir = [Environment]::GetEnvironmentVariable($EnvVarName)
        if ([string]::IsNullOrEmpty($toolsDir)) { return '' }
        return (Join-Path $toolsDir '..\IDE\devenv.com')
    }
    $vs2005 = LegacyDevEnvPath 'VS80COMNTOOLS'
    $vs2008 = LegacyDevEnvPath 'VS90COMNTOOLS'
    $vs2010 = LegacyDevEnvPath 'VS100COMNTOOLS'
    $vs2012 = LegacyDevEnvPath 'VS110COMNTOOLS'
    $vs2013 = LegacyDevEnvPath 'VS120COMNTOOLS'
    $vs2015 = LegacyDevEnvPath 'VS140COMNTOOLS'

    $msbuild35 = ''
    if (Test-Path "$env:windir\Microsoft.NET\Framework\v3.5\MSBuild.exe") { $msbuild35 = "$env:windir\Microsoft.NET\Framework\v3.5\MSBuild.exe" }
    if (Test-Path "$env:windir\Microsoft.NET\Framework64\v3.5\MSBuild.exe") { $msbuild35 = "$env:windir\Microsoft.NET\Framework64\v3.5\MSBuild.exe" }
    $msbuild40 = ''
    if (Test-Path "$env:windir\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe") { $msbuild40 = "$env:windir\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" }
    $msbuildBuildTools = ''
    if (Test-Path (Join-Path $buildToolsFolder 'MSBuild\Current\Bin\MSBuild.exe')) { $msbuildBuildTools = Join-Path $buildToolsFolder 'MSBuild\Current\Bin\MSBuild.exe' }

    # DEVENV resolution cascade -- mirrors the .bat's fallthrough order
    # exactly: later checks overwrite earlier ones unless a short-circuit
    # (openni / WindowsPhone81 / vs2015 / vs2022) stops the cascade early.
    $devEnvKind = ''
    $devEnvPath = ''
    if ($msbuild35) { $devEnvKind = 'MSBuild35'; $devEnvPath = $msbuild35 }
    if ($msbuild40) { $devEnvKind = 'MSBuild40'; $devEnvPath = $msbuild40 }
    if ($msbuildBuildTools) { $devEnvKind = 'MSBuildBuildTools'; $devEnvPath = $msbuildBuildTools }
    if ($vs2005 -and (Test-Path $vs2005)) { $devEnvKind = 'VS2005'; $devEnvPath = $vs2005 }
    if ($vs2008 -and (Test-Path $vs2008)) { $devEnvKind = 'VS2008'; $devEnvPath = $vs2008 }
    if ($vs2010 -and (Test-Path $vs2010)) { $devEnvKind = 'VS2010'; $devEnvPath = $vs2010 }

    $skipToBuildType = $false
    if ($ExtraModules -eq 'OpenNI') { $skipToBuildType = $true }

    if (-not $skipToBuildType) {
        if ($vs2012 -and (Test-Path $vs2012)) { $devEnvKind = 'VS2012'; $devEnvPath = $vs2012 }
        if ($vs2013 -and (Test-Path $vs2013)) { $devEnvKind = 'VS2013'; $devEnvPath = $vs2013 }
        if ($vs2015 -and (Test-Path $vs2015)) { $devEnvKind = 'VS2015'; $devEnvPath = $vs2015 }

        $pinnedEarly = ($Toolchain -eq 'WindowsPhone81') -or ($Toolchain -eq 'VS2015')
        if (-not $pinnedEarly) {
            if ($vs2017Dir -and (Test-Path $vs2017)) { $devEnvKind = 'VS2017'; $devEnvPath = $vs2017 }
            if ($vs2019Dir -and (Test-Path $vs2019)) { $devEnvKind = 'VS2019'; $devEnvPath = $vs2019 }
            if ($vs2022Dir -and (Test-Path $vs2022)) { $devEnvKind = 'VS2022'; $devEnvPath = $vs2022 }
            if ($Toolchain -ne 'VS2022') {
                if ($vs2026Dir -and (Test-Path $vs2026)) { $devEnvKind = 'VS2026'; $devEnvPath = $vs2026 }
            }
        }
    }

    # BUILD_TYPE / CMAKE_CONF selection, keyed on which DEVENV kind won.
    $osMode = ''
    if ($Arch -eq 'x86_64') { $osMode = ' Win64' }
    if ($Arch -eq 'arm') { $osMode = ' ARM' }
    if ($Arch -eq 'arm64') { $osMode = ' ARM64' }

    $buildArch = ''
    if ($Arch -eq 'x86_64') { $buildArch = 'x64' }
    if ($Arch -eq 'x86') { $buildArch = 'Win32' }
    if ($Arch -eq 'arm') { $buildArch = 'ARM' }
    if ($Arch -eq 'arm64') { $buildArch = 'ARM64' }

    $buildConfigArg = '/Build Release'
    if ($devEnvKind -in @('MSBuild35', 'MSBuild40', 'MSBuildBuildTools')) { $buildConfigArg = '/property:Configuration=Release' }

    $cmakeGenerator = $null
    $cmakeArchArg = $null
    switch ($devEnvKind) {
        'MSBuild35' { $cmakeGenerator = "Visual Studio 12 2005$osMode" }
        'MSBuild40' { $cmakeGenerator = 'Visual Studio 16'; $cmakeArchArg = $buildArch }
        'MSBuildBuildTools' { $cmakeGenerator = 'Visual Studio 16'; $cmakeArchArg = $buildArch }
        'VS2005' { $cmakeGenerator = "Visual Studio 8 2005$osMode" }
        'VS2008' { $cmakeGenerator = "Visual Studio 9 2008$osMode" }
        'VS2010' { $cmakeGenerator = "Visual Studio 10$osMode" }
        'VS2012' { $cmakeGenerator = "Visual Studio 11$osMode" }
        'VS2013' { $cmakeGenerator = "Visual Studio 12$osMode" }
        'VS2015' { $cmakeGenerator = "Visual Studio 14$osMode" }
        'VS2017' { $cmakeGenerator = "Visual Studio 15$osMode" }
        'VS2019' { $cmakeGenerator = 'Visual Studio 16'; $cmakeArchArg = $buildArch }
        'VS2022' { $cmakeGenerator = 'Visual Studio 17'; $cmakeArchArg = $buildArch }
        'VS2026' { $cmakeGenerator = 'Visual Studio 18'; $cmakeArchArg = $buildArch }
        default { throw "Could not detect a usable Visual Studio / MSBuild installation." }
    }

    # CMake.exe detection (prefers VS2022's bundled CMake unless pinned to
    # vs2022/CUDA needs VS2026's bundled one; falls back to a system install).
    $cmakeExe = 'cmake.exe'
    if (Test-Path (Join-Path $programFilesX86 'CMake\bin\cmake.exe')) { $cmakeExe = Join-Path $programFilesX86 'CMake\bin\cmake.exe' }
    if (Test-Path (Join-Path $programFiles 'CMake\bin\cmake.exe')) { $cmakeExe = Join-Path $programFiles 'CMake\bin\cmake.exe' }
    if ($env:ProgramW6432 -and (Test-Path (Join-Path $env:ProgramW6432 'CMake\bin\cmake.exe'))) { $cmakeExe = Join-Path $env:ProgramW6432 'CMake\bin\cmake.exe' }
    if ($vs2022Dir -and (Test-Path (Join-Path $vs2022Dir 'Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe'))) {
        $cmakeExe = Join-Path $vs2022Dir 'Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe'
    }
    if ($Toolchain -ne 'VS2022') {
        if ($vs2026Dir -and (Test-Path (Join-Path $vs2026Dir 'Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe'))) {
            $cmakeExe = Join-Path $vs2026Dir 'Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe'
        }
    }

    return [PSCustomObject]@{
        DevEnvKind        = $devEnvKind
        DevEnvPath        = $devEnvPath
        BuildConfigArg    = $buildConfigArg
        CMakeGenerator    = $cmakeGenerator
        CMakeArchArg      = $cmakeArchArg
        CMakeExe          = $cmakeExe
        OsMode            = $osMode
        BuildArch         = $buildArch
        BuildToolsFolder  = $buildToolsFolder
        Vs2017Dir         = $vs2017Dir
        Vs2019Dir         = $vs2019Dir
        Vs2022Dir         = $vs2022Dir
        Vs2026Dir         = $vs2026Dir
        VsBuildToolsDir   = $vsBuildToolsDir
        MSBuildBuildTools = $msbuildBuildTools
    }
}

# ---------------------------------------------------------------------------
# sqlite3 fallback (ported from this session's Build_Binary_x86.bat fix)
#
# PROJ (built from source to satisfy Emgu.CV.Extern's unconditional GeoTIFF
# dependency) needs a sqlite3 executable plus the SQLite3 dev library/header,
# which aren't guaranteed to be installed. If missing, compile a standalone
# copy from the sqlite3.c amalgamation already vendored by the vtk submodule,
# using no-op stand-ins for VTK's symbol-mangling/export headers so it
# compiles with normal, unmangled sqlite3_* symbol names.
# ---------------------------------------------------------------------------

function Set-Sqlite3Fallback {
    param(
        [Parameter(Mandatory = $true)][string]$BuildFolder,
        [Parameter(Mandatory = $true)][string]$RootSrcFolder,
        [Parameter(Mandatory = $true)][string]$VcVarsScript
    )

    $existing = Get-Command sqlite3.exe -ErrorAction SilentlyContinue
    if ($existing) { return }

    $localDir = Join-Path $BuildFolder '_local_sqlite3'
    $vtkSqliteSrcDir = Join-Path $RootSrcFolder 'vtk\ThirdParty\sqlite\vtksqlite'

    if (Test-Path (Join-Path $localDir 'bin\sqlite3.exe')) {
        Set-Sqlite3EnvVars -LocalDir $localDir
        return
    }
    if (-not (Test-Path (Join-Path $vtkSqliteSrcDir 'sqlite3.c'))) {
        Write-Warning "sqlite3 not found and vtk submodule source is missing; PROJ's configure step may fail."
        return
    }
    if (-not (Test-Path $VcVarsScript)) {
        Write-Warning "sqlite3 not found and no vcvars script could be resolved for this toolchain; PROJ's configure step may fail."
        return
    }

    Write-Host "sqlite3 not found on this machine. Building a standalone copy from the vendored VTK sqlite3 amalgamation, needed by PROJ/GeoTIFF."

    $srcDir = Join-Path $localDir 'src'
    $includeDir = Join-Path $localDir 'include'
    $libDir = Join-Path $localDir 'lib'
    $binDir = Join-Path $localDir 'bin'
    New-Item -ItemType Directory -Force -Path $srcDir, $includeDir, $libDir, $binDir | Out-Null

    Copy-Item (Join-Path $vtkSqliteSrcDir 'sqlite3.c') $srcDir -Force
    Copy-Item (Join-Path $vtkSqliteSrcDir 'sqlite3.h') $srcDir -Force
    Copy-Item (Join-Path $vtkSqliteSrcDir 'shell.c') $srcDir -Force
    Copy-Item (Join-Path $vtkSqliteSrcDir 'sqlite3.h') $includeDir -Force

    # sqlite3.c/sqlite3.h #include two CMake-generated headers in VTK's real
    # build: vtk_sqlite_mangle.h (renames every sqlite3_* symbol so VTK's
    # copy can't clash with others in the same process) and
    # vtksqlite_export.h (the dllexport/import macro). No-op stand-ins let
    # the amalgamation compile standalone with normal symbol names.
    @'
#ifndef vtk_sqlite_mangle_h
#define vtk_sqlite_mangle_h
#endif
'@ | Set-Content (Join-Path $srcDir 'vtk_sqlite_mangle.h') -Encoding ASCII

    @'
#ifndef VTKSQLITE_EXPORT_H
#define VTKSQLITE_EXPORT_H
#define SQLITE_API
#define VTKSQLITE_DEPRECATED
#define VTKSQLITE_NO_EXPORT
#endif
'@ | Set-Content (Join-Path $srcDir 'vtksqlite_export.h') -Encoding ASCII

    Copy-Item (Join-Path $srcDir 'vtk_sqlite_mangle.h') $includeDir -Force
    Copy-Item (Join-Path $srcDir 'vtksqlite_export.h') $includeDir -Force

    # Run the actual compile inside a nested cmd.exe /c so the vcvars-set
    # environment (cl.exe/lib.exe on PATH) doesn't leak into this script's
    # own process environment.
    $buildScript = Join-Path $localDir 'build_sqlite3.bat'
    @"
@call "$VcVarsScript"
cd /d "$srcDir"
cl /nologo /c /O2 /DSQLITE_THREADSAFE=1 /DSQLITE_ENABLE_COLUMN_METADATA sqlite3.c
lib /nologo sqlite3.obj /OUT:"$libDir\sqlite3.lib"
cl /nologo /O2 /DSQLITE_THREADSAFE=1 /Fe:"$binDir\sqlite3.exe" shell.c sqlite3.c
"@ | Set-Content $buildScript -Encoding ASCII

    Invoke-Native -FilePath "$env:windir\System32\cmd.exe" -ArgumentList '/c', $buildScript

    Set-Sqlite3EnvVars -LocalDir $localDir
}

function Set-Sqlite3EnvVars {
    param([Parameter(Mandatory = $true)][string]$LocalDir)
    $env:SQLite3_ROOT = $LocalDir
    $env:PATH = "$LocalDir\bin;$env:PATH"
}

# ---------------------------------------------------------------------------
# Third-party dependency builds
# ---------------------------------------------------------------------------

function Build-ThirdPartyDependency {
    <#
    Configures + installs one of freetype2/harfbuzz/hdf5/eigen -- the four
    dependencies that all follow the same "mkdir; cmake -G ...; cmake
    --build --target INSTALL" pattern in the .bat.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$SourceDir,
        [Parameter(Mandatory = $true)][string]$BuildFolderName,
        [Parameter(Mandatory = $true)][string]$CMakeExe,
        [Parameter(Mandatory = $true)][string[]]$GeneralCMakeConfigFlags,
        [string[]]$ExtraFlags = @()
    )
    $buildDir = Join-Path $SourceDir $BuildFolderName
    New-Item -ItemType Directory -Force -Path $buildDir | Out-Null
    Push-Location $buildDir
    try {
        $flags = @($GeneralCMakeConfigFlags) + $ExtraFlags + @('..')
        Invoke-Native -FilePath $CMakeExe -ArgumentList $flags
        Invoke-Native -FilePath $CMakeExe -ArgumentList '--build', '.', '--config', 'Release', '--target', 'INSTALL'
    }
    finally {
        Pop-Location
    }
}

function Build-Vtk {
    param(
        [Parameter(Mandatory = $true)][string]$RootSrcFolder,
        [Parameter(Mandatory = $true)][string]$BuildFolderName,
        [Parameter(Mandatory = $true)][string]$CMakeExe,
        [Parameter(Mandatory = $true)][string[]]$GeneralCMakeConfigFlags
    )
    $vtkDir = Join-Path $RootSrcFolder 'vtk'
    $buildDir = Join-Path $vtkDir $BuildFolderName
    New-Item -ItemType Directory -Force -Path $buildDir | Out-Null
    Push-Location $buildDir
    try {
        $flags = @($GeneralCMakeConfigFlags) + @(
            '-DBUILD_TESTING:BOOL=FALSE',
            '-DBUILD_SHARED_LIBS:BOOL=TRUE',
            '-DVTK_MODULE_ENABLE_VTK_RenderingContext2D:STRING=YES',
            '-DVTK_MODULE_ENABLE_VTK_IOImage:STRING=YES',
            '-DVTK_MODULE_ENABLE_VTK_IOGeometry:STRING=YES',
            '-DVTK_MODULE_ENABLE_VTK_IOExport:STRING=YES',
            '-DVTK_MODULE_ENABLE_VTK_RenderingFreeType:STRING=YES',
            '-DVTK_MODULE_ENABLE_VTK_png:STRING=YES',
            '..'
        )
        Invoke-Native -FilePath $CMakeExe -ArgumentList $flags
        Invoke-Native -FilePath $CMakeExe -ArgumentList '--build', '.', '--config', 'Release', '--target', 'INSTALL'
        return $buildDir
    }
    finally {
        Pop-Location
    }
}

function Build-OpenVino {
    param(
        [Parameter(Mandatory = $true)][string]$RootSrcFolder,
        [Parameter(Mandatory = $true)][string]$BuildFolderName,
        [Parameter(Mandatory = $true)][string]$CMakeExe,
        [Parameter(Mandatory = $true)][string[]]$GeneralCMakeConfigFlags
    )
    $openVinoDir = Join-Path $RootSrcFolder '3rdParty\openvino'
    $buildDir = Join-Path $openVinoDir $BuildFolderName

    # Apply the VS2022-compatibility patch only the first time this build
    # folder is created, matching the .bat's own guard against re-applying
    # an already-applied patch on subsequent runs.
    if (-not (Test-Path $buildDir)) {
        Push-Location $openVinoDir
        try {
            Invoke-Native -FilePath 'git' -ArgumentList 'apply', '../0001-Patch-manager.cpp-for-Visual-Studio-2022.patch'
        }
        finally {
            Pop-Location
        }
    }

    New-Item -ItemType Directory -Force -Path $buildDir | Out-Null
    Push-Location $buildDir
    try {
        $flags = @($GeneralCMakeConfigFlags) + @('-DENABLE_JS:BOOL=OFF', '-DENABLE_SAMPLES:BOOL=OFF', '..')
        Invoke-Native -FilePath $CMakeExe -ArgumentList $flags
        Invoke-Native -FilePath $CMakeExe -ArgumentList '--build', '.', '--config', 'Release', '--target', 'INSTALL'
    }
    finally {
        Pop-Location
    }
}

function Get-CudaArchBinOption {
    param(
        [Parameter(Mandatory = $true)][string]$CudaSdkDir,
        [string]$Override = ''
    )
    if ($Override) { return $Override }

    if (-not (Test-Path $CudaSdkDir)) { return '' }

    # Table ported verbatim from Build_Binary_x86.bat lines ~529-548.
    $table = [ordered]@{
        'v8.0'  = '6.0 6.1'
        'v9.0'  = '6.0 6.1 7.0'
        'v9.1'  = '6.0 6.1 7.0'
        'v10.0' = '6.0 6.1 7.0 7.5'
        'v10.1' = '6.0 6.1 7.0 7.5'
        'v11.0' = '6.0 6.1 7.0 7.5 8.0'
        'v11.1' = '5.2 6.0 6.1 7.0 7.5 8.0 8.6'
        'v11.2' = '5.2 6.0 6.1 7.0 7.5 8.0 8.6'
        'v11.3' = '5.2 6.0 6.1 7.0 7.5 8.0 8.6'
        'v11.6' = '5.2 6.0 6.1 7.0 7.5 8.0 8.6'
        'v11.8' = '5.2 6.0 6.1 7.0 7.5 8.0 8.6 8.9 9.0'
        'v12.0' = '5.2 6.0 6.1 7.0 7.5 8.0 8.6 8.9 9.0'
        'v12.6' = '5.2 6.0 6.1 7.0 7.5 8.0 8.6 8.9 9.0'
        'v12.8' = '5.2 6.0 6.1 7.0 7.5 8.0 8.6 8.9 9.0 12.0'
        'v12.9' = '5.2 6.0 6.1 7.0 7.5 8.0 8.6 8.9 9.0 12.0'
        'v13.0' = '7.5 8.0 8.6 8.9 9.0 12.0'
        'v13.1' = '7.5 8.0 8.6 8.9 9.0 12.0'
        'v13.2' = '7.5 8.0 8.6 8.9 9.0 12.0'
    }
    $leaf = Split-Path $CudaSdkDir -Leaf
    if ($table.Contains($leaf)) { return $table[$leaf] }
    return '5.2 6.1 7.5'
}

function Find-CudaSdkDir {
    # Prefers CUDA 13.2, then walks the same version list as the .bat,
    # newest first, checking CUDA_PATH_V<version> environment variables.
    $versions = @(
        '13_2', '13_1', '13_0', '12_9', '12_8', '12_6', '12_0',
        '11_8', '11_6', '11_3', '11_1', '11_0',
        '10_1', '10_0', '9_1', '9_0', '8_0', '7_5'
    )
    foreach ($v in $versions) {
        $envName = "CUDA_PATH_V$v"
        $dir = [Environment]::GetEnvironmentVariable($envName)
        if ($dir -and (Test-Path $dir)) { return $dir }
    }
    return $env:CUDA_PATH
}

function Find-CudaHostCompiler {
    param(
        [Parameter(Mandatory = $true)]$VsEnv
    )
    switch ($VsEnv.DevEnvKind) {
        'VS2017' { $vsToolsRoot = $VsEnv.Vs2017Dir }
        'VS2019' { $vsToolsRoot = $VsEnv.Vs2019Dir }
        'VS2022' { $vsToolsRoot = $VsEnv.Vs2022Dir }
        'VS2026' { $vsToolsRoot = $VsEnv.Vs2026Dir }
        'MSBuildBuildTools' { $vsToolsRoot = $VsEnv.BuildToolsFolder }
        default { $vsToolsRoot = $null }
    }
    if (-not $vsToolsRoot) { return $null }
    $msvcDir = Get-ChildItem (Join-Path $vsToolsRoot 'VC\Tools\MSVC') -Directory -ErrorAction SilentlyContinue |
    Sort-Object Name -Descending | Select-Object -First 1
    if (-not $msvcDir) { return $null }
    return (Join-Path $msvcDir.FullName 'bin\Hostx64\x64\cl.exe')
}

# ---------------------------------------------------------------------------
# Main
# ---------------------------------------------------------------------------

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..\..')
$buildFolderName = "build_$Arch"
$buildFolder = Join-Path $repoRoot $buildFolderName
New-Item -ItemType Directory -Force -Path $buildFolder | Out-Null

$isUwp = $Toolchain -in @('WindowsStore10', 'WindowsPhone81', 'WindowsStore81')

$vsEnv = Resolve-VisualStudioEnvironment -Arch $Arch -Toolchain $Toolchain -ExtraModules $ExtraModules
Write-Host "Using $($vsEnv.DevEnvKind): $($vsEnv.DevEnvPath)"
Write-Host "CMake: $($vsEnv.CMakeExe)"

Push-Location $buildFolder
try {
    if (Test-Path 'CMakeCache.txt') { Remove-Item 'CMakeCache.txt' -Force }

    $installFolder = Join-Path $buildFolder 'install'
    $installFolderFwd = ConvertTo-ForwardSlash $installFolder
    $hdf5Dir = "$installFolderFwd/cmake/hdf5"
    $openVinoInstallDir = "$installFolderFwd/runtime/cmake"
    $tbbInstallDir = (ConvertTo-ForwardSlash $repoRoot) + '/3rdParty/openvino/temp/tbb/cmake'
    $vtkInstallDir = "$installFolderFwd/lib/cmake/vtk-9.6"

    $generalFlags = New-Object System.Collections.Generic.List[string]
    $generalFlags.Add('-DCMAKE_POLICY_VERSION_MINIMUM=3.5')
    $generalFlags.Add('-DCMAKE_BUILD_TYPE:STRING=Release')
    $generalFlags.Add("-DCMAKE_INSTALL_PREFIX:STRING=$installFolderFwd")
    $generalFlags.Add("-DCMAKE_FIND_ROOT_PATH:STRING=$installFolderFwd;$openVinoInstallDir;$tbbInstallDir;$vtkInstallDir")
    if ($Toolchain -eq 'WindowsStore10') {
        $generalFlags.Add('-DCMAKE_SYSTEM_NAME:String=WindowsStore')
        if ($vsEnv.DevEnvKind -eq 'VS2017') { $generalFlags.Add('-DCMAKE_SYSTEM_VERSION:String=10.0.14393.0') }
        if ($vsEnv.DevEnvKind -in @('VS2019', 'VS2022', 'VS2026', 'MSBuildBuildTools')) { $generalFlags.Add('-DCMAKE_SYSTEM_VERSION:String=10.0.18362.0') }
    }

    $cmakeArgs = New-Object System.Collections.Generic.List[string]
    $cmakeArgs.Add('-G'); $cmakeArgs.Add($vsEnv.CMakeGenerator)
    if ($vsEnv.CMakeArchArg) { $cmakeArgs.Add('-A'); $cmakeArgs.Add($vsEnv.CMakeArchArg) }
    $cmakeArgs.AddRange($generalFlags)

    $emguFlags = New-Object System.Collections.Generic.List[string]
    $emguFlags.AddRange($cmakeArgs)
    $emguFlags.AddRange([string[]]@(
            '-DBUILD_DOCS:BOOL=FALSE',
            '-DBUILD_TESTS:BOOL=FALSE',
            '-DBUILD_opencv_apps:BOOL=FALSE',
            '-DBUILD_opencv_java:BOOL=FALSE',
            '-DBUILD_opencv_python2:BOOL=FALSE',
            '-DBUILD_opencv_python3:BOOL=FALSE',
            '-DBUILD_opencv_python_bindings_generator:BOOL=FALSE',
            '-DBUILD_opencv_python_tests:BOOL=FALSE',
            '-DBUILD_opencv_ts:BOOL=FALSE',
            '-DBUILD_WITH_DEBUG_INFO:BOOL=FALSE',
            '-DBUILD_WITH_STATIC_CRT:BOOL=FALSE',
            '-DWITH_OPENGL:BOOL=FALSE',
            '-DCMAKE_DISABLE_FIND_PACKAGE_PNG:BOOL=TRUE',
            '-DCMAKE_IGNORE_PATH:STRING=C:/python-virt/python37/Library;C:/python-virt/python37/Library/include;C:/python-virt/python37/Library/lib',
            '-DVIDEOIO_PLUGIN_LIST:STRING=ffmpeg'
        ))

    $eigenDir = $null
    $vtkBuildDir = $null
    $buildType = 'OPEN_SOURCE'

    # --- Component set (contrib modules) --------------------------------
    if ($ComponentSet -eq 'Full') {
        if ($Arch -notin @('arm', 'arm64')) {
            $freetypeDir = Join-Path $repoRoot '3rdParty\freetype2'
            Build-ThirdPartyDependency -SourceDir $freetypeDir -BuildFolderName $buildFolderName `
                -CMakeExe $vsEnv.CMakeExe -GeneralCMakeConfigFlags $cmakeArgs -ExtraFlags @(
                '-DCMAKE_DISABLE_FIND_PACKAGE_ZLIB:BOOL=TRUE',
                '-DCMAKE_DISABLE_FIND_PACKAGE_BZip2:BOOL=TRUE',
                '-DCMAKE_DISABLE_FIND_PACKAGE_PNG:BOOL=TRUE',
                '-DCMAKE_DISABLE_FIND_PACKAGE_HarfBuzz:BOOL=TRUE'
            )

            $harfbuzzDir = Join-Path $repoRoot 'harfbuzz'
            Build-ThirdPartyDependency -SourceDir $harfbuzzDir -BuildFolderName $buildFolderName `
                -CMakeExe $vsEnv.CMakeExe -GeneralCMakeConfigFlags $cmakeArgs -ExtraFlags @('-DHB_HAVE_FREETYPE:BOOL=TRUE')

            if (-not $isUwp) {
                $hdf5SrcDir = Join-Path $repoRoot 'hdf5'
                Build-ThirdPartyDependency -SourceDir $hdf5SrcDir -BuildFolderName $buildFolderName `
                    -CMakeExe $vsEnv.CMakeExe -GeneralCMakeConfigFlags $cmakeArgs -ExtraFlags @(
                    '-DBUILD_SHARED_LIBS:BOOL=FALSE',
                    '-DBUILD_TESTING:BOOL=FALSE',
                    '-DHDF5_BUILD_EXAMPLES:BOOL=FALSE',
                    '-DHDF5_BUILD_TOOLS:BOOL=FALSE'
                )
            }
        }

        $openCvExtraModulesDir = Join-Path $repoRoot 'opencv_contrib\modules'
        $emguFlags.Add("-DOPENCV_EXTRA_MODULES_PATH:String=$(ConvertTo-ForwardSlash $openCvExtraModulesDir)")
        $emguFlags.Add('-DEMGU_CV_WITH_TESSERACT:BOOL=TRUE')

        $buildVtk = (-not $isUwp) -and ($Arch -notin @('arm', 'arm64')) -and ($Toolchain -in @('OpenVino', 'IntelOpenVino'))
        if ($buildVtk) {
            $vtkBuildDir = Build-Vtk -RootSrcFolder $repoRoot -BuildFolderName $buildFolderName -CMakeExe $vsEnv.CMakeExe -GeneralCMakeConfigFlags $cmakeArgs
            $emguFlags.Add("-DVTK_DIR:String=$(ConvertTo-ForwardSlash $vtkBuildDir)")
        }
    }
    elseif ($ComponentSet -eq 'Mini') {
        # flann must stay enabled: in OpenCV 5 imgproc depends on geometry, which depends on flann.
        $emguFlags.AddRange([string[]]@(
                '-DBUILD_opencv_calib:BOOL=FALSE',
                '-DBUILD_opencv_dnn:BOOL=FALSE',
                '-DBUILD_opencv_photo:BOOL=FALSE',
                '-DBUILD_opencv_features:BOOL=FALSE',
                '-DBUILD_opencv_video:BOOL=FALSE'
            ))
        $emguFlags.Add('-DEMGU_CV_WITH_TESSERACT:BOOL=FALSE')
        $emguFlags.Add('-DEMGU_CV_WITH_FREETYPE:BOOL=FALSE')
        $emguFlags.Add('-DEMGU_CV_WITH_DEPTHAI:BOOL=FALSE')
    }
    else {
        # Core
        $emguFlags.Add('-DEMGU_CV_WITH_TESSERACT:BOOL=FALSE')
        $emguFlags.Add('-DEMGU_CV_WITH_FREETYPE:BOOL=FALSE')
        $emguFlags.Add('-DEMGU_CV_WITH_DEPTHAI:BOOL=FALSE')
    }

    # --- Performance tests ------------------------------------------------
    # Disabled for CUDA, Intel compiler, and UWP builds (compilation errors
    # in those configurations), enabled otherwise.
    $skipPerfTests = $Cuda -or ($Toolchain -in @('Intel', 'IntelOpenVino')) -or $isUwp
    if ($skipPerfTests) {
        $emguFlags.Add('-DBUILD_opencv_ts:BOOL=OFF')
        $emguFlags.Add('-DBUILD_PERF_TESTS:BOOL=OFF')
    }
    else {
        $emguFlags.Add('-DBUILD_opencv_ts:BOOL=ON')
        $emguFlags.Add('-DBUILD_PERF_TESTS:BOOL=ON')
    }

    # --- Extra modules -----------------------------------------------------
    if ($ExtraModules -eq 'NonFree') {
        $emguFlags.Add('-DOPENCV_ENABLE_NONFREE:BOOL=TRUE')
    }
    if ($ExtraModules -eq 'DepthAI') {
        $env:CMAKE_POLICY_VERSION_MINIMUM = '3.10'  # Hunter build error workaround.
        $emguFlags.Add('-DEMGU_CV_WITH_DEPTHAI:BOOL=TRUE')
    }
    # OpenNI is intentionally not ported: it requires OPEN_NI_LIB/OPEN_NI_INCLUDE
    # environment variables from a discontinued SDK that nothing in the
    # current wrapper-script inventory uses. Flag here rather than silently
    # no-op if it's ever requested.
    if ($ExtraModules -eq 'OpenNI') {
        throw "OpenNI is not ported to this script (unused by any current wrapper script; requires the discontinued OpenNI SDK). Use Build_Binary_x86.bat if you need it."
    }

    if ($Documentation) { $emguFlags.Add('-DEMGU_CV_DOCUMENTATION_BUILD:BOOL=TRUE') }

    $emguFlags.Add("-DCMAKE_INSTALL_PREFIX:STRING=$installFolderFwd")

    # --- Eigen (built unconditionally, for every arch/variant) -------------
    $eigenSrcDir = Join-Path $repoRoot 'eigen'
    $eigenBuildDir = Join-Path $eigenSrcDir $buildFolderName
    New-Item -ItemType Directory -Force -Path $eigenBuildDir | Out-Null
    Push-Location $eigenBuildDir
    try {
        Invoke-Native -FilePath $vsEnv.CMakeExe -ArgumentList ($cmakeArgs + @('..'))
        Invoke-Native -FilePath $vsEnv.CMakeExe -ArgumentList '--build', '.', '--config', 'Release', '--target', 'INSTALL'
        $eigenDir = $eigenBuildDir
    }
    finally {
        Pop-Location
    }

    # --- CUDA ---------------------------------------------------------------
    if ($Cuda) {
        $cudaHostCompiler = Find-CudaHostCompiler -VsEnv $vsEnv
        $cudaSdkDir = Find-CudaSdkDir
        if (Test-Path $cudaSdkDir) {
            $cudaArchBinOption = Get-CudaArchBinOption -CudaSdkDir $cudaSdkDir -Override $CudaArchBin
            $emguFlags.Add('-DCUDA_64_BIT_DEVICE_CODE:BOOL=TRUE')
            $emguFlags.Add('-DWITH_CUDA:BOOL=TRUE')
            $emguFlags.Add('-DCUDA_VERBOSE_BUILD:BOOL=TRUE')
            $emguFlags.Add("-DCUDA_TOOLKIT_ROOT_DIR:String=$(ConvertTo-ForwardSlash $cudaSdkDir)")
            $emguFlags.Add("-DCUDA_SDK_ROOT_DIR:String=$(ConvertTo-ForwardSlash $cudaSdkDir)")
            $emguFlags.Add('-DWITH_CUBLAS:BOOL=TRUE')
            $emguFlags.Add('-DBUILD_SHARED_LIBS:BOOL=TRUE')
            $emguFlags.Add('-DOPENCV_SKIP_DLLMAIN_GENERATION=ON')
            $emguFlags.Add("-DCUDA_ARCH_BIN:STRING=$cudaArchBinOption")
            $emguFlags.Add('-DBUILD_opencv_world:BOOL=TRUE')
            $emguFlags.Add('-DCUDA_NVCC_FLAGS:STRING=--expt-relaxed-constexpr --std=c++17')
            $emguFlags.Add('-DCMAKE_CXX_STANDARD:STRING=17')
            $emguFlags.Add('-DWITH_ONNXRUNTIME:BOOL=ON')
            $emguFlags.Add('-DDOWNLOAD_ONNXRUNTIME_GPU:BOOL=ON')
            if ($cudaHostCompiler) { $emguFlags.Add("-DCUDA_HOST_COMPILER:String=$(ConvertTo-ForwardSlash $cudaHostCompiler)") }
            $nvcuvidHeader = Join-Path $cudaSdkDir 'include\nvcuvid.h'
            if (Test-Path $nvcuvidHeader) { $emguFlags.Add('-DWITH_NVCUVID:BOOL=TRUE') }
        }
    }
    else {
        $emguFlags.Add('-DWITH_CUDA:BOOL=FALSE')
        $emguFlags.Add('-DBUILD_SHARED_LIBS:BOOL=FALSE')
    }

    # --- OpenVINO -------------------------------------------------------------
    if ($Toolchain -in @('OpenVino', 'IntelOpenVino')) {
        Build-OpenVino -RootSrcFolder $repoRoot -BuildFolderName $buildFolderName -CMakeExe $vsEnv.CMakeExe -GeneralCMakeConfigFlags $cmakeArgs
        $emguFlags.Add('-DWITH_OPENVINO:BOOL=TRUE')
        $emguFlags.Add('-DOPENCV_DNN_OPENVINO:BOOL=TRUE')
        $emguFlags.Add('-DDNN_PLUGIN_LIST:STRING=all')
        $emguFlags.Add("-DOpenVINO_DIR:STRING=$openVinoInstallDir")
        $emguFlags.Add("-Dngraph_DIR:STRING=$openVinoInstallDir")
        $emguFlags.Add("-DTBB_DIR:STRING=$tbbInstallDir")
        $emguFlags.Add('-DENABLE_CXX11:BOOL=TRUE')
        $buildType = 'COMMERCIAL'
    }
    else {
        $emguFlags.Add('-DWITH_OPENVINO:BOOL=FALSE')
    }

    # --- Intel compiler vs. plain Visual Studio -------------------------------
    $isIntel = $Toolchain -in @('Intel', 'IntelOpenVino')
    if ($isIntel) {
        $intelCompilerDir = $env:ICPP_COMPILER20
        if (-not $intelCompilerDir) {
            throw "Toolchain 'Intel'/'IntelOpenVino' requires the ICPP_COMPILER20 environment variable (set by the Intel oneAPI/Parallel Studio installer)."
        }
        $intelDir = Join-Path $intelCompilerDir 'bin'
        $intelArch = if ($vsEnv.OsMode -eq ' Win64') { 'intel64' } else { 'ia32' }
        $intelDevEnvMap = @{ 'VS2012' = 'vs2012'; 'VS2013' = 'vs2013'; 'VS2015' = 'vs2015'; 'VS2017' = 'vs2017'; 'VS2019' = 'vs2019'; 'VS2022' = 'vs2022' }
        $intelDevEnv = if ($intelDevEnvMap.ContainsKey($vsEnv.DevEnvKind)) { $intelDevEnvMap[$vsEnv.DevEnvKind] } else { '' }

        $tbbVars = Join-Path $intelCompilerDir 'tbb\bin\tbbvars.bat'
        Invoke-Native -FilePath "$env:windir\System32\cmd.exe" -ArgumentList '/c', "call `"$tbbVars`" $intelArch $intelDevEnv"

        if (Test-Path $intelDir) {
            $buildType = 'COMMERCIAL'
            $intelTbbInclude = Join-Path $intelCompilerDir 'tbb\include'
            $emguFlags.Add('-DWITH_TBB:BOOL=TRUE')
            $emguFlags.Add('-DMKL_WITH_TBB:BOOL=TRUE')
            $emguFlags.Add("-DTBB_INCLUDE_DIR:String=$(ConvertTo-ForwardSlash $intelTbbInclude)")
            $emguFlags.Add('-DCV_ICC:BOOL=TRUE')
        }
        $emguFlags.Add('-DWITH_OPENCL:BOOL=TRUE')
        $emguFlags.Add('-DWITH_MSMF:BOOL=TRUE')
    }
    else {
        if ($Toolchain -eq 'Commercial') { $buildType = 'COMMERCIAL' }
        $emguFlags.Add('-DWITH_LAPACK:BOOL=FALSE')

        if ($isUwp) {
            $emguFlags.AddRange([string[]]@(
                    '-DNETFX_CORE:BOOL=TRUE',
                    '-DWITH_DIRECTX:BOOL=FALSE',
                    '-DWITH_OPENEXR:BOOL=FALSE',
                    '-DWITH_TIFF:BOOL=FALSE',
                    '-DEMGU_CV_WITH_TIFF:BOOL=FALSE',
                    '-DWITH_PNG:BOOL=TRUE',
                    '-DWITH_DSHOW:BOOL=FALSE',
                    '-DWITH_WIN32UI:BOOL=FALSE',
                    '-DWITH_VFW:BOOL=FALSE',
                    '-DWITH_MSMF:BOOL=FALSE',
                    '-DWITH_FFMPEG:BOOL=FALSE',
                    '-DWITH_OPENCL:BOOL=FALSE',
                    '-DWITH_EIGEN:BOOL=TRUE',
                    "-DEigen3_DIR:STRING=$eigenDir",
                    '-DEMGU_ENABLE_SSE:BOOL=FALSE'
                ))
        }
        else {
            $emguFlags.Add('-DWITH_OPENCL:BOOL=TRUE')
            $emguFlags.Add('-DWITH_MSMF:BOOL=TRUE')
        }
    }

    # --- ARM vs. x86/x64 CPU dispatch/IPP -------------------------------------
    # Applies regardless of Intel vs. plain-Visual-Studio toolchain above --
    # in the .bat both paths converge on the same :CONFIG_ARM section via
    # GOTO before this point.
    if ($Arch -in @('arm', 'arm64')) {
        if ($buildType -eq 'COMMERCIAL') {
            $emguFlags.Add('-DCV_ENABLE_INTRINSICS:BOOL=ON')
            $emguFlags.Add('-DCPU_BASELINE:STRING=NEON')
        }
        else {
            $emguFlags.Add('-DCV_ENABLE_INTRINSICS:BOOL=OFF')
            $emguFlags.Add('-DCPU_BASELINE:STRING=')
        }
        $emguFlags.Add('-DEMGU_ENABLE_SSE:BOOL=FALSE')
        $emguFlags.Add('-DWITH_IPP:BOOL=OFF')
        $emguFlags.Add('-DEMGU_CV_WITH_FREETYPE:BOOL=OFF')
        $emguFlags.Add('-DBUILD_opencv_freetype:BOOL=OFF')
    }
    else {
        $emguFlags.Add('-DEMGU_ENABLE_SSE:BOOL=TRUE')
        $cpuDispatchFlags = ''
        if ($buildType -eq 'COMMERCIAL') {
            $ippBuildFlags = @('-DWITH_IPP:BOOL=TRUE')
            if ($Arch -eq 'x86') { $cpuDispatchFlags = 'SSE4_1;SSE4_2' }
            if ($Arch -eq 'x86_64') { $cpuDispatchFlags = 'SSE4_1;SSE4_2;AVX;AVX2;AVX512F' }
        }
        else {
            $ippBuildFlags = @('-DWITH_IPP:BOOL=FALSE')
        }
        $emguFlags.AddRange([string[]]$ippBuildFlags)
        $emguFlags.Add("-DCPU_DISPATCH:STRING=$cpuDispatchFlags")
        # MLAS x86 assembly kernels (SgemmKernelSse2.S) require GNU as, which the
        # Visual Studio generator cannot invoke for .S files — the .obj is never
        # produced and the DNN link fails. Disable MLAS for 32-bit x86 Windows;
        # the DNN module falls back to its built-in SGEMM.
        if ($Arch -eq 'x86') { $emguFlags.Add('-DWITH_MLAS:BOOL=FALSE') }
    }

    # --- Run CMake configure -------------------------------------------------
    # Derive the matching vcvars script from the already-resolved DevEnvPath
    # (e.g. "...\Common7\IDE\devenv.com" -> "...\VC\Auxiliary\Build\vcvars64.bat")
    # rather than building a fresh path from raw installationPath variables.
    $vcVarsName = 'vcvars64.bat'
    if ($Arch -eq 'x86') { $vcVarsName = 'vcvars32.bat' }
    if ($Arch -eq 'arm') { $vcVarsName = 'vcvarsamd64_arm.bat' }
    if ($Arch -eq 'arm64') { $vcVarsName = 'vcvarsamd64_arm64.bat' }
    $vcVarsScript = $vsEnv.DevEnvPath -replace 'Common7\\IDE\\devenv\.com', "VC\Auxiliary\Build\$vcVarsName"

    Set-Sqlite3Fallback -BuildFolder $buildFolder -RootSrcFolder $repoRoot -VcVarsScript $vcVarsScript

    Invoke-Native -FilePath $vsEnv.CMakeExe -ArgumentList ($emguFlags.ToArray() + @('..'))

    # --- Optional build/package/doc/nuget targets ----------------------------
    if ($Build) {
        $targets = New-Object System.Collections.Generic.List[string]
        $targets.Add('cvextern')
        if ($Package) { $targets.Add('PACKAGE') }
        if ($Documentation) { $targets.Add('Emgu.CV.Document') }
        if ($Nuget) { $targets.Add('Emgu.CV.runtime.windows.nuget') }

        # One combined invocation with all target names after a single
        # --target, matching the .bat's %CMAKE_BUILD_TARGET% (a single
        # space-separated string), not one invocation per target.
        Invoke-Native -FilePath $vsEnv.CMakeExe -ArgumentList (@('--build', '.', '--config', 'Release', '--target') + $targets.ToArray())
    }
}
finally {
    Pop-Location
}
