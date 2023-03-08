using System.ComponentModel;
using System.Xml.Serialization;
using XmlDeserializer.Models;

namespace DialogueEditor.BLL
{
    [XmlType(nameof(Step))]
    public class StepExtension : INotifyPropertyChanged
    {
        private readonly MainWindowViewModel _model;
        private string tag;
        private List<VariantNotify> variants;

        [XmlAttribute("key")]
        public string Tag
        {
            get => tag;
            set
            {
                if (tag != null)
                {
                    _model.ChangeTag(value, tag);
                }
                tag = value;
                OnPropertyChanged(nameof(Tag));
            }
        }
        public string Question { get; set; }
        public string Answer { get; set; }

        [XmlArrayItem(nameof(Variant))]
        public List<VariantNotify> Variants
        {
            get => variants; set
            {
                variants = value;
                OnPropertyChanged(nameof(Variants));
            }
        }

        public StepExtension() { }

        public StepExtension(MainWindowViewModel model)
        {
            _model = model;
        }

        public StepExtension(IStep step, string tag, MainWindowViewModel model)
        {
            _model = model;

            Tag = tag;

            Question = step.Question;
            Answer = step.Answer;

            Variants = step.Variants.Select(v => new VariantNotify(v)).ToList();
        }

        public StepExtension(StepExtension step, string tag, MainWindowViewModel model)
        {
            _model = model;

            Tag = tag;

            Question = step.Question;
            Answer = step.Answer;

            Variants = step.Variants.Select(v => new VariantNotify(v)).ToList();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
