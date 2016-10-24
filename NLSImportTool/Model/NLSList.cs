using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLSImportTool.Model
{
    public class NLS
    {
        public string Name { get; set; }
        public string ISOCode { get; set; }
        public NLS(string name, string isoCode)
        {
            this.Name = name;
            this.ISOCode = isoCode;
        }
    }

    public class NLSList
    {
        private static NLSList _instance;

        public static NLSList Instance
        {
            get
            {
                return _instance ?? (_instance = new NLSList());
            }
        }

        public List<NLS> List { get; set; }
        public NLSList()
        {
            List = new List<NLS>();
            List.Add(new NLS("AR", "ar"));
            List.Add(new NLS("PT-BR", "pt-BR"));
            List.Add(new NLS("BR", "pt-BR"));
            List.Add(new NLS("CR", "hr"));
            List.Add(new NLS("CZ", "cs"));
            List.Add(new NLS("DK", "da"));
            List.Add(new NLS("FI", "fi"));
            List.Add(new NLS("FR", "fr"));
            List.Add(new NLS("GK", "el"));
            List.Add(new NLS("GR", "de"));
            List.Add(new NLS("HE", "he"));
            List.Add(new NLS("HU", "hu"));
            List.Add(new NLS("IT", "it"));
            List.Add(new NLS("JP", "ja"));
            List.Add(new NLS("KR", "ko"));
            List.Add(new NLS("NL", "nl"));
            List.Add(new NLS("NO", "nb"));
            List.Add(new NLS("PL", "pl"));
            List.Add(new NLS("PO", "pt"));
            List.Add(new NLS("RO", "ro"));
            List.Add(new NLS("RU", "ru"));
            List.Add(new NLS("SC", "zh-Hans"));
            List.Add(new NLS("SH", "sr-Latn"));
            List.Add(new NLS("SK", "sk"));
            List.Add(new NLS("SL", "sl"));
            List.Add(new NLS("SP", "es"));
            List.Add(new NLS("SV", "sv"));
            List.Add(new NLS("TC", "zh-Hant"));
            List.Add(new NLS("TU", "tr"));
            List.Add(new NLS("TR", "tr"));
            List.Add(new NLS("UK", "uk"));
            List.Add(new NLS("UA", "uk"));
        }
    }
}
