using System.ComponentModel;
using XmlDeserializer.Models;
using static System.Net.Mime.MediaTypeNames;

namespace DialogueEditor.BLL
{
    public class VariantNotify : IVariant
    {
        private readonly MainWindowViewModel _model;
        private string? linkTeg;

        public string Question { get; set; }
        public string Answer { get; set; }
        public string? LinkTeg
        {
            get => linkTeg;
            set
            {
                if(linkTeg != null) 
                {
                    _model.ChangeTag(value, linkTeg);
                }
                
                linkTeg = value;
            }
        }

        public VariantNotify(IVariant variant, MainWindowViewModel model)
        {
            Question = variant.Question;
            Answer = variant.Answer;
            LinkTeg = variant.LinkTeg;

            _model = model;
        }
    }
}
