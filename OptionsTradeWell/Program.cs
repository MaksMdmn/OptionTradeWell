using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NDde.Server;
using OptionsTradeWell.assistants;
using OptionsTradeWell.model;
using OptionsTradeWell.model.interfaces;
using OptionsTradeWell.presenter;
using OptionsTradeWell.view;
using OptionsTradeWell.view.interfaces;

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

            ITerminalOptionDataCollector dataCollector = new OptionsQuikDdeDataCollector(14);
            IDerivativesDataRender dataRender = new DerivativesDataRender();
            MainForm mainForm = new MainForm();

            dataCollector.EstablishConnection();

            Thread.Sleep(5000);

            MainPresenter presenter = new MainPresenter(dataCollector, mainForm, dataRender);


            Application.Run(mainForm);
            //NativeMethods.FreeConsole();
        }

    }
}
