# Roslyn Analyzers Runner

`Roslyn Analyzers Runner` is a command-line tool that wraps the `.NET Compiler Platform (Roslyn)` in order to analyze C# files independently and write the results to a JSON file which makes it easy to be reused by another tools.

## How to use

First, install the following packages:

- .NET Core Runtime (see [Install the .NET Core Runtime](https://docs.microsoft.com/dotnet/core/install/runtime))

- Roslyn Analyzers Runner
```shell script
> mkdir ./RoslynAnalyzersRunner
> curl -SL https://github.com/sider/roslyn-analyzers-runner/releases/download/v0.1.1/Sider.RoslynAnalyzersRunner.0.1.1.linux-x64.tar.gz | tar -zxvC ./RoslynAnalyzersRunner
```
Then, download the analyzers and configure the dependencies. Run commands below after setting up [nuget](https://docs.microsoft.com/nuget/install-nuget-client-tools).

```shell script
> nuget install Microsoft.CodeAnalysis.Analyzers
> nuget install Microsoft.CodeAnalysis.FxCopAnalyzers
```
You will get FxCopAnalyzers and its dependencies in the current directory. You also need to specify the path to the analyzer DLLs by editing analyzers.json.

```shell script
> vi ./RoslynAnalyzersRunner/analyzers.json
[
  "PATH_TO_/Microsoft.CodeAnalysis.Analyzers.3.0.0/analyzers/dotnet/cs/Microsoft.CodeAnalysis.Analyzers.dll",
  "PATH_TO_/Microsoft.CodeAnalysis.Analyzers.3.0.0/analyzers/dotnet/cs/Microsoft.CodeAnalysis.CSharp.Analyzers.dll",
  "PATH_TO_/Microsoft.CodeQuality.Analyzers.3.0.0/analyzers/dotnet/cs/Microsoft.CodeQuality.Analyzers.dll",
  "PATH_TO_/Microsoft.CodeQuality.Analyzers.3.0.0/analyzers/dotnet/cs/Microsoft.CodeQuality.CSharp.Analyzers.dll",
  "PATH_TO_/Microsoft.NetCore.Analyzers.3.0.0/analyzers/dotnet/cs/Microsoft.NetCore.Analyzers.dll",
  "PATH_TO_/Microsoft.NetCore.Analyzers.3.0.0/analyzers/dotnet/cs/Microsoft.NetCore.CSharp.Analyzers.dll",
  "PATH_TO_/Microsoft.NetFramework.Analyzers.3.0.0/analyzers/dotnet/cs/Microsoft.NetFramework.Analyzers.dll",
  "PATH_TO_/Microsoft.NetFramework.Analyzers.3.0.0/analyzers/dotnet/cs/Microsoft.NetFramework.CSharp.Analyzers.dll"
]
```

Now you can run with your source code to analyze.
```shell script
> cd ./RoslynAnalyzersRunner
> Sider.RoslynAnalyzersRunner Example1.cs Example2.cs
```
Or, you can write results to a JSON file.
```shell script
> Sider.RoslynAnalyzersRunner --outputfile result.json Example1.cs Example2.cs
```

## Developer guide

### Prerequisites

- .NET Core SDK (see [Download and install .NET Core](https://docs.microsoft.com/dotnet/core/install/sdk))
- dotnet tarball (see [Packaging utilities for .NET Core](https://github.com/qmfrederik/dotnet-packaging))

Install the dotnet-tarball utility as:
```shell script
> dotnet tool install --global dotnet-tarball
```
The dotnet-tarball is installed in the ~/.dotnet/tools directory. Please make sure that the directory is included correctly in the PATH environment variable. See [dotnet tool install](https://docs.microsoft.com/dotnet/core/tools/dotnet-tool-install).

- Microsoft.CodeAnalysis.Analyzers (see [nuget.org](https://www.nuget.org/packages/Microsoft.CodeAnalysis.Analyzers/))
- Microsoft.CodeAnalysis.FxCopAnalyzers (see [nuget.org](https://www.nuget.org/packages/Microsoft.CodeAnalysis.FxCopAnalyzers/))

Install the nuget packages as:
```shell script
PM> Install-Package Microsoft.CodeAnalysis.Analyzers -Version 3.0.0
PM> Install-Package Microsoft.CodeAnalysis.FxCopAnalyzers -Version 3.0.0
```

Or, you can use the dotnet command as:
```shell script
> mkdir dummy_project; cd $_
> dotnet new console
> dotnet add package Microsoft.CodeAnalysis.Analyzers --version 3.0.0
> dotnet add package Microsoft.CodeAnalysis.FxCopAnalyzers --version 3.0.0
```

### How to build

Follow the steps below:
```shell script
> git clone https://github.com/sider/roslyn-analyzers-runner.git
> cd roslyn-analyzers-runner/
> dotnet build
> dotnet test
> cd RoslynAnalyzersRunner/
> dotnet run
```

Let's package the binary files as a tarball archive.
```shell script
> ./build.sh
```
You can find the compiled and packaged tarball archive in bin/Release/netcoreapp3.1/linux-x64.

### Change analyzers and their versions

You can change the analyzers and their versions at runtime by changing a list of analyzers defined in [analyzers.json](RoslynAnalyzersRunner/analyzers.json).

# License

See [LICENSE](LICENSE).
