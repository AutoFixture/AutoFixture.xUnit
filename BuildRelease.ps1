# This PowerShell script assumes that MSBuild and other .NET SDK utilities are available.
# This is most easily enabled by pulling in VSVars32.bat into PS. See e.g.
# http://blogs.msdn.com/b/ploeh/archive/2008/04/09/visualstudio2008powershell.aspx
# to see how this can be done.

function Build-Solutions ($buildConfiguration)
{
    dir Src/*.sln | % { `
        write-host "Building $_ ($buildConfiguration)"
    	msbuild /nologo /clp:Summary /Verbosity:quiet /p:Configuration=$buildConfiguration $_
    	}
}

function Run-Tests
{
    dir .\Src\*Test\bin\Release\*Test.dll | % {
        & '.\Lib\xUnit 1.5\xunit.console.exe' $_
        }
}

function Copy-Output
{
    copy .\Src\AutoFixture\bin\Release\Ploeh.AutoFixture.dll .\Release
    copy .\Src\AutoFixture\bin\Release\Ploeh.AutoFixture.XML .\Release
    
    copy .\Src\SemanticComparison\bin\Release\Ploeh.SemanticComparison.dll .\Release
    copy .\Src\SemanticComparison\bin\Release\Ploeh.SemanticComparison.XML .\Release
    
    copy .\Src\AutoMoq\bin\Release\Ploeh.AutoFixture.AutoMoq.dll .\Release
    copy .\Src\AutoMoq\bin\Release\Ploeh.AutoFixture.AutoMoq.XML .\Release
    
    copy .\Src\AutoMoq\bin\Release\Ploeh.AutoFixture.AutoMoq.dll .\Release
    copy .\Src\AutoMoq\bin\Release\Ploeh.AutoFixture.AutoMoq.XML .\Release
    
    copy .\Src\AutoFixture.xUnit.net\bin\Release\Ploeh.AutoFixture.Xunit.dll .\Release
    copy .\Src\AutoFixture.xUnit.net\bin\Release\Ploeh.AutoFixture.Xunit.XML .\Release
}

rd .\Release* -Recurse
md Release
Build-Solutions Verify
Build-Solutions Release
Run-Tests
Copy-Output