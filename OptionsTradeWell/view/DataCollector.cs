using System.IO;
using System.Text;
using System.Threading;

namespace OptionsTradeWell.view
{
    public class DataCollector
    {
        private string filePath;
        private Encoding encoding;

        public DataCollector(string filePath, Encoding encoding)
        {
            this.filePath = filePath;
            this.encoding = encoding;
        }

        public void SaveData(string data)
        {
            File.AppendAllText(filePath, data, encoding);
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