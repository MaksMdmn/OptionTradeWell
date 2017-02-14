using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NLog;
using OptionsTradeWell.model;
using OptionsTradeWell.model.exceptions;
using OptionsTradeWell.presenter.interfaces;
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
        private PositionManager posManager;
        private PositionManager quikPosManager;
        private System.Timers.Timer writtingTimer = new System.Timers.Timer();

        public MainPresenter(ITerminalOptionDataCollector dataCollector, IMainForm mainForm, IDerivativesDataRender dataRender)
        {
            LOGGER.Info("MainPresenter creation...");

            this.dataCollector = dataCollector;
            this.mainForm = mainForm;
            this.dataRender = dataRender;
            this.posManager = new PositionManager();
            this.quikPosManager = new PositionManager();

            mainForm.OnStartUpClick += MainFormOnStartUpClick;
            mainForm.OnPosUpdateButtonClick += MainForm_OnPosUpdateButtonClick;
            mainForm.OnTotalResetPositionInfoClick += MainFormOnTotalResetPositionInfoClick;
            mainForm.OnGetPosFromQuikClick += MainForm_OnGetPosFromQuikClick;
            mainForm.OnActPosUpdateButtonClick += MainForm_OnActPosUpdateButtonClick;


            LOGGER.Info("MainPresenter created.");
        }

        private void MainForm_OnActPosUpdateButtonClick(object sender, PositionTableArgs e)
        {
            try
            {
                PositionManager tempPosManager = ParseUserArgsPositionForManager(new PositionManager(), e.userArgs);

                List<string[]> tempPosTableData = new List<string[]>();

                tempPosManager.UpdateGeneralParametres();

                if (tempPosManager.Futures != null)
                {
                    tempPosTableData.Add(CreateActualPosTableDataRowFromFut(tempPosManager.Futures));
                }

                foreach (Option opt in tempPosManager.Options)
                {
                    tempPosTableData.Add(CreateActualPosTableDataRowFromOpt(opt));
                }

                mainForm.UpdateActualPositionTableData(tempPosTableData);

            }
            catch (Exception e2)
            {
                LOGGER.Error("An exception when event of ACTUAL position update was enabled:{0}", e2.ToString());
                throw;
            }
        }

        private void MainForm_OnGetPosFromQuikClick(object sender, EventArgs e)
        {
            try
            {
                List<string[]> tempPosTableData = new List<string[]>();

                quikPosManager.UpdateGeneralParametres();

                if (quikPosManager.Futures != null
                    && quikPosManager.Futures.Position.Quantity != 0)
                {
                    tempPosTableData.Add(CreateActualPosTableDataRowFromFut(quikPosManager.Futures));
                }

                foreach (Option opt in quikPosManager.Options)
                {
                    tempPosTableData.Add(CreateActualPosTableDataRowFromOpt(opt));
                }

                mainForm.UpdateActualPositionTableData(tempPosTableData);

            }
            catch (Exception e2)
            {
                LOGGER.Error("An exception when event of request of Quik position was enabled:{0}", e2.ToString());
                throw;
            }
        }

        private void MainFormOnTotalResetPositionInfoClick(object sender, EventArgs e)
        {
            if (posManager != null)
            {
                posManager.CleanAllPositions();
                posManager.ResetFixedPnLValue();
                mainForm.UpdatePositionTableData(new List<string[]>() { });
                mainForm.UpdateTotalInfoTable(new double[] { 0, 0, 0, 0, 0, 0, 0 });
                mainForm.UpdatePositionChartData(new List<double[]>() { });
            }
        }

        private void MainForm_OnPosUpdateButtonClick(object sender, PositionTableArgs e)
        {
            try
            {
                List<string[]> tempPosTableData = new List<string[]>();
                List<double[]> tempPosChartData = new List<double[]>();

                posManager.CleanAllPositions();
                ParseUserArgsPositionForManager(posManager, e.userArgs);

                posManager.UpdateGeneralParametres();

                if (posManager.Futures != null)
                {
                    tempPosTableData.Add(CreatePosTableDataRowFromFut(posManager.Futures));
                }

                foreach (Option opt in posManager.Options)
                {
                    tempPosTableData.Add(CreatePosTableDataRowFromOpt(opt));
                }

                mainForm.UpdatePositionTableData(tempPosTableData);
                mainForm.UpdateTotalInfoTable(new double[]
                {
                    Math.Round(posManager.CalculatePositionCurPnL(), 0),
                    Math.Round(posManager.CalculatePositionPnL(), 2),
                    Math.Round(posManager.FixedPnL, 2),
                    Math.Round(posManager.TotalDelta, 4),
                    Math.Round(posManager.TotalGamma, 4),
                    Math.Round(posManager.TotalVega, 4),
                    Math.Round(posManager.TotalTheta, 4)
                });

                double minStr = dataCollector.CalculateMinImportantStrike();
                double maxStr = dataCollector.CalculateMaxImportantStrike();



                for (double i = minStr; i <= maxStr; i += Settings.Default.StrikeStep)
                {
                    tempPosChartData.Add(new double[]
                    {
                        i,
                        posManager.CalculateCurApproxPnL(i),
                        posManager.CalculateExpirationPnL(i)
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

        private void MainFormOnStartUpClick(object sender, EventArgs e)
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
                dataCollector.OnActualPosChanged += DataCollector_OnActualPosChanged;

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

        private void DataCollector_OnActualPosChanged(object sender, TerminalPosEventArgs e)
        {
            if (e.cls == DerivativesClasses.FUTURES)
            {
                dataCollector.GetBasicFutures().Position.Quantity = e.actualPos;
                quikPosManager.Futures = dataCollector.GetBasicFutures();
            }
            else if (e.cls == DerivativesClasses.OPTIONS)
            {
                dataCollector.GetOption(e.ticker).Position.Quantity = e.actualPos;
                quikPosManager.AddOrChangeExistingOptionsPosition(dataCollector.GetOption(e.ticker));
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

        private string[] CreateActualPosTableDataRowFromFut(Futures fut)
        {
            string[] rowData = new string[10];
            int indexCount = 4;

            rowData[0] = "F";
            rowData[1] = "0";
            rowData[2] = Convert.ToString(fut.Position.EnterPrice);
            rowData[3] = Convert.ToString(fut.Position.Quantity);
            rowData[4] = Convert.ToString(fut.Position.GetMarketPriceToClose(fut.GetTradeBlotter()));
            rowData[5] = Convert.ToString(fut.Position.CalcCurrentPnL(fut.GetTradeBlotter()));
            rowData[6] = Convert.ToString(fut.Position.CalcCurrentPnLInCurrency(fut.GetTradeBlotter(), fut.PriceStep, fut.PriceStepValue));
            rowData[7] = Convert.ToString(0.0);
            rowData[8] = Convert.ToString(0.0);
            rowData[9] = Convert.ToString(dataCollector.GetBasicFutures().MarginRequirement);

            return rowData;

        }

        private string[] CreateActualPosTableDataRowFromOpt(Option opt)
        {
            string[] rowData = new string[10];
            int indexCount = 4;

            rowData[0] = opt.OptionType == OptionType.Call ? "C" : "P";
            rowData[1] = Convert.ToString(opt.Strike);
            rowData[2] = Convert.ToString(opt.Position.EnterPrice);
            rowData[3] = Convert.ToString(opt.Position.Quantity);
            rowData[4] = Convert.ToString(opt.Position.GetMarketPriceToClose(opt.GetTradeBlotter()));
            rowData[5] = Convert.ToString(opt.Position.CalcCurrentPnL(opt.GetTradeBlotter()));
            rowData[6] = Convert.ToString(opt.Position.CalcCurrentPnLInCurrency(opt.GetTradeBlotter(), opt.PriceStep, opt.PriceStepValue));
            rowData[7] = Convert.ToString(dataCollector.GetCoverMarginRequirement(opt.Strike, opt.OptionType));
            rowData[8] = Convert.ToString(dataCollector.GetNotCoverMarginRequirement(opt.Strike, opt.OptionType));
            rowData[9] = Convert.ToString(dataCollector.GetBuyerMarginRequirement(opt.Strike, opt.OptionType));

            return rowData;
        }

        private PositionManager ParseUserArgsPositionForManager(PositionManager manager, List<string[]> userData)
        {
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
                    throw new NotImplementedException();
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

                        manager.AddOption(Option.GetFakeOption(OptionType.Call, strike, enterPrice,
                            remainingDays, quantity, futBlotter, optBlotter, priceStep, priceVal));
                    }
                    catch (QuikDdeException e1)
                    {
                        mainForm.UpdateMessageWindow(e1.Message);
                        LOGGER.Error("ParseUserArgs, exception in call section: {0}", e1.ToString());
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

                        manager.AddOption(Option.GetFakeOption(OptionType.Put, strike, enterPrice,
                            remainingDays, quantity, futBlotter, optBlotter, priceStep, priceVal));
                    }
                    catch (QuikDdeException e1)
                    {
                        mainForm.UpdateMessageWindow(e1.Message);
                        LOGGER.Error("ParseUserArgs, exception in put section: {0}", e1.ToString());
                    }
                }
                else if (type.Equals(UserPosTableTypes.F))
                {
                    futBlotter = dataCollector.GetBasicFutures().GetTradeBlotter();
                    priceStep = dataCollector.GetBasicFutures().PriceStep;
                    priceVal = dataCollector.GetBasicFutures().PriceStepValue;

                    manager.AddFutures(Futures.GetFakeFutures(enterPrice, quantity, futBlotter, priceStep,
                        priceVal));
                }
                else
                {
                    mainForm.UpdateMessageWindow("incorrect type of instrument: " + type);
                    LOGGER.Error("ParseUserArgs, incorrect type of instrument, futures section: {0}", type);
                }
            }

            return manager;
        }

    }
}