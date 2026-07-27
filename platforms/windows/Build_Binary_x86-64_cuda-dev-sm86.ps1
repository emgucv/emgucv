<#
.SYNOPSIS
    PowerShell equivalent of Build_Binary_x86-64_cuda-dev-sm86.bat.
    Configure-only (no -Build), matching the original's "configure" %7 value.
#>
& "$PSScriptRoot\Build_Binary.ps1" -Arch x86_64 -Cuda -ExtraModules NonFree -CudaArchBin '8.6'
