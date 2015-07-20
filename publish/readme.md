## Publishing to Nuget

[Creating and Publishing a Nuget Package](http://docs.nuget.org/docs/creating-packages/creating-and-publishing-a-package)

Bump version number in ..\LeanKit.API.Client.Library\Properties\AssemblyInfo.cs

Bump version number in .\LeanKit.API.Client.Library.dll.nuspec

Copy latest .dll into .\lib\net40

`nuget pack .\LeanKit.API.Client.Library.dll.nuspec`

`nuget push .\LeanKit.API.Client.1.2.4.nupkg`