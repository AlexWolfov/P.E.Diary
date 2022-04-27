using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.E.Diary.Classes
{
    public class TestParticipator
    {
        public TestParticipator(string surname, string name, double result)
        {
            Фамилия = surname;
            Имя = name;
            Результат = result;
        }

        public string Фамилия { get; private set; }
        public string Имя { get; private set; }
        public double Результат { get; set; }
    }
}
