# .NET 8.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 8.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 8.0 upgrade.
3. Upgrade SpendingControl.Domain\SpendingControl.Domain.csproj
4. Upgrade SpendingControl.Application\SpendingControl.Application.csproj
5. Upgrade SpendingControl.Infrastructure\SpendingControl.Infrastructure.csproj
6. Upgrade SpendingControl.Api\SpendingControl.Api.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                        | Current Version | New Version | Description                                   |
|:------------------------------------|:---------------:|:-----------:|:----------------------------------------------|

### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### SpendingControl.Domain\\SpendingControl.Domain.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net8.0`

Other changes:
  - <place other changes here>

#### SpendingControl.Application\\SpendingControl.Application.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net8.0`

Other changes:
  - <place other changes here>

#### SpendingControl.Infrastructure\\SpendingControl.Infrastructure.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net8.0`

Other changes:
  - <place other changes here>

#### SpendingControl.Api\\SpendingControl.Api.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net8.0`

Other changes:
  - <place other changes here>
