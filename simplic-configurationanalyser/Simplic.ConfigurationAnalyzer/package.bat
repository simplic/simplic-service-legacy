del *.nupkg
nuget pack Simplic.ConfigurationAnalyzer.csproj -properties Configuration=Debug
nuget push *.nupkg -Source http://simplic.biz:10380/nuget