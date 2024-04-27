using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
using System.Text.Json;

namespace Amadon.Services
{
    internal static class PersistentData
    {
        public static IList<ItemForToc> ExpandedNodesList = new List<ItemForToc>();

        public static SearchData SearchData = new SearchData();

        public static GenericData GenericData = new GenericData();

        private static string PathPersistentData(string dataName)
        {
            return Path.Combine(StaticObjects.Parameters.ApplicationDataFolder, $"{dataName}.json");
        }



        //public static bool SetItem<T>(string storageName, string key, T data, CancellationToken cancellationToken = default)
        //{
        //    if (string.IsNullOrWhiteSpace(key) || data == null)
        //        return false;
        //    string pathLocalStorage = DataInitializer.GetLocalStorageFolder(storageName);
        //    string json = File.ReadAllText(pathLocalStorage);
        //    Dictionary<string,object> dicData = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        //    if (dicData == null) dicData = new Dictionary<string, object>();
        //    dicData.Add(key, data);
        //    json = JsonSerializer.Serialize<Dictionary<string, object>>(dicData);
        //    File.WriteAllText(pathLocalStorage, json);
        //    return true;
        //}

        //public static T? GetItem<T>(string storageName, string key)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        return default(T);

        //    string pathLocalStorage = DataInitializer.GetLocalStorageFolder(storageName);
        //    string json = File.ReadAllText(pathLocalStorage);
        //    Dictionary<string, object> dicData = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        //    if (dicData == null || !dicData.ContainsKey(key)) return default(T);
        //    dicData.TryGetValue(key, out object obj);
        //    if (obj == null) return default(T);
        //    return (T)obj;
        //}

        private static void Serialize<T>(T o, string name)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            string jsonString = JsonSerializer.Serialize<T>(o, options);
            File.WriteAllText(PathPersistentData(name), jsonString);
        }

        private static T DeSerialize<T>(string name) where T : new()
        {
            StaticObjects.Logger.Info($"»»»» Deserialize persistent object {name}");
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };

            string filePath = PathPersistentData(name);
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            return new T();
        }


        /// <summary>
        /// Serialize the persistent objects
        /// </summary>
        public static void Serialize()
        {
            try
            {
                Serialize<IList<ItemForToc>>(ExpandedNodesList, "ExpandedNodesList");
                Serialize<SearchData>(SearchData, "SearchData");
                Serialize<GenericData>(GenericData, "GenericData");
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("Serialize persistent objects failure", ex);
            }
        }

        /// <summary>
        /// Deserialize persistent objects
        /// </summary>
        /// <returns></returns>
        public static bool Deserialize()
        {
            try
            {
                ExpandedNodesList = (IList<ItemForToc>)DeSerialize<List<ItemForToc>>("ExpandedNodesList");
                SearchData = DeSerialize<SearchData>("SearchData");
                GenericData= DeSerialize<GenericData>("GenericData");
                return true;
            }
            catch (Exception ex)
            {
                ExpandedNodesList= (IList<ItemForToc>)new List<ItemForToc>();
                StaticObjects.Logger.Error("»»»» Deserialize persistent objects failure, returning default", ex);
                return false;
            }
        }



    }
}
