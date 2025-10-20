using Calculator; 
using Spectre.Console;
using System;




internal class Program
{
    static void Main()

    {
        // Preload some saved operations for demonstration

        /*
        Random random = new Random();
        for (int i = 0; i < 10; i++)
        {
            double number = Math.Round(random.NextDouble() * 100, 2); 
            SavedOperations.Add(number.ToString());
        }
        */

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        bool finishProgram = false;
        int totalCalculations = 0;

        Console.Title = "Text Calculator";
        UserInterface.DrawCalculator();
        do
        {
            UserInterface.ShowOperand(new string(' ', 25), 2);
            UserInterface.ShowTotalCalculations(totalCalculations++);

            var (firtstOperand, secondOperand) = UserInterface.GetFormattedOperands();
            if (firtstOperand == "escape" || secondOperand == "escape") 
            { 
                finishProgram = true; 
                break; 
            }
            var result = CalculatorEngine.Calculate(firtstOperand, secondOperand);
            UserInterface.ShowOperand(result[1], 1, result[0]);
            firtstOperand = "";
            secondOperand = "";
            UserInterface.ShowSavedOperations(justShow:true);

        } while (!finishProgram);

        Console.SetCursorPosition(45,8);
        Console.WriteLine("E X I T E D   C A L C U L A T O R");
        Console.SetCursorPosition(45,10);
        Console.WriteLine("Press any key to close...");
        Console.ReadKey(true);
    }

}