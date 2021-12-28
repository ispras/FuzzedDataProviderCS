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
        private char[] _data;
        private int _length;
        private bool _exitAppOnInsufficientData;

        /// <param name="exitAppOnInsufficientData">
        /// If set to true, the fuzzing iteration will be finished when the fuzzed data is over.
        /// </param>
        public FuzzedDataProviderCS(char[] data, int length, bool exitAppOnInsufficientData = false)
        {
            _data = data;
            _length = length;
            _exitAppOnInsufficientData = exitAppOnInsufficientData;
        }

        /// <summary>
        /// This property tells you if fuzzed data is over, also finishes the fuzzing iteration if exitAppOnInsufficientData is True.        
        /// </summary>
        /// <returns>
        /// If fuzzed data is over, but you ask for more, it returns True, otherwise False.
        /// </returns>
        public bool InsufficientData
        {
            get => InsufficientData;
            private set
            {
                InsufficientData = value;
                if (_exitAppOnInsufficientData && InsufficientData)
                    System.Environment.Exit(0);
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
        private bool Check(int step) =>
            _length + step >= _data.Length ? false : true;

        /// <summary>
        /// Moves pointer forth and set InsufficientData to True when reaches the end of the data.
        /// </summary>
        /// <param name="step">
        /// Step of the splitting pointer (in chars).
        /// </param>        
        private void Advance(int step)
        {
            _length += step;
            if (_length >= _data.Length)
                InsufficientData = true;
        }

        public T ConsumeSimpleType<T>()
        {
            https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types
            https://stackoverflow.com/questions/828807/what-is-the-base-class-for-c-sharp-numeric-value-types
            var t = typeof(T);
            return (T)t;
        }
    }
}