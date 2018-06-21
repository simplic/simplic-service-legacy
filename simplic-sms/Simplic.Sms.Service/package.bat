del *.nupkg
nuget pack Simplic.Sms.Service.csproj -properties Configuration=Debug
nuget push *.nupkg -Source http://simplic.biz:10380/nuget