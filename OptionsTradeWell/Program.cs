using OptionsTradeWell.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OptionsTradeWell.model.assistants;
using OptionsTradeWell.model.entities;

namespace OptionsTradeWell
{
    static class Program
    {

        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            NativeMethods.AllocConsole();

            Futures fut = new Futures("test", "test", DateTime.Now, 5000, 5000, 5000, 5000);
            Option opt = new Option(fut, OptionType.Put, 21, 56, 5000, 5000, 5000);

            TradeBlotter blotter4fut = new TradeBlotter();
            blotter4fut.BidPrice = 54.12;
            TradeBlotter blotter4opt = new TradeBlotter();
            blotter4opt.BidPrice = 3.15;

            fut.AssignTradeBlotter(blotter4fut);
            opt.AssignTradeBlotter(blotter4opt);

            Console.WriteLine("opt: " + opt.OptionType);
            Console.WriteLine("is TB okay: " + opt.IsTradeBlotterAssigned());
            Console.WriteLine("implVol: " + opt.GetUpdatedImplVol());
            Console.WriteLine("delta: " + opt.GetUpdatedDelta());
            Console.WriteLine("gamma: " + opt.GetUpdatedGamma());
            Console.WriteLine("vega: " + opt.GetUpdatedVega());
            Console.WriteLine("theta: " + opt.GetUpdatedTheta());

            Console.ReadLine();

            Application.Run(new Form1());
            NativeMethods.FreeConsole();
        }
    }
}
