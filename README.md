# FuzzedDataProviderCS

*Made as a part of __Competence Center Community__ activities (Telegram: https://t.me/sdl_community)*

FuzzedDataProvider for C#, inspired by Google's FuzzedDataProvider. Look at: 
- common description and the conception of Structure Aware Fuzzing  https://github.com/google/fuzzing/blob/master/docs/split-inputs.md#fuzzed-data-provider
- source code https://github.com/llvm-mirror/compiler-rt/blob/master/include/fuzzer/FuzzedDataProvider.h
- example https://fuchsia.googlesource.com/fuchsia/+/dbda4024104e/examples/fuzzers/cpp/fuzzed-data-provider.cc
- abstract (mostly in Russian) [Structure_Aware_Fuzzing_and_logical_erros.pdf](Docs/Structure_Aware_Fuzzing_and_logical_erros.pdf) 

Written in **.NET Standard 2.1**.

Made for using with C#-fuzzers [like sharpfuzz](https://github.com/Metalnem/sharpfuzz). 

### HowTo (Test run):

Install:
- as sources to be built (`git clone https://github.com/ispras/FuzzedDataProviderCS`);
- as a **nuget `dotnet add package FuzzedDataProviderCS`**).

Ubuntu 20.04, install [.NET 6](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu#2004-), then:

```
dotnet build
dotnet test FuzzedDataProviderCSTest
```

In case of troubles with Debuger (because of different target platforms of Library and Test), make `dotnet clean`, and then build all the projects for x64, like: 
```
dotnet build -a x64 FuzzedDataProviderCSLibrary/FuzzedDataProviderCSLibrary.csproj
dotnet build -a x64 FuzzedDataProviderCSTest/FuzzedDataProviderCSTest.csproj
```
Also try to restart restart VSCode after rebuild (developed and tested with VSCode).

### HowTo (Main concept and commands):

1. Create FuzzedDataProviderCS class instance. You must pass to the constructor an array to be parsed (mandatory property `data`).You can instruct the instance to exit the program (and, in case of fuzzing, move to the next iteration) when all the fuzzed data was consumed, but not all concuming calls were done (arbitrary property `exitAppOnInsufficientData`).

2. Start consuming the data using consuming functions (all public functions are self-documented):

- ConsumeByte()
- ConsumeChar()
- ConsumeInt16()
- ConsumeUInt16()
- ConsumeInt32()
- ConsumeUInt32()
- ConsumeInt64()
- ConsumeUInt64()
- ConsumeDouble()
- ConsumeDateTime()
- ConsumeEnum()
- ConsumeBytes()
- ConsumeRemainingBytes()
- ConsumeString()
- ConsumeRemainingAsString()

3. Most of the functions allows you to set a range or a set of possible values. For example:
- you can instruct the instance to consume an Int32 in a Range [-8; 20359];
- you can instruct the instance to consume a String where all of the symbols must belong to a Set of ['a', 'B', '8', 'Ă'];
- etc.

4. When the data to be consumed is over, but you ordere the instance to consume more, insufficient bytes will be filled with 0x00 (in case of `exitAppOnInsufficientData` was set to default value `false` in instance constructor).

### HowTo (Quick Example):

The code 

```
using FuzzedDataProviderCSLibrary;

...

public void TestComplex()
    {
        byte[] testArr = { 0x01, 0x02, 0x00, 0x41, 0x00, 0x41, 0x01, 0x02 };
        
        var fdp = new FuzzedDataProviderCS(testArr, exitAppOnInsufficientData : false); //Create instance
        var resultUInt16 = fdp.ConsumeUInt16(); //Consume 2 bytes and convert it to UInt16
        var resultBytes = fdp.ConsumeBytes(2); //Consume 2 bytes and copy it to Byte[]
        var resultStr = fdp.ConsumeRemainingAsString(new HashSet<char>() { '\u0043', '\x0044', '\x45' }); //Consume all the remaining data (4 bytes), convert it to string (Unicode), and map all of them into the *Bag of Chars* (a kind of hashing)
        var resultDT = fdp.ConsumeDateTime(); //The data is over, but because of exitAppOnInsufficientData : false 4 zeroes will be read and coverted to DateTime          
    }

```

will construct:


```
resultUInt16,h: 0x0102
resultBytes,h: {byte[0x00000002]} 0x00, 0x41
resultStr: "EC" //Yeah, the magic of mapping of A'\x41' and Ă'\x0102' to C'\u0043' and E'\x45'
resultDT: {1/1/0001 12:00:00 AM} //Smallest possible DateTime
```

You could see a plenty of usings and results in [UnitTest1.cs](FuzzedDataProviderCSTest/UnitTest1.cs). 

### Tasks:
1. Templatize it using Generics/Abstract class.
2. Test in DNF/Win.
3. Make shims and test for another encoding order (need fix for Pose library https://github.com/tonerdo/pose/issues/69).
4. Add Array/List consumers.
