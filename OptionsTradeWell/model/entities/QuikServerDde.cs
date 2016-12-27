using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using NDde.Server;
using OptionsTradeWell.model.interfaces;

namespace OptionsTradeWell.model.entities
{
    public class QuikServerDde : DdeServer
    {

        private Dictionary<string, QuikTableDde> tablesMap = new Dictionary<string, QuikTableDde>();
        private readonly string serverName;
        private readonly string splitToken;
        private System.Timers.Timer timer = new System.Timers.Timer();

        public QuikServerDde(string serverName, string splitToken) : base(serverName)
        {
            this.serverName = serverName;
            this.splitToken = splitToken;
            timer.Elapsed += this.OnTimerElapsed;
            timer.Interval = 1000;
            timer.SynchronizingObject = this.Context;
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public void EstablishConnection(Dictionary<string, QuikTableDde> topicsTableMap)
        {
            if (topicsTableMap == null)
            {
                throw new NotImplementedException();
            }
            this.tablesMap = topicsTableMap;
            Register();
        }

        public void BreakConnection()
        {
            throw new NotImplementedException();
        }

        #region NDDE overriding

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
            string key = conversation.Topic;
            StringBuilder sb = new StringBuilder();

            if (tablesMap.ContainsKey(key))
            {
                foreach (Object o in XlTableFormat.Read(data))
                {
                    sb.Append(o);
                    sb.Append(splitToken);
                }

                tablesMap[key].AddNewDdeData(sb.ToString());
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

        #endregion

        private void OnTimerElapsed(object sender, ElapsedEventArgs args)
        {
            // DUNNO WHAT TO DO, BUD
        }



    }
}

