using Microsoft.Win32;
using System.Windows;
using Useful;

namespace FileAccessTest
{
    /// <summary>
    /// Simple application to test reading writing to files. 
    /// </summary>
    public partial class MainWindow : Window
    {
        //test filename
        //File properties for the file needs to be set to "copy..."
        private string testDummy = "./resources/slimer.txt";

        //contents of the file
        private List<string> rows = new List<string>();

        //working path for application
        private string workPath = "";

        public MainWindow()
        {
            InitializeComponent();

            ShowInfo();
        }

        /// <summary>
        /// Some extra information about where the program thinks that it is writing to
        /// </summary>
        private void ShowInfo()
        {
            //This will return the full path and include the file
            string exeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            //This will return the current working path to the current folder:           
            workPath = System.IO.Path.GetDirectoryName(exeFilePath);

            string output = "Executing location: " + exeFilePath + "\n";
            output += "Working location: " + workPath;
            
            txtLocation.Text = output;

        }

        /// <summary>
        /// Load contents of the test file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a file";
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            // Set the starting location
            openFileDialog.InitialDirectory = workPath;

            if (openFileDialog.ShowDialog() == true)
            {
                testDummy = openFileDialog.FileName;                                
            }

            string output = "";
            try
            {
                rows = UsefulFileAccess.ReadFromFile(testDummy);

                output = "Successfully read file: " + testDummy + "\n";
                output += "Number of lines: " + rows.Count.ToString();

                string fileContents = "";
                foreach (string row in rows)
                {
                    fileContents += row + "\n";
                }
                txtFileContent.Text = fileContents;

            }
            catch (Exception ex)
            {
                output = "Error reading from file: " + testDummy + "\n";
                output += ex.Message;

            }
            txtInfo.Text = output;
        }

        /// <summary>
        /// Save contents to the test file. Can overwrite or append
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //to overwrite or append?
            bool append = (cbAppend.IsChecked == true) ? true : false;

            string output = txtFileContent.Text;
            try
            {
                UsefulFileAccess.WriteToFile(testDummy, output, append);

                output = "Successfully wrote to file: " + testDummy + "\n";
                output += "Appending to file: " + append;
            }
            catch (Exception ex)
            {
                output = "Error writing to file: " + testDummy + "\n";
                output += ex.Message;

            }
            txtInfo.Text = output;
        }

        /// <summary>
        /// Close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //This is the common and recommended way to close a WPF window from within the window itself.
            Close();

            //closes all open windows within your application, including the main window.
            //Application.Current.Shutdown();
        }

        /// <summary>
        /// Close the application after checking with user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Hand) == MessageBoxResult.Cancel)
            {
                //cancel the close event
                e.Cancel = true;
            }
        }
    }
}