# CleanArch.AspNetCore

![GitHub last commit](https://img.shields.io/github/last-commit/gabbium/dotnet-cleanarch-aspnetcore)
![Sonar Quality Gate](https://img.shields.io/sonar/quality_gate/gabbium_dotnet-cleanarch-aspnetcore?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Coverage](https://img.shields.io/sonar/coverage/gabbium_dotnet-cleanarch-aspnetcore?server=https%3A%2F%2Fsonarcloud.io)
![NuGet](https://img.shields.io/nuget/v/Gabbium.CleanArch.AspNetCore)

A lightweight **ASP.NET Core integration library** for [dotnet-cleanarch](https://github.com/gabbium/dotnet-cleanarch), providing:

-   ✅ **Result → HTTP mappers**
-   ✅ **Minimal API endpoint discovery**
-   ✅ **ProblemDetails integration**

Designed to **reduce boilerplate** when exposing Clean Architecture building blocks through ASP.NET Core.

---

## ✨ Features

-   ✅ **Map `Result` and `Result<T>` from the domain to proper HTTP responses**
-   ✅ **`Validation` errors → `ValidationProblemDetails` (grouped by field)**
-   ✅ **`Problem` errors → `ProblemDetails` (single business rule error)**
-   ✅ **Automatic Minimal API endpoint registration** via `IEndpoint`
-   ✅ **Seamless integration with `dotnet-cleanarch` core errors**

---

## 🧱 Tech Stack

| Layer   | Stack                             |
| ------- | --------------------------------- |
| Runtime | .NET 9                            |
| Package | NuGet                             |
| CI/CD   | GitHub Actions + semantic-release |

---

## 📦 Installation

```bash
dotnet add package Gabbium.CleanArch.AspNetCore
```

---

## 🚀 Usage

**1️⃣ Mapping Results to HTTP**

```csharp
app.MapPost("/users", async (CreateUserCommand command, IMediator mediator) =>
{
    Result<UserDto> result = await mediator.Send(command);

    return result.IsSuccess
        ? Results.Ok(result.Value)
        : CustomResults.Problem(result);
});
```

✅ `CustomResults.Problem(result)` automatically maps:

-   `Validation` → `400 Bad Request`
-   `NotFound` → `404 Not Found`
-   `Conflict` → `409 Conflict`
-   `Unauthorized` → `401 Unauthorized`
-   `Forbidden` → `403 Forbidden`
-   `Failure` (default) → `500 Internal Server Error`

---

**2️⃣ Minimal API Endpoint Discovery**

Define endpoints implementing a **abstract class**, for example:

```csharp
public class CreateUserEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        ...
    }
}
```

Then register & map automatically:

```csharp
var app = builder.Build();

app.MapEndpoints(Assembly.GetExecutingAssembly());
```

---

## 🪪 License

This project is licensed under the MIT License – see [LICENSE](LICENSE) for details.
