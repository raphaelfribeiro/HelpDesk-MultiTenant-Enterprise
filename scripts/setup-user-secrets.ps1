<#
.SYNOPSIS
    Interactive helper to populate dotnet user-secrets for the three HelpDesk runtime projects.

.DESCRIPTION
    Prompts for each secret value and writes it to the appropriate project's user-secrets store.
    Press Enter on a prompt to skip that key (existing value, if any, is preserved).

    User-secrets are stored under %APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json
    and are NEVER committed to source control.

.NOTES
    Run from the repo root:
        pwsh ./scripts/setup-user-secrets.ps1
#>

[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$projects = @(
    @{
        Name = 'HelpDesk.API'
        Path = 'src/HelpDesk.API/HelpDesk.API.csproj'
        Keys = @(
            'ConnectionStrings:Default',
            'Jwt:Key',
            'CosmosDb:ConnectionString',
            'ServiceBus:ConnectionString',
            'EventHub:ConnectionString'
        )
    },
    @{
        Name = 'HelpDesk.AuditWorker'
        Path = 'src/HelpDesk.AuditWorker/HelpDesk.AuditWorker.csproj'
        Keys = @(
            'EventHub:ConnectionString',
            'BlobStorage:ConnectionString',
            'CosmosDb:ConnectionString'
        )
    },
    @{
        Name = 'HelpDesk.NotificationWorker'
        Path = 'src/HelpDesk.NotificationWorker/HelpDesk.NotificationWorker.csproj'
        Keys = @(
            'ServiceBus:ConnectionString'
        )
    }
)

foreach ($project in $projects) {
    Write-Host ""
    Write-Host "=== $($project.Name) ===" -ForegroundColor Cyan

    if (-not (Test-Path $project.Path)) {
        Write-Warning "Project not found: $($project.Path). Skipping."
        continue
    }

    foreach ($key in $project.Keys) {
        $value = Read-Host "  $key (Enter to skip)"
        if ([string]::IsNullOrWhiteSpace($value)) {
            Write-Host "    -> skipped" -ForegroundColor DarkGray
            continue
        }

        & dotnet user-secrets set $key $value --project $project.Path | Out-Null
        if ($LASTEXITCODE -ne 0) {
            Write-Warning "    Failed to set $key on $($project.Name)"
        } else {
            Write-Host "    -> set" -ForegroundColor Green
        }
    }
}

Write-Host ""
Write-Host "Done. Inspect a project's secrets with:" -ForegroundColor Yellow
Write-Host "  dotnet user-secrets list --project src/HelpDesk.API/HelpDesk.API.csproj"
