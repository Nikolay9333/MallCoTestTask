using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TestTaskMoloko.Models
{
    public class PackageResponse
    {
        [XmlArray("send")] public List<MessageResponse> Messages { get; set; }

        [MaxLength(256)] [XmlElement("error")] public string ErrCode { get; set; }
    }
}