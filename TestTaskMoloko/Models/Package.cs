using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TestTaskMoloko.Models
{
    public class Package
    {
        [XmlArray("send")] public List<Message> Messages { get; set; }
        [XmlAttribute("login")] public string Login { get; set; }
        [XmlAttribute("password")] public string Password { get; set; }
    }
}