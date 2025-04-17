# Northwind CRM .NET 9.0 Migration

This is a .NET 9.0 Migration Example.
- Database: MSSQL SERVER
- Backend: WEB API, DB First, Entity Framework, Domain Driven Design.
- UI: React. 

## Northwind REST WEB Api

### VS Code Debug Options:
It will run in local chrome browser 
- either with SSL at the link https://localhost:7251/swagger/index.html
- or without SSL at the link http://localhost:5205/swagger/index.html

To make it run like this please add fallowing two files under the ```\src\NorthwindApi\.vscode``` folde.

**tasks.json:**
```json
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "build",
      "command": "dotnet",
      "type": "process",
      "args": [
        "build",
        "${workspaceFolder}/NorthwindApi.csproj"
      ],
      "group": {
        "kind": "build",
        "isDefault": true
      },
      "problemMatcher": "$msCompile"
    }
  ]
}
```
**launch.json**
```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": ".NET Core Launch (web)",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/bin/Debug/net9.0/NorthwindApi.dll",
            "args": [],
            "cwd": "${workspaceFolder}",
            "stopAtEntry": false,
            "serverReadyAction": {
                "action": "openExternally",
                "pattern": "\\bNow listening on:\\s+(https?://\\S+)",
                "uriFormat": "%s/swagger/index.html"
            },
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            },
            "sourceFileMap": {
                "/Views": "${workspaceFolder}/Views"
            }
        }
    ]
}

```


