del *.nupkg
nuget pack Simplic.ConfigurationAnalyzer.Service.csproj -properties Configuration=Debug
nuget push *.nupkg -Source http://simplic.biz:10380/nuget