using System.IO;
using System.Runtime.CompilerServices;

namespace Useful
{
    /// <summary>
    /// This class contains some generic methods that could be used in several programs without being edited.
    /// </summary>
    public class UsefulFileAccess
    {
        /// <summary>
        /// Read data from a file. This works 
        /// </summary>
        /// <param name="filename">The name of the file to read.</param>
        /// <returns>List containing the lines in the file.</returns>
        /// <exception cref="Exception">Any errors are thrown up to be dealty with.</exception>
        public static List<string> ReadFromFile(string filename)
        {
            List<string> rows = new List<string>();
            try
            {
                if (File.Exists(filename))
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            rows.Add(line);
                        }
                    }
                }
                else
                {
                    throw new Exception(String.Format("Reading exception: {0} does not exist", filename));
                }
            }
            catch (FileNotFoundException fnfe)
            {
                throw new Exception("Exception thrown: " + fnfe.Message);
            }
            catch (IOException ioe)
            {
                throw new Exception("Exception thrown: " + ioe.Message);
            }
            return rows;
        }

        /// <summary>
        /// Overwrite the data in the file with the supplied data.
        /// </summary>
        /// <param name="filename">The file to be written to.</param>
        /// <param name="data">The data to be written to the file.</param>
        public static void WriteToFile(string filename, string data)
        {
            UsefulFileAccess.WriteToFile(filename, data, true);
        }

        /// <summary>
        /// Write data to the specified file. The contents of the file can be overwritten or appended to as
        /// required.
        /// </summary>
        /// <param name="filename">The file to be written to.</param>
        /// <param name="data">The data to be written to the file.</param>
        /// <param name="append">Data will be appended if this is true, else data will be overwritten.</param>
        /// <exception cref="Exception">Any errors are thrown up to be dealt with.</exception>
        public static void WriteToFile(string filename, string data, bool append)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename, append))
                {
                    writer.Write(data);
                }
            }
            catch (IOException ioe)
            {
                throw new Exception("Exception thrown: " + ioe.Message);
            }            
        }
    }
}