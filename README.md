# Code analyzer for .NET Core

How to run:
```shell script
> codeanalyzer --targets ../../../../test_codeanalyzer/example/Class2.cs ../../../../test_codeanalyzer/example/Class3.cs
```

## Prerequisites

- .NET Core SDK (see [Download and install .NET Core](https://docs.microsoft.com/ja-jp/dotnet/core/install/sdk))
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
