## Publishing to Nuget

[Creating and Publishing a Nuget Package](http://docs.nuget.org/docs/creating-packages/creating-and-publishing-a-package)

Copy latest .dll into .\lib\net40

`nuget pack .\LeanKit.API.Client.Library.dll.nuspec`

`nuget push .\LeanKit.API.Client.1.1.0.nupkg`