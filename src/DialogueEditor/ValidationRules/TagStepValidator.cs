using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DialogueEditor.ValidationRules
{
    public class TagStepValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value == null)
            {
                return new ValidationResult(false, "Значение не может быть пустым");
            }

            var window = (MainWindow)Application.Current.MainWindow;
            var steps = window.viewModel.TagSteps;
            if(steps == null)
            {
                return new ValidationResult(true, "");
            }

            var tags = steps.Keys.ToList();

            foreach ( var tag in tags)
            {

                if (tag == null)
                {
                    continue;
                }

                if(tag == value.ToString())
                {
                    return new ValidationResult(true, "Тэг найден");
                }
            }

            return new ValidationResult(false,"");
        }
    }
}
