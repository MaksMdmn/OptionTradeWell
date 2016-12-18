using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionsTradeWell.model.interfaces
{
    public interface IDataHandler
    {
        bool IsConnected();

        void EstablishConnection(string serverName, List<string> topics);

        void BreakConnection();

        string GetDataByTopic(string topic);
    }
}
