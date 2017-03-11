param(
	[Parameter(Mandatory=$True,Position=1)]
	[string]$proj,
	[string]$source = "Ivan",
	[switch]$push
)

nuget pack $proj
if ($push)
{
	# get last modifed nupkg file (this file we generate before)
	$latest = Get-ChildItem *.nupkg | Sort-Object LastAccessTime -Descending | Select-Object -First 1
	$package_name = [regex]::match($latest.Name, '(d+\.d+\.d+\.d+)\.nupkg$').Groups[2].Value
	echo $package_name
	
	Try
	{
		nuget delete ModularSystem.Common 1.0.0 -Source $source
	}
	Catch [system.exception]
	{
	}
	nuget push ./ModularSystem.Common.1.0.0.0.nupkg -Source $source
}