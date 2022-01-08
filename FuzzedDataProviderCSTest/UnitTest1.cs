using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FuzzedDataProviderCSLibrary;

namespace FuzzedDataProviderCSTest
{

    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Everything is possible under the Moon... So check it!
        /// </summary>
        [TestMethod]
        public void TestTypes()
        {
            Assert.AreEqual(0, Byte.MinValue);

            Assert.AreEqual(1, sizeof(System.Byte));
            Assert.AreEqual(2, sizeof(System.Char));
            Assert.AreEqual(2, sizeof(System.UInt16));
            Assert.AreEqual(2, sizeof(System.Int16));
            Assert.AreEqual(4, sizeof(System.UInt32));
            Assert.AreEqual(4, sizeof(System.Int32));
            Assert.AreEqual(8, sizeof(System.UInt64));
            Assert.AreEqual(8, sizeof(System.Int64));
            Assert.AreEqual(8, sizeof(System.Double));
        }

        #region Waiting for Pose bugfix https://github.com/tonerdo/pose/issues/69
        // Shim isLittleEndianShim = Shim.
        //     Replace(() => FuzzedDataProviderCS.IsLittleEndian).
        //     With(() => false);
        // PoseContext.Isolate(() =>
        // {
        //     fdp = new FuzzedDataProviderCS(testArr1, false);
        //     result = fdp.ConsumeInt32();                
        // }, isLittleEndianShim);
        #endregion

        /// <summary>
        /// Main test complex for all signed values
        /// </summary>
        [TestMethod]
        public void TestConsumeInt32EnoughData()
        {
            byte[] testArr0 = { 0x00, 0x00, 0x00, 0x00 };
            byte[] testArr1 = { 0x00, 0x00, 0x00, 0x01 };
            byte[] testArr2 = { 0xDE, 0xAD, 0xBE, 0xEF };
            byte[] testArr3 = { 0x00, 0x00, 0x00, 0x03 };
            byte[] testArr4 = { 0xFF, 0xFF, 0xFE, 0xF2 };
            byte[] testArr5 = { 0xFF, 0xFF, 0xFF, 0xFF };
            byte[] testArr6 = { 0x7F, 0xFF, 0xFF, 0xFF };
            byte[] testArr7 = { 0x80, 0x00, 0x00, 0x00 };
            byte[] testArr8 = { 0x80, 0x00, 0x00, 0x01 };


            var fdp = new FuzzedDataProviderCS(testArr1, false);
            var result = fdp.ConsumeInt32();
            Assert.AreEqual(1, result);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeInt32();
            Assert.AreEqual(-559038737, result);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeInt32(min: -1, max: 0);
            Assert.AreEqual(0, result);

            fdp = new FuzzedDataProviderCS(testArr0, false);
            result = fdp.ConsumeInt32(min: -1, max: 0);
            Assert.AreEqual(-1, result);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeInt32(min: -20, max: -17);
            Assert.AreEqual(-19, result);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeInt32(min: -20, max: -17);
            Assert.AreEqual(-17, result);

            fdp = new FuzzedDataProviderCS(testArr0, false);
            result = fdp.ConsumeInt32(min: 888, max: 900);
            Assert.AreEqual(888, result);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeInt32(min: 888, max: 900);
            Assert.AreEqual(891, result);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeInt32(min: 888, max: 889);
            Assert.AreEqual(889, result);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeInt32(min: 0, max: 2);
            Assert.AreEqual(0, result);

            fdp = new FuzzedDataProviderCS(testArr4, false);
            result = fdp.ConsumeInt32();
            Assert.AreEqual(-270, result);
            Assert.AreEqual(false, fdp.InsufficientData); //Just for extra non-important test

            fdp = new FuzzedDataProviderCS(testArr5, false);
            result = fdp.ConsumeInt32();
            Assert.AreEqual(-1, result);

            fdp = new FuzzedDataProviderCS(testArr5, false);
            result = fdp.ConsumeInt32();
            Assert.AreEqual(-1, result);

            fdp = new FuzzedDataProviderCS(testArr6, false);
            result = fdp.ConsumeInt32();
            Assert.AreEqual(Int32.MaxValue, result);

            fdp = new FuzzedDataProviderCS(testArr7, false);
            result = fdp.ConsumeInt32();
            Assert.AreEqual(Int32.MinValue, result);

            fdp = new FuzzedDataProviderCS(testArr8, false);
            result = fdp.ConsumeInt32(Int32.MinValue, 0);
            Assert.AreEqual(Int32.MinValue, result);
        }

        /// <summary>
        /// Main test complex for all signed values
        /// </summary>
        [TestMethod]
        public void TestConsumeInt32NotEnough()
        {
            byte[] testArr0 = { };

            byte[] testArr1 = { 0x01 };

            byte[] testArr2 = { 0x01, 0x00, 0x00, 0x00, 0x01 };

            byte[] testArr3 = { 0x00, 0x00, 0x03 };

            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeInt32();
            Assert.AreEqual(0, result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeInt32();
            Assert.AreEqual(16777216, result);
            Assert.AreEqual(false, fdp.InsufficientData);
            result = fdp.ConsumeInt32();
            Assert.AreEqual(16777216, result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeInt32(16777214, 16777215);
            Assert.AreEqual(16777214, result);
            Assert.AreEqual(false, fdp.InsufficientData);
            result = fdp.ConsumeInt32(16777214, 16777215);
            Assert.AreEqual(16777214, result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeInt32();
            Assert.AreEqual(768, result);
            Assert.AreEqual(true, fdp.InsufficientData);
            result = fdp.ConsumeInt32();
            Assert.AreEqual(0, result);
            Assert.AreEqual(true, fdp.InsufficientData);
        }

        /// <summary>
        /// Main test complex for all unsigned values
        /// </summary>
        [TestMethod]
        public void TestConsumeUInt32EnoughData()
        {
            byte[] testArr0 = { 0x00, 0x00, 0x00, 0x00 };
            byte[] testArr1 = { 0x00, 0x00, 0x00, 0x01 };
            byte[] testArr2 = { 0xDE, 0xAD, 0xBE, 0xEF };
            byte[] testArr3 = { 0x00, 0x00, 0x00, 0x03 };
            byte[] testArr4 = { 0x00, 0x00, 0x00, 0x04 };
            byte[] testArr5 = { 0xFF, 0xFF, 0xFF, 0xFF };

            var fdp = new FuzzedDataProviderCS(testArr1, false);
            var result = fdp.ConsumeUInt32();
            Assert.AreEqual((UInt32)1, result);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeUInt32();
            Assert.AreEqual(3735928559, result);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeUInt32(min: 0, max: UInt32.MaxValue);
            Assert.AreEqual((UInt32)1, result);

            fdp = new FuzzedDataProviderCS(testArr0, false);
            result = fdp.ConsumeUInt32(min: 0, max: 1);
            Assert.AreEqual((UInt32)0, result);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeUInt32(min: 17, max: 20);
            Assert.AreEqual((UInt32)20, result);

            fdp = new FuzzedDataProviderCS(testArr4, false);
            result = fdp.ConsumeUInt32(min: 17, max: 20);
            Assert.AreEqual((UInt32)17, result);
            Assert.AreEqual(false, fdp.InsufficientData); //Just for extra non-important test

            fdp = new FuzzedDataProviderCS(testArr0, false);
            result = fdp.ConsumeUInt32();
            Assert.AreEqual(UInt32.MinValue, result);

            fdp = new FuzzedDataProviderCS(testArr5, false);
            result = fdp.ConsumeUInt32();
            Assert.AreEqual(UInt32.MaxValue, result);

            fdp = new FuzzedDataProviderCS(testArr0, false);
            result = fdp.ConsumeUInt32(UInt32.MinValue, 0);
            Assert.AreEqual(UInt32.MinValue, result);

            fdp = new FuzzedDataProviderCS(testArr5, false);
            result = fdp.ConsumeUInt32(0, UInt32.MaxValue);
            Assert.AreEqual(UInt32.MaxValue, result);
        }

        /// <summary>
        /// Main test complex for all unsigned values
        /// </summary>
        [TestMethod]
        public void TestConsumeUInt32NotEnoughData()
        {
            byte[] testArr0 = { 0xFF, 0xFF, 0xFF };
            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeUInt32();
            Assert.AreEqual((UInt32)4294967040, result);
            Assert.AreEqual(true, fdp.InsufficientData);
        }

        [TestMethod]
        public void TestConsumeInt16()
        {
            byte[] testArr0 = { 0xFF };
            byte[] testArr1 = { 0x80, 0x00 };
            byte[] testArr2 = { 0x7F, 0xFF };

            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeInt16();
            Assert.AreEqual((Int16)(-256), result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeInt16(Int16.MinValue, Int16.MaxValue);
            Assert.AreEqual(-32768, result);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeInt16(Int16.MinValue, Int16.MaxValue);
            Assert.AreEqual(32767, result);
        }

        [TestMethod]
        public void TestConsumeUInt16()
        {
            byte[] testArr0 = { 0xFF };
            byte[] testArr1 = { 0x00, 0x00 };
            byte[] testArr2 = { 0xFF, 0xFF };

            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeUInt16();
            Assert.AreEqual(65280, result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeUInt16(0, UInt16.MaxValue);
            Assert.AreEqual(UInt16.MinValue, result);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeUInt16(0, UInt16.MaxValue);
            Assert.AreEqual(UInt16.MaxValue, result);
        }

        [TestMethod]
        public void TestConsumeInt64()
        {
            byte[] testArr0 = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            byte[] testArr1 = { 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] testArr2 = { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeInt64();
            Assert.AreEqual((Int64)(-256), result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeInt64(Int64.MinValue, Int64.MaxValue);
            Assert.AreEqual(-9223372036854775808, result);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeInt64(Int64.MinValue, Int64.MaxValue);
            Assert.AreEqual(9223372036854775807, result);
        }

        [TestMethod]
        public void TestConsumeUInt64()
        {
            byte[] testArr0 = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            byte[] testArr1 = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] testArr2 = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeUInt64();
            Assert.AreEqual(18446744073709551360, result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeUInt64(0, UInt64.MaxValue);
            Assert.AreEqual(UInt64.MinValue, result);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeUInt64(0, UInt64.MaxValue);
            Assert.AreEqual(UInt64.MaxValue, result);
        }

        [TestMethod]
        public void TestConsumeByte()
        {
            byte[] testArr0 = { };
            byte[] testArr1 = { 0x00 };
            byte[] testArr2 = { 0xFF };
            byte[] testArr3 = { 0x05 };

            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeByte();
            Assert.AreEqual((Byte)0, result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeByte(0, Byte.MaxValue);
            Assert.AreEqual(Byte.MinValue, result);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeByte(0, Byte.MaxValue);
            Assert.AreEqual(Byte.MaxValue, result);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeByte(1, 3);
            Assert.AreEqual((Byte)3, result);
        }

        [TestMethod]
        public void TestConsumeBytes()
        {
            byte[] testArr0 = { };
            byte[] testArr1 = { 0x00, 0x05, 0x07 };
            byte[] testArr2 = { 0x00, 0x05 };
            byte[] testArr3 = { 0x07 };
            byte[] testArr4 = { 0x00, 0x00, 0x00 };
            byte[] testArr5 = { 0x00 };
            byte[] testArr6 = { 0x05, 0x07 };
            byte[] testArr7 = { 0x00, 0x05, 0x07, 0x03 };
            byte[] testArr8 = { 0x02, 0x04, 0x03, 0x02, 0x02 };

            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeBytes();
            Assert.AreEqual(0, result.Length);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeBytes(2);
            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(Enumerable.SequenceEqual(testArr2, result));
            Assert.AreEqual(false, fdp.InsufficientData);
            result = fdp.ConsumeBytes(1);
            Assert.IsTrue(Enumerable.SequenceEqual(testArr3, result));
            Assert.AreEqual(false, fdp.InsufficientData);
            result = fdp.ConsumeBytes(3);
            Assert.IsTrue(Enumerable.SequenceEqual(testArr4, result));
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeRemainingBytes();
            Assert.IsTrue(Enumerable.SequenceEqual(testArr1, result));
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeBytes(1);
            Assert.IsTrue(Enumerable.SequenceEqual(testArr5, result));
            Assert.AreEqual(false, fdp.InsufficientData);
            result = fdp.ConsumeRemainingBytes();
            Assert.IsTrue(Enumerable.SequenceEqual(testArr6, result));
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr7, false);
            result = fdp.ConsumeBytes(5, 0x02, 0x04);
            Assert.IsTrue(Enumerable.SequenceEqual(testArr8, result));
            Assert.AreEqual(true, fdp.InsufficientData);
        }

        [TestMethod]
        public void TestConsumeChar()
        {
            byte[] testArr0 = { };
            byte[] testArr1 = { 0x23 };
            byte[] testArr2 = { 0x88, 0xAA };
            byte[] testArr3 = { 0x00, 0x25 };
            byte[] testArr4 = { 0x00, 0x26 };
            byte[] testArr5 = { 0x01, 0x02 };
            byte[] testArr6 = { 0x00, 0x41 };

            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeChar();
            Assert.AreEqual('\0', result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeChar();
            Assert.AreEqual('\u2300', result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeChar();
            Assert.AreEqual('\u88AA', result);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeChar(new HashSet<char>());
            Assert.AreEqual('\u88AA', result);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeChar(new HashSet<char>() { '\x22' });
            Assert.AreEqual('\u0022', result);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeChar(new HashSet<char>() { '\x20', '\x21', '\x22' });
            Assert.AreEqual('\u0021', result);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr4, false);
            result = fdp.ConsumeChar(new HashSet<char>() { '\x20', '\x21', '\x22' });
            Assert.AreEqual('\u0022', result);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr5, false);
            result = fdp.ConsumeChar();
            Assert.AreEqual('Ă', result);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr6, false);
            result = fdp.ConsumeChar();
            Assert.AreEqual('A', result);
            Assert.AreEqual(false, fdp.InsufficientData);
        }

        ///<summary>
        ///Tested in Ubuntu 20.04, where UTF-8_no_BOM is to be the default one.
        ///</summary>
        [TestMethod]
        public void TestConsumeString()
        {
            byte[] testArr0 = { };
            byte[] testArr1 = { 0x01, 0x02, 0x00, 0x41, 0x00, 0x42, 0x01, 0x02 };
            byte[] testArr2 = { 0x01, 0x02, 0x00, 0x41 };
            byte[] testArr3 = { 0x01, 0x02, 0x00, 0x41, 0x00 };

            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeString();
            Assert.AreEqual(String.Empty, result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeString(4);
            Assert.AreEqual("ĂABĂ", result);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeString(3);
            Assert.AreEqual("ĂA\0", result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeString(3);
            Assert.AreEqual("ĂA\0", result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeRemainingAsString();
            Assert.AreEqual("ĂA", result);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeRemainingAsString();
            Assert.AreEqual("ĂA\0", result);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            var resultChr = fdp.ConsumeChar();
            var resultStr = fdp.ConsumeString(3);
            Assert.AreEqual(3, resultStr.Length);
            Assert.AreEqual(resultChr, resultStr[2]);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeRemainingAsString(new HashSet<char>() { '\u0043', '\x0044', '\x45' });
            Assert.AreEqual("CECC", result);
            Assert.AreEqual(false, fdp.InsufficientData);
        }

        enum testEnum
        {
            None = 0,
            Test1 = 4,
            Test2 = 9
        }

        [TestMethod]
        public void TestConsumeEnum()
        {
            byte[] testArr0 = { 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x08 };

            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeEnum<testEnum>();
            Assert.AreEqual(4, result);
            result = fdp.ConsumeEnum<testEnum>();
            Assert.AreEqual(9, result);
            result = fdp.ConsumeEnum<testEnum>();
            Assert.AreEqual(0, result);
            Assert.AreEqual(true, fdp.InsufficientData);
        }

        [TestMethod]
        public void TestConsumeDateTime()        {
            byte[] testArr0 = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] testArr1 = { 0x2b, 0xca, 0x28, 0x75, 0xf4, 0x37, 0x3f, 0xff };
            byte[] testArr2 = { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            byte[] testArr3 = { 0x11, 0x03, 0x08, 0x30, 0x0A, 0x50, 0x0B, 0x0C };
            byte[] testArr4 = { 0x20, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00 };
            
            var fdp = new FuzzedDataProviderCS(testArr0, false);
            var result = fdp.ConsumeDateTime();
            Assert.AreEqual(DateTime.MinValue, result);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeDateTime();
            Assert.AreEqual(DateTime.MaxValue, result);
            Assert.AreEqual(false, fdp.InsufficientData);
            
            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeDateTime(
                min : new DateTime(1992, 1, 5), max : new DateTime(2020, 2, 3));
            Assert.AreEqual(result.Year, 2016);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr3, false);
            result = fdp.ConsumeDateTime(
                min : new DateTime(1992, 1, 5), max : new DateTime(2020, 2, 3));
            Assert.AreEqual(result.Year, 2001);
            Assert.AreEqual(false, fdp.InsufficientData);
           
            fdp = new FuzzedDataProviderCS(testArr4, false);
            result = fdp.ConsumeDateTime(
                min : new DateTime(1992, 1, 5), max : new DateTime(2020, 2, 3));
            Assert.AreEqual(result.Year, 1998);
            Assert.AreEqual(true, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr1, false);
            result = fdp.ConsumeDateTime(
                min : new DateTime(1992, 1, 5), max : new DateTime(2020, 2, 3));
            Assert.AreEqual(result.Year, 1994);
            Assert.AreEqual(false, fdp.InsufficientData);

            fdp = new FuzzedDataProviderCS(testArr0, false);
            result = fdp.ConsumeDateTime(
                min : new DateTime(1992, 1, 5), max : new DateTime(2020, 2, 3));
            Assert.AreEqual(result.Year, 1992);
            Assert.AreEqual(false, fdp.InsufficientData);
        }
    
      
    }
}
