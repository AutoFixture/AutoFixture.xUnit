# AutoFixture.xUnit

[![License](https://img.shields.io/badge/license-MIT-green)](https://raw.githubusercontent.com/AutoFixture/AutoFixture.xUnit/master/LICENCE.txt)
[![NuGet version](https://img.shields.io/nuget/v/AutoFixture.xUnit?logo=nuget)](https://www.nuget.org/packages/AutoFixture.xUnit)
[![NuGet preview version](https://img.shields.io/nuget/vpre/AutoFixture.xUnit?logo=nuget)](https://www.nuget.org/packages/AutoFixture.xUnit)
[![NuGet downloads](https://img.shields.io/nuget/dt/AutoFixture.xUnit)](https://www.nuget.org/packages/AutoFixture.xUnit)

[AutoFixture.xUnit](https://github.com/AutoFixture/AutoFixture.xUnit) is a .NET library that integrates [AutoFixture](https://github.com/AutoFixture/AutoFixture) with xUnit 1.x, allowing you to effortlessly generate test data for your unit tests.
By automatically populating your test parameters, it helps you write cleaner, more maintainable tests without having to manually construct test objects.

> [!WARNING]
> While this package is still being developed, the xUnit 1 package is deprecated.<br/>
> This package is intended only for legacy projects that are still using xUnit 1.x.<br/>

## Table of Contents

- [Installation](#installation)
- [Getting Started](#getting-started)
- [Integrations](#integrations)
- [Contributing](#contributing)
- [License](#license)

## Installation

AutoFixture packages are distributed via NuGet.<br />
To install the packages you can use the integrated package manager of your IDE, the .NET CLI, or reference the package directly in your project file.

```cmd
dotnet add package AutoFixture.xUnit --version x.x.x
```

```xml
<PackageReference Include="AutoFixture.xUnit" Version="x.x.x" />
```

## Getting Started

### Basic Usage

`AutoFixture.xUnit` provides an `[AutoData]` attribute that automatically populates test method parameters with generated data.

For example, imagine you have a simple calculator class:

```c#
public class Calculator
{
	public int Add(int a, int b) => a + b;
}
```

You can write a test using AutoFixture to provide the input values:

```c#
using Xunit;
using AutoFixture.xUnit;

public class CalculatorTests
{
    [Theory, AutoData]
    public void Add_SimpleValues_ReturnsCorrectResult(
        Calculator calculator, int a, int b)
    {
        // Act
        int result = calculator.Add(a, b);

        // Assert
        Assert.AreEqual(a + b, result);
    }
}
```

### Inline Auto-Data

You can also combine auto-generated data with inline arguments using the `[InlineAutoData]` attribute.
This allows you to specify some parameters while still letting AutoFixture generate the rest.

```c#
using Xunit;
using AutoFixture.xUnit;
using AutoFixture;

public class CalculatorTests
{
    [Theory, InlineAutoData(5, 8)]
    public void Add_SpecificValues_ReturnsCorrectResult(
        int a, int b, Calculator calculator)
    {
        // Act
        int result = calculator.Add(a, b);

        // Assert
        Assert.AreEqual(13, result);
    }
}
```

### Freezing Dependencies

AutoFixture's `[Frozen]` attribute can be used to ensure that the same instance of a dependency is injected into multiple parameters.

For example, if you have a consumer class that depends on a shared dependency:

```c#
public class Dependency { }

public class Consumer
{
    public Dependency Dependency { get; }

    public Consumer(Dependency dependency)
    {
        Dependency = dependency;
    }
}
```

You can freeze the Dependency so that all requests for it within the test will return the same instance:

```c#
using Xunit;
using AutoFixture.xUnit;
using AutoFixture;

public class ConsumerTests
{
    [Theory, AutoData]
    public void Consumer_UsesSameDependency(
        [Frozen] Dependency dependency, Consumer consumer)
    {
        // Assert
        Assert.AreSame(dependency, consumer.Dependency);
    }
}
```

## Integrations

AutoFixture offers a variety of utility packages and integrations with most of the major mocking libraries and testing frameworks.

> [!NOTE]
> Since AutoFixture tries maintain compatibility with a large number of package versions, the packages bundled with AutoFixture might not contain the latest features of your (e.g. mocking) library.<br />
> Make sure to install the latest version of the integrated library package, alongside the AutoFixture packages.

### Core packages

The core packages offer the full set of AutoFixture's features without requring any testing framework or third party integration.

| Product | Package | Stable | Preview | Downloads |
|---------|---------|--------|---------|-----------|
| The core package | [AutoFixture](http://www.nuget.org/packages/AutoFixture) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture)](https://www.nuget.org/packages/AutoFixture) | [![NuGet](https://img.shields.io/nuget/vpre/autofixture)](https://www.nuget.org/packages/AutoFixture) | ![NuGet](https://img.shields.io/nuget/dt/autofixture) |
| Assertion idioms | [AutoFixture.Idioms](http://www.nuget.org/packages/AutoFixture.Idioms) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.Idioms)](https://www.nuget.org/packages/AutoFixture.Idioms) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.Idioms)](https://www.nuget.org/packages/AutoFixture.Idioms) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.idioms) |
| Seed extensions | [AutoFixture.SeedExtensions](http://www.nuget.org/packages/AutoFixture.SeedExtensions) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.SeedExtensions)](https://www.nuget.org/packages/AutoFixture.SeedExtensions) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.SeedExtensions)](https://www.nuget.org/packages/AutoFixture.SeedExtensions) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.seedextensions) |

### Mocking libraries

AutoFixture offers integations with most major .NET mocking libraries.<br/>
These integrations enable such features as configuring mocks, auto-injecting mocks, etc.

| Product | Package | Stable | Preview | Downloads |
|---------|---------|--------|---------|-----------|
| Moq | [AutoFixture.AutoMoq](http://www.nuget.org/packages/AutoFixture.AutoMoq) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.AutoMoq)](https://www.nuget.org/packages/AutoFixture.AutoMoq) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoMoq)](https://www.nuget.org/packages/AutoFixture.AutoMoq) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.automoq) |
| NSubstitute | [AutoFixture.AutoNSubstitute](http://www.nuget.org/packages/AutoFixture.AutoNSubstitute) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.AutoNSubstitute)](https://www.nuget.org/packages/AutoFixture.AutoNSubstitute) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoNSubstitute)](https://www.nuget.org/packages/AutoFixture.AutoNSubstitute) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.AutoNSubstitute) |
| FakeItEasy | [AutoFixture.AutoFakeItEasy](http://www.nuget.org/packages/AutoFixture.AutoFakeItEasy) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.AutoFakeItEasy)](https://www.nuget.org/packages/AutoFixture.AutoFakeItEasy) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoFakeItEasy)](https://www.nuget.org/packages/AutoFixture.AutoFakeItEasy) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.AutoFakeItEasy) |
| Rhino Mocks | [AutoFixture.AutoRhinoMocks](http://www.nuget.org/packages/AutoFixture.AutoRhinoMocks) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.AutoRhinoMocks)](https://www.nuget.org/packages/AutoFixture.AutoRhinoMocks) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoRhinoMocks)](https://www.nuget.org/packages/AutoFixture.AutoRhinoMocks) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.AutoRhinoMocks) |

### Testing frameworks

AutoFixture offers integrations with most major .NET testing frameworks.<br />
These integrations enable auto-generation of test cases, combining auto-generated data with inline arguments, etc.

| Product | Package | Stable | Preview | Downloads |
|---------|---------|--------|---------|-----------|
| xUnit v3 | [AutoFixture.Xunit3](http://www.nuget.org/packages/AutoFixture.Xunit3) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.Xunit3)](https://www.nuget.org/packages/AutoFixture.Xunit3) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.Xunit3)](https://www.nuget.org/packages/AutoFixture.Xunit3) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.xUnit3) |
| xUnit v2 | [AutoFixture.Xunit2](http://www.nuget.org/packages/AutoFixture.Xunit2) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.Xunit2)](https://www.nuget.org/packages/AutoFixture.Xunit2) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.Xunit2)](https://www.nuget.org/packages/AutoFixture.Xunit2) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.xUnit2) |
| xUnit v1 | [AutoFixture.Xunit](http://www.nuget.org/packages/AutoFixture.Xunit) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.Xunit)](https://www.nuget.org/packages/AutoFixture.Xunit) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.Xunit)](https://www.nuget.org/packages/AutoFixture.Xunit) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.XUnit) |
| NUnit v4 | [AutoFixture.NUnit4](http://www.nuget.org/packages/AutoFixture.NUnit4) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.NUnit4)](https://www.nuget.org/packages/AutoFixture.NUnit4) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.NUnit4)](https://www.nuget.org/packages/AutoFixture.NUnit4) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.NUnit4) |
| NUnit v3 | [AutoFixture.NUnit3](http://www.nuget.org/packages/AutoFixture.NUnit3) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.NUnit3)](https://www.nuget.org/packages/AutoFixture.NUnit3) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.NUnit3)](https://www.nuget.org/packages/AutoFixture.NUnit3) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.NUnit3) |
| NUnit v2 | [AutoFixture.NUnit2](http://www.nuget.org/packages/AutoFixture.NUnit2) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.NUnit2)](https://www.nuget.org/packages/AutoFixture.NUnit2) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.NUnit2)](https://www.nuget.org/packages/AutoFixture.NUnit2) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.NUnit2) |
| Foq | [AutoFixture.AutoFoq](http://www.nuget.org/packages/AutoFixture.AutoFoq) | [![NuGet](https://img.shields.io/nuget/v/AutoFixture.AutoFoq)](https://www.nuget.org/packages/AutoFixture.AutoFoq) | [![NuGet](https://img.shields.io/nuget/vpre/AutoFixture.AutoFoq)](https://www.nuget.org/packages/AutoFixture.AutoFoq) | ![NuGet](https://img.shields.io/nuget/dt/autofixture.AutoFoq) |

You can check the compatibility with your target framework version on the [wiki](https://github.com/AutoFixture/AutoFixture/wiki#net-platforms-compatibility-table) or on the [NuGet](https://www.nuget.org/profiles/AutoFixture) website.

## Contributing

Contributions to `AutoFixture.xUnit` are welcome!
If you would like to contribute, please review our [contributing guidelines](https://github.com/AutoFixture/AutoFixture.xUnit/blob/maste/CONTRIBUTING.md) and open an issue or pull request.

## License

AutoFixture is Open Source software and is released under the [MIT license](https://raw.githubusercontent.com/AutoFixture/AutoFixture.xUnit/master/LICENCE.txt).<br />
The licenses allows the use of AutoFixture libraries in free and commercial applications and libraries without restrictions.

### .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
