using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NDde.Server;
using OptionsTradeWell.assistants;

namespace OptionsTradeWell
{
    static class Program
    {

        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);




            //NativeMethods.AllocConsole();

            //Futures fut = new Futures("test", "test", DateTime.Now, 5000, 5000, 5000, 5000);
            //Option opt = new Option(fut, OptionType.Put, 21, 56, 5000, 5000, 5000);

            //TradeBlotter blotter4fut = new TradeBlotter();
            //blotter4fut.BidPrice = 54.12;
            //TradeBlotter blotter4opt = new TradeBlotter();
            //blotter4opt.BidPrice = 3.15;

            //fut.AssignTradeBlotter(blotter4fut);
            //opt.AssignTradeBlotter(blotter4opt);

            //Console.WriteLine("opt: " + opt.OptionType);
            //Console.WriteLine("is TB okay: " + opt.IsTradeBlotterAssigned());
            //Console.WriteLine("implVol: " + opt.GetUpdatedImplVol());
            //Console.WriteLine("delta: " + opt.GetUpdatedDelta());
            //Console.WriteLine("gamma: " + opt.GetUpdatedGamma());
            //Console.WriteLine("vega: " + opt.GetUpdatedVega());
            //Console.WriteLine("theta: " + opt.GetUpdatedTheta());

            //Console.WriteLine("--------------------DDE--------------------");
            //Console.WriteLine();
            //Console.WriteLine();

            //string token = ":-:";
            //Dictionary<string, QuikTableDde> myTableTopicMapping = new Dictionary<string, QuikTableDde>();
            //QuikServerDde server = new QuikServerDde("myapp", token);
            //QuikTableDde optionTable = new QuikTableDde(3, "strike always unique", new[] { token }, new List<string>() { "bidCall", "askCall", "strike", "bidPut", "askPut" });
            //QuikTableDde marginTable = new QuikTableDde(6, "strike again", new[] { token }, new List<string>() { "type", "strike", "MARGIN NOT COVER", "MARGIN COVER", "BUYER MARGIN", "OPTIONS CODE" });
            //QuikTableDde quoteTable = new QuikTableDde(1, "asset code", new[] { token }, new List<string>() { "code", "price", "days to expiry", "BUYER MARGIN", "Commis", "Price step", "time", "lot", "maturity" });
            //QuikTableDde blotterOILtable = new QuikTableDde(2, "price", new[] { token }, new List<string>() { "buy", "price", "sell" });

            //myTableTopicMapping.Add("DESK", optionTable);
            //myTableTopicMapping.Add("MARGIN_TABLE", marginTable);
            //myTableTopicMapping.Add("QUOTES", quoteTable);
            //myTableTopicMapping.Add("TEST", blotterOILtable);
            //server.EstablishConnection(myTableTopicMapping);

            //Console.WriteLine("give you time to start DDE server...");
            //Thread.Sleep(5000);

            //string prevVal = "";
            //string newVal = "";
            //while (true)
            //{
            //    newVal = blotterOILtable.GetRowByUniqueColumnValue("55,26").ToString();
            //    if (!prevVal.Equals(newVal))
            //    {
            //        Console.WriteLine(newVal);
            //        prevVal = newVal;
            //    }
            //}


            //Console.ReadLine();



            Application.Run(new MainForm());
            NativeMethods.FreeConsole();
        }
        
    }
}
