using System;
using System.Collections.Generic;
using System.Text;

namespace AmadonStandardLib.Classes
{
    public class Diagnostic
    {
        public string Message { get; set; } = "";

        public bool IsError { get; set; } = false;
    }
}
