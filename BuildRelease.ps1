function Build-Solutions
{
    .$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild .\BuildRelease.msbuild
}

function Create-NugetPackages
{
    ""
    "Creating NuGet packages"
	& '.\Lib\NuGet\nuget.exe' pack .\Nuget\2.1\AutoFixture.2.1.nuspec -BasePath Release -o Release
	& '.\Lib\NuGet\nuget.exe' pack .\Nuget\2.1\AutoFixture.AutoMoq.2.1.nuspec -BasePath Release -o Release
	& '.\Lib\NuGet\nuget.exe' pack .\Nuget\2.1\AutoFixture.Xunit.2.1.nuspec -BasePath Release -o Release
	& '.\Lib\NuGet\nuget.exe' pack .\Nuget\2.1\AutoFixture.AutoRhinoMocks.2.1.nuspec -BasePath Release -o Release
}

Build-Solutions
Create-NugetPackages