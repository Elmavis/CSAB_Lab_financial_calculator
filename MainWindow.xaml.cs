using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;

namespace Lab_financial_calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int accuracy = 5; //кол-во знаков после запятой

        public MainWindow()
        {
            InitializeComponent();
        }

        /**
         * true - число валидное
         * false - формат числа неверный
         */
        private bool Validate(String numStr, out BigInteger number, out double ostatok)
        {
            bool isNumberValid = false;

            ostatok = 0;
            number = 0;

            //содержит экспоненциальную форму записи
            if (ContainsExponent(numStr))
                return false;

            numStr = numStr.Replace('.', ',');
            String[] arr = numStr.Split(',');

            if (arr.Length > 2)
                return false;
            else if (arr.Length == 2)
            {
                if (!Double.TryParse("0," + arr[1], out ostatok))
                    return false;
                else ostatok = Math.Round(ostatok, accuracy); //метод округления - норм?
            }
            else if (arr.Length == 1)
                ostatok = 0.0;

            numStr = arr[0];

            int spacesNumber = 0;
            String numStrWithoutSpaces = "";
            foreach (char ch in numStr)
            {
                if (ch == ' ')
                    spacesNumber++;
                else
                    numStrWithoutSpaces += ch;
            }

            if (!(BigInteger.TryParse(numStrWithoutSpaces, out number)))
                return false;

            //проверка на валидную расстановку пробелов
            if (spacesNumber != 0)
            {
                if ((numStrWithoutSpaces.Length - 1) / 3 != spacesNumber)
                    return false;
                else
                {
                    String numberWithCorrectSpaces = ToString(number);
                    if (numberWithCorrectSpaces.Length != numStr.Length)
                        return false;
                    for (int i = 0; i < numStr.Length; i++)
                        if (numStr[i] != numberWithCorrectSpaces[i])
                            return false;
                }
            }

            return true;
        }

        private String SpecialOutput(BigInteger number)
        {
            String formatedNumber = "";
            String numberStr = number.ToString();
            for (int i = 0; i < numberStr.Length; i++)
            {
                if (i % 3 == 0 && i != 0 && !(numberStr[0] == '-' && i == numberStr.Length - 1) )
                    formatedNumber += " ";
                formatedNumber += numberStr[numberStr.Length - 1 - i];
            }
            return new string(formatedNumber.ToCharArray().Reverse().ToArray()); ;
        }

        //заготовка под особый вывод
        private String ToString(BigInteger num)
        {
            return SpecialOutput(num);
        }

        /*
         * К нам поступает 2 числа
         * Мы обрезаем их до 5 знаков после запятой 
         * Производим вычисления
         * Ответ округляем до 2-х знаков после запятой
         */

        private String DeleteLastZeros(String numberStr)
        {
            for (int i = 0; i < numberStr.Length; i++)
            {
                if (numberStr[numberStr.Length - 1 - i] != '0')
                {
                    numberStr = numberStr.Substring(0, numberStr.Length - i);
                    break;
                }
            }

            return numberStr;
        }

        private bool ContainsExponent(String str)
        {
            return str.Contains("e") || str.Contains("E");
        }

        private void BtMain_Click(object sender, RoutedEventArgs e)
        {
            BigInteger maxValue = BigInteger.Parse("1000000000000000");
            BigInteger minValue = BigInteger.Parse("-1000000000000000");
            int signsNumberAfterDot = 2;

            BigInteger num1;
            BigInteger num2;
            BigInteger result;

            double ostatok1 = 0.0, ostatok2 = 0.0;

            bool isNum1OK = Validate(tb1.Text, out num1, out ostatok1);
            bool isNum2OK = Validate(tb2.Text, out num2, out ostatok2);

            if (!isNum1OK)
            {
                lNum1.Content = "Ошибка: неверный формат числа!";
                lNum2.Content = "-";
                lResult.Content = "-";
                lOperation.Content = "-";
            }
            else if (!isNum2OK)
            {
                lNum2.Content = "Ошибка: неверный формат числа!";
                lNum1.Content = "-";
                lResult.Content = "-";
                lOperation.Content = "-";
            }
            else if (num1 > maxValue)
            {
                lNum1.Content = "Ошибка: слишком большое число!";
                lNum2.Content = "-";
                lResult.Content = "-";
                lOperation.Content = "-";
            }
            else if (num1 < minValue)
            {
                lNum1.Content = "Ошибка: слишком маленькое число!";
                lNum2.Content = "-";
                lResult.Content = "-";
                lOperation.Content = "-";
            }
            else if (num2 > maxValue)
            {
                lNum2.Content = "Ошибка: слишком большое число!";
                lNum1.Content = "-";
                lResult.Content = "-";
                lOperation.Content = "-";
            }
            else if (num2 < minValue)
            {
                lNum2.Content = "Ошибка: слишком маленькое число!";
                lNum1.Content = "-";
                lResult.Content = "-";
                lOperation.Content = "-";
            }
            else if ((bool)rbPlus.IsChecked)
            {
                lOperation.Content = "Сложение";
                result = num1 + num2;

                double ostResult = ostatok1 + ostatok2;
                if (ostResult > 1)
                {
                    result++;
                    ostResult--;
                }

                String ostatok1Str = ostatok1.ToString();
                String ostatok2Str = ostatok2.ToString();
                String ostatokResultStr = ostResult.ToString();

                if (ostatok1 > 0 && !ContainsExponent(ostatok1.ToString()))
                    ostatok1Str = ostatok1Str.Substring(2, Math.Min((ostatok1Str.Length - 2), accuracy));
                else ostatok1Str = "0";
                if (ostatok2 > 0 && !ContainsExponent(ostatok2.ToString()))
                    ostatok2Str = ostatok2Str.Substring(2, Math.Min((ostatok2Str.Length - 2), accuracy));
                else ostatok2Str = "0";
                if (ostResult > 0 && !ContainsExponent(ostResult.ToString()))
                    ostatokResultStr = ostatokResultStr.Substring(2, Math.Min((ostatokResultStr.Length - 2), signsNumberAfterDot));
                else ostatokResultStr = "0";

                lNum1.Content = ToString(num1) + "." + ostatok1Str;
                lNum2.Content = ToString(num2) + "." + ostatok2Str;
                if (result < maxValue && result > minValue)
                    lResult.Content = ToString(result) + "." + (ostatokResultStr);
                else lResult.Content = "Ошибка: переполнение!";
            }
            else if ((bool)rbMinus.IsChecked)
            {
                lOperation.Content = "Вычитание";
                result = num1 - num2;

                double ostResult = ostatok1 - ostatok2;
                if (ostResult < 0)
                {
                    result--;
                    ostResult += 1;
                }

                String ostatok1Str = ostatok1.ToString();
                String ostatok2Str = ostatok2.ToString();
                String ostatokResultStr = ostResult.ToString();

                if (ostatok1 > 0 && !ContainsExponent(ostatok1.ToString()))
                    ostatok1Str = ostatok1Str.Substring(2, Math.Min((ostatok1Str.Length - 2), accuracy));
                else ostatok1Str = "0";
                if (ostatok2 > 0 && !ContainsExponent(ostatok2.ToString()))
                    ostatok2Str = ostatok2Str.Substring(2, Math.Min((ostatok2Str.Length - 2), accuracy));
                else ostatok2Str = "0";
                if (ostResult > 0 && !ContainsExponent(ostatokResultStr.ToString()))
                    ostatokResultStr = ostatokResultStr.Substring(2, Math.Min((ostatokResultStr.Length - 2), signsNumberAfterDot));
                else ostatokResultStr = "0";

                lNum1.Content = ToString(num1) + "." + ostatok1Str;
                lNum2.Content = ToString(num2) + "." + ostatok2Str;
                if (result < maxValue && result > minValue)
                    lResult.Content = ToString(result) + "." + ostatokResultStr;
                else lResult.Content = "Ошибка: переполнение!";
            }
            else if ((bool)rbMulti.IsChecked)
            {
                lOperation.Content = "Умножение";
                num1 = num1 * (int)Math.Pow(10, accuracy) + (int)(ostatok1 * Math.Pow(10, accuracy));
                num2 = num2 * (int)Math.Pow(10, accuracy) + (int)(ostatok2 * Math.Pow(10, accuracy));

                result = num1 * num2;

                String ostatokResultStr = "0";

                result /= (int)Math.Pow(10, accuracy);
                String resultStr = result.ToString();

                if (result > 0)
                {
                    if (resultStr.Length > accuracy)
                        ostatokResultStr = resultStr.Substring(Math.Max(0, resultStr.Length - accuracy), accuracy);
                    else
                    {
                        ostatokResultStr = resultStr;

                        String addLeft = "";
                        if (ostatokResultStr.Length < accuracy)
                        {
                            int addLeftNum = accuracy - ostatokResultStr.Length;
                            while (addLeftNum > 0)
                            {
                                addLeft += "0";
                                addLeftNum--;
                            }
                        }
                        ostatokResultStr = addLeft + ostatokResultStr;
                    }

                    ostatokResultStr = DeleteLastZeros(ostatokResultStr.Substring(0, signsNumberAfterDot));
                    //если после запятой только нули, то делать так, чтоб был один ноль (не добавлено)
                }
                result /= (int)Math.Pow(10, accuracy);
                num1 /= (int)Math.Pow(10, accuracy);
                num2 /= (int)Math.Pow(10, accuracy);

                String ostatok1Str = ostatok1.ToString();
                String ostatok2Str = ostatok2.ToString();

                if (ostatok1 > 0 && !ContainsExponent(ostatok1.ToString()))
                    ostatok1Str = ostatok1Str.Substring(2, Math.Min((ostatok1Str.Length - 2), accuracy));
                else ostatok1Str = "0";
                if (ostatok2 > 0 && !ContainsExponent(ostatok2.ToString()))
                    ostatok2Str = ostatok2Str.Substring(2, Math.Min((ostatok2Str.Length - 2), accuracy));
                else ostatok2Str = "0";

                lNum1.Content = ToString(num1) + "." + ostatok1Str;
                lNum2.Content = ToString(num2) + "." + ostatok2Str;
                if (result < maxValue && result > minValue)
                    lResult.Content = ToString(result) + "." + ostatokResultStr;
                else lResult.Content = "Ошибка: переполнение!";
            }
            else if ((bool)rbDiv.IsChecked)
            {
                lOperation.Content = "Деление";
                num1 = num1 * (int)Math.Pow(10, accuracy) + (int)(ostatok1 * Math.Pow(10, accuracy));
                num2 = num2 * (int)Math.Pow(10, accuracy) + (int)(ostatok2 * Math.Pow(10, accuracy));

                if (num2 == 0)
                {
                    lNum2.Content = "Ошибка: на ноль делить нельзя!";
                    lNum1.Content = "-";
                    lResult.Content = "-";
                }
                else
                {
                    bool isNum1Negative = false;
                    bool isNum2Negative = false;
                    bool isNumResultNegative = false;
                    if (num1 < 0)
                    {
                        num1 *= -1;
                        isNum1Negative = true;
                        isNumResultNegative = !isNumResultNegative;
                    }
                    if (num2 < 0)
                    {
                        num2 *= -1;
                        isNum2Negative = true;
                        isNumResultNegative = !isNumResultNegative;
                    }
                    //отриц
                    result = num1 / num2;

                    BigInteger ostatokResultTemp = num1 % num2;
                    double ostatokResult = (double)ostatokResultTemp / (double)num2;

                    num1 /= (int)Math.Pow(10, accuracy);
                    num2 /= (int)Math.Pow(10, accuracy);

                    String ostatok1Str = ostatok1.ToString();
                    String ostatok2Str = ostatok2.ToString();
                    String ostatokResultStr = ostatokResult.ToString();

                    if (ostatok1 > 0 && !ContainsExponent(ostatok1.ToString()))
                        ostatok1Str = ostatok1Str.Substring(2, Math.Min((ostatok1Str.Length - 2), accuracy));
                    else ostatok1Str = "0";
                    if (ostatok2 > 0 && !ContainsExponent(ostatok2.ToString()))
                        ostatok2Str = ostatok2Str.Substring(2, Math.Min((ostatok2Str.Length - 2), accuracy));
                    else ostatok2Str = "0";
                    if (ostatokResult > 0 && !ContainsExponent(ostatokResult.ToString()))
                        ostatokResultStr = ostatokResultStr.Substring(2, Math.Min((ostatokResultStr.Length - 2), signsNumberAfterDot));
                    else ostatokResultStr = "0";

                    lNum1.Content = (isNum1Negative ? "-" : "") + ToString(num1) + "." + ostatok1Str;
                    lNum2.Content = (isNum2Negative ? "-" : "") + ToString(num2) + "." + ostatok2Str;
                    lResult.Content = (isNumResultNegative ? "-" : "") + ToString(result) + "." + ostatokResultStr;
                }
            }
        }
    }
}
