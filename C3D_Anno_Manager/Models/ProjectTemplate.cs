using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
    [XmlRoot(ElementName = "SEWERNOTES")]
    public class SEWERNOTES
    {
        [XmlAttribute(AttributeName = "number")]
        public string Number { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "PAVINGNOTES")]
    public class PAVINGNOTES
    {
        [XmlAttribute(AttributeName = "number")]
        public string Number { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "GRADINGNOTES")]
    public class GRADINGNOTES
    {
        [XmlAttribute(AttributeName = "number")]
        public string Number { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "IRRIGATIONNOTES")]
    public class IRRIGATIONNOTES
    {
        [XmlAttribute(AttributeName = "number")]
        public string Number { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "WATERNOTES")]
    public class WATERNOTES
    {
        [XmlAttribute(AttributeName = "number")]
        public string Number { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "RECLAIMEDWATERNOTES")]
    public class RECLAIMEDWATERNOTES
    {
        [XmlAttribute(AttributeName = "number")]
        public string Number { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "STORMNOTES")]
    public class STORMNOTES
    {
        [XmlAttribute(AttributeName = "number")]
        public string Number { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "KeyNotes")]
    public class KeyNotes
    {
        [XmlElement(ElementName = "SEWERNOTES")]
        public List<SEWERNOTES> SEWERNOTES { get; set; }
        [XmlElement(ElementName = "PAVINGNOTES")]
        public List<PAVINGNOTES> PAVINGNOTES { get; set; }
        [XmlElement(ElementName = "GRADINGNOTES")]
        public List<GRADINGNOTES> GRADINGNOTES { get; set; }
    }

}

