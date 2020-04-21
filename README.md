# InTechNet.Api

InTechNet back-end in .NET Core 3.1

## Installation

> You can find the detailed documentation in french [here](https://pbouillon.gitbook.io/intechnet/)

### Prerequisites

In order to correctly proceed to the installation you will need:

- [.NET Core 3.1](https://dotnet.microsoft.com/download)
- C# 8.0
- An IDE such as [VS Code](https://code.visualstudio.com/) or [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
- A running [PostgreSQL](https://www.postgresql.org/) server

### Clone the repository

Start by creating a local copy of this repository

```bash
~$ git clone https://github.com/pBouillon/InTechNet.API
```

> If you want to work on new features, you can create your very own fork

### Initialize the project

#### Setup the connection to the database

Change the appropriate connection parameters to your PostgreSQL server in
the `appsettings.json` file, in the section `ConnectionStrings`

```json
"ConnectionStrings": {
  "InTechNetDatabase": "Host=localhost;Database=InTechNet;Username=postgres;Password=root"
}
```

> If you want to change the connection string only for your development environment, you can specify it in `appsettings.Development.json`

#### Install the dependencies

To update all required NuGet packages, execute the following:

```bash
InTechNet.Api$ cd InTechNet.Api
InTechNet.Api/InTechNet.Api$ dotnet restore
```

#### Compile the project

When you still are in the `InTechNet.Api/` folder, run the following:

```bash
InTechNet.Api/InTechNet.Api$ dotnet build
```

> By default, the target is `Debug`, if you want to compile in `Release` mode, add `--configuration Release`

### Run the API

Here is how you can start the compilation in Release:

```bash
InTechNet.Api/InTechNet.Api$ dotnet publish --configuration Release
InTechNet.Api/InTechNet.Api$ cd InTechNet.Api/bin/Release/netcoreapp3.1
InTechNet.Api/InTechNet.Api/InTechNet.Api/bin/Release/netcoreapp3.1$ dotnet InTechNet.Api.dll
```

That's it ! The API is now reachable on port 5001 by default. You can access it
on `http://localhost:5001`. You can also use the Swagger UI with the generated
documentation on `http://localhost:5001/swagger`.

On startup, the programm will check if the database is up and will create the
appropriates tables if any is missing.
