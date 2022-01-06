# FuzzedDataProviderCS
FuzzedDataProvider for C#, inspired by https://github.com/llvm-mirror/compiler-rt/blob/master/include/fuzzer/FuzzedDataProvider.h


HowTo:
1. Run `dotnet test FuzzedDataProviderCSTest` for tests.
2. In case of troubles with Debuger (because of different target platforms of Library and Test), make `dotnet clean`, and then build all the projects for x64, like `dotnet build -a x64 FuzzedDataProviderCSLibrary/FuzzedDataProviderCSLibrary.csproj` and `dotnet build -a x64 FuzzedDataProviderCSTest/FuzzedDataProviderCSTest.csproj`. Also try to restart restart VSCode after rebuild.

Extras for tests:
- cd FuzzedDataProviderCSTest && dotnet add package Pose   //for Shimming some System methods.

Tasks:
1. Read about FDP
2. Read about splicling in C#
3. Write backend
4. Write first cutters
5. Write tests
6. Test with sharpfuzz
7. Push to ispras
8. Write to community

