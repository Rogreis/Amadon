using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
using AmadonStandardLib.UbClasses;

namespace Amadon.Services
{
    public class TOC_Service
    {
        private static Translation GetTranslation(short id) 
        {
            Translation trans = StaticObjects.Book.GetTranslation(id);
            if (trans == null) 
            {
                throw new Exception($"Translation not found while generating table of contents {id}");
            }
            return trans;
        }

        public static TOCdata GetToc(TOCdata data)
        {
            if (data.UpdateTocId1)
            {
                Translation trans = GetTranslation(data.TranslationId1);
                data.TocId1 = trans.TableOfContents;
                data.TitleTranslation1 = trans.Description;
            }
            return data;
        }

        public static Task<TOCdata> GetTocTable(TOCdata data)
        {
            return Task.FromResult(GetToc(data));
        }
    }
}
