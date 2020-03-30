# Code analyzer for .NET Core

`dotnet_codeanalyzer` is a command-line tool that wraps the `.NET Compiler Platform` in order to analyze C# files independently and write the results to a JSON file which makes it easy to be reused by another tools.

## Installation

First, install the following packages:

- .NET Core Runtime (see [Install the .NET Core Runtime](https://docs.microsoft.com/dotnet/core/install/runtime))

- dotnet_codeanalyzer
```shell script
> export PATH=/usr/local/bin/dotnet_codeanalyzer:$PATH
> sudo mkdir -p /usr/local/bin/dotnet_codeanalyzer
> curl -SL https://github.com/sider/dotnet_codeanalyzer/releases/download/v0.1.1/codeanalyzer.0.1.1.linux-x64.tar.gz | sudo tar -zxC /usr/local/bin/dotnet_codeanalyzer
```
Then, download the analyzers and configure the dependencies.

```shell script
> sudo apt-get install nuget
> nuget install Microsoft.CodeAnalysis.Analyzers
> nuget install Microsoft.CodeAnalysis.FxCopAnalyzers
```
You will get FxCopAnalyzers and its dependencies in the current directory. You also need to specify the path to the analyzer DLLs by editing analyzers.json.

```shell script
> sudo vi /usr/local/bin/dotnet_codeanalyzer/analyzers.json
[
  "PATH_TO_/Microsoft.CodeAnalysis.Analyzers.2.9.8/analyzers/dotnet/cs/Microsoft.CodeAnalysis.Analyzers.dll",
  "PATH_TO_/Microsoft.CodeAnalysis.Analyzers.2.9.8/analyzers/dotnet/cs/Microsoft.CodeAnalysis.CSharp.Analyzers.dll",
  "PATH_TO_/Microsoft.CodeQuality.Analyzers.2.9.8/analyzers/dotnet/cs/Microsoft.CodeQuality.Analyzers.dll",
  "PATH_TO_/Microsoft.CodeQuality.Analyzers.2.9.8/analyzers/dotnet/cs/Microsoft.CodeQuality.CSharp.Analyzers.dll",
  "PATH_TO_/Microsoft.NetCore.Analyzers.2.9.8/analyzers/dotnet/cs/Microsoft.NetCore.Analyzers.dll",
  "PATH_TO_/Microsoft.NetCore.Analyzers.2.9.8/analyzers/dotnet/cs/Microsoft.NetCore.CSharp.Analyzers.dll",
  "PATH_TO_/Microsoft.NetFramework.Analyzers.2.9.8/analyzers/dotnet/cs/Microsoft.NetFramework.Analyzers.dll",
  "PATH_TO_/Microsoft.NetFramework.Analyzers.2.9.8/analyzers/dotnet/cs/Microsoft.NetFramework.CSharp.Analyzers.dll"
]
```

Now you can run with your source code to analyze.
```shell script
> codeanalyzer --targets Example1.cs Example2.cs
```
Or, you can generate results as a JSON file.
```shell script
> codeanalyzer --outputfile result.json --targets Example1.cs Example2.cs
```


## Prerequisites

- .NET Core SDK (see [Download and install .NET Core](https://docs.microsoft.com/dotnet/core/install/sdk))
- dotnet tarball (see [Packaging utilities for .NET Core](https://github.com/qmfrederik/dotnet-packaging))

Install the dotnet-tarball utility as:
```shell script
> dotnet tool install --global dotnet-tarball
```

- Microsoft.CodeAnalysis.Analyzers (see [nuget.org](https://www.nuget.org/packages/Microsoft.CodeAnalysis.Analyzers/))
- Microsoft.CodeAnalysis.FxCopAnalyzers (see [nuget.org](https://www.nuget.org/packages/Microsoft.CodeAnalysis.FxCopAnalyzers/))

Install the nuget packages as:
```shell script
PM> Install-Package Microsoft.CodeAnalysis.Analyzers -Version 2.9.8
PM> Install-Package Microsoft.CodeAnalysis.FxCopAnalyzers -Version 2.9.8
```

Or, you can use the dotnet command as:
```shell script
> mkdir dummy_project; cd $_
> dotnet new console
> dotnet add package Microsoft.CodeAnalysis.Analyzers --version 2.9.8
> dotnet add package Microsoft.CodeAnalysis.FxCopAnalyzers --version 2.9.8
```

## How to build

Follow the steps below:
```shell script
> git clone https://github.com/sider/dotnet_codeanalyzer.git
> cd dotnet_codeanalyzer
> dotnet build
> cd test_codeanalyzer
> dotnet test
> cd dotnet_codeanalyzer
> dotnet run
```

Let's package the binary files as a tarball archive:
```shell script
> ./build.sh
```
You can find the compiled and packaged tarball archive in bin/Release/netcoreapp3.1/linux-x64.

## Change analyzers and their versions

You can change the analyzers and their versions by changing a list of analyzers defined in [analyzers.json](dotnet_codeanalyzer/analyzers.json).

# License

See [LICENSE](LICENSE).
