using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace TestTaskMoloko.Models
{
    public class Message
    {
        [XmlAttribute("id")] [Column("id")] public long Id { get; set; }

        [XmlAttribute("receiver")]
        [Column("receiver")]
        [MinLength(2), MaxLength(30)]
        public string Receiver { get; set; }

        [XmlAttribute("sender")]
        [Column("sender")]
        [MinLength(2), MaxLength(30)]
        public string Sender { get; set; }

        [XmlText] public string SmsText { get; set; }

        [XmlIgnore]
        [Column("is_received")]
        [DefaultValue("false")]
        [Required]
        public bool IsReceived { get; set; }
    }
}
