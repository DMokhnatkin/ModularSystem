param(
	[string]$source = "Ivan",
	[switch]$push
)

function bldnuget([string]$dir, [string]$source, [switch]$push)
{
    $tmp = Join-Path -Path $dir -ChildPath "*.csproj"
    $proj = Get-ChildItem $tmp | Select-Object -First 1 | % {
        $_.FullName
    }
    $outputPath = Join-Path -Path $dir -ChildPath "Deploy"
    ./nuget.exe pack $proj -outputdirectory $outputPath
    if ($push)
    {
		$tmp = Join-Path -Path $outputPath -ChildPath *.nupkg
		# get last modifed nupkg file (this file we generate before)
		$package = Get-ChildItem $tmp | Sort-Object LastAccessTime -Descending | Select-Object -First 1 | % {
			$_.FullName
		}
		echo $package
	    ./nuget.exe push $package -Source $source
    }
}

bldnuget -dir "..\src\ModularSystem.Common" -source $source -push:$push
bldnuget -dir "..\src\ModularSystem.Common.Wpf" -source $source -push:$push
bldnuget -dir "..\src\ModularSystem.Communication" -source $source -push:$push