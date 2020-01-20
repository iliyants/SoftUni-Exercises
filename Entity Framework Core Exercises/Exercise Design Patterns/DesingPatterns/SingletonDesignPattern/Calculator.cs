using System;
using System.Collections.Generic;
using System.Text;

namespace SingletonDesignPattern
{
    public sealed class Calculator
    {
        private Calculator()
        {
        }

        private static Calculator instance = null;
        public static Calculator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Calculator();
                }
                return instance;
            }
        }
        public double ValueOne { get; set; }
        public double ValueTwo { get; set; }
        public double Sum()
        {
            return ValueOne + ValueTwo;
        }
        public double Subtract()
        {
            return ValueOne - ValueTwo;
        }
        public double Multiply()
        {
            return ValueOne * ValueTwo;
        }
        public double Divide()
        {
            return ValueOne / ValueTwo;
        }
    }
}
