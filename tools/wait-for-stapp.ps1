#!/usr/bin/env pwsh

param ($stapp)

do {
    $status = az staticwebapp environment show -n $stapp | ConvertFrom-Json | Select-Object -ExpandProperty status
} while ($status -ne "Ready")
