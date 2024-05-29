using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zadanie2PDN.Commands;


namespace Zadanie2PDN.ViewModels
{
    public class ViewModelMain : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand AddNumberCommand { get; set; }
        public ICommand ClearScreenCommand { get; set; }
        public ICommand GetResultCommand { get; set; }
        public ICommand AddOperationCommand { get; set; }

        public ICommand AddOperationCommand2 { get; set; }

        private DataTable _dataTable = new DataTable();


        public ViewModelMain()
        {
            ScreenVal = "0";
            ScreenVal2 = "0";
            AddNumberCommand = new RelayCommand(AddNumber);
            AddOperationCommand = new RelayCommand(AddOperation, CanAddOperation);
            ClearScreenCommand = new RelayCommand(ClearScreen);
            GetResultCommand = new RelayCommand(GetResult, CanGetResult);
            AddOperationCommand2 = new RelayCommand(AddOperation2, CanAddOperation);
        }
        private void AddNumber(object obj)
        {
            var number = obj as string;
         
            if (ScreenVal == "0" && number != ",")
                ScreenVal = string.Empty;
            else if (number == "," && _availableOperations.Contains(ScreenVal.Substring(ScreenVal.Length - 1)))
                number = "0,";
            ScreenVal += number;
            _isLastSignAnOperation = false;


        }
        private List<string> _availableOperations = new List<string> { "+", "-", "/", "*", "%", "*0,01","roundup", "rounddown" };
        private void AddOperation(object obj)
        {
            var operation = obj as string;
            ScreenVal += operation;
            _isLastSignAnOperation = true;
        }

        private void AddOperation2(object obj)
        {
            var operation = obj as string;
            ScreenVal += operation;
            _isLastSignAnOperation = false;
        }

        private void ClearScreen(object obj)
        {
            ScreenVal = "0";
            _isLastSignAnOperation = false;
        }

        private void GetResult(object obj)
        {
            if (ScreenVal.Contains("roundup"))
            {
                var numberString = ScreenVal.Replace("roundup", "");
                var number = double.Parse(numberString);
                ScreenVal2 = ScreenVal;
                ScreenVal = Math.Ceiling(number).ToString();
                return;
            }
            if (ScreenVal.Contains("rounddown"))
            {
                var numberString = ScreenVal.Replace("rounddown", "");
                var number = double.Parse(numberString);
                ScreenVal2 = ScreenVal;
                ScreenVal = Math.Floor(number).ToString();
                return;
            }
            if (ScreenVal.Contains("root"))
            {
                var numberString = ScreenVal.Replace("root", "");
                var number = double.Parse(numberString);
                ScreenVal2 = ScreenVal;
                ScreenVal = Math.Sqrt(number).ToString();
                return;
            }
            if (ScreenVal.Contains("inverse"))
            {
                var numberString = ScreenVal.Replace("inverse", "");
                var number = double.Parse(numberString);
                ScreenVal2 = ScreenVal;
                ScreenVal = (1/number).ToString();
                return;
            }
            if (ScreenVal.Contains("factorial"))
            {
                var numberString = ScreenVal.Replace("factorial", "");
                var number = Math.Round(double.Parse(numberString));
                ScreenVal2 = ScreenVal;
                ScreenVal = Factorial((int)number).ToString();
                return;
            }
            if (ScreenVal.Contains("logarithm"))
            {
                var numberString = ScreenVal.Replace("logarithm", "");
                var number = double.Parse(numberString);
                ScreenVal2 = ScreenVal;
                ScreenVal = Math.Log10(number).ToString();
                return;
            }

            var result = Math.Round(Convert.ToDouble(_dataTable.Compute(ScreenVal.Replace(",", "."), "")), 2);
            ScreenVal2 = ScreenVal;
            ScreenVal = result.ToString();

        }

        private int Factorial(int input)
        {
            int result = 1;
            for (int i = 2; i <= input; i++)
            {
                result = result * i;
            }
            return result;
        }

        private bool _isLastSignAnOperation;

        private bool CanGetResult(object obj)
        {
            return !_isLastSignAnOperation;
        }

        private bool CanAddOperation(object obj)
        {
            return !_isLastSignAnOperation;
        }


        //
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _screenVal;
        private string _screenVal2;
        public string ScreenVal
        {
            get { return _screenVal; }
            set
            {
                _screenVal = value;
                OnPropertyChanged();
            }
        }
        public string ScreenVal2
        {
            get { return _screenVal2; }
            set
            {
                _screenVal2 = value;
                OnPropertyChanged();
            }
        }
    }

}
