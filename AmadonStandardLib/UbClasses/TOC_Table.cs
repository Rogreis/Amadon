using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadonStandardLib.UbClasses
{
    public class TOC_Table
    {
        public string Title { get; set; } = "";

        public List<TOC_Entry> Parts { get; set; } = new List<TOC_Entry>();
    }
}
