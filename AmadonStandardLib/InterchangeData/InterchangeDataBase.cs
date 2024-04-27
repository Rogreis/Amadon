using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using static Lucene.Net.Queries.Function.ValueSources.MultiFunction;

namespace AmadonStandardLib.InterchangeData
{
    public abstract class InterchangeDataBase
    {
        // Output data
        public string ErrorMessage { get; set; } = "";  // Anything != string.Empty is an error

        #region MyRegion
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            WriteIndented = true,
        };


        public static string Serialize<T>(T obj)
        {
            var jsonString = JsonSerializer.Serialize(obj, options);
            return jsonString;
        }


        public static T? Deserialize<T>(string jsonString) where T : InterchangeDataBase
        {
            if (jsonString == null)
                return default(T);
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }

        public static void DumpProperties(InterchangeDataBase obj)
        {
            LibraryEventsControl.FireSendMessage("");
            LibraryEventsControl.FireSendMessage("»»»» Properties dump: »»»»");
            if (obj == null)
            {
                LibraryEventsControl.FireSendMessage("null object");
                return;
            }

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);
                string xx= value.GetType().ToString();

                if (value.GetType().ToString().StartsWith("System.Collections.Generic.List`1[AmadonStandardLib.InterchangeData.SearchResult"))
                {
                    LibraryEventsControl.FireSendMessage("Search results:");
                    foreach (SearchResult o in (List<SearchResult>)value)
                    {
                        LibraryEventsControl.FireSendMessage($"   {o}");
                    }
                } else if (property.PropertyType.GetInterfaces().ToList().Contains(typeof(IList)))
                {
                    LibraryEventsControl.FireSendMessage("List results:");
                    foreach (object o in (IList)value)
                    {
                        LibraryEventsControl.FireSendMessage($"   {o}");
                    }
                }
                else
                {
                    LibraryEventsControl.FireSendMessage(property.Name + ": " + value);
                }
            }
        }


        #endregion
    }
}
