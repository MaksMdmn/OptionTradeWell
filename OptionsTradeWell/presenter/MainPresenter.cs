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
        private static double FILE_WRITTING_PERIODICITY = Settings.Default.AutoImplVolaUpdateMs;

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

            mainForm.OnStartUp += MainForm_OnStartUp;

        }

        private void MainForm_OnStartUp(object sender, EventArgs e)
        {
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


            UpdateImplVolData(mainForm.UpdateImplVolChartData);

            StartWrittingToFile();
        }

        //private void MainFormOnSettingsInFormChanged(object sender, EventArgs e)
        //{
        //    //changes in view
        //    writtingTimer.Interval = Settings.Default.MinActualStrikeUpdateTimeSec;
        //    MainForm.VIEW_NUMBER_OF_IMPL_VOL_VALUES = Settings.Default.DisplayedPeriodOfImplVol;
        //    MainForm.maxValueY = Settings.Default.ChartsMaxYValue;
        //    MainForm.minValueY = Settings.Default.ChartsMinYValue;
        //    MainForm.stepY = Settings.Default.ChartsStepYValue;
        //    UpdateImplVolData(mainForm.ReloadImplVolChartData);

        //    //changes in calculations
        //    GreeksCalculator.DAYS_IN_YEAR = Settings.Default.DaysInYear;
        //    GreeksCalculator.NUMBER_OF_DECIMAL_PLACES = Settings.Default.RoundTo;
        //    GreeksCalculator.MAX_VOLA_VALUE = Settings.Default.MaxValueOfImplVol;
        //        /*have event that would appear and make full recalc*/
        //    dataCollector.NumberOfTrackingOptions = Settings.Default.NumberOfTrackingOptions;

        //}

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

        private void UpdateImplVolData(Action<string[]> method)
        {
            if (fileDataSaver.IsFileDataExists())
            {
                string[] tempStartImplVolData = fileDataSaver.GetAllData();
                for (int i = 0; i < tempStartImplVolData.Length; i++)
                {
                    if (!string.IsNullOrEmpty(tempStartImplVolData[i]))
                    {
                        method(tempStartImplVolData[i].Split(new char[] { ' ' }));
                    }
                }
            }
        }
    }
}