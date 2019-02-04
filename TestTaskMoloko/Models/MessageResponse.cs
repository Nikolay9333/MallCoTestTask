using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TestTaskMoloko.Models
{
    public class MessageResponse
    {
        [XmlAttribute("id")] public long Id { get; set; }
        [XmlAttribute("server_id")] public long ServerId { get; set; }
        [MaxLength(256)]
        [XmlText] public string SmsText { get; set; }

        [XmlElement("error")] public string MsgErrCode { get; set; }
    }
}