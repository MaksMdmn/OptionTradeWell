using System;
using NLog;
using OptionsTradeWell.model.exceptions;
using OptionsTradeWell.Properties;

namespace OptionsTradeWell.model
{
    public static class GreeksCalculator
    {
        public static double DAYS_IN_YEAR = Settings.Default.DaysInYear;
        public static double MAX_VOLA_VALUE = Settings.Default.MaxValueOfImplVol;
        public static int NUMBER_OF_DECIMAL_PLACES = Settings.Default.RoundTo;
        private static Logger LOGGER = LogManager.GetCurrentClassLogger();

        public static double CalculateDelta(OptionType type, double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
            LOGGER.Trace("requested delta calculation with following args: type={0}, spotPrice={1}, strike={2}, daysLeft={3}, daysInYear={4}, vola={5}",
                type, spotPrice, strike, daysLeft, daysInYear, vola);
            double result = 0d;

            switch (type)
            {
                case OptionType.Call:
                    result = CalculateDistributionOfStNrmDstr(Calculate_d1(spotPrice, strike, daysLeft, daysInYear, vola));
                    break;
                case OptionType.Put:
                    result = CalculateDistributionOfStNrmDstr(Calculate_d1(spotPrice, strike, daysLeft, daysInYear, vola)) - 1;
                    break;
                default:
                    break;
            }

            return GetRoundValue(result);
        }

        public static double CalculateDelta(OptionType type, double distr_d1)
        {
            LOGGER.Trace("requested delta calculation with following args: type={0}, distr_d1={1}",
                type, distr_d1);
            double result = 0d;

            switch (type)
            {
                case OptionType.Call:
                    result = distr_d1;
                    break;
                case OptionType.Put:
                    result = distr_d1 - 1;
                    break;
                default:
                    break;
            }
            return GetRoundValue(result);
        }

        public static double CalculateGamma(double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
            LOGGER.Trace("requested gamma calculation with following args: spotPrice={0}, strike={1}, daysLeft={2}, daysInYear={3}, vola={4}",
                spotPrice, strike, daysLeft, daysInYear, vola);
            double optionTime = daysLeft / daysInYear;
            double d1 = Calculate_d1(spotPrice, strike, daysLeft, daysInYear, vola);

            return GetRoundValue(GreeksDistribution(d1) / (spotPrice * vola * Math.Sqrt(optionTime)));
        }

        public static double CalculateGamma(double spotPrice, double distr_d1, double optionTime, double vola)
        {
            LOGGER.Trace("requested gamma calculation with following args: spotPrice={0}, distr_d1={1}, optionTime={2}, vola={3}",
                spotPrice, distr_d1, optionTime, vola);
            return GetRoundValue(distr_d1 / (spotPrice * vola * Math.Sqrt(optionTime)));
        }

        public static double CalculateVega(double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
            LOGGER.Trace("requested vega calculation with following args: spotPrice={0}, strike={1}, daysLeft={2}, daysInYear={3}, vola={4}",
                spotPrice, strike, daysLeft, daysInYear, vola);
            double optionTime = daysLeft / daysInYear;
            double d1 = Calculate_d1(spotPrice, strike, daysLeft, daysInYear, vola);

            return GetRoundValue((spotPrice * Math.Sqrt(optionTime) * GreeksDistribution(d1)) / 100);
        }

        public static double CalculateVega(double spotPrice, double distr_d1, double optionTime)
        {
            LOGGER.Trace("requested gamma calculation with following args: spotPrice={0}, distr_d1={1}, optionTime={2}",
                spotPrice, distr_d1, optionTime);
            return GetRoundValue((spotPrice * Math.Sqrt(optionTime) * distr_d1) / 100);
        }

        public static double CalculateTheta(double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
            LOGGER.Trace("requested theta calculation with following args: spotPrice={0}, strike={1}, daysLeft={2}, daysInYear={3}, vola={4}",
                spotPrice, strike, daysLeft, daysInYear, vola);
            double optionTime = daysLeft / daysInYear;
            double d1 = Calculate_d1(spotPrice, strike, daysLeft, daysInYear, vola);

            return GetRoundValue(-(spotPrice * vola * GreeksDistribution(d1)) / (2 * Math.Sqrt(optionTime)) / daysInYear);
        }

