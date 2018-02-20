using System;
using System.Collections.Generic;

namespace BotMathematics
{
    public class FormulaSolver
    {
        public List<string> steps;
        public List<string> Steps
        {
            get;
            set;
        }

        public static bool IsNumber(char c)
        {
            switch (c)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return true;
                default:
                    return false;
            }
        }
        public static bool IsLetter(char c)
        {
            switch ((c.ToString().ToLower())[0])
            {
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                    return true;
                default:
                    return false;
            }
        }

        public static bool ValidateFormula(string formula)
        {
            if(formula?.Length == 0)
            {
                return false;
            }

            formula = formula.Replace(" ", "");

            bool foundEqualSign = false;
            bool foundOperator = false;
            char previousOperator = ' ';
            int variablesFound = 0;

            foreach(char c in formula)
            {
                if (IsNumber(c))
                {
                    foundOperator = false; // If there was an operator, there now can be one again other than + and -
                    continue;
                }
                else if (IsLetter(c))
                {
                    foundOperator = false;
                    ++variablesFound;
                    continue;
                }
                else
                {
                    switch (c)
                    {
                        case '=':
                            if (foundEqualSign)
                            {
                                return false; // If one already was found, there shouldnt be a second one
                            }
                            foundEqualSign = true;
                            break;
                        case '-':
                        case '+':
                        case '*':
                        case '/':
                            if (foundOperator)
                            {
                                if(c == '*' || c == '/')
                                {
                                    // If there has been an operator before and there now comes an * or /,
                                    // then there is something wrong with the formula
                                    return false;
                                }
                                else
                                {
                                    if(previousOperator == '+' || previousOperator == '-')
                                    {
                                        // If the previous Operator was + or -, then there may be another + or -, but not * or /
                                        if(c == '*' || c == '/')
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            continue;
                    }
                }
            } // Foreach

            if(!foundEqualSign || variablesFound == 0)
            {
                return false;
            }
            return true;
        }

        private string[] SplitFormulaIntoSegments(string formula)
        {
            List<string> tempSegmentList = new List<string>();
            string currentSegment = "";
            char previousChar = ' ';

            foreach(char c in formula)
            {
                if (IsNumber(c))
                {
                    currentSegment += c;
                }
                else if (IsLetter(c))
                {
                    currentSegment += c;
                }
                else
                {
                    switch (c)
                    {
                        case '+':
                        case '-':
                            // Falls es + oder - ist, muss geguckt werden ob der vorherige
                            // Charakter eine Zahl oder ein Buchstabe war und falls, dann 
                            // muss ein neues Segment angefangen werden.
                            if(IsLetter(previousChar) || IsNumber(previousChar))
                            {
                                tempSegmentList.Add(currentSegment);
                                currentSegment = "";
                            }
                            currentSegment += c;
                            break;
                        case '*':
                        case '/':
                            // Es muss ein neues Segment angefangen werden.
                            tempSegmentList.Add(currentSegment);
                            currentSegment = c.ToString();
                            break;
                        case '=':
                            // Das = soll als ein Segment eingeteilt sein
                            tempSegmentList.Add(currentSegment);
                            currentSegment = c.ToString();
                            tempSegmentList.Add(currentSegment);
                            currentSegment = "";
                            break;
                        default:
                            throw new Exception("Not Excepted case: " + c);
                    }
                }
                previousChar = c;
            }
            if(!string.IsNullOrWhiteSpace(currentSegment))
            {
                tempSegmentList.Add(currentSegment);
            }
            return tempSegmentList.ToArray();
        }

        public string SolveFormula(string formula)
        {
            if (!ValidateFormula(formula))
            {
                return "";
            }

            formula = formula.Replace(" ", "");

            // Erstmal sollte es in die einzelnen Segmente unterteilt werden
            // Bsp: 3x + 7 = 16 sollte in '3x', '+7', '=', '16' unterteilt werden.
            string[] formulaSegments = SplitFormulaIntoSegments(formula);

            // Danach sollte sie dann Stück für Stück umgestellt werden
            // 3x + 7 = 16  | -7
            // 3x = 9       | /3
            // x = 3
            // Zum Schluss wird dann die fertige Formel zurückgegeben

            return "";
        }
    }
}
