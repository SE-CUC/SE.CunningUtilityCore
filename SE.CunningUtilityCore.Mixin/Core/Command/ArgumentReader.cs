using System;

namespace IngameScript
{
    public struct ArgumentReader
    {
        private readonly string _input;
        private int _cursor;

        public ArgumentReader(string input)
        {
            _input = input ?? "";
            _cursor = 0;
        }

        public string Next()
        {
            if (_cursor >= _input.Length)
                return null;

            while (_cursor < _input.Length && char.IsWhiteSpace(_input[_cursor]))
                _cursor++;

            if (_cursor >= _input.Length)
                return null;

            int start = _cursor;
            bool inQuotes = false;
            
            if (_input[_cursor] == '"')
            {
                inQuotes = true;
                inQuotes = true;
                start++;
                _cursor++;
            }

            while (_cursor < _input.Length)
            {
                if (inQuotes)
                {
                    if (_input[_cursor] == '"')
                    {
                        inQuotes = false;
                        int length = _cursor - start;
                        _cursor++;
                        return _input.Substring(start, length);
                    }
                }
                else
                {
                    if (char.IsWhiteSpace(_input[_cursor]))
                    {
                        return _input.Substring(start, _cursor++ - start);
                    }
                }
                _cursor++;
            }

            if (inQuotes)
            {
                return _input.Substring(start);
            }

            return _input.Substring(start);
        }

        public string Rest()
        {
            if (_cursor >= _input.Length)
                return "";
            
            while (_cursor < _input.Length && char.IsWhiteSpace(_input[_cursor]))
                _cursor++;
            
            return _input.Substring(_cursor);
        }
    }
}
