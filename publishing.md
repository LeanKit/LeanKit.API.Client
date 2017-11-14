## Publishing to Nuget

[Creating and Publishing a Nuget Package for .NET Standard 2.0](https://docs.microsoft.com/en-us/nuget/guides/create-net-standard-packages-vs2017)

Bump `PackageVersion` number in ..\LeanKit.API.Client\LeanKit.API.Client.csproj

```
msbuild /t:pack /p:Configuration=Release
```

[Publish to Nuget](https://docs.microsoft.com/en-us/nuget/create-packages/publish-a-package)

Copy latest .dll into .\lib\net40

`nuget pack .\LeanKit.API.Client.Library.dll.nuspec`

`nuget push .\LeanKit.API.Client.2.0.0.nupkg`