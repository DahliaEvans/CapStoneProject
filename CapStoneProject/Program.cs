using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapStoneTestCode
{
    class Program
    {
        // ***************************
        // Title: The Derivative Calculator
        // Application Type: framework-Console
        // Description: The application takes the non-simplified derivative of a function of x (doesn't inclue, trig, logarithms, floating points, roots, etc.)
        // Author: Dahlia Evans
        // Date Created: 4/28/2019
        // Last Modified: 4/28/2019
        // ***************************

        static void Main(string[] args)
        {
            DisplayWelcomeScreen();
            while (true)
            {
                string userString;
                string output;
                Console.Clear();
                DisplayInstructions();

                userString = Console.ReadLine();
                if (userString.ToUpper() == "E")
                {
                    DisplayClosingScreen();
                    break;
                }
                output = CalculateDerivative(userString);
                if (output.StartsWith("(") && output.EndsWith(")"))
                {
                    output = output.Substring(1, output.Length - 2);
                }
                Console.WriteLine(" f'(x) = " + output);

                DisplayContinuePrompt();
            }
        }

        static void DisplayInstructions()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(" Please use the following syntax when entering a function:");
            Console.WriteLine(" When adding or subtracting terms, use a space before and after a plus sign or minus sign.");
            Console.WriteLine(" When multiplying or dividing terms DO NOT use a space before or after the multiplication or divisin sign.");
            Console.WriteLine(" Use x as your variable.");
            Console.WriteLine(" Use ^ to indicate that you are about to enter an exponent.");
            Console.WriteLine(" Use +, -, *, / for addition, subtraction, multiplication, and division.");
            Console.WriteLine(" Use parentheses when necessary (using parentheses incorrectly may result in an incorrect calculation).");
            Console.WriteLine(" Use integer values for coefficients and exponents.");
            Console.WriteLine(" Enter 'E' to exit.");
            Console.WriteLine(" Please note that this calculator finds the derivative of a function, but does not symplify that derivative.");
            Console.WriteLine();
            Console.WriteLine(" Please enter your function below.");
            Console.Write(" f(x) = ");
        }

        static string CalculateDerivative(string input)
        {

            if (TrySimple(input, out int a, out int n)) // simple term
            {
                a = n * a;
                n -= 1;
                return PrettifySingleTerm("x", a, n);
            }

            int depth = 0;
            int openIndex = 0;
            int closeIndex = 0;
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '(':
                        if (depth == 0)
                        {
                            openIndex = i;
                        }
                        depth++;
                        break;
                    case ')':
                        depth--;
                        if (depth == 0)
                        {
                            closeIndex = i;
                        }
                        break;
                    case ' ':
                        if (depth == 0)
                        {
                            if (input.Substring(i + 1, 2) == "+ ")
                            {
                                string additiveOutput = CalculateDerivative(input.Substring(0, i));
                                additiveOutput += " + ";
                                additiveOutput += CalculateDerivative(input.Substring(i + 3));
                                return "(" + additiveOutput + ")";
                            }
                            if (input.Substring(i + 1, 2) == "- ")
                            {
                                string additiveOutput = CalculateDerivative(input.Substring(0, i));
                                additiveOutput += " - ";
                                additiveOutput += CalculateDerivative(input.Substring(i + 3));
                                return "(" + additiveOutput + ")";
                            }
                            if (input[i + 1] == '/')
                            {
                                return "Error";
                            }
                        }
                        break;
                    case '*':
                        if (depth == 0)
                        {
                            string fx = input.Substring(0, i);
                            string fpx = CalculateDerivative(fx);
                            string gx = input.Substring(i + 1);
                            string gpx = CalculateDerivative(gx);
                            return fpx + "(" + gx + ")" + " + " + gpx + "(" + fx + ")";
                        }
                        break;
                    case '/':
                        if (depth == 0)
                        {
                            string fx = input.Substring(0, i);
                            string fpx = CalculateDerivative(fx);
                            string gx = input.Substring(i + 1);
                            string gpx = CalculateDerivative(gx);
                            string gxs = gx + " * " + gx;
                            return gx + "(" + fpx + ")" + " - " + fx + "(" + gpx + ")" + " / " + "(" + gxs + ")";
                        }
                        break;
                    default:
                        break;
                }
            }
            if (closeIndex == 0)
            {
                return "Error";
            }

            string aAsString = input.Substring(0, openIndex);
            string nAsString = input.Substring(closeIndex + 1);
            if (nAsString.Length > 0 && !nAsString.StartsWith("^"))
            {
                return "Error";
            }
            if (openIndex > 0 && !int.TryParse(aAsString, out a))
            {
                return "Error";
            }
            if (nAsString.Length > 1 && !int.TryParse(nAsString.Substring(1), out n))
            {
                return "Error";
            }
            string u = input.Substring(openIndex + 1, closeIndex - openIndex - 1);
            string output = PrettifySingleTerm("(" + u + ")", n * a, n - 1) + " * ";
            output += CalculateDerivative(u);

            return output;
        }

        static string PrettifySingleTerm(string u, int a, int n)
        {
            string output = null;

            if (a == 0)
            {
                return "0";
            }
            if (a != 1)
            {
                output = a.ToString();
            }
            if (n == 0)
            {
                if (a == 1)
                {
                    return "1";
                }
                return output;
            }
            if (n == 1)
            {
                return output + u;
            }
            return output + u + "^" + n.ToString();
        }

        static bool TrySimple(string input, out int a, out int n)
        {
            // variables:
            string aAsString;
            string nAsString;

            a = 1;
            n = 1;

            if (input.Contains("x"))
            {
                int xIndex = input.IndexOf("x");
                aAsString = input.Substring(0, xIndex);
                nAsString = input.Substring(xIndex + 1);
                if (nAsString.Length > 0 && !nAsString.StartsWith("^"))
                {
                    return false;
                }
                if (xIndex > 0 && !int.TryParse(aAsString, out a))
                {
                    return false;
                }
                if (nAsString.Length > 1 && !int.TryParse(nAsString.Substring(1), out n))
                {
                    return false;
                }

                return true;
            }
            n = 0;
            return int.TryParse(input, out a);
        }

        static void DisplayWelcomeScreen()
        {
            Console.WriteLine();
            Console.WriteLine("\tWelcome to The Derivative Calculator!");
            Console.WriteLine(" This application takes the non-simplified");
            Console.WriteLine(" derivative of a function of x that");
            Console.WriteLine(" doesn't inclue, trig, logarithms, floating points, roots, etc.");
            DisplayContinuePrompt();
        }

        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\tThank you for using The Derivative Calculator!");
            DisplayExitPrompt();
        }

        static void DisplayExitPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
