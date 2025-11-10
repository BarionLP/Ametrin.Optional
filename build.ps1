Remove-Item "$env:USERPROFILE\.nuget\packages\ametrin.optional.analyzer" -Recurse
Remove-Item "$env:USERPROFILE\.nuget\packages\ametrin.optional" -Recurse
# Remove-Item "$env:USERPROFILE\.nuget\packages\ametrin.optional.testing.tunit" -Recurse
# dotnet clean
dotnet pack .\analyzer\ -c release
dotnet pack .\src\ -c release
dotnet pack .\testing\TUnit\ -c release
dotnet build
dotnet test
