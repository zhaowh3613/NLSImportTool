using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NLSImportTool.Model
{
    [XmlRoot(ElementName = "root")]
    public class Resources
    {
        public Resources()
        {
            //ResheaderList = new Resheader()[]{ };
            //DataList = new Data()[];
        }
        // [XmlElement("resheader", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        //[XmlArrayItem("resheader", typeof(Resheader))]
        //public Resheader Resheader;
        // [XmlElement("data", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        //[XmlArrayItem("data", typeof(Data))]
        //public Data Data;

        //[XmlElement("resheader", IsNullable = false)]
        //public Resheader[] ResheaderList;
        [XmlElement("data", IsNullable = false)]
        public Data[] DataList;
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Resheader
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlElement("value", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string Value { get; set; }
    }


    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Data
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlElement("value", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string Value { get; set; }
    }
}
