# dotnet clean
Remove-Item "$env:USERPROFILE\.nuget\packages\ametrin.optional.analyzer" -Recurse
Remove-Item "$env:USERPROFILE\.nuget\packages\ametrin.optional" -Recurse
# Remove-Item "$env:USERPROFILE\.nuget\packages\ametrin.optional.testing.tunit" -Recurse
dotnet pack .\analyzer\ -c Release
dotnet pack .\src\ -c Release
dotnet pack .\testing\TUnit\ -c Release
dotnet build
dotnet run --project .\test\
