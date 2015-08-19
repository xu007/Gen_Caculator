using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleCaculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            RunCaculator();
        }

        public static void RunCaculator()
        {
            Console.WriteLine("Caculator is started, type 'exit' to end.");
            while (true)
            {
                Console.WriteLine("Please type in the expression:");
                var input = Console.ReadLine();
                if (string.Compare(input, "exit", StringComparison.OrdinalIgnoreCase) == 0) break;

                if (IsErrorInput(input))
                {
                    Console.WriteLine("Invalid expression, please try again...");
                }
                else
                {
                    try
                    {
                        var result = Caculate(input);
                        Console.WriteLine("Result = " + result);
                    }
                    catch (DivideByZeroException ed)
                    {
                        Console.WriteLine("Invalid expression (DivideByZero), please try again... ");
                    }
                    catch (ArithmeticException ea)
                    {
                        Console.WriteLine("Invalid expression (invalid operation), please try again... " + ea.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Invalid expression, please try again... " + e.Message);
                    }
                }
            }//while loop
        }

        private static int Caculate(string input)
        {
            var numArray = input.Split(Ops, StringSplitOptions.None);
            if (numArray.Length < 2) throw new ArgumentException("Invalid input numbers");

            string inputItem = numArray[0];
            int inputInt;
            if (!Int32.TryParse(inputItem.Trim(), out inputInt))
                throw new ArgumentException("Invalid input numbers");
            
            int result = inputInt;
            int start = inputItem.Length;
            int i = 1;
            while (i < numArray.Length)
            {
                inputItem = numArray[i];
                if (!Int32.TryParse(inputItem.Trim(), out inputInt))
                    throw new ArgumentException("Invalid input numbers");

                var op = input.Substring(start, 1);
                switch (op)
                {
                    case "+":
                        result += inputInt;
                        break;
                    case "-":
                        result -= inputInt;
                        break;
                    case "*":
                        result *= inputInt;
                        break;
                    case "/":
                        result /= inputInt;
                        break;
                    default:
                        throw new ArgumentException("Invalid input operator");
                }
                
                start += 1 + inputItem.Length;
                i++;
            }

            return result;
        }

        private static readonly string[] Ops = {"+", "-", "*", "/"};

        /// <summary>
        /// check for corner cases,
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static bool IsErrorInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (Ops.Any(op => input.StartsWith(op) || input.EndsWith(op))) return true;

            var numArray = input.Split(Ops, StringSplitOptions.None);
            if (numArray.Any(string.IsNullOrWhiteSpace)) return true;
            if (numArray.Any(i => Regex.IsMatch(i.Trim(), @"\D"))) return true;

            return false;
        }
    }//class
}