        public static double CalculateTheta(double spotPrice, double ditsr_d1, double daysInYear, double optionTime, double vola, bool overloadProblem_nvm)
        {
            LOGGER.Trace("requested theta calculation (method with overload hardcore bool param) with following args: spotPrice={0}, strike={1}, daysLeft={2}, daysInYear={3}, vola={4}",
                spotPrice, ditsr_d1, daysInYear, optionTime, vola);
            return GetRoundValue(-(spotPrice * vola * ditsr_d1) / (2 * Math.Sqrt(optionTime)) / daysInYear);
        }

        public static double CalculateOptionPrice_BS(OptionType type, double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
            LOGGER.Trace("requested option price calculation with following args: type={0}, spotPrice={1}, strike={2}, daysLeft={3}, daysInYear={4}, vola={5}",
                type, spotPrice, strike, daysLeft, daysInYear, vola);
            double d1 = 0.0;
            double d2 = 0.0;
            double interestRate = 0.0;
            double optionTime = daysLeft / daysInYear;
            double dBlackScholes = 0.0;

            d1 = Calculate_d1(spotPrice, strike, daysLeft, daysInYear, vola);
            d2 = Calculate_d2(spotPrice, strike, daysLeft, daysInYear, vola);

            switch (type)
            {
                case OptionType.Call:
                    dBlackScholes = spotPrice * CalculateDistributionOfStNrmDstr(d1) - strike * Math.Exp(-interestRate * optionTime) * CalculateDistributionOfStNrmDstr(d2);
                    break;
                case OptionType.Put:
                    dBlackScholes = strike * Math.Exp(-interestRate * optionTime) * CalculateDistributionOfStNrmDstr(-d2) - spotPrice * CalculateDistributionOfStNrmDstr(-d1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return dBlackScholes;
        }
        public static double Calculate_d1(double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
            LOGGER.Trace("requested d1 calculation with following args: spotPrice={0}, strike={1}, daysLeft={2}, daysInYear={3}, vola={4}",
                spotPrice, strike, daysLeft, daysInYear, vola);
            double interestRate = 0.0;
            double optionTime = daysLeft / daysInYear;

            double tempRes = (Math.Log(spotPrice / strike) + (interestRate + (vola * vola) / 2.0) * optionTime) / (vola * Math.Sqrt(optionTime));

            return (Math.Log(spotPrice / strike) + (interestRate + (vola * vola) / 2.0) * optionTime) / (vola * Math.Sqrt(optionTime));
        }
        public static double Calculate_d2(double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
            LOGGER.Trace("requested d1 calculation with following args: spotPrice={0}, strike={1}, daysLeft={2}, daysInYear={3}, vola={4}",
                spotPrice, strike, daysLeft, daysInYear, vola);
            double optionTime = daysLeft / daysInYear;

            return Calculate_d1(spotPrice, strike, daysLeft, daysInYear, vola) - vola * Math.Sqrt(optionTime);
        }
        public static double CalculateDistributionOfStNrmDstr(double value)
        {
            LOGGER.Trace("requested calculation of distribution of following value: {0}", value);
            double result;

            double L = 0.0;
            double K = 0.0;
            double dCND = 0.0;
            const double a1 = 0.31938153;
            const double a2 = -0.356563782;
            const double a3 = 1.781477937;
            const double a4 = -1.821255978;
            const double a5 = 1.330274429;
            L = Math.Abs(value);
            K = 1.0 / (1.0 + 0.2316419 * L);
            dCND = 1.0 - 1.0 / Math.Sqrt(2 * Convert.ToDouble(Math.PI.ToString())) *
                Math.Exp(-L * L / 2.0) * (a1 * K + a2 * K * K + a3 * Math.Pow(K, 3.0) +
                a4 * Math.Pow(K, 4.0) + a5 * Math.Pow(K, 5.0));

            if (value < 0)
            {
                result = 1.0 - dCND;
            }
            else
            {
                result = dCND;
            }
            return result;
        }

        public static double GreeksDistribution(double value)
        {
            LOGGER.Trace("requested 'greeks' distribution of following value: {0}", value);
            double result = Math.Exp(value * value * 0.5 * -1) / (Math.Sqrt(2 * Math.PI));
            return result;
        }

        public static double CalculateImpliedVolatility(OptionType type, double spotPrice, double strike, double daysLeft, double daysInYear, double optionPrice, double volaGuess)
        {
            LOGGER.Trace("requested implVolatility calculation with following args: type={0}, spotPrice={1}, strike={2}, daysLeft={3}, daysInYear={4}, optionPrice={5}, guess={6}",
                type, spotPrice, strike, daysLeft, daysInYear, optionPrice, volaGuess);
            double dVol = 0.00001;
            double epsilon = 0.00001;
            double maxIterNumber = 100;
            double vol1 = volaGuess;
            int i = 1;

            double vol2 = 0.0;
            double tempVal1 = 0.0;
            double tempVal2 = 0.0;
            double dx = 0.0;


            while (true)
            {
                tempVal1 = CalculateOptionPrice_BS(type, spotPrice, strike, daysLeft, daysInYear, vol1);
                vol2 = vol1 - dVol;
                tempVal2 = CalculateOptionPrice_BS(type, spotPrice, strike, daysLeft, daysInYear, vol2);
                dx = (tempVal2 - tempVal1) / dVol;

                if (Math.Abs(dx) < epsilon || i == maxIterNumber)
                {
                    break;
                }

                vol1 = vol1 - (optionPrice - tempVal1) / dx;

                i++;
            }

            //double tempResult = GetRoundValue(vol1);

            //return tempResult > MAX_VOLA_VALUE || tempResult < 0 ? 0.00 : tempResult;

            return GetRoundValue(vol1);
        }

        public static double GetFilteredVolatilityValue(double value)
        {
            return value > MAX_VOLA_VALUE || value < 0 ? 0.00 : value;
        }

        public static double CalculateOptionPnLOnExpiration(Option option, double expirPrice)
        {
            double result = 0.0;

            if (option.Position.Quantity == 0)
            {
                throw new BasicModelException("option's position is zero, PnL calculations are impossible to do.");
            }
            else
            {
                if (option.OptionType == OptionType.Call)
                {
                    double deltaInPrices = expirPrice - option.Strike;
                    double optPremium = -1 * option.Position.EnterPrice * option.Position.Quantity;
                    int optPosizion = option.Position.Quantity;

                    if (option.Position.Quantity > 0)
                    {
                        if (expirPrice > option.Strike)
                        {
                            result = deltaInPrices * optPosizion + optPremium;
                        }
                        else
                        {
                            result = optPremium;
                        }
                    }
                    else
                    {
                        if (expirPrice < option.Strike)
                        {
                            result = optPremium;
                        }
                        else
                        {
                            result = deltaInPrices * optPosizion + optPremium;
                        }
                    }
                }

                if (option.OptionType == OptionType.Put)
                {
                    double deltaInPrices = option.Strike - expirPrice;
                    double optPremium = -1 * option.Position.EnterPrice * option.Position.Quantity;
                    int optPosizion = option.Position.Quantity;

                    if (option.Position.Quantity > 0)
                    {
                        if (expirPrice < option.Strike)
                        {
                            result = deltaInPrices * optPosizion + optPremium;
                        }
                        else
                        {
                            result = optPremium;
                        }
                    }
                    else
                    {
                        if (expirPrice > option.Strike)
                        {
                            result = optPremium;
                        }
                        else
                        {
                            result = deltaInPrices * optPosizion + optPremium;
                        }
                    }
                }
            }

            return result;
        }


        private static double GetRoundValue(double value)
        {
            LOGGER.Trace("requested rounding of value: {0}, decimal places: {1}", value, NUMBER_OF_DECIMAL_PLACES);
            return Math.Round(value, NUMBER_OF_DECIMAL_PLACES);
        }


    }
}