using System; 
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class CalculatorEngine()
    {
        internal static string[] Calculate(string sFirstOperand, string sSecondOperand)
        {
            string oneOperandCalculation = "cCsStT√";
            string[] result = new string[2];

            if (sFirstOperand.Any(c => oneOperandCalculation.Contains(c)))
            {
                result = OneOperandCalulation(sFirstOperand);
            }
            else
            { 
                result = mathCalculation(sFirstOperand, sSecondOperand);
            }
            return (result);
        }

        internal static string[] OneOperandCalulation(string sOperand)
        {
            string[] result = new string[2];
            string operatorSymbol = sOperand.Substring(sOperand.Length - 1, 1);
            double.TryParse(sOperand.Substring(0, sOperand.Length - 1), out double operand);
            switch (operatorSymbol)
            {
                case "c":
                case "C":
                    result[0] = (Math.Cos(operand)).ToString("F5");
                    result[1] = $"Cos({operand}) = {result[0]}";
                    break;
                case "s":
                case "S":
                    result[0] = (Math.Sin(operand)).ToString("F5");
                    result[1] = $"Sin({operand}) = {result[0]}";
                    break;
                case "t":
                case "T":
                    result[0] = (Math.Tan(operand)).ToString("F5");
                    result[1] = $"Tan({operand}) = {result[0]}";
                    break;
                case "√":
                    result[0] = (Math.Sqrt(operand)).ToString("F5");
                    result[1] = $"√{operand} = {result[0]}";
                    break;
                default:
                    break;
            }
            SavedOperations.Add(result[0]);
            return result;
        }
        internal static string[] mathCalculation(string sFirstOperand, string sSecondOperand)
        {
            string[] result = new string[2];
            bool noError = true;
            string firstOperator = sFirstOperand.Substring(sFirstOperand.Length - 1, 1);
            double.TryParse(sFirstOperand.Substring(0, sFirstOperand.Length - 1), out double firstOperand);
            string secondOperator = sSecondOperand.Substring(sSecondOperand.Length - 1, 1);
            double.TryParse(sSecondOperand.Substring(0, sSecondOperand.Length - 1), out double secondOperand);




            switch (firstOperator)
            {
                case "+":
                    result[0] = (firstOperand + secondOperand).ToString();
                    result[1] = $"{firstOperand} + {secondOperand} = {result[0]}";
                    break;
                case "-":
                    result[0] = (firstOperand - secondOperand).ToString();
                    result[1] = $"{firstOperand} - {secondOperand} = {result[0]}";
                    break;
                case "*":
                    result[0] = (firstOperand * secondOperand).ToString();
                    result[1] = $"{firstOperand} * {secondOperand} = {result[0]}";
                    break;
                case "/":
                    if (secondOperand == 0)
                    {
                        result[0] = "";
                        result[1] = "Error: Division by zero";
                        noError = false;
                        break;
                    }
                    else
                    {
                        result[0] = (firstOperand / secondOperand).ToString();
                        result[1] = ($"{firstOperand} / {secondOperand} = {result[0]}");
                        break;
                    }
                case "^":
                    result[0] = (Math.Pow(firstOperand, secondOperand)).ToString();
                    result[1] = $"{firstOperand} ^ {secondOperand} = {result[0]}";
                    break;
                case "%":
                    result[0] = (firstOperand/100 * secondOperand).ToString();
                    result[1] = $"{firstOperand} % {secondOperand} = {result[0]}";
                    break;
                default:
                    break;
            }
            if (noError)
            {
                SavedOperations.Add(result[0]);
            }
            return result;
        }
    }


}

