using System;
using System.Collections.Generic;
using System.Text;
using OptionsTradeWell.model;
using OptionsTradeWell.model.exceptions;
using OptionsTradeWell.model.interfaces;
using OptionsTradeWell.Properties;
using OptionsTradeWell.view;
using OptionsTradeWell.view.interfaces;

namespace OptionsTradeWell.presenter
{
    public class MainPresenter
    {
        private readonly ITerminalOptionDataCollector dataCollector;
        private readonly IMainForm mainForm;
        private readonly IDerivativesDataRender dataRender;
        private readonly FileDataSaver fileDataSaver;
        private PositionManager positionManager;
        private System.Timers.Timer writtingTimer = new System.Timers.Timer();

        public MainPresenter(ITerminalOptionDataCollector dataCollector, IMainForm mainForm, IDerivativesDataRender dataRender)
        {
            this.dataCollector = dataCollector;
            this.mainForm = mainForm;
            this.dataRender = dataRender;
            this.positionManager = new PositionManager();
            //fileDataSaver = new FileDataSaver(FILE_PATH, Encoding.GetEncoding("windows-1251")); //hardcore D:

            mainForm.OnStartUp += MainForm_OnStartUp;
            mainForm.OnPosUpdateButtonClick += MainForm_OnPosUpdateButtonClick;

        }

        private void MainForm_OnPosUpdateButtonClick(object sender, PositionTableArgs e)
        {

            positionManager.CleanAllPositions();

            List<string[]> tempPosTableData = new List<string[]>();
            List<double[]> tempPosChartData = new List<double[]>();

            List<string[]> userData = e.userArgs;

            string type;
            double enterPrice;
            int quantity;
            double strike;

            double remainingDays;
            double priceStep;
            double priceVal;
            TradeBlotter futBlotter;
            TradeBlotter optBlotter;

            foreach (string[] data in userData)
            {
                type = data[0];
                enterPrice = Convert.ToDouble(data[2]);
                quantity = Convert.ToInt32(data[3]);

                if (!String.IsNullOrEmpty(data[1]))
                {
                    strike = Convert.ToDouble(data[1]);
                }
                else
                {
                    strike = 0.0;
                }


                if (type.Equals(UserPosTableTypes.TYPE_CALL))
                {
                    futBlotter = dataCollector.GetBasicFutures().GetTradeBlotter();
                    optBlotter = dataCollector.GetOption(strike, OptionType.Call).GetTradeBlotter();
                    remainingDays = dataCollector.GetOption(strike, OptionType.Call).RemainingDays;
                    priceStep = dataCollector.GetOption(strike, OptionType.Call).PriceStep;
                    priceVal = dataCollector.GetOption(strike, OptionType.Call).PriceStepValue;

                    positionManager.AddOption(Option.GetFakeOption(OptionType.Call, strike, enterPrice, remainingDays, quantity, futBlotter, optBlotter, priceStep, priceVal));
                }

                else if (type.Equals(UserPosTableTypes.TYPE_PUT))
                {
                    futBlotter = dataCollector.GetBasicFutures().GetTradeBlotter();
                    optBlotter = dataCollector.GetOption(strike, OptionType.Put).GetTradeBlotter();
                    remainingDays = dataCollector.GetOption(strike, OptionType.Put).RemainingDays;
                    priceStep = dataCollector.GetOption(strike, OptionType.Call).PriceStep;
                    priceVal = dataCollector.GetOption(strike, OptionType.Call).PriceStepValue;

                    positionManager.AddOption(Option.GetFakeOption(OptionType.Put, strike, enterPrice, remainingDays, quantity, futBlotter, optBlotter, priceStep, priceVal));
                }
                else if (type.Equals(UserPosTableTypes.TYPE_FUT))
                {
                    futBlotter = dataCollector.GetBasicFutures().GetTradeBlotter();
                    priceStep = dataCollector.GetBasicFutures().PriceStep;
                    priceVal = dataCollector.GetBasicFutures().PriceStepValue;

                    positionManager.AddFutures(Futures.GetFakeFutures(enterPrice, quantity, futBlotter, priceStep, priceVal));
                }
                else
                {
                    throw new IllegalViewDataException("incorrect type of instrument in posTable: " + type);
                }
            }

            positionManager.UpdateGeneralParametres();

            if (positionManager.Futures != null)
            {
                tempPosTableData.Add(CreatePosTableDataRowFromFut(positionManager.Futures));
            }

            foreach (Option opt in positionManager.Options)
            {
                tempPosTableData.Add(CreatePosTableDataRowFromOpt(opt));
            }

            mainForm.UpdatePositionTableData(tempPosTableData);
            mainForm.UpdateTotalInfoTable(new double[]
            {
                Math.Round(positionManager.CalculatePositionCurPnL(),0),
                Math.Round(positionManager.CalculatePositionPnL(),2),
                Math.Round(positionManager.FixedPnL,2),
                Math.Round(positionManager.TotalDelta,4),
                Math.Round(positionManager.TotalGamma,4),
                Math.Round(positionManager.TotalVega,4),
                Math.Round(positionManager.TotalTheta,4)
            });

            double minStr = dataCollector.CalculateMinImportantStrike();
            double maxStr = dataCollector.CalculateMaxImportantStrike();
            for (double i = minStr; i <= maxStr; i++)
            {
                tempPosChartData.Add(new double[]
                {
                    i,
                    positionManager.CalculateCurApproxPnL(i, minStr, maxStr),
                    positionManager.CalculateExpirationPnL(i)
                });
            }

            mainForm.UpdatePositionChartData(tempPosChartData);
        }

        private void MainForm_OnStartUp(object sender, EventArgs e)
        {
            dataCollector.EstablishConnection();

            while (!dataCollector.IsConnected())
            {
            }

            dataCollector.OnOptionsDeskChanged += DataCollector_OnOptionsDeskChanged;
            dataCollector.OnSpotPriceChanged += DataCollector_OnSpotPriceChanged;

            double minStrike = dataCollector.CalculateMinImportantStrike();
            double maxStrike = dataCollector.CalculateMaxImportantStrike();

            mainForm.UpdateViewData(
                MakeDataList(minStrike, maxStrike));

        }

        private void DataCollector_OnOptionsDeskChanged(object sender, OptionEventArgs e)
        {
            double tempStrike = e.opt.Strike;
            mainForm.UpdateViewData(
                 MakeDataList(tempStrike, tempStrike));
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

        private string[] CreatePosTableDataRowFromFut(Futures fut)
        {
            string[] rowData = new string[12];
            int indexCount = 4;

            rowData[0] = "F";
            rowData[1] = "0";
            rowData[2] = Convert.ToString(fut.Position.EnterPrice);
            rowData[3] = Convert.ToString(fut.Position.Quantity);

            foreach (double futData in dataRender.GetRenderDataFromFutures(fut))
            {
                rowData[indexCount++] = Convert.ToString(futData);
            }

            return rowData;

        }

        private string[] CreatePosTableDataRowFromOpt(Option opt)
        {
            string[] rowData = new string[12];
            int indexCount = 4;

            rowData[0] = opt.OptionType == OptionType.Call ? "C" : "P";
            rowData[1] = Convert.ToString(opt.Strike);
            rowData[2] = Convert.ToString(opt.Position.EnterPrice);
            rowData[3] = Convert.ToString(opt.Position.Quantity);


            foreach (double futData in dataRender.GetRenderDataFromOption(opt))
            {
                rowData[indexCount++] = Convert.ToString(futData);
            }

            return rowData;
        }

    }
}