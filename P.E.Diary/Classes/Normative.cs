using NCalc;
using System;
using System.Collections.Generic;
using System.IO;

namespace P.E.Diary
{
    public class Normative
    {
        public static readonly int minAge = 10;
        public int Id;
        public string Name;
        public string Formula;
        public string Type;
        public List<Dictionary<string, double>> Ranges; //Список диапозонов оценивания по возрасту

        public Normative(int id, string formula, string type, string name)
        {
            Id = id;
            Formula = formula;
            Type = type;
            Name = name;
            LoadRanges();
        }

        public Normative(string formula, string type, string name)
        {
            Formula = formula;
            Type = type;
            Name = name;
        }

        #region Creating and editing ranges' table

        public void InsertRanges(string fileName)
        {
            Ranges = new List<Dictionary<string, double>>();
            for (int i = 0; i < 11; i++)
            {
                Ranges.Add(new Dictionary<string, double>());
            }
            if (fileName != null && fileName != "")
            {
                var reader = new StreamReader(fileName);
                reader.ReadLine(); //в первой строке только заголовки
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var data = line.Split(';');
                    string key = data[0];
                    for (int i = 1; i < data.Length; i++)
                    {
                        Ranges[i - 1][key] = Convert.ToDouble(data[i]);
                    }
                }
            }

        }

        public void LoadRanges()
        {
            Ranges = SqlReader.ReturnNormativeRanges(this);
        }

        #endregion

        private bool InRange(double number, double min, double max)
        {
            return number >= min && number <= max;
        }

        public int GetMark(double result, Pupil pupil)
        {
            string formula = Formula;
            formula = formula.Replace("R", result.ToString());
            formula = formula.Replace("W", pupil.Weight.ToString());
            formula = formula.Replace("A", pupil.Age.ToString());
            formula = formula.Replace("H", pupil.Height.ToString());
            if (pupil.Gender == "Ж")
            {
                formula = formula.Replace("S", "1");
            }
            else
            {
                formula = formula.Replace("S", "0");
            }
            try
            {
                result = Convert.ToDouble(new Expression(formula).Evaluate());
            }
            catch (NCalc.EvaluationException exception)
            {
                FoolProof.UniversalProtection(
                    string.Format("Ошибка в формуле, перепроверьте норматив '{0}'", Name));
            }

            //все физкультурные нормативы для отдельно взтого ученика являются монотонным фуекциями
            //это позволяет нам точно определть будет с каждой следубщей отметкой
            //необходимое значение увеличиваться или уменьшаться
            Dictionary<string, double> currentColumn; //колонка с подходящим возрастом
            try
            {
                currentColumn = Ranges[pupil.Age - minAge]; //колонка с подходящим возрастом
            }
            catch
            {
                FoolProof.UniversalProtection("Нет возвраста ученика в таблице");
                return 0;
            }
            if (Ranges[0]["5max"] > Ranges[0]["1min"]) //если лучшее значение больше худшего
            {
                if (result > currentColumn["5max"])
                {
                    return 6; //аналог 5+
                }
                if (InRange(result, currentColumn["4max"], currentColumn["5max"]))
                {
                    return 5;
                }
                if (InRange(result, currentColumn["3max"], currentColumn["4max"]))
                {
                    return 4;
                }
                if (InRange(result, currentColumn["2max"], currentColumn["3max"]))
                {
                    return 3;
                }
                if (InRange(result, currentColumn["1max"], currentColumn["2max"]))
                {
                    return 2;
                }
            }
            else //если лучшее значение меньше худшего
            {
                if (result < currentColumn["5min"])
                {
                    return 6;
                }
                if (InRange(result, currentColumn["5min"], currentColumn["4min"]))
                {
                    return 5;
                }
                if (InRange(result, currentColumn["4min"], currentColumn["3min"]))
                {
                    return 4;
                }
                if (InRange(result, currentColumn["3min"], currentColumn["2min"]))
                {
                    return 3;
                }
                if (InRange(result, currentColumn["2min"], currentColumn["1min"]))
                {
                    return 2;
                }
            }
            return 0;
        }
    }
}