using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Calculator.Enums;
using static System.Net.Mime.MediaTypeNames;


namespace Calculator
{
    internal class UserInterface
    {
        private static int keyX = 0;
        private static int keyY = 0;
        private static readonly string decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        internal static void DrawCalculator()
        {
            Console.Clear();

            DrawBox<Perimeter>(); 

            DrawBox<Display>();

            DrawBox<SavedCalculations>();

            keypad((int)Perimeter.top, (int)Perimeter.left);
            ShowHelp("calculator");
        }
        internal static void ShowHelp(string helpType, bool ClearListHelp = false)
        {
            string[] calculatorHelp = new string[]
            {"Calculator Help:",
            " ",
            "s = Sine        => 5s Calculates Sine(5)",
            "c = Cosine      => 5c Calculates Cosine(5)",
            "t = Tangent     => 5t Calculates Tangent(5)",
            "π = Pi          => Inserts the value of Pi",
            "√ = Square Root => 9√ Calculates Square Root of 9",
            "% = Percentage  => 50% Calculates 50 percent of the second operand",
            "^ = Power       => 2^3 Calculates 2 to the power of 3",
            " ",
            "- Use Arrow Keys to navigate, Spacebar to select.",
            "- Backspace or ⇐ to delete last digit.",
            "- Use ⮜ to clear the operand.",
            "- Press L to access the Saved Calculations List,",
            "- Esc to exit."
            };

            string[] listHelp = new string[]
            {"Saved Calculations Help:",
            " ",
            "- Use Arrow Keys to navigate the list.",
            "- Press Spacebar to select a calculation.",
            "- Press Delete to remove a calculation from the list.",
            "- Press X to delete the whole list."
            };
            string[] helpToPrint = Array.Empty<string>();
            int top = 0;

            if (helpType == "calculator")
            {
                helpToPrint = calculatorHelp;
                top = 12;
            }
            else if (helpType == "list")
            {
                top = 3;
                helpToPrint = listHelp;
            }

            for (int i = 0; i < helpToPrint.Length; i++)
            {
                Console.SetCursorPosition(45, top + i);
                if (ClearListHelp)
                    Console.Write(new string(' ', helpToPrint[i].Length));
                else
                    Console.Write(helpToPrint[i]);
            }
        }

        internal static void ShowTotalCalculations(int totalCalculations)
        {
            string totalCalcString = $"Calculations: {totalCalculations}";
            Console.SetCursorPosition((int)Perimeter.left + 2, (int)Perimeter.top + 1);
            Console.Write(totalCalcString.PadRight((int)Perimeter.width - 4));
        }

