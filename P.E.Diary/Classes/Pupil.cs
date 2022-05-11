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
            return normative.GetMark(testResult, this);
        }
    }
}