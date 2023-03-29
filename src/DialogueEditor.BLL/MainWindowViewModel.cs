using System.ComponentModel;
using System.Xml.Serialization;
using XmlDeserializer.Deserializer.Implementations;

namespace DialogueEditor.BLL
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, StepExtension>? tagSteps;

        private string? selectedTag;
        private StepExtension selectedStep;

        private string isShowStepDialog;

        private string _path;

        public Dictionary<string, StepExtension>? TagSteps
        {
            get => tagSteps;

            set
            {
                tagSteps = value;
                OnPropertyChanged(nameof(TagSteps));
            }
        }

        public StepExtension SelectedStep
        {
            get => selectedStep;
            set
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
            _path = path;

            TagSteps = DeserializeSteps(_path);

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
                Variants = new List<VariantNotify> {new VariantNotify()}
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
            SelectedStep = newStep;
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

        public void Save()
        {
            if (TagSteps == null)
                return;

            XmlSerializer serializer = new XmlSerializer(typeof(List<StepExtension>));

            using (FileStream fileStream = new FileStream(_path, FileMode.Create))
            {
                serializer.Serialize(fileStream, TagSteps.Values.ToList());
            }
        }

        public void CreateFile()
        {
            var newSteps = new List<StepExtension>()
            {
                new StepExtension()
                {
                    Question = "Слова детектива",
                    Answer = "Слова npc",
                    Tag = "dialog_1",
                    Variants = new List<VariantNotify>()
                    {
                        new VariantNotify()
                        {
                            Question = "Слова детектива",
                            Answer = "Слова npc"
                        }
                    }
                }
            };

            var nameFile = "dialog.xml";
            var path = AppDomain.CurrentDomain.BaseDirectory + nameFile;

            var serializer = new XmlSerializer(typeof(List<StepExtension>));
            
            if(File.Exists(path))
            {
                path = path.Remove(path.Length - nameFile.Length, nameFile.Length);
                
                var rnd = new Random();
                path = path + "dialog" + rnd.Next(100000, 2000000) + ".xml";
            }

            _path = path; 
            
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fileStream, newSteps);
                fileStream.Close();
            }

            TagSteps = DeserializeSteps(path);

            SelectedTag = null;
            IsShowStepDialog = "Hidden";
        }

        private Dictionary<string, StepExtension> DeserializeSteps(string path)
        {
            var deserializer = new Deserializer(path);
            var result = deserializer.GetAllSteps();
            deserializer.Dispose();
            
            var tagSteps = new Dictionary<string, StepExtension>();

            foreach (var step in result)
            {
                tagSteps.TryAdd(step.Key, new StepExtension(step.Value, step.Key, this));
            }

            return tagSteps;
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