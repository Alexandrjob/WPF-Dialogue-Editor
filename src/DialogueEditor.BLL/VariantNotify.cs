using System.Xml.Serialization;
using XmlDeserializer.Models;

namespace DialogueEditor.BLL
{
    [XmlRoot(nameof(Variant))]
    public class VariantNotify : IVariant
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string? LinkTag { get; set; }

        public VariantNotify()
        {
            
        }

        public VariantNotify(IVariant variant)
        {
            Question = variant.Question;
            Answer = variant.Answer;
            LinkTag = variant.LinkTag;
        }
    }
}
