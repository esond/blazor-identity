# blazor-identity | [![continuous](https://github.com/esond/blazor-identity/actions/workflows/continuous.yml/badge.svg)](https://github.com/esond/blazor-identity/actions/workflows/continuous.yml)

An example Blazor application that accesses an API service, both secured with
ASP.NET Identity.

## Overview

The system has two primary components:

- A Blazor web application application that serves as the front-end
- An ASP.NET Core web api that serves as the backend.

Both are secured with cookies created by the ASP.NET Identity framework. Users
are stored in SQL Server using Entity Framework.

Both applications are secured using cookies generated by the ASP.NET Identity
framework.

## Notes

Configuring cookie creation with a shared Data Protection key was the key to
getting this to work harmoniously. The common data protection key is set up
using [this](https://github.com/esond/blazor-identity/blob/93e3d3fe26231ff314e1dfdc0d8c18d3a3e20497/src/Web/Server/ProgramExtensions.cs#L27-L29)
configuration:

```csharp
services.AddDataProtection()
    .SetApplicationName("BlazorIdentity");
```

For more information on configuring ASP.NET Core data protection, see the
[official docs](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview)
