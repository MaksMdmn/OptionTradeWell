using System;
using System.Collections.Generic;
using System.Timers;
using NDde.Server;
using OptionsTradeWell.assistants;

namespace OptionsTradeWell.model
{
    public class QuikServerDde : DdeServer
    {
        private readonly string serverName;
        private System.Timers.Timer timer = new System.Timers.Timer();
        private readonly Dictionary<string, int> topicRowLengthMap;

        public delegate void DataHandlerMethod(string topic, string[] data);
        public event DataHandlerMethod OnDataUpdate;

        internal QuikServerDde(string serverName, Dictionary<string, int> topicRowLengthMap) : base(serverName)
        {
            this.serverName = serverName;
            this.topicRowLengthMap = topicRowLengthMap;
            timer.Elapsed += this.OnTimerElapsed;
            timer.Interval = 1000;
            timer.SynchronizingObject = this.Context;
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
            throw new NotImplementedException();
        }

        protected override PokeResult OnPoke(DdeConversation conversation, string item, byte[] data, int format)
        {
            int rowLength = topicRowLengthMap[conversation.Topic];
            string[] dataArr = new string[rowLength];
            int dataCounter = -2; //because of 2 metadata symbols like: some number (D:) and type

            {
                foreach (Object o in XlTableFormat.Read(data))
                {
                    dataCounter++;
                    if (dataCounter < 0)
                    {
                        continue;
                    }

                    dataArr[dataCounter % rowLength] = o.ToString();
                    if ((dataCounter + 1) % rowLength == 0 && dataCounter != 0)
                    {
                        if (OnDataUpdate != null)
                        {
                            OnDataUpdate(conversation.Topic, dataArr);
                        }
                        dataArr = new string[rowLength];
                    }
                }
            }

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

        private void OnTimerElapsed(object sender, ElapsedEventArgs args)
        {
            // DUNNO WHAT TO DO, BUD
        }
    }
}

