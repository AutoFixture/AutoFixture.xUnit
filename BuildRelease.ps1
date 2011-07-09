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

function Copy-Output
{
    copy .\Src\AutoFixture\bin\Release\Ploeh.AutoFixture.dll .\Release
    copy .\Src\AutoFixture\bin\Release\Ploeh.AutoFixture.XML .\Release
    
    copy .\Src\SemanticComparison\bin\Release\Ploeh.SemanticComparison.dll .\Release
    copy .\Src\SemanticComparison\bin\Release\Ploeh.SemanticComparison.XML .\Release
    
    copy .\Src\AutoMoq\bin\Release\Ploeh.AutoFixture.AutoMoq.dll .\Release
    copy .\Src\AutoMoq\bin\Release\Ploeh.AutoFixture.AutoMoq.XML .\Release
    
    copy .\Src\AutoRhinoMock\bin\Release\Ploeh.AutoFixture.AutoRhinoMock.dll .\Release
    copy .\Src\AutoRhinoMock\bin\Release\Ploeh.AutoFixture.AutoRhinoMock.XML .\Release
    
    copy .\Src\AutoFixture.xUnit.net\bin\Release\Ploeh.AutoFixture.Xunit.dll .\Release
    copy .\Src\AutoFixture.xUnit.net\bin\Release\Ploeh.AutoFixture.Xunit.XML .\Release
}

Build-Solutions
Copy-Output
Create-NugetPackages