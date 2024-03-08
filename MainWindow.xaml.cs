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
        private string testDummy;// = "./resources/slimer.txt";

        //contents of the file. One string for each row in the file.
        private List<string> rows;

        //working path for application
        private string workPath = "";

        /// <summary>
        /// Main starting point.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //used to sync the checkbox in the setting menu with the checkbox in the main panel
            DataContext = new OverwriteViewModel();

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

        }

        /// <summary>
        /// Load contents of the test file. This is the event handler for the Open button and Open menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a file";
            // Set it so that only text files are shown.
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            // Set the starting location
            openFileDialog.InitialDirectory = workPath;

            string output = "";

            if (openFileDialog.ShowDialog() == true)
            {
                testDummy = openFileDialog.FileName;

                //clear and initialise ready for fresh data.                
                rows = new List<string>();

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
            }
            else
            {
                output = "No file selected.";
            }
            txtInfo.Text = output;
        }

        /// <summary>
        /// Save button clicked. This the event handler for the Save button and the Save menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //to overwrite or append?
            bool append = (cbAppend.IsChecked != true) ? true : false;

            string output = txtFileContent.Text;
            try
            {
                if (MessageBox.Show("This will save to the existing file.\nAre you sure?", this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Hand) == MessageBoxResult.OK)
                {

                    UsefulFileAccess.WriteToFile(testDummy, output, append);

                    output = "Successfully wrote to file: " + testDummy + "\n";
                    output += "Appending to file: " + append;
                }
                else
                {
                    if (append)
                        output = "Appending data to existing file has been cancelled";
                    else
                        output = "Overwriting existing file has been cancelled";
                }
            }
            catch (Exception ex)
            {
                output = "Error writing to file: " + testDummy + "\n";
                output += ex.Message;

            }
            txtInfo.Text = output;
        }

        /// <summary>
        /// Close the application. This is the event handler for the exit button and exit menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //This is the common and recommended way to close a WPF window from within the window itself.
            Close();

            //closes all open windows within your application, including the main window. Microsoft
            //documentation suggests that Close is the method that should normally by used.
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

        /// <summary>
        /// Show additional information 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbMoreInfo_Checked(object sender, RoutedEventArgs e)
        {
            txtLocation.Text = "Working folder:\n" + workPath;
            txtLocation.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hide the additional information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbMoreInfo_Unchecked(object sender, RoutedEventArgs e)
        {
            txtLocation.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Demo only. Action handler for the Info menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Info menu item clicked", "For demo only", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Demo only. Action handler for the About menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("About menu item clicked", "For demo only", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Clear any loaded data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (rows != null)
                rows.Clear();
            txtFileContent.Clear();
            txtInfo.Text = "Loaded data has been cleared.";
            txtLocation.Text = "";
        }
    }
}