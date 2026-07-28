<#
.SYNOPSIS
    Build x86_64 with CUDA (NonFree extra modules, arch bin 8.6) for local dev.
    Configure-only (no -Build), matching the original's "configure" %7 value.
#>
& "$PSScriptRoot\Build_Binary.ps1" -Arch x86_64 -Cuda -ExtraModules NonFree -CudaArchBin '8.6'
