using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionsTradeWell.model.entities
{
    public class QuikTableDde
    {
        private Dictionary<string, QuikRowDde> rowsMap;
        private string lastUpdaterRowId;

        public QuikTableDde(int uniqueColumnNumber, string uniqueColumnDescription, string[] separator, List<string> titles)
        {
            this.UniqueColumnNumber = uniqueColumnNumber - 1;
            this.UniqueColumnDescription = uniqueColumnDescription;
            this.Separator = separator;
            this.Titles = titles;
            LastUpdateTime = DateTime.Now;

            rowsMap = new Dictionary<string, QuikRowDde>();
        }

        public string UniqueColumnDescription { get; }

        public int UniqueColumnNumber { get; }

        public string[] Separator { get; }

        public List<string> Titles { get; }

        public DateTime LastUpdateTime { get; private set; }


        public void AddNewDdeData(string data)
        {
            ParseDataByRowMap(data);
            RefreshLastUpdateTime();
        }

        public QuikRowDde GetRowByUniqueColumnValue(string columnValue)
        {
            return rowsMap[columnValue];
        }

        public List<QuikRowDde> GetAllRows()
        {
            List<string> tempList = rowsMap.Keys.ToList();
            tempList.Sort();

            List<QuikRowDde> answer = new List<QuikRowDde>();

            foreach (string key in tempList)
            {
                answer.Add(rowsMap[key]);
            }

            return answer;
        }

        private void ParseDataByRowMap(string data)
        {
            string[] tempDataArr = data.Split(this.Separator, StringSplitOptions.None);
            List<string> tempDataList = new List<string>(tempDataArr);

            if (tempDataList[0].Contains("System"))
            {
                tempDataList.RemoveAt(0);
            }

            QuikRowDde rowData = null;
            string rowKey = null;
            int rowSize = Titles.Count;
            int titleIndex = 0;

            for (int i = 0; i < tempDataList.Count; i++)
            {
                //check if we need new row object
                if (i % rowSize == 0 || i == 0)
                {

                    // here's we should avoid 1st iteration when object == null
                    if (rowData != null && rowKey != null)
                    {
                        if (rowsMap.ContainsKey(rowKey))
                        {
                            rowsMap[rowKey] = rowData;
                        }
                        else
                        {
                            rowsMap.Add(rowKey, rowData);
                        }
                    }

                    rowData = new QuikRowDde();
                    titleIndex = 0;
                }

                //fulfill row object
                rowData.AddParameter(Titles[titleIndex], tempDataList[i]);

                //if we would find our unique column - this is our key in map, so we memorize it.
                if (titleIndex == this.UniqueColumnNumber)
                {
                    rowKey = tempDataList[i];
                }

                titleIndex++;
            }


        }
        private void RefreshLastUpdateTime()
        {
            this.LastUpdateTime = DateTime.Now;
        }



    }
}
