using System.Collections.Generic;
using System.ComponentModel;
using XmlDeserializer.Deserializer.Implementations;
using XmlDeserializer.Models;

namespace DialogueEditor.BLL
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, StepExtension>? tagSteps;
        private List<StepExtension> steps;

        private string? selectedTag;

        public Dictionary<string, StepExtension>? TagSteps
        {
            get => tagSteps;

            set
            {
                tagSteps = value;
                OnPropertyChanged(nameof(TagSteps));
            }
        }
        public List<StepExtension> Steps 
        { 
            get => steps; 
            set
            {
                steps = value;
                OnPropertyChanged(nameof(Steps));
            }
        }
        public string? SelectedTag
        {
            get => selectedTag;
            set
            {
                selectedTag = value;
                // Фильтрация элементов по выбранному ключу
                Steps = TagSteps?.Where(kv => kv.Key == selectedTag).Select(kv => kv.Value).ToList() ?? new List<StepExtension>();
                OnPropertyChanged(nameof(SelectedTag));
            }
        }

        public void GetSteps()
        {
            var deserializer = new Deserializer("Dialog.xml");
            var result = deserializer.GetAllSteps();

            var boxTagSteps = new Dictionary<string, StepExtension>();

            foreach (var step in result)
            {
                boxTagSteps.TryAdd(step.Key, new StepExtension(step.Value, this));
            }
            TagSteps = boxTagSteps;
        }

        public void ChangeTag(string newTag, string oldTag)
        {
            var step = TagSteps[oldTag];
            
            TagSteps.Remove(oldTag);
            TagSteps.Add(newTag, step);

            var result = new Dictionary<string, StepExtension>();
            foreach (var item in TagSteps)
            {
                result.Add(item.Key, item.Value);
            }
            TagSteps = result;
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