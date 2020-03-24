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
