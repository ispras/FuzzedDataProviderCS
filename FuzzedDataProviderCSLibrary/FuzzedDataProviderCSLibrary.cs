using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Int16 ConsumeInt16(Int16 min = Int16.MinValue, Int16 max = Int16.MaxValue)
        {
            Int16 result;
            int step = sizeof(Int16);
            if (CheckIfEnoughData(step))
            {
                var toBeConverted = _data.AsSpan(_offset, step);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadInt16BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadInt16LittleEndian(toBeConverted);
            }
            else
            {
                Span<byte> toBeConverted = stackalloc byte[step];
                toBeConverted.Fill(0x00);
                _data.AsSpan(_offset).CopyTo(toBeConverted);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadInt16BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadInt16LittleEndian(toBeConverted);
            }
            Advance(step);
            if (min != Int16.MinValue || max != Int16.MaxValue)
                result = (Int16)(Math.Abs(result % (max - min + 1)) + min); //https://stackoverflow.com/a/11853619
            return result;
        }

        public UInt16 ConsumeUInt16(UInt16 min = UInt16.MinValue, UInt16 max = UInt16.MaxValue)
        {
            UInt16 result;
            int step = sizeof(UInt16);
            if (CheckIfEnoughData(step))
            {
                var toBeConverted = _data.AsSpan(_offset, step);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadUInt16BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadUInt16LittleEndian(toBeConverted);
            }
            else
            {
                Span<byte> toBeConverted = stackalloc byte[step];
                toBeConverted.Fill(0x00);
                _data.AsSpan(_offset).CopyTo(toBeConverted);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadUInt16BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadUInt16LittleEndian(toBeConverted);
            }
            Advance(step);
            if (min != UInt16.MinValue || max != UInt16.MaxValue)
                result = (UInt16)(result % (max - min + 1) + min);
            return result;
        }    
        
        public Int64 ConsumeInt64(Int64 min = Int64.MinValue, Int64 max = Int64.MaxValue)
        {
            Int64 result;
            int step = sizeof(Int64);
            if (CheckIfEnoughData(step))
            {
                var toBeConverted = _data.AsSpan(_offset, step);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadInt64BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadInt64LittleEndian(toBeConverted);
            }
            else
            {
                Span<byte> toBeConverted = stackalloc byte[step];
                toBeConverted.Fill(0x00);
                _data.AsSpan(_offset).CopyTo(toBeConverted);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadInt64BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadInt64LittleEndian(toBeConverted);
            }
            Advance(step);
            if (min != Int64.MinValue || max != Int64.MaxValue)
                result = Math.Abs(result % (max - min + 1)) + min;
            return result;
        }

        public UInt64 ConsumeUInt64(UInt64 min = UInt64.MinValue, UInt64 max = UInt64.MaxValue)
        {
            UInt64 result;
            int step = sizeof(UInt64);
            if (CheckIfEnoughData(step))
            {
                var toBeConverted = _data.AsSpan(_offset, step);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadUInt64BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadUInt64LittleEndian(toBeConverted);
            }
            else
            {
                Span<byte> toBeConverted = stackalloc byte[step];
                toBeConverted.Fill(0x00);
                _data.AsSpan(_offset).CopyTo(toBeConverted);
                result = IsLittleEndian ?
                    BinaryPrimitives.ReadUInt64BigEndian(toBeConverted) :
                    BinaryPrimitives.ReadUInt64LittleEndian(toBeConverted);
            }
            Advance(step);
            if (min != UInt64.MinValue || max != UInt64.MaxValue)
                result = result % (max - min + 1) + min;
            return result;
        }
    
        public Byte ConsumeByte(Byte min = Byte.MinValue, Byte max = Byte.MaxValue)
        {
            Byte result;
            int step = sizeof(Byte);
            result = CheckIfEnoughData(step) ? _data[_offset] : Byte.MinValue;          
            Advance(step);
            if (min != byte.MinValue || max != Byte.MaxValue)
                result = (Byte)(Math.Abs(result % (max - min + 1)) + min);
            return result;
        }

        public Char ConsumeChar(HashSet<Char> ? bagOfChars = null)
        {
            Char result;
            int step = sizeof(Char);
            Span<byte> toBeConverted = stackalloc byte[step];
            toBeConverted.Fill(0x00);

            if (CheckIfEnoughData(step))
                _data.AsSpan(_offset, step).CopyTo(toBeConverted);
            else
                _data.AsSpan(_offset).CopyTo(toBeConverted);                
            if (IsLittleEndian)
                    toBeConverted.Reverse();
            result = BitConverter.ToChar(toBeConverted);
            Advance(step);            
            if (bagOfChars is null || bagOfChars.Count == 0)
                return result;
            else
                return bagOfChars.ElementAt((Int32)(result) % bagOfChars.Count);
        }
    }  
}