﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NDde.Server;
using NLog;
using OptionsTradeWell.assistants;
using OptionsTradeWell.model;
using OptionsTradeWell.presenter;
using OptionsTradeWell.presenter.interfaces;
using OptionsTradeWell.view;
using OptionsTradeWell.view.interfaces;

namespace OptionsTradeWell
{
    static class Program
    {
        private static Logger LOGGER = LogManager.GetCurrentClassLogger();

        [STAThread]
        static void Main()
        {
            LOGGER.Info("Program started.");
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                ITerminalOptionDataCollector dataCollector = new QuikDdeDataCollector();
                IDerivativesDataRender dataRender = new DerivativesDataRender();
                MainForm mainForm = new MainForm();

                MainPresenter presenter = new MainPresenter(dataCollector, mainForm, dataRender);

                Application.Run(mainForm);
            }
            catch (Exception e)
            {
                LOGGER.Fatal("Program stopped. Main method exception, we are in a serious trouble, bud: {0}", e.ToString());
                throw;
            }
            LOGGER.Info("Program stopped (successfully).");
        }

    }
}
