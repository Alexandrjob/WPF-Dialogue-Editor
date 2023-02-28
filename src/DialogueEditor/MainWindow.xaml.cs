using DialogueEditor.BLL;
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
            viewModel.GetSteps();
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
            //var button = sender as Button;
            //var step = button.Tag;
            //var variant = button.Name;

            //viewModel.DeleteVariant((StepExtension)step);
        }
    }
}