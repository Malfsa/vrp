using System;
using System.Globalization;
using System.Windows.Controls;

namespace WpfApp2
{
    public class NumericValidationRule : ValidationRule
    {
        public enum ValidationType
        {
            Integer,
            Double
        }

        public ValidationType Type { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(false, "Поле не может быть пустым.");
            }

            switch (Type)
            {
                case ValidationType.Integer:
                    if (int.TryParse(value.ToString(), out _))
                    {
                        return ValidationResult.ValidResult;
                    }
                    return new ValidationResult(false, "Введите корректное целое число.");

                case ValidationType.Double:
                    if (double.TryParse(value.ToString(), out double result) && result >= 0 && result <= 100)
                    {
                        return ValidationResult.ValidResult;
                    }
                    return new ValidationResult(false, "Введите число от 0 до 100.");
            }

            return new ValidationResult(false, "Некорректный формат.");
        }
    }
}






