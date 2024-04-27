using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmadonStandardLib.Classes
{
    /// <summary>
    /// Used to create and show a table of contents
    /// </summary>
    public class ItemForToc
    {
        public TOC_Entry? Entry { get; set; }
        public string Text { get; set; } = "";
        public bool Expanded { get; set; } = false;

        public ItemForToc[] Children
        {
            get
            {
                return WorkChildren.ToArray();
            }
        }
        public List<ItemForToc> WorkChildren = new List<ItemForToc>();



        public override string ToString()
        {
            return $"{Text}{WorkChildren?.ToList().Count}";
        }
    }

 }
