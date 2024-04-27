using AmadonStandardLib.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmadonStandardLib.InterchangeData
{
    public class SettingsData : InterchangeDataBase
    {
        // Input and output data
        public Parameters? Param { get; set; } = null;
    }
}
