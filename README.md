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

Ubuntu 20.04, install [.NET 6], (https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu#2004-), then build from sources and test it:

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

### HowTo (Full Example with Sharpfuzz):

1. Read the guide and install the [sharpfuzz](https://github.com/Metalnem/sharpfuzz#installation).
2. Create new library project `dotnet new classlib -o TestLib` and add a simple class into Program.cs, that has a public function, throwing an error in case of wrong parameter values combination.
```
namespace TestLib;
public class Class1
{
    public static void BadFunction(UInt16 v1, Byte[] v2, String v3, DateTime v4)
    {
        if (v2[1]==0xFA)
            if (v1==0x1013)
                if (v3.Length == 4)
                    if (v3[2] == 'W')
                        if (v4.DayOfWeek == DayOfWeek.Friday)
                            throw new Exception();
    }

}
```
3. Create new console project for tests `dotnet new console`.
4. Install FuzzedDataProviderCS package form nuget `dotnet add package FuzzedDataProviderCS`. Add sharpfuzz package too `dotnet add package SharpFuzz`. Add reference to the test library `dotnet add test.csproj reference TestLib/TestLib.csproj`. Your .csproj file should looks like the code below now:

```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>false</ImplicitUsings>
    <Nullable>enable</Nullable>    
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="FuzzedDataProviderCS" Version="1.1.7" />
    <PackageReference Include="SharpFuzz" Version="1.6.2" />
  </ItemGroup>

  <ItemGroup>    
    <ProjectReference Include="..\TestLib\TestLib.csproj" />
  </ItemGroup>

</Project>
```

5. Add sharpfuzz wrapper and FuzzedDataProviderCS-wrapper into Program.cs.
```
using System;
using System.IO;
using SharpFuzz;
using FuzzedDataProviderCSLibrary;
using System.Collections.Generic;

namespace Test
{
    public class Program
    {
        private static void FuzzTarget(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                var fdp = new FuzzedDataProviderCS(
                    ms.ToArray(), exitAppOnInsufficientData: false);

                var v1 = fdp.ConsumeUInt16();
                var v2 = fdp.ConsumeBytes(3);
                var v3_len = fdp.ConsumeByte();
                var v3 = fdp.ConsumeString(
                    length : v3_len, new HashSet<char>() { '5', '+', 'W', 'X', 'A' });
                var v4 = fdp.ConsumeDateTime();
                
                TestLib.Class1.BadFunction(v1, v2, v3, v4);
            }
        }
        public static void Main(string[] args)
        {

            using (Stream s = new MemoryStream(new byte[3]{0x33, 0x33, 0x44}))
            {
                Fuzzer.Run(stream => FuzzTarget(stream)); //Using sharpfuzz Run(Action<Stream>) overload
            }            
        }
    }
}
```

6. Build the project, then according to [sharpfuzz usage](https://github.com/Metalnem/sharpfuzz#usage) instrument TestLib.dll (**the one in the /bin subdirectory of test console project**, not the one on the TestLib/bin!) and fuzz the code. I`ve got a crash after ~1.50 of one-core fuzzing. **Right now sharpfuzz instrumenter doesn\`t work with .NET6, so install net-sdk-5.0 just for instrumenting purpose**.

7. Open the crashing sample with a HEX-viewer and check that the data corresponds the param values of TestLib crashing function.




### Tasks:
1. Templatize it using Generics/Abstract class.
2. Test in DNF/Win.
3. Make shims and test for another encoding order (need fix for Pose library https://github.com/tonerdo/pose/issues/69).
4. Add Array/List consumers.
