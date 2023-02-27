using System.ComponentModel;
using XmlDeserializer.Models;

namespace DialogueEditor.BLL
{
    public class StepExtension: INotifyPropertyChanged
    { 
        public string Question { get; set; }
        public string Answer { get; set; }

        public List<VariantNotify> Variants { get ; set ; }

        public StepExtension(IStep step, MainWindowViewModel model)
        {
            Question = step.Question;
            Answer = step.Answer;
           
            Variants = step.Variants.Select(v=> new VariantNotify(v, model)).ToList();
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
