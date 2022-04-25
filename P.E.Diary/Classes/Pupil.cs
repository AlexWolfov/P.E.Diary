using System;
using NCalc;

namespace P.E.Diary
{
    public class Pupil
    {
        public int Id;
        public int Age;
        private string _birthday;
        public string Birthday
        {
            get
            {
                return _birthday;
            }
            set
            {
                _birthday = value;
                DateTime.TryParse(value, out DateTime date);
                TimeSpan years =  DateTime.Now.Subtract(date);
                Age = new DateTime(1, 1, 1).Add(years).Year - 1;
            }
        }
        public int Height; 
        public int Weight;
        public string Gender;
        public string Name;
        public string Surname;
        public readonly int Grade;
        public readonly string Letter;

        public Pupil(int grade, string letter)
        {
            Grade = grade;
            Letter = letter;
            Name = "Empty";
            Height = 0;
            Weight = 0;
            Gender = "М";
            Birthday = "01.01.2000";
            Surname = "Empty";
        }

        public int GetMark(Normative normative, double testResult) //возвращает результат норматива для текущего норматива
        {
            if (testResult == 0)
            {
                return 0;
            }
            string formula = normative.Formula;
            formula = formula.Replace("R", testResult.ToString());
            formula = formula.Replace("W", Weight.ToString());
            formula = formula.Replace("A", Age.ToString());
            formula = formula.Replace("H", Height.ToString());
            if (Gender == "Ж")
            {
                formula = formula.Replace("S", "1");
            }
            else
            {
                formula = formula.Replace("S", "0");
            }

            try
            {
                var result = new Expression(formula).Evaluate();
                if (result.GetType() == typeof(int))
                {
                    if ((int) result > 5)
                    {
                        return 6; //аналог 5+
                    }
                    else
                    {
                        return (int) result;
                    }
                }

                if (result.GetType() == typeof(double))
                {
                    if ((double) result > 5)
                    {
                        return 6; //аналог 5+
                    }

                    return (int) Math.Round((double) result);
                }

                return 0;
            }
            catch (NCalc.EvaluationException exception)
            {
                FoolProof.UniversalProtection(
                    string.Format("Ошибка в формуле, перепроверьте норматив '{0}'", normative.Name));
            }
            return 0;
        }
    }
}