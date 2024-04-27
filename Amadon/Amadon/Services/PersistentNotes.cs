using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
using System.Text.Json;
using System.Text.RegularExpressions;
using static Lucene.Net.Search.FieldCache;

namespace Amadon.Services
{
    internal class PersistentNotes
    {
        private const string NotesExtension = ".notes";

        public static UserNotes NotesList = new UserNotes();

        public static List<string> NoteFilesList = new();


        private static string PathPersistentData(string notesName)
        {
            return Path.Combine(StaticObjects.Parameters.ApplicationDataFolder, $"{notesName}{NotesExtension}");
        }

        private static void Serialize<T>(T o, string notesName)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
            string jsonString = JsonSerializer.Serialize<T>(o, options);
            File.WriteAllText(PathPersistentData(notesName), jsonString);
        }

        private static T DeSerialize<T>(string notesName) where T : new()
        {
            StaticObjects.Logger.Info($"»»»» Deserialize persistent object {notesName}");
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };

            string filePath = PathPersistentData(notesName);
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
        public static void Serialize(string notesName)
        {
            try
            {
                StaticObjects.Logger.Info($"»»»» Serialize persistent object {notesName}");
                Serialize<UserNotes>(NotesList, notesName);
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
        public static bool Deserialize(string notesName)
        {
            try
            {
                if (File.Exists (PathPersistentData(notesName))) 
                {
                    NotesList = DeSerialize<UserNotes>(notesName);
                    return true;
                }
                NotesList = new();
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("»»»» Deserialize PersistentNotes objects failure, returning default", ex);
                return false;
            }
        }

        public static List<string> GetNoteFilesList()
        {
            NoteFilesList = new();
            foreach (string filePath in Directory.EnumerateFiles(StaticObjects.Parameters.ApplicationDataFolder, $"*{NotesExtension}"))
            {
                NoteFilesList.Add(Path.GetFileNameWithoutExtension(filePath));
            }
            if (NoteFilesList.Count == 0) NoteFilesList.Add("My UB Notes");
            return NoteFilesList;
        }


        public static int NotesCount()
        {
            // Regular expression pattern to match "Number " followed by 1 to 4 digits
            string pattern = @"Notes \d{1,4}\b";

            // Count occurrences
            int count = 0;
            // var allTitles = myList.Select(x => x.Title).ToList();
            foreach (string notesTitle in NotesList.Notes.FindAll(n => n.Entry.Paper == StaticObjects.Parameters.Entry.Paper).Select(n => n.Title))
            {
                if (Regex.IsMatch(notesTitle, pattern)) count++;
            }
            return count;
        }

    }
}
