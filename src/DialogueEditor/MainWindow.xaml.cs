using DialogueEditor.BLL;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace DialogueEditor
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel(); 
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Open_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            MessageBoxResult result = MessageBoxResult.Cancel;

            if (viewModel.TagSteps !=  null)
            {
                string messageBoxText = "В данный момент вы уже работаете с файлом. Вы уверены что хотите открыть другой файл? Все изменения будут утеряны";
                string caption = "Открытие файла";
                MessageBoxButton button = MessageBoxButton.OKCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                if (result != MessageBoxResult.OK)
                    return;
            }

            

            try
            {
                viewModel.GetSteps(openFileDialog.FileName);
            }
            catch (System.Exception)
            {

                string messageBoxText = "Выбран не тот файл или файл содержит ошибки";
                string caption = "Ошибка чтения файла";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;

                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }

        private void Button_Click_Add_Step(object sender, RoutedEventArgs e)
        {
            viewModel.CreateStage();
        }

        private void Button_Click_Save_In_File(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
        }

        private void Button_Click_Delete_Step(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string tag = button.Tag.ToString();

            viewModel.DeleteStep(tag);
        }

        private void Button_Click_Add_Variant(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var step = button.Tag;

           viewModel.AddVariant((StepExtension)step);
        }

        private void Button_Click_Delete_Variant(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var varuant = button.Tag;
            

            viewModel.DeleteVariant((VariantNotify)varuant);
        }
    }
}