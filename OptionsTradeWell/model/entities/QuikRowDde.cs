using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptionsTradeWell.model.interfaces;

namespace OptionsTradeWell.model.entities
{
    public class QuikRowDde
    {
        private Dictionary<string, string> mapOfValues = new Dictionary<string, string>();

        public void AddParameter(string name, string value)
        {
            mapOfValues.Add(name, value);
        }

        public void UpdateParameter(string name, string value)
        {
            mapOfValues[name] = value;
        }

        public string GetParameter(string name)
        {
            return mapOfValues[name];
        }

        public double GetNumericParameter(string name)
        {
            return Double.Parse(GetParameter(name));
        }

        public override string ToString()
        {
            StringBuilder answer = new StringBuilder();
            List<string> tempList = mapOfValues.Keys.ToList();
            tempList.Sort();
            foreach (string key in tempList)
            {
                answer.Append(key);
                answer.Append(": ");
                answer.Append(mapOfValues[key]);
                answer.Append(" ");
            }

            return answer.ToString();
        }
    }
}
