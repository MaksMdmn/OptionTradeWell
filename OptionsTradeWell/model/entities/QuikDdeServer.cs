using System;
using System.Collections.Generic;
using System.Timers;
using NDde.Server;
using OptionsTradeWell.model.interfaces;

namespace OptionsTradeWell.model.entities
{
    public class QuikDdeServer : IDataHandler
    {
        private Dictionary<string, string> recievedTopics = new Dictionary<string, string>();
        private MyDdeServer activeServer;

        public string ServerName { get; private set; }
        public List<string> Topics { get; private set; }

        public bool IsConnected()
        {
            // DUNNO YET
            return true;
        }

        public void EstablishConnection(string serverName, List<string> topics)
        {
            this.ServerName = serverName;
            this.Topics = topics;
            activeServer = new MyDdeServer(serverName);
            activeServer.Register();
        }

        public void BreakConnection()
        {
            // DUNNO YET
        }

        public string GetDataByTopic(string topic)
        {
            string result = null;
            recievedTopics.TryGetValue(topic, out result);

            return result;
        }

        private class MyDdeServer : DdeServer
        {
            private System.Timers.Timer timer = new System.Timers.Timer();

            public MyDdeServer(string service) : base(service)
            {
                timer.Elapsed += this.OnTimerElapsed;
                timer.Interval = 1000;
                timer.SynchronizingObject = this.Context;
            }

            private void OnTimerElapsed(object sender, ElapsedEventArgs args)
            {
                // DUNNO WHAT TO DO, BUD
            }

            public override void Register()
            {
                base.Register();
                timer.Start();
            }

            public override void Unregister()
            {
                timer.Stop();
                base.Unregister();
            }

            protected override void OnDisconnect(DdeConversation conversation)
            {
                // NEED IMPLEMENTATION - METHOD WORKS GOOD
            }

            protected override PokeResult OnPoke(DdeConversation conversation, string item, byte[] data, int format)
            {
                // NEED IMPLEMENTATION - METHOD WORKS GOOD

                Console.WriteLine("OnPoke:".PadRight(16)
                    + " Service='" + conversation.Service + "'"
                    + " Topic='" + conversation.Topic + "'"
                    + " Handle=" + conversation.Handle.ToString()
                    + " Item='" + item + "'"
                    + " Data=" + data.Length.ToString()
                    + " Format=" + format.ToString());


                Console.WriteLine("----------------");


                int counter = 0;
                foreach (Object o in XlTableFormat.Read(data))
                {
                    //Console.Write(o + " ");
                    //if (o.Equals("Call") || o.Equals("Put"))
                    //{
                    //    Console.WriteLine();
                    //}
                    double res;
                    if (double.TryParse(o.ToString(), out res))
                    {
                        counter++;
                        Console.Write(o.ToString() + " ");
                    }



                    if (counter % 5 == 0)
                    {
                        Console.WriteLine();
                    }


                    //double result;
                    //if (double.TryParse(o.ToString(), out result))
                    //{
                    //    if (result > 32.00)
                    //    {
                    //        Console.WriteLine(result);
                    //    }
                    //}
                }

                // Tell the client that the data was processed.
                return PokeResult.Processed;
            }

            protected override bool OnStartAdvise(DdeConversation conversation, string item, int format)
            {
                return format == 1;
            }

            protected override void OnStopAdvise(DdeConversation conversation, string item)
            {
            }

            protected override ExecuteResult OnExecute(DdeConversation conversation, string command)
            {
                return ExecuteResult.Processed;
            }

            protected override RequestResult OnRequest(DdeConversation conversation, string item, int format)
            {
                return RequestResult.NotProcessed;
            }

            protected override byte[] OnAdvise(string topic, string item, int format)
            {
                return null;
            }
            protected override bool OnBeforeConnect(string topic)
            {
                return true;
            }

            protected override void OnAfterConnect(DdeConversation conversation)
            {

            }


        }
    }


}
