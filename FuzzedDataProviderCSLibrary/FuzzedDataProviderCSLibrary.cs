using System;
using System.Buffers.Binary;

namespace FuzzedDataProviderCSLibrary
{
    /// <summary>
    /// The goal of this class is to split fuzzed data in chunks in predictable manner.    
    /// </summary>
    /// <returns>
    /// Returns an object of FuzzedDataProviderCS.
    /// </returns>
    public class FuzzedDataProviderCS
    {
        private byte[] _data;
        private int _length;
        private int _offset = 0;
        private bool _exitAppOnInsufficientData;
        private bool _insufficientData = false;
        
        /// <param name="exitAppOnInsufficientData">
        /// If set to true, the fuzzing iteration will be finished when the fuzzed data is over.
        /// </param>
        public FuzzedDataProviderCS(byte[] data, bool exitAppOnInsufficientData = false)
        {
            _data = data;
            _length = data.Length;
            _exitAppOnInsufficientData = exitAppOnInsufficientData;
            _insufficientData = _length < 1;
        }

        public static bool IsLittleEndian
        {            
            get => BitConverter.IsLittleEndian;
        } 

        /// <summary>
        /// This property tells you if fuzzed data is over, also finishes the fuzzing iteration if exitAppOnInsufficientData is True.        
        /// </summary>
        /// <returns>
        /// If fuzzed data is over, but you ask for more, it returns True, otherwise False.
        /// </returns>
        public bool InsufficientData
        {
            get => _insufficientData;
            private set
            {
                _insufficientData = value;
                if (_exitAppOnInsufficientData && _insufficientData)
                    Environment.Exit(0);
            }
        }

        /// <summary>
        /// Check if pointer could be moved forth on step amount of bytes.
        /// </summary>
        /// <param name="step">
        /// Step of the splitting pointer (in chars).
        /// </param>
        /// <returns>
        /// Returns false in case of insufficient data for the next advance, otherwise true.
        /// </returns>
        private bool CheckIfEnoughData(int step)
        {
            if (_offset + step > _data.Length)
            {
                InsufficientData = true;
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Moves pointer forth and set InsufficientData to True when reaches the end of the data.
        /// </summary>
        /// <param name="step">
        /// Step of the splitting pointer (in chars).
        /// </param>        
        private void Advance(int step) =>
            _offset += step;

        public Int32 ConsumeInt32(Int32 min = Int32.MinValue, Int32 max = Int32.MaxValue)
        {
            Int32 result;
            int step = sizeof(Int32);
            if (CheckIfEnoughData(step))
            {
                var toBeConverted = _data.AsSpan(_offset, step);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadInt32BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadInt32LittleEndian(toBeConverted);
            }
            else
            {
                Span<byte> toBeConverted = stackalloc byte[step];
                toBeConverted.Fill(0x00);
                _data.AsSpan(_offset).CopyTo(toBeConverted);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadInt32BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadInt32LittleEndian(toBeConverted);
            }
            Advance(step);
            if (min != Int32.MinValue || max != Int32.MaxValue)
                result = Math.Abs(result % (max - min + 1)) + min;
            return result;
        }

        public UInt32 ConsumeUInt32(UInt32 min = UInt32.MinValue, UInt32 max = UInt32.MaxValue)
        {
            UInt32 result;
            int step = sizeof(UInt32);
            if (CheckIfEnoughData(step))
            {
                var toBeConverted = _data.AsSpan(_offset, step);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadUInt32BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadUInt32LittleEndian(toBeConverted);
            }
            else
            {
                Span<byte> toBeConverted = stackalloc byte[step];
                toBeConverted.Fill(0x00);
                _data.AsSpan(_offset).CopyTo(toBeConverted);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadUInt32BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadUInt32LittleEndian(toBeConverted);
            }
            Advance(step);
            if (min != UInt32.MinValue || max != UInt32.MaxValue)
                result = result % (max - min + 1) + min;
            return result;
        }    
    }  
}