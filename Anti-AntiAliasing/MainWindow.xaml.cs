using System.Windows;
using System.Windows.Forms;

namespace Anti_AntiAliasing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnProcessImage_Click(object sender, RoutedEventArgs e)
        {
            BusinessLayer.GetListOfFiles();
            BusinessLayer.ProcessImages();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Store the file path in the filePath string variable in the Data class
                    Data.FilePath = fbd.SelectedPath;
                }

                //Visual display of the string variable when the button is clicked
                tBoxFilePath.Text = Data.FilePath;
            }
        }
    }
}