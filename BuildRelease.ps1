function Build-Solutions
{
    .$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild .\BuildRelease.msbuild
}

function Create-NugetPackages
{
    ""
    "Creating NuGet packages"
	& '.\Lib\Nuget 1.2\nuget.exe' pack .\Nuget\2.1\AutoFixture.2.1.nuspec -b Release -o Release
	& '.\Lib\Nuget 1.2\nuget.exe' pack .\Nuget\2.1\AutoFixture.AutoMoq.2.1.nuspec -b Release -o Release
	& '.\Lib\Nuget 1.2\nuget.exe' pack .\Nuget\2.1\AutoFixture.Xunit.2.1.nuspec -b Release -o Release
	& '.\Lib\Nuget 1.2\nuget.exe' pack .\Nuget\2.1\AutoFixture.AutoRhinoMocks.2.1.nuspec -b Release -o Release
}

Build-Solutions
Create-NugetPackages