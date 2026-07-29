<#
.SYNOPSIS
    Rebuilds all four Android ABIs without cleaning existing build output
    first.

.DESCRIPTION
    PowerShell port of build_all.bat -- a thin wrapper around
    rebuild_all.ps1 -NoClean (was `call rebuild_all noclean` in the .bat).
#>
& (Join-Path $PSScriptRoot 'rebuild_all.ps1') -NoClean