        internal static string keypad(int top, int left)
        {

            string[,] buttons = new string[5, 5]
            {
            { "s", "c", "t", "π", "%" },
            { "7", "8", "9", "^", "/" },
            { "4", "5", "6", "√", "*" },
            { "1", "2", "3", "⇐", "-" },
            { "0", decimalSeparator,"=", "⮜", "+" },

            };

            int buttonWidth = 5;
            int buttonHeight = 3;
            int buttonTopStart = top + 13;
            int buttonLeftStart = left + 3;

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    int bx = buttonLeftStart + (col * buttonWidth);
                    int by = buttonTopStart + (row * buttonHeight);

                    KeyPadDrawBox(bx, by, buttonWidth, buttonHeight);
                    Console.SetCursorPosition(bx + 2, by + 1);
                    if (row == keyY && col == keyX)
                    {
                        AnsiConsole.Markup($"[blue]{buttons[row, col]}[/]");
                    }
                    else
                        Console.Write(buttons[row, col]);
                }
            }
            return buttons[keyY, keyX];
        }

        private static void KeyPadDrawBox(int left, int top, int width, int height)
        {
            Console.SetCursorPosition(left, top);
            Console.Write("┌" + new string('─', width - 2) + "┐");

            // sides
            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write("│" + new string(' ', width - 2) + "│");
            }

            // bottom border
            Console.SetCursorPosition(left, top + height - 1);
            Console.Write("└" + new string('─', width - 2) + "┘");
        }

        static void DrawBox<T>() where T : Enum
        {
            int left = (int)Enum.Parse(typeof(T), "left");
            int top = (int)Enum.Parse(typeof(T), "top");
            int width = (int)Enum.Parse(typeof(T), "width");
            int height = (int)Enum.Parse(typeof(T), "height");

            // top border
            Console.SetCursorPosition(left, top);
            Console.Write("┌" + new string('─', width - 2) + "┐");

            // sides
            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write("│" + new string(' ', width - 2) + "│");
            }

            // bottom border
            Console.SetCursorPosition(left, top + height - 1);
            Console.Write("└" + new string('─', width - 2) + "┘");
        }

        internal static string GetOperand(string operand = "", bool firstOperand = true)
        {
            bool decimalUsed = operand.Contains(',');
            bool exit = false;
            bool firstKey = true;

            const int maxLength = 24;
            const int displayRow = 2;
            string clearLine = new string(' ', maxLength);
            string permitedOperators = "+-*/π√%tTcCsSlL^";

            while (!exit)
            {
                string key = GetKeypadInput();

                switch (key)
                {
                    case string s when decimal.TryParse(s, out _):
                        if (firstKey)
                        {
                            operand = "";
                            ShowOperand(clearLine, displayRow);
                            firstKey = false;
                        }

                        if (operand.Length < maxLength)
                        {
                            operand += key;
                            ShowOperand(operand, displayRow);
                        }
                        break;

                    case ",":
                    case ".":
                        if (!decimalUsed)
                        {
                            if (firstKey)
                            {
                                operand = "0" + decimalSeparator;
                                firstKey = false;
                            }
                            else if (operand.Length < maxLength)
                            {
                                operand += decimalSeparator;
                            }

                            ShowOperand(operand, displayRow);
                            decimalUsed = true;
                        }
                        break;
                    case "π":
                        operand = Math.PI.ToString("F10");
                        ShowOperand(clearLine, displayRow);
                        ShowOperand(operand, displayRow);
                        firstKey = false;
                        decimalUsed = true;
                        break;
                    /*case "%":
                        if (!firstKey)
                        {
                            operand = operand+"%";
                            ShowOperand(clearLine, displayRow);
                            ShowOperand(operand, displayRow);
                            exit = true;
                        }
                        break;
                    */

                    case "⇐":
                        if (operand.Length > 0)
                        {
                            operand = operand[..^1];
                            ShowOperand(clearLine, displayRow);
                            ShowOperand(operand, displayRow);

                            decimalUsed = operand.Contains(',');

                            if (operand.Length == 0)
                            {
                                operand = "0";
                                ShowOperand(clearLine, displayRow);
                                ShowOperand(operand, displayRow);

                                operand = "";
                                firstKey = true;
                                decimalUsed = false;
                            }
                        }
                        break;
                    case "⮜":
                        operand = "0";
                        ShowOperand(clearLine, displayRow);
                        ShowOperand(operand, displayRow);

                        operand = "";
                        firstKey = true;
                        decimalUsed = false;
                        break;
                    case "l":
                    case "L":
                        operand = GetSavedOperation();
                        ShowSavedOperations(justShow: true);
                        ShowOperand(clearLine, displayRow);
                        ShowOperand(operand, displayRow);
                        firstKey = false;
                        if (operand.Contains(decimalSeparator))
                        {
                            decimalUsed = true;
                        }
                        break;


                    case "escape":
                        operand = "escape";
                        exit = true;
                        break;

                    default:
                        bool isNumber = decimal.TryParse(operand, out _);
                        bool isEquals = key == "=";

                        if (isNumber && operand.Length < maxLength)
                        {
                            if (!firstOperand && isEquals)
                            {
                                exit = true;
                                operand += key;
                            }
                            else if (firstOperand && !isEquals && permitedOperators.Contains(key))
                            {
                                exit = true;
                                operand += key;
                                ShowOperand(operand, displayRow);
                            }
                        }
                        break;
                }
            }

            return operand;
        }

        static internal string GetKeypadInput()
        {
            bool exit = false;

            string capturedKey = "";

            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); // true hides the pressed key from screen

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        keyY--;
                        keyY = WrapAround(row: keyY);
                        keypad((int)Enums.Perimeter.top, (int)Enums.Perimeter.left);
                        break;
                    case ConsoleKey.DownArrow:
                        keyY++;
                        keyY = WrapAround(row: keyY);
                        keypad((int)Enums.Perimeter.top, (int)Enums.Perimeter.left);
                        break;
                    case ConsoleKey.LeftArrow:
                        keyX--;
                        keyX = WrapAround(col: keyX);
                        keypad((int)Enums.Perimeter.top, (int)Enums.Perimeter.left);
                        break;
                    case ConsoleKey.RightArrow:
                        keyX++;
                        keyX = WrapAround(col: keyX);
                        keypad((int)Enums.Perimeter.top, (int)Enums.Perimeter.left);
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        capturedKey = "escape";
                        return capturedKey;
                    case ConsoleKey.Spacebar:
                        capturedKey = keypad((int)Enums.Perimeter.top, (int)Enums.Perimeter.left);
                        exit = true;
                        return capturedKey;
                    case ConsoleKey.Backspace:
                        capturedKey = "⇐";
                        exit = true;
                        break;

                    default:
                        capturedKey = keyInfo.KeyChar.ToString();
                        if (capturedKey != "")
                            exit = true;
                        break;
                }
            } while (!exit);
            return capturedKey;
        }

        private static int WrapAround(int row = -2, int col = -2)
        {
            int colRow = 0;

            if (col == -2)
            {
                if (row < 0)
                    return 4;
                if (row > 4)
                    return 0;
                colRow = row;
            }
            if (row == -2)
            {
                if (col < 0)
                    return 4;
                if (col > 4)
                    return 0;
                colRow = col;
            }
            return colRow;
        }

        internal static void ShowOperand(string operand, int row, string bigValue = "")
        {
            string maxLength = new string(' ', 25);
            string scientific = "";
            Console.SetCursorPosition(((int)Display.left + (int)Display.width - 1) - maxLength.Length, (int)Display.top + row);
            Console.Write(maxLength);

            if (decimal.TryParse(bigValue, out decimal bigNumber))
            {
                scientific = bigNumber.ToString("E");
            }

            if (operand.Length < 26)
            {
                Console.SetCursorPosition(((int)Display.left + (int)Display.width - 1) - (operand.Length), (int)Display.top + row);

                Console.Write(operand);
            }
            else if (bigValue.Length < 25)
            {
                Console.SetCursorPosition(((int)Display.left + (int)Display.width - 1) - (bigValue.Length), (int)Display.top + row);
                Console.Write(bigValue);
            }
            else if (scientific.Length < 25 && (scientific.Length > 0))
            {
                Console.SetCursorPosition(((int)Display.left + (int)Display.width - 1) - (scientific.Length), (int)Display.top + row);
                Console.Write(scientific);
            }
            else
            {
                operand = "Number to big to display";
                Console.SetCursorPosition(((int)Display.left + (int)Display.width - 1) - (operand.Length), (int)Display.top + row);
                Console.Write(operand);
            }
        }

        internal static (string fisrtOperand, string secondOperand) GetFormattedOperands()
        {
            string firstOperand = "";
            string secondOperand = "";
            string operand = "";
            string startOperand = "0";
            string clearOperand = new string(' ', 25);
            string onlyOneOperand = "cCsStT√";

            ShowOperand(startOperand, 2);
            firstOperand = GetOperand(operand, true);
            if (!firstOperand.Any(c => onlyOneOperand.Contains(c)))
            {
                if (firstOperand == "escape") return (firstOperand, startOperand);
                ShowOperand(clearOperand, 2);
                ShowOperand(clearOperand, 1);
                ShowOperand(firstOperand, 1);
                ShowOperand(startOperand, 2);
                secondOperand = GetOperand(firstOperand: false);
            }
            return (firstOperand, secondOperand);
        }

        internal static string ShowSavedOperations(int startPosition = 0, int listPosition = 0, bool justShow = false)
        {
            int count = 0;
            string operand = "";
            for (int i = 0; i < (int)Enums.SavedCalculations.height - 2; i++)
            {
                Console.SetCursorPosition((int)Enums.SavedCalculations.left + 1, (int)Enums.SavedCalculations.top + 1 + count);
                if (SavedOperations.List.Count > count + startPosition)
                {
                    if (listPosition == i && justShow == false)
                    {
                        AnsiConsole.Markup($"[blue]{SavedOperations.List[startPosition + i].PadLeft((int)Enums.SavedCalculations.width - 2)}[/]");
                        operand = SavedOperations.List[startPosition + i];
                    }
                    else
                        Console.WriteLine(SavedOperations.List[startPosition + i].PadLeft((int)Enums.SavedCalculations.width - 2));
                    count++;
                }
                else
                {
                    Console.WriteLine(new string(' ', (int)Enums.SavedCalculations.width - 2));
                    count++;
                }
            }
            return operand;
        }

        internal static string GetSavedOperation()
        {
            bool exit = false;
            int listPosition = 0;
            int startPosition = 0;
            string operand = "";

            ShowSavedOperations(startPosition, listPosition);
            ShowHelp("list");
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        listPosition--;
                        if (listPosition < 0)
                        {
                            listPosition = 0;
                            if (startPosition > 0)
                            {
                                startPosition--;
                            }
                        }
                        ShowSavedOperations(startPosition, listPosition);
                        break;
                    case ConsoleKey.DownArrow:
                        listPosition++;
                        if (listPosition > (int)Enums.SavedCalculations.height - 3)
                        {
                            listPosition = (int)Enums.SavedCalculations.height - 3;
                            if (SavedOperations.List.Count > startPosition + 5)
                            {
                                startPosition++;
                            }
                        }
                        ShowSavedOperations(startPosition, listPosition);
                        break;
                    case ConsoleKey.Spacebar:
                        operand = ShowSavedOperations(startPosition, listPosition);
                        exit = true;
                        break;
                    case ConsoleKey.Delete:
                        SavedOperations.RemoveAt(listPosition + startPosition);
                        if (SavedOperations.List.Count == 0)
                        {
                            ShowHelp("list", ClearListHelp: true);
                            return "0";
                        }
                        if (listPosition + startPosition > SavedOperations.List.Count - 1)
                        {
                            if (startPosition > 0)
                            {
                                startPosition--;
                            }
                            else
                            {
                                listPosition--;
                            }
                        }
                        ShowSavedOperations(startPosition, listPosition);
                        break;
                    case ConsoleKey.X:
                        SavedOperations.Clear();
                        ShowHelp("list", ClearListHelp: true);
                        return "0";
                    default:
                        break;
                }
            } while (!exit);
            ShowHelp("list", ClearListHelp: true);
            return operand;
        }
    }
}
