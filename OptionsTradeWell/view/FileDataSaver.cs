using System.IO;
using System.Text;
using System.Threading;

namespace OptionsTradeWell.view
{
    public class FileDataSaver
    {
        private readonly string filePath;
        private readonly Encoding encoding;

        public FileDataSaver(string filePath, Encoding encoding)
        {
            this.filePath = filePath;
            this.encoding = encoding;
        }

        public void SaveData(string data)
        {
            File.AppendAllText(filePath, data + "\r", encoding);
        }

        public string[] GetAllData()
        {
            return File.ReadAllLines(filePath, encoding);
        }

        public string GetLastLine()
        {
            string[] tempDataArr = GetAllData();
            return tempDataArr[tempDataArr.Length - 1];
        }

    }
}