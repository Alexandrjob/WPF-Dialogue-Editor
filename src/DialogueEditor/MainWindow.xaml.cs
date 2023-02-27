using DialogueEditor.BLL;
using System.Windows;

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
    }
}