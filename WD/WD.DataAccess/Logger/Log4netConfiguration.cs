using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD.DataAccess.Logger
{
    [Serializable]
    [System.Xml.Serialization.XmlRoot("configuration", Namespace = "", IsNullable = false)]
    public class Log4netConfiguration
    {
        [System.Xml.Serialization.XmlElement("log4net")]
        public Log4net Log4net { get; set; }

    }
    [Serializable]
    [System.Xml.Serialization.XmlRoot("log4net", Namespace = "", IsNullable = true)]
    public class Log4net
    {
        [System.Xml.Serialization.XmlElement("appender")]
        public List<Appender> Appenders { get; set; }
    }
    [Serializable]
    [System.Xml.Serialization.XmlRoot("appender", Namespace = "", IsNullable = true)]
    public class Appender
    {

        [System.Xml.Serialization.XmlAttribute("name")]
        public string Name { get; set; }
        [System.Xml.Serialization.XmlElement("file")]
        public FilePath File { get; set; }
    }
    [Serializable]
    [System.Xml.Serialization.XmlRoot("appender", Namespace = "", IsNullable = false)]
    public class FilePath
    {

        [System.Xml.Serialization.XmlAttribute("value")]
        public string FullPath { get; set; }
    }
}
