<#
.SYNOPSIS
    Build x86_64 with CUDA and documentation.
#>
& "$PSScriptRoot\Build_Binary.ps1" -Arch x86_64 -Cuda -Documentation
