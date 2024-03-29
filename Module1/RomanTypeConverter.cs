﻿using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace DataProcessor
{
    class RomanTypeConverter:ITypeConverter
    {
        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text == "I") return 1;
            if (text == "II") return 2;
            if (text == "V") return 5;
            throw new ArgumentOutOfRangeException(nameof(text));
        }
    }
}
