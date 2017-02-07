using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NLog;
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
        private static Logger LOGGER = LogManager.GetCurrentClassLogger();

        private readonly ITerminalOptionDataCollector dataCollector;
        private readonly IMainForm mainForm;
        private readonly IDerivativesDataRender dataRender;
        private readonly FileDataSaver fileDataSaver;
        private PositionManager positionManager;
        private System.Timers.Timer writtingTimer = new System.Timers.Timer();

        public MainPresenter(ITerminalOptionDataCollector dataCollector, IMainForm mainForm, IDerivativesDataRender dataRender)
        {
            LOGGER.Info("MainPresenter creation...");

            this.dataCollector = dataCollector;
            this.mainForm = mainForm;
            this.dataRender = dataRender;
            this.positionManager = new PositionManager();
            //fileDataSaver = new FileDataSaver(FILE_PATH, Encoding.GetEncoding("windows-1251")); //hardcore D:

            mainForm.OnStartUp += MainForm_OnStartUp;
            mainForm.OnPosUpdateButtonClick += MainForm_OnPosUpdateButtonClick;
            mainForm.OnTotalResetPositionInfo += MainForm_OnTotalResetPositionInfo;

            LOGGER.Info("MainPresenter created.");

        }

        private void MainForm_OnTotalResetPositionInfo(object sender, EventArgs e)
        {
            if (positionManager != null)
            {
                positionManager.CleanAllPositions();
                positionManager.ResetFixedPnLValue();
                mainForm.UpdatePositionTableData(new List<string[]>() { });
                mainForm.UpdateTotalInfoTable(new double[] { 0, 0, 0, 0, 0, 0, 0 });
                mainForm.UpdatePositionChartData(new List<double[]>() { });
            }
        }

        private void MainForm_OnPosUpdateButtonClick(object sender, PositionTableArgs e)
        {
            try
            {
                positionManager.CleanAllPositions();

                List<string[]> tempPosTableData = new List<string[]>();
                List<double[]> tempPosChartData = new List<double[]>();

                List<string[]> userData = e.userArgs;

                UserPosTableTypes type;
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
                    if (!UserPosTableTypes.TryParse(data[0], out type))
                    {
                        return;
                    }

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


                    if (type.Equals(UserPosTableTypes.C))
                    {
                        try
                        {
                            futBlotter = dataCollector.GetBasicFutures().GetTradeBlotter();
                            optBlotter = dataCollector.GetOption(strike, OptionType.Call).GetTradeBlotter();
                            remainingDays = dataCollector.GetOption(strike, OptionType.Call).RemainingDays;
                            priceStep = dataCollector.GetOption(strike, OptionType.Call).PriceStep;
                            priceVal = dataCollector.GetOption(strike, OptionType.Call).PriceStepValue;

                            positionManager.AddOption(Option.GetFakeOption(OptionType.Call, strike, enterPrice,
                                remainingDays, quantity, futBlotter, optBlotter, priceStep, priceVal));
                        }
                        catch (QuikDdeException e1)
                        {
                            mainForm.UpdateMessageWindow(e1.Message);
                            LOGGER.Error("Update position event, exception in call section: {0}", e1.ToString());
                        }
                    }

                    else if (type.Equals(UserPosTableTypes.P))
                    {
                        try
                        {
                            futBlotter = dataCollector.GetBasicFutures().GetTradeBlotter();
                            optBlotter = dataCollector.GetOption(strike, OptionType.Put).GetTradeBlotter();
                            remainingDays = dataCollector.GetOption(strike, OptionType.Put).RemainingDays;
                            priceStep = dataCollector.GetOption(strike, OptionType.Call).PriceStep;
                            priceVal = dataCollector.GetOption(strike, OptionType.Call).PriceStepValue;

                            positionManager.AddOption(Option.GetFakeOption(OptionType.Put, strike, enterPrice,
                                remainingDays, quantity, futBlotter, optBlotter, priceStep, priceVal));
                        }
                        catch (QuikDdeException e1)
                        {
                            mainForm.UpdateMessageWindow(e1.Message);
                            LOGGER.Error("Update position event, exception in put section: {0}", e1.ToString());
                        }
                    }
                    else if (type.Equals(UserPosTableTypes.F))
                    {
                        futBlotter = dataCollector.GetBasicFutures().GetTradeBlotter();
                        priceStep = dataCollector.GetBasicFutures().PriceStep;
                        priceVal = dataCollector.GetBasicFutures().PriceStepValue;

                        positionManager.AddFutures(Futures.GetFakeFutures(enterPrice, quantity, futBlotter, priceStep,
                            priceVal));
                    }
                    else
                    {
                        mainForm.UpdateMessageWindow("incorrect type of instrument: " + type);
                        LOGGER.Error("incorrect type of instrument, futures section: {0}", type);
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
                    Math.Round(positionManager.CalculatePositionCurPnL(), 0),
                    Math.Round(positionManager.CalculatePositionPnL(), 2),
                    Math.Round(positionManager.FixedPnL, 2),
                    Math.Round(positionManager.TotalDelta, 4),
                    Math.Round(positionManager.TotalGamma, 4),
                    Math.Round(positionManager.TotalVega, 4),
                    Math.Round(positionManager.TotalTheta, 4)
                });

                double minStr = dataCollector.CalculateMinImportantStrike();
                double maxStr = dataCollector.CalculateMaxImportantStrike();



                for (double i = minStr; i <= maxStr; i += Settings.Default.StrikeStep)
                {
                    tempPosChartData.Add(new double[]
                    {
                        i,
                        positionManager.CalculateCurApproxPnL(i),
                        positionManager.CalculateExpirationPnL(i)
                    });
                }

                mainForm.UpdatePositionChartData(tempPosChartData);
            }
            catch (Exception e2)
            {
                LOGGER.Error("An exception when event of position update was enabled:{0}", e2.ToString());
                throw;
            }
        }

        private void MainForm_OnStartUp(object sender, EventArgs e)
        {
            try
            {
                dataCollector.EstablishConnection();

                while (!dataCollector.IsConnected())
                {
                    Thread.Sleep(500);
                }

                dataCollector.OnOptionsDeskChanged += DataCollector_OnOptionsDeskChanged;
                dataCollector.OnSpotPriceChanged += DataCollector_OnSpotPriceChanged;

                double minStrike = dataCollector.CalculateMinImportantStrike();
                double maxStrike = dataCollector.CalculateMaxImportantStrike();

                mainForm.UpdateViewData(
                    MakeDataList(minStrike, maxStrike));
            }
            catch (Exception e3)
            {
                LOGGER.Error("An exception when event of startUp was enabled:{0}", e3.ToString());
                throw;
            }
        }

        private void DataCollector_OnOptionsDeskChanged(object sender, OptionEventArgs e)
        {
            try
            {
                double tempStrike = e.opt.Strike;
                mainForm.UpdateViewData(
                     MakeDataList(tempStrike, tempStrike));
            }
            catch (Exception e4)
            {
                LOGGER.Error("An exception when event of options desk data changed was enabled:{0}", e4.ToString());
                throw;
            }
        }

        private void DataCollector_OnSpotPriceChanged(object sender, OptionEventArgs e)
        {
            try
            {
                string[] data = new[]
                {
                Convert.ToString(e.opt.Futures.GetTradeBlotter().AskPrice),
                e.opt.Futures.Ticker,
                Convert.ToString(e.opt.RemainingDays)

            };
                mainForm.UpdateFuturesData(data);
            }
            catch (Exception e5)
            {
                LOGGER.Error("An exception when event of options desk data changed was enabled:{0}", e5.ToString());
                throw;
            }
        }

        private List<double[]> MakeDataList(double minStrike, double maxStrike)
        {
            List<double[]> resultList = new List<double[]>();
            Option call;
            Option put;
            for (double i = minStrike; i <= maxStrike; i += Settings.Default.StrikeStep)
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