using System;

namespace OptionsTradeWell.model
{
    public static class GreeksCalculator
    {

        public static double DAYS_IN_YEAR = 365;
        public static double MAX_VOLA_VALUE = 3.0;
        public static int NUMBER_OF_DECIMAL_PLACES = 4;

        public static double CalculateDelta(OptionType type, double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
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

        public static double CalculateDelta(OptionType type, double ditsr_d1)
        {
            double result = 0d;

            switch (type)
            {
                case OptionType.Call:
                    result = ditsr_d1;
                    break;
                case OptionType.Put:
                    result = ditsr_d1 - 1;
                    break;
                default:
                    break;
            }
            return GetRoundValue(result);
        }

        public static double CalculateGamma(double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
            double optionTime = daysLeft / daysInYear;
            double d1 = Calculate_d1(spotPrice, strike, daysLeft, daysInYear, vola);

            return GetRoundValue(GreeksDistribution(d1) / (spotPrice * vola * Math.Sqrt(optionTime)));
        }

        public static double CalculateGamma(double spotPrice, double ditsr_d1, double optionTime, double vola)
        {
            return GetRoundValue(ditsr_d1 / (spotPrice * vola * Math.Sqrt(optionTime)));
        }

        public static double CalculateVega(double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
            double optionTime = daysLeft / daysInYear;
            double d1 = Calculate_d1(spotPrice, strike, daysLeft, daysInYear, vola);

            return GetRoundValue((spotPrice * Math.Sqrt(optionTime) * GreeksDistribution(d1)) / 100);
        }

        public static double CalculateVega(double spotPrice, double ditsr_d1, double optionTime)
        {
            return GetRoundValue((spotPrice * Math.Sqrt(optionTime) * ditsr_d1) / 100);
        }

        public static double CalculateTheta(double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
            double optionTime = daysLeft / daysInYear;
            double d1 = Calculate_d1(spotPrice, strike, daysLeft, daysInYear, vola);

            return GetRoundValue(-(spotPrice * vola * GreeksDistribution(d1)) / (2 * Math.Sqrt(optionTime)) / daysInYear);
        }

        public static double CalculateTheta(double spotPrice, double ditsr_d1, double daysInYear, double optionTime, double vola, bool overloadProblem_nvm)
        {
            return GetRoundValue(-(spotPrice * vola * ditsr_d1) / (2 * Math.Sqrt(optionTime)) / daysInYear);
        }

        public static double CalculateOptionPrice_BS(OptionType type, double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
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
            double interestRate = 0.0;
            double optionTime = daysLeft / daysInYear;

            return (Math.Log(spotPrice / strike) + (interestRate + (vola * vola) / 2.0) * optionTime) / (vola * Math.Sqrt(optionTime));
        }
        public static double Calculate_d2(double spotPrice, double strike, double daysLeft, double daysInYear, double vola)
        {
            double optionTime = daysLeft / daysInYear;

            return Calculate_d1(spotPrice, strike, daysLeft, daysInYear, vola) - vola * Math.Sqrt(optionTime);
        }
        public static double CalculateDistributionOfStNrmDstr(double value)
        {
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


        public static double CalculateImpliedVolatility(OptionType type, double spotPrice, double strike, double daysLeft, double daysInYear, double optionPrice, double volaGuess)
        {
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

        public static double GreeksDistribution(double value)
        {
            double result = 1 / (Math.Sqrt(2 * Math.PI)) * Math.Exp(value * value * 0.5 * -1);
            return result;
        }

        public static double GetFilteredVolatilityValue(double value)
        {
            return value > MAX_VOLA_VALUE || value < 0 ? 0.00 : value;
        }


        private static double GetRoundValue(double value)
        {
            return Math.Round(value, NUMBER_OF_DECIMAL_PLACES);
        }


    }
}