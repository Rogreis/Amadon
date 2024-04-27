using System;
using System.Collections.Generic;
using System.Text;

namespace AmadonStandardLib.InterchangeData
{
    public class InitResult : InterchangeDataBase
    {

        public bool LoggerOk { get; set; } = false;
        public bool ParameterOk { get; set; } = false;
        public bool TranslationsOk { get; set; } = false;
        public bool TranslationsOnly { get; set; } = false;
    }
}
