using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AmadonStandardLib.InterchangeData
{
    /// <summary>
    /// Implements the concept of user notes
    /// </summary>
    public class UserNote
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();

        public TOC_Entry Entry { get; set; } = new TOC_Entry();

        public string Title { get; set; } = "no title";

        public string Notes { get; set; } = "";

        /// <summary>
        /// Property to be shown in the grid
        /// </summary>
        [JsonIgnore]
        public string Reference
        {
            get
            {
                return Entry.Reference;
            }
            set
            {
                Entry= TOC_Entry.FromHref(value);
            }
        }
        public UserNote() 
        {
            Entry = StaticObjects.Parameters.Entry;
        }
    }

    public class UserNotes
    {
        public List<UserNote> Notes { get; set; } = new List<UserNote>();

    }

}
