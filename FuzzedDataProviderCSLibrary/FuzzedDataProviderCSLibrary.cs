﻿using System;
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

        /// <summary>
        /// Consume 4 (sizeof(Int32)) bytes and parse it into Int32. If there is not enough data, empties will be filled with zeroes.
        /// </summary>
        /// <param name="min">
        /// Minimal value of possible Range.
        /// </param>
        /// <param name="max">
        /// Maximal value of possible Range.
        /// </param>
        /// <returns>
        /// Returns Int32.
        /// </returns>
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
                if (_offset < _length)
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

        /// <summary>
        /// Consume 4 (sizeof(UInt32)) bytes and parse it into UInt32. If there is not enough data, empties will be filled with zeroes.
        /// </summary>
        /// <param name="min">
        /// Minimal value of possible Range.
        /// </param>
        /// <param name="max">
        /// Maximal value of possible Range.
        /// </param>
        /// <returns>
        /// Returns UInt32.
        /// </returns>
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
                if (_offset < _length)
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

        /// <summary>
        /// Consume 2 (sizeof(Int16)) bytes and parse it into Int16. If there is not enough data, empties will be filled with zeroes.
        /// </summary>
        /// <param name="min">
        /// Minimal value of possible Range.
        /// </param>
        /// <param name="max">
        /// Maximal value of possible Range.
        /// </param>
        /// <returns>
        /// Returns Int16.
        /// </returns>
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
                if (_offset < _length)
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

        /// <summary>
        /// Consume 2 (sizeof(UInt16)) bytes and parse it into UInt16. If there is not enough data, empties will be filled with zeroes.
        /// </summary>
        /// <param name="min">
        /// Minimal value of possible Range.
        /// </param>
        /// <param name="max">
        /// Maximal value of possible Range.
        /// </param>
        /// <returns>
        /// Returns UInt16.
        /// </returns>
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
                if (_offset < _length)
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

        /// <summary>
        /// Consume 8 (sizeof(Int64)) bytes and parse it into Int64. If there is not enough data, empties will be filled with zeroes.
        /// </summary>
        /// <param name="min">
        /// Minimal value of possible Range.
        /// </param>
        /// <param name="max">
        /// Maximal value of possible Range.
        /// </param>
        /// <returns>
        /// Returns Int64.
        /// </returns>
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
                if (_offset < _length)
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

        /// <summary>
        /// Consume 8 (sizeof(UInt64)) bytes and parse it into UInt64. If there is not enough data, empties will be filled with zeroes.
        /// </summary>
        /// <param name="min">
        /// Minimal value of possible Range.
        /// </param>
        /// <param name="max">
        /// Maximal value of possible Range.
        /// </param>
        /// <returns>
        /// Returns UInt64.
        /// </returns>
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
                if (_offset < _length)
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

        /// <summary>
        /// Consume 1 (sizeof(Byte)) byte and parse it into Byte. If there is not enough data, empties will be filled with zeroes.
        /// </summary>
        /// <param name="min">
        /// Minimal value of possible Range.
        /// </param>
        /// <param name="max">
        /// Maximal value of possible Range.
        /// </param>
        /// <returns>
        /// Returns Byte.
        /// </returns>
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

        /// <summary>
        /// Consume 2 (sizeof(Char)) bytes and parse it into Char. 
        /// Unicode encoding is used, BE or LE is determined automatically using BitConverter.IsLittleEndian. 
        /// The goal is to give the user ability to construct fuzzinag sample all the time in a constant manner. 
        /// If there is not enough data, empties will be filled with zeroes.
        /// </summary>
        /// <param name="bagOfChars">
        /// Possible values of chars. All the incoming chars will me mapped to this set.
        /// </param>       
        /// <returns>
        /// Returns Char.
        /// </returns>
        public Char ConsumeChar(HashSet<Char>? bagOfChars = null)
        {            
            int step = sizeof(Char);
            Span<Char> result = stackalloc Char[1];
            Span<Byte> toBeConverted = stackalloc Byte[step];
            toBeConverted.Fill(0x00); 

            if (CheckIfEnoughData(step))
                _data.AsSpan(_offset, step).CopyTo(toBeConverted);
            else if (_offset < _length)
                _data.AsSpan(_offset).CopyTo(toBeConverted);

            if (IsLittleEndian)
                Encoding.BigEndianUnicode.GetChars(toBeConverted, result); 
            else            
                Encoding.Unicode.GetChars(toBeConverted, result); // BitConverter.ToChar(toBeConverted);
            Advance(step);

            if (bagOfChars is null || bagOfChars.Count == 0)
                return result[0];
            else
                return bagOfChars.ElementAt((Int32)(result[0]) % bagOfChars.Count);
        }
        
        private Byte[] _ConsumeBytes(
            Int32 ? length = 0, Byte min = Byte.MinValue, Byte max = Byte.MaxValue)
        {
            int step = length is null ? 
                _data.Length - _offset : 
                sizeof(Byte) * (Int32)length;

            var result = new Byte[step];
            Array.Copy(_data, _offset, result, 0,
                CheckIfEnoughData(step) ? step : _data.Length - _offset);
            Advance(step);
            if (min != byte.MinValue || max != Byte.MaxValue)
                for (int i = 0; i < result.Length; i++)
                    result[i] = (Byte)(Math.Abs(result[i] % (max - min + 1)) + min);
            return result;
        }
       
        /// <summary>
        /// Consume bytes. If there is not enough data, empties will be filled with zeroes.
        /// </summary>
        /// <param name="length">
        /// Consume Byte array of predefined length.
        /// </param>
        /// <param name="min">
        /// Minimal value of possible Range.
        /// </param>
        /// <param name="max">
        /// Maximal value of possible Range.
        /// </param>      
        /// <returns>
        /// Returns Byte[].
        /// </returns>
        public Byte[] ConsumeBytes(
            Int32 length = 0, Byte min = Byte.MinValue, Byte max = Byte.MaxValue) =>
                _ConsumeBytes(length, min, max);        

        /// <summary>
        /// Consume all the remaining data as bytes. 
        /// </summary>      
        /// <param name="min">
        /// Minimal value of possible Range.
        /// </param>
        /// <param name="max">
        /// Maximal value of possible Range.
        /// </param>      
        /// <returns>
        /// Returns Byte[].
        /// </returns>
        public Byte[] ConsumeRemainingBytes(
            Byte min = Byte.MinValue, Byte max = Byte.MaxValue) =>
                _ConsumeBytes(null, min, max);

        private String _ConsumeString(
            Int32 ? length = 0, HashSet<Char>? bagOfChars = null)
        {
            int step = length is null ?
                (_data.Length - _offset) + ((_data.Length - _offset) % sizeof(Char) == 0 ? 0 : 1) : 
                sizeof(Char) * (int)length;
            var toBeConverted = ConsumeBytes(step);            
            Advance(step);

            var strBuilder = IsLittleEndian ? 
                new StringBuilder(Encoding.BigEndianUnicode.GetString(toBeConverted)) :
                new StringBuilder(Encoding.Unicode.GetString(toBeConverted));

            if (!(bagOfChars is null) && bagOfChars.Count != 0)
                for (int i = 0; i < strBuilder.Length; i++)
                    strBuilder[i] = bagOfChars.ElementAt(
                        (Int32)(strBuilder[i]) % bagOfChars.Count);
                        
            return strBuilder.ToString();
        }

        /// <summary>
        /// Consume string.
        /// Unicode encoding is used, BE or LE is determined automatically using BitConverter.IsLittleEndian. 
        /// The goal is to give the user ability to construct fuzzinag sample all the time in a constant manner. 
        /// If there is not enough data, empties will be filled with zeroes. 
        /// </summary>
        /// <param name="length">
        /// Consume Byte array of predefined length.
        /// </param>
        /// <param name="bagOfChars">
        /// Possible values of chars. All the incoming chars will me mapped to this set.
        /// </param>      
        /// <returns>
        /// Returns String.
        /// </returns>
        public String ConsumeString(
            Int32 length = 0, HashSet<Char>? bagOfChars = null)=>
                _ConsumeString(length, bagOfChars);

        /// <summary>
        /// Consume all the remaining data as string.
        /// Unicode encoding is used, BE or LE is determined automatically using BitConverter.IsLittleEndian. 
        /// The goal is to give the user ability to construct fuzzinag sample all the time in a constant manner. 
        /// If there is not enough data, empties will be filled with zeroes.       
        /// <param name="bagOfChars">
        /// Possible values of chars. All the incoming chars will me mapped to this set.
        /// </param>      
        /// <returns>
        /// Returns String.
        /// </returns>
        public String ConsumeRemainingAsString(HashSet<Char>? bagOfChars = null) =>
                _ConsumeString(null, bagOfChars);
        
        /// <summary>
        /// Consume 4 bytes and cast it to a enum<T> value. It will be mapped into enum<T>  values (not to an random int).        
        /// If there is not enough data, empties will be filled with zeroes. 
        /// <returns>
        /// Returns Int32 value of enum<T> element.
        /// </returns>
        public Int32 ConsumeEnum<T> () where T : Enum
        {
            Int32 result = ConsumeInt32();
            var values = Enum.GetValues(typeof(T));
            return (Int32)values.GetValue(result % values.Length);
        }

        /// <summary>
        /// Consume 8 bytes (sizeof(Double)) bytes and parse it into Double.
        /// If there is not enough data, empties will be filled with zeroes. 
        /// <returns>
        /// Returns Double.
        /// </returns>
        public Double ConsumeDouble () 
        {
            Double result;
            int step = sizeof(Double);
            if (CheckIfEnoughData(step))
            {
                var toBeConverted = _data.AsSpan(_offset, step);
                result = BitConverter.ToDouble(toBeConverted);
            }
            else
            {
                Span<byte> toBeConverted = stackalloc byte[step];
                toBeConverted.Fill(0x00);
                if (_offset < _length)
                    _data.AsSpan(_offset).CopyTo(toBeConverted);
                result = BitConverter.ToDouble(toBeConverted);
            }
            Advance(step);           
            return result;
        }

        /// <summary>
        /// Consume 8 bytes (sizeof(Int64)) bytes and parse it into DateTime.
        /// If there is not enough data, empties will be filled with zeroes. 
        /// <returns>
        /// Returns DateTime.
        /// </returns>
        public DateTime ConsumeDateTime() =>
            ConsumeDateTime(DateTime.MinValue, DateTime.MaxValue);

        /// <summary>
        /// Consume 8 bytes (sizeof(Int64)) bytes and parse it into DateTime.
        /// If there is not enough data, empties will be filled with zeroes.
        /// <param name="min">
        /// Minimal value of possible Range.
        /// </param>
        /// <param name="max">
        /// Maximal value of possible Range.
        /// </param>       
        /// <returns>
        /// Returns DateTime.
        /// </returns>
        public DateTime ConsumeDateTime(DateTime min, DateTime max) 
        {
            Int64 toBeConverted = ConsumeInt64();
            Advance(sizeof(Int64));           

            Int64 minDTTics = Math.Max(DateTime.MinValue.Ticks, min.Ticks); 
            Int64 maxDTTics = Math.Min(DateTime.MaxValue.Ticks, max.Ticks); 
                        
            return new DateTime(
                Math.Abs(toBeConverted % (maxDTTics - minDTTics + 1)) + minDTTics);
        }
    }
}