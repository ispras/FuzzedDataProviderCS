using System;

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
        public void TestSizesOfTypes()
        {
            Assert.AreEqual(1, sizeof(System.Byte));
            Assert.AreEqual(2, sizeof(System.Char));
            Assert.AreEqual(2, sizeof(System.UInt16));
            Assert.AreEqual(2, sizeof(System.Int16));
            Assert.AreEqual(4, sizeof(System.UInt32));
            Assert.AreEqual(4, sizeof(System.Int32));
            Assert.AreEqual(8, sizeof(System.UInt64));
            Assert.AreEqual(8, sizeof(System.Int64));
            Assert.AreEqual(8, sizeof(System.Double));
            Assert.AreEqual(16, sizeof(System.Decimal));
        }

        #region Waiting for Pose bugfix
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
            byte[] testArr4 = { 0xFF, 0xFF, 0xFE, 0xF2 };
            byte[] testArr5 = { 0xFF, 0xFF, 0xFF, 0xFF };
            byte[] testArr6 = { 0x7F, 0xFF, 0xFF, 0xFF };
            byte[] testArr7 = { 0x80, 0x00, 0x00, 0x00 };
            byte[] testArr8 = { 0x80, 0x00, 0x00, 0x01 };


            var fdp = new FuzzedDataProviderCS(testArr1, false);
            var result = fdp.ConsumeUInt32();
            Assert.AreEqual((UInt32)1, result);

            fdp = new FuzzedDataProviderCS(testArr2, false);
            result = fdp.ConsumeUInt32();
            Assert.AreEqual(3735928559, result);

            // fdp = new FuzzedDataProviderCS(testArr1, false);
            // result = fdp.ConsumeUInt32(min: -1, max: 0);
            // Assert.AreEqual(0, result);

            fdp = new FuzzedDataProviderCS(testArr0, false);
            result = fdp.ConsumeUInt32(min: 0, max: 1);
            Assert.AreEqual((UInt32)0, result);

            // fdp = new FuzzedDataProviderCS(testArr1, false);
            // result = fdp.ConsumeUInt32(min: -20, max: -17);
            // Assert.AreEqual(-19, result);

            // fdp = new FuzzedDataProviderCS(testArr3, false);
            // result = fdp.ConsumeUInt32(min: -20, max: -17);
            // Assert.AreEqual(-17, result);

            // fdp = new FuzzedDataProviderCS(testArr0, false);
            // result = fdp.ConsumeUnt32(min: 888, max: 900);
            // Assert.AreEqual(888, result);

            // fdp = new FuzzedDataProviderCS(testArr3, false);
            // result = fdp.ConsumeUInt32(min: 888, max: 900);
            // Assert.AreEqual(891, result);

            // fdp = new FuzzedDataProviderCS(testArr3, false);
            // result = fdp.ConsumeUInt32(min: 888, max: 889);
            // Assert.AreEqual(889, result);

            // fdp = new FuzzedDataProviderCS(testArr3, false);
            // result = fdp.ConsumeUInt32(min: 0, max: 2);
            // Assert.AreEqual(0, result);
            
            // fdp = new FuzzedDataProviderCS(testArr4, false);
            // result = fdp.ConsumeUInt32();
            // Assert.AreEqual(-270, result);       
            // Assert.AreEqual(false, fdp.InsufficientData); //Just for extra non-important test

            // fdp = new FuzzedDataProviderCS(testArr5, false);
            // result = fdp.ConsumeUInt32();
            // Assert.AreEqual(-1, result);

            // fdp = new FuzzedDataProviderCS(testArr5, false);
            // result = fdp.ConsumeUInt32();
            // Assert.AreEqual(-1, result);
            
            // fdp = new FuzzedDataProviderCS(testArr6, false);
            // result = fdp.ConsumeUInt32();
            // Assert.AreEqual(Int32.MaxValue, result);

            // fdp = new FuzzedDataProviderCS(testArr7, false);
            // result = fdp.ConsumeUInt32();
            // Assert.AreEqual(Int32.MinValue, result);

            // fdp = new FuzzedDataProviderCS(testArr8, false);
            // result = fdp.ConsumeUInt32(UInt32.MinValue, 0);
            // Assert.AreEqual(Int32.MinValue, result);
        }
    }
}