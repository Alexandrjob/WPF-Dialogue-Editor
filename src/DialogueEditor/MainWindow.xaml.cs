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

        private void Button_Click_Open_File(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            

            if (openFileDialog.ShowDialog() != true)
                return;

            if (viewModel.TagSteps != null)
            {
                var message =
                    "В данный момент вы уже работаете с файлом. Вы уверены что хотите открыть другой файл? Все изменения будут утеряны";
                var title = "Открытие файла";
                var button = MessageBoxButton.OKCancel;
                var icon = MessageBoxImage.Warning;

                var result = ShowMessage(message, title, button, icon);
                if (result != MessageBoxResult.OK)
                    return;
            }

            try
            {
                viewModel.GetSteps(openFileDialog.FileName);
            }
            catch (System.Exception)
            {
                ShowMessage("Ошибка чтения файла", "Выбран не тот файл или файл содержит ошибки",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private MessageBoxResult ShowMessage(string title, string message, MessageBoxButton button,
            MessageBoxImage icon)
        {
            return MessageBox.Show(message, title, button, icon, MessageBoxResult.Yes);
        }

        #region Events

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

            viewModel.AddVariant((StepExtension) step);
        }

        private void Button_Click_Delete_Variant(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var varuant = button.Tag;


            viewModel.DeleteVariant((VariantNotify) varuant);
        }

        private void Button_Click_Create_File(object sender, RoutedEventArgs e)
        {
        }

        #endregion
    }
}