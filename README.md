# FuzzedDataProviderCS
FuzzedDataProvider for C#, inspired by Google's FuzzedDataProvider. Look at: 
- common description https://github.com/google/fuzzing/blob/master/docs/split-inputs.md#fuzzed-data-provider
- source code https://github.com/llvm-mirror/compiler-rt/blob/master/include/fuzzer/FuzzedDataProvider.h
- example https://fuchsia.googlesource.com/fuchsia/+/dbda4024104e/examples/fuzzers/cpp/fuzzed-data-provider.cc


### HowTo (First run):

```
dotnet build
dotnet test FuzzedDataProviderCSTest
```

In case of troubles with Debuger (because of different target platforms of Library and Test), make `dotnet clean`, and then build all the projects for x64, like: 
```
dotnet build -a x64 FuzzedDataProviderCSLibrary/FuzzedDataProviderCSLibrary.csproj
dotnet build -a x64 FuzzedDataProviderCSTest/FuzzedDataProviderCSTest.csproj
```
Also try to restart restart VSCode after rebuild.

### Tasks:
1. Templatize it using Generics/Abstract class.
2. Test in DNF/Win.
3. Make shims and test for another encoding order (need fix for Pose library https://github.com/tonerdo/pose/issues/69).