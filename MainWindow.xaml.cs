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
        public MainWindow()
        {
            InitializeComponent();
        }

        /**
         * true - число нормальное
         * false - формат числа неверный
         */
        private bool Validate(String numStr, out BigInteger bi, out double ost)
        {
            bool isOK = true;
            ost = 0;
            bi = 0;
            numStr = numStr.Replace('.', ',');
            String[] arr = numStr.Split(',');
            if (arr.Length > 2)
                return false;
            else if (arr.Length == 2)
            {
                if (!Double.TryParse("0," + arr[1], out ost))
                    return false;
                else ost = Math.Round(ost, 2);
            }
            else if (arr.Length == 1)
                ost = 0;

            numStr = arr[0];
            isOK = BigInteger.TryParse(numStr, out bi);
            if (!isOK) //false
                return false;
            //содержит экспоненциальную форму записи
            if (numStr.Contains("e") || numStr.Contains("E"))
                return false;

            return isOK;
        }

        private String ToString(BigInteger num)
        {
            return num.ToString();
        }

        private void BtMain_Click(object sender, RoutedEventArgs e)
        {
            BigInteger maxValue = BigInteger.Parse("1000000000000000");
            BigInteger minValue = BigInteger.Parse("-1000000000000000");

            BigInteger num1;
            BigInteger num2;
            BigInteger result;

            double ost1, ost2;


            bool isNum1OK = Validate(tb1.Text, out num1, out ost1);
            bool isNum2OK = Validate(tb2.Text, out num2, out ost2);

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
                result = num1 + num2;

                double ostResult = ost1 + ost2;
                if(ostResult > 1)
                {
                    result++;
                    ostResult--;
                }
                ost1 = Math.Floor(ost1 * 100);
                ost2 = Math.Floor(ost2 * 100);
                ostResult = Math.Floor(ostResult * 100);

                lNum1.Content = ToString(num1) + "." + ost1.ToString();
                lNum2.Content = ToString(num2) + "." + ost2.ToString();
                lOperation.Content = "Сложение";
                if (result < maxValue && result > minValue)
                    lResult.Content = ToString(result) + "." + ostResult;
                else lResult.Content = "Ошибка: переполнение!";
            }
            else if ((bool)rbMinus.IsChecked)
            {
                result = num1 - num2;

                double ostResult = ost1 - ost2;
                if(ostResult < 0)
                {
                    result--;
                    ostResult += 1;
                }
                ost1 = Math.Floor(ost1 * 100);
                ost2 = Math.Floor(ost2 * 100);
                ostResult = Math.Floor(ostResult * 100);

                lNum1.Content = ToString(num1) + "." + ost1.ToString();
                lNum2.Content = ToString(num2) + "." + ost2.ToString();
                lOperation.Content = "Вычитание";
                if (result < maxValue && result > minValue)
                    lResult.Content = ToString(result) + "." + ostResult;
                else lResult.Content = "Ошибка: переполнение!";
            }
        }
    }
}
