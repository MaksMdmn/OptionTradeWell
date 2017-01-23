using System;
using System.Collections.Generic;
using System.Text;
using OptionsTradeWell.model;
using OptionsTradeWell.model.interfaces;
using OptionsTradeWell.Properties;
using OptionsTradeWell.view;
using OptionsTradeWell.view.interfaces;

namespace OptionsTradeWell.presenter
{
    public class MainPresenter
    {
        private static int UNIQUE_STRIKE_INDEX_IN_ARRAY = Settings.Default.UniqueIndexInDdeDataArray;
        private static string FILE_PATH = Settings.Default.PathToVolatilityFile;
        private static double FILE_WRITTING_PERIODICITY = Settings.Default.AutoWrittingToFileMs;

        private readonly ITerminalOptionDataCollector dataCollector;
        private readonly IMainForm mainForm;
        private readonly IDerivativesDataRender dataRender;
        private readonly FileDataSaver fileDataSaver;
        private System.Timers.Timer writtingTimer = new System.Timers.Timer();

        public MainPresenter(ITerminalOptionDataCollector dataCollector, IMainForm mainForm, IDerivativesDataRender dataRender)
        {
            this.dataCollector = dataCollector;
            this.mainForm = mainForm;
            this.dataRender = dataRender;
            fileDataSaver = new FileDataSaver(FILE_PATH, Encoding.GetEncoding("windows-1251")); //hardcore D:

            dataCollector.EstablishConnection();

            while (!dataCollector.IsConnected())
            {

            }

            dataCollector.OnOptionsDeskChanged += DataCollector_OnOptionsDeskChanged;
            dataCollector.OnSpotPriceChanged += DataCollector_OnSpotPriceChanged;
            dataCollector.OnBasedParametersChanged += DataCollector_OnBasedParametersChanged;

            double minStrike = dataCollector.CalculateMinImportantStrike();
            double maxStrike = dataCollector.CalculateMaxImportantStrike();

            mainForm.UpdatePrimaryViewData(
                MakeDataList(minStrike, maxStrike),
                UNIQUE_STRIKE_INDEX_IN_ARRAY);

            StartWrittingToFile();
        }

        private void DataCollector_OnBasedParametersChanged(object sender, EventArgs e)
        {
            double minStrike = dataCollector.CalculateMinImportantStrike();
            double maxStrike = dataCollector.CalculateMaxImportantStrike();

            mainForm.UpdatePrimaryViewData(
                MakeDataList(minStrike, maxStrike),
                UNIQUE_STRIKE_INDEX_IN_ARRAY);
        }

        private void DataCollector_OnOptionsDeskChanged(object sender, OptionEventArgs e)
        {
            double tempStrike = e.opt.Strike;
            Option call = dataCollector.GetOption(tempStrike, OptionType.Call);
            Option put = dataCollector.GetOption(tempStrike, OptionType.Put);
            call.UpdateAllGreeksTogether();
            put.UpdateAllGreeksTogether();
            double[] tempData = dataRender.GetRenderDataFromOptionPair(call, put);

            mainForm.UpdateRowInViewDataMap(tempData, UNIQUE_STRIKE_INDEX_IN_ARRAY);
        }

        private void DataCollector_OnSpotPriceChanged(object sender, OptionEventArgs e)
        {
            string[] data = new[]
            {
                Convert.ToString(e.opt.Futures.GetTradeBlotter().AskPrice),
                e.opt.Futures.Ticker,
                Convert.ToString(e.opt.RemainingDays)

            };
            mainForm.UpdateFuturesData(data);
        }

        private List<double[]> MakeDataList(double minStrike, double maxStrike)
        {
            List<double[]> resultList = new List<double[]>();
            Option call;
            Option put;
            for (double i = minStrike; i <= maxStrike; i++)
            {
                call = dataCollector.GetOption(i, OptionType.Call);
                put = dataCollector.GetOption(i, OptionType.Put);

                call.UpdateAllGreeksTogether();
                put.UpdateAllGreeksTogether();

                resultList.Add(dataRender.GetRenderDataFromOptionPair(call, put));
            }

            return resultList;
        }

        private void StartWrittingToFile()
        {
            writtingTimer.Elapsed += WrittingTimer_Elapsed;
            writtingTimer.Interval = FILE_WRITTING_PERIODICITY;
            writtingTimer.Enabled = true;
        }

        private void WrittingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));
            sb.Append(" ");
            sb.Append(dataCollector.GetOption(dataCollector.CalculateActualStrike(), OptionType.Call).BuyVol * 100);
            sb.Append(" ");
            sb.Append(dataCollector.GetOption(dataCollector.CalculateActualStrike(), OptionType.Put).BuyVol * 100);
            fileDataSaver.SaveData(sb.ToString());

            mainForm.UpdateImplVolChartData(sb.ToString().Split(new char[] { ' ' }));
        }
    }
}