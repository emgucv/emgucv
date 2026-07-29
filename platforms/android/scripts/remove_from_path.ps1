<#
.SYNOPSIS
    Removes a substring from an environment variable (e.g. a stale entry
    from PATH), collapsing any resulting double-semicolons.

.DESCRIPTION
    PowerShell port of remove_from_path.bat. Environment variables are
    process-wide in .NET, not scoped to PowerShell's own variable scoping,
    so unlike wincfg.ps1 this does not need to be dot-sourced -- calling it
    with `& .\remove_from_path.ps1 -VariableName PATH -Value '...'` already
    mutates $env:PATH for the whole process, matching the original script's
    `call remove_from_path.bat PATH "..."` (batch CALL shares environment
    with the caller, no subshell is spawned).

.PARAMETER VariableName
    Name of the environment variable to edit, e.g. "PATH". Was %~1 in the
    .bat.

.PARAMETER Value
    Substring to remove from it. Was %~2 in the .bat.
#>
param(
    [Parameter(Mandatory = $true)][string]$VariableName,
    [Parameter(Mandatory = $true)][string]$Value
)

$current = [Environment]::GetEnvironmentVariable($VariableName, 'Process')
if ($null -eq $current) { return }

$updated = $current.Replace($Value, '')
$updated = $updated.Replace(';;', ';')
$updated = $updated.Replace(';;', ';')

[Environment]::SetEnvironmentVariable($VariableName, $updated, 'Process')
