using System.ComponentModel;
using System.Xml.Serialization;
using XmlDeserializer.Deserializer.Implementations;

namespace DialogueEditor.BLL
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, StepExtension>? tagSteps;
        private List<StepExtension> steps;

        private string? selectedTag;
        private StepExtension selectedStep;
        private string isShowStepDialog;

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

        public StepExtension SelectedStep
        {
            get => selectedStep; set
            {
                selectedStep = value;
                OnPropertyChanged(nameof(selectedStep));
            }
        }

        public string? SelectedTag
        {
            get => selectedTag;
            set
            {
                selectedTag = value;
                // Фильтрация элементов по выбранному ключу
                //Steps = TagSteps?.Where(kv => kv.Key == selectedTag).Select(kv => kv.Value).ToList() ?? new List<StepExtension>();
                SelectedStep = TagSteps?.Where(kv => kv.Key == selectedTag).Select(kv => kv.Value).FirstOrDefault();
                if (selectedTag == null)
                    IsShowStepDialog = "Hidden";
                else
                    IsShowStepDialog = "Visible";

                OnPropertyChanged(nameof(SelectedTag));
            }
        }

        public string IsShowStepDialog
        {
            get => isShowStepDialog;
            set
            {
                isShowStepDialog = value;
                OnPropertyChanged(nameof(isShowStepDialog));
            }
        }

        public MainWindowViewModel()
        {
            IsShowStepDialog = "Hidden";
        }

        public void GetSteps(string path)
        {
            var deserializer = new Deserializer(path);
            var result = deserializer.GetAllSteps();

            var boxTagSteps = new Dictionary<string, StepExtension>();

            foreach (var step in result)
            {
                boxTagSteps.TryAdd(step.Key, new StepExtension(step.Value, step.Key, this));
            }
            TagSteps = boxTagSteps;

            SelectedTag = null;
            IsShowStepDialog = "Hidden";
        }

        public void DeleteStep(string? tag)
        {
            TagSteps.Remove(tag);
            TagSteps = CreateDictionary();
        }

        public void ChangeTag(string newTag, string oldTag)
        {
            ChangeTagInDictionary(oldTag, newTag);

            TagSteps = CreateDictionary();
            SelectedTag = newTag;
        }

        public void CreateStage()
        {
            var step = new StepExtension(this)
            {
                Tag = "dialog" + TagSteps.Count,
                Variants = new List<VariantNotify> { new VariantNotify() }
            };

            var result = CreateDictionary();
            result.Add("dialog" + TagSteps.Count, step);
            TagSteps = result;

            SelectedTag = step.Tag;
        }

        public void AddVariant(StepExtension step)
        {
            var boxVariants = new List<VariantNotify>();
            foreach (var variant in step.Variants)
            {
                boxVariants.Add(variant);
            }
            boxVariants.Add(new VariantNotify());

            step.Variants = boxVariants;
        }

        public void DeleteVariant(VariantNotify variant)
        {
            SelectedStep.Variants.Remove(variant);

            var newStep = new StepExtension(SelectedStep, SelectedStep.Tag, this);
            //newStep.Variants.Remove(variant);
            SelectedStep = newStep;

        }

        public void Save()
        {
            if (TagSteps == null)
            {
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<StepExtension>));

            // Создание потока для записи данных в файл
            using (FileStream fileStream = new FileStream("output.xml", FileMode.Create))
            {
                // Сериализация объекта в поток
                serializer.Serialize(fileStream, TagSteps.Values.ToList());
            }
        }

        private void ChangeTagInDictionary(string oldTag, string newTag)
        {
            var step = TagSteps[oldTag];
            TagSteps.Remove(oldTag);
            TagSteps.Add(newTag, step);
        }

        private Dictionary<string, StepExtension> CreateDictionary()
        {
            var result = new Dictionary<string, StepExtension>();
            foreach (var item in TagSteps)
            {
                result.Add(item.Key, item.Value);
            }

            return result;
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