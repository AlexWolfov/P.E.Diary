using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace P.E.Diary;

public class SqlReader
{
    private static readonly int CurrentSchoolYear = DateTime.Now.Year + DateTime.Now.Month / 6;
    private static readonly string ConnectionString = "Data Source=.\\Database;Version=3";

    #region Database creation

    public static void CreateDataBase()
    {
        SQLiteConnection.CreateFile("Database");
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = @"
                    create table Normatives
                    (
                        NormativeId integer
                            primary key autoincrement,
                        Name        varchar(50),
                        Formula     varchar(100),
                        Type        varchar(30)
                    );

                    create table Pupils
                    (
                        PupilId    integer
                            primary key autoincrement,
                        Graduation integer,
                        Letter     varchar(3),
                        Name       varchar(50),
                        Height     integer,
                        Weight     integer,
                        Gender     varchar(1),
                        Birthday   varchar(10),
                        Surname    varchar(50)
                    );

                    create table Tests
                    (
                        TestId      integer
                            primary key autoincrement,
                        PupilId     int
                            references Pupils,
                        Result      varchar(10),
                        NormativeId int
                            references Normatives,
                        Date        varchar(10)
                    );

                    create table MarksTable
                    (
                        NormativeId integer
                            references Normatives,
                        Mark        varchar(4),
                        Age         integer,
                        Result      varchar(5)
                    );

                ";
                command.ExecuteScalar();

            }
        }
    }


    #endregion

    #region SchoolClass

    public static Dictionary<string, SchoolClass>
        LoadClasses() //Загрузка всех классов с учениками, ключи - номера (с буквами) школьных классов
    {
        Dictionary<string, SchoolClass> result = new Dictionary<string, SchoolClass>();
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = "SELECT DISTINCT Graduation, Letter FROM Pupils";
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                //достаем номера школьных классов
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string key = Convert.ToString(11 - (reader.GetInt32(0) - CurrentSchoolYear)) + " " +
                                  reader.GetString(1);
                        result.Add(key, new SchoolClass(11 - (reader.GetInt32(0) - CurrentSchoolYear),
                            reader.GetString(1), new List<Pupil>()));
                    }
                }

                command.CommandText = "SELECT * FROM Pupils WHERE not(Name is 'BaseClassName')"; //достаем учеников
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string key = Convert.ToString(11 - (reader.GetInt32(1) - CurrentSchoolYear)) + " " +
                                  reader.GetString(2);
                        result[key].Pupils.Add(new Pupil(
                                11 - (reader.GetInt32(1) - CurrentSchoolYear),
                                reader.GetString(2)) //присваиваем ученикам пармаетры из базы
                        {
                            Id = reader.GetInt32(0),
                            Height = reader.GetInt32(4),
                            Name = reader.GetString(3),
                            Weight = reader.GetInt32(5),
                            Gender = reader.GetString(6),
                            Birthday = reader.GetString(7),
                            Surname = reader.GetString(8)
                        });
                    }
                }
            }
        }

        return result;
    }

    public static void NewClass(int Grade, string Letter) //создание нового класса
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "INSERT INTO main.Pupils (Graduation, Letter, Name, Height, Weight, Gender, Birthday, Surname) VALUES ({0}, '{1}', 'BaseClassName', 0, 0, 'M', '2000:00:00', 'BaseClassSurname')",
                Convert.ToString(11 - Grade + CurrentSchoolYear),
                Letter); //создаем строку, которая будет считыватья при поиске классов, но не будет считыватья при поиске учеников из-за имени
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
        }
    }

    public static void DeleteClass(int Grade, string Letter) //удаление класса
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "DELETE FROM Pupils WHERE (Graduation = {0}) and (Letter = '{1}')",
                Convert.ToString(11 - Grade + CurrentSchoolYear), Letter);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
        }
    }

    #endregion

    #region Pupil

    public static Pupil ReturnPupil(int pupilId)
    {
        Pupil result;
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format("SELECT * FROM Pupils WHERE PupilId = {0} ", pupilId);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    result = new Pupil(
                            11 - (reader.GetInt32(1) - CurrentSchoolYear),
                            reader.GetString(2)) //присваиваем ученикам пармаетры из базы
                    {
                        Id = reader.GetInt32(0),
                        Height = reader.GetInt32(4),
                        Name = reader.GetString(3),
                        Weight = reader.GetInt32(5),
                        Gender = reader.GetString(6),
                        Birthday = reader.GetString(7),
                        Surname = reader.GetString(8)
                    };
                }
            }
        }
        return result;
    }

    public static void EditPupil(Pupil pupil) //изменение ученика
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "UPDATE Pupils SET Graduation = {0}, Letter = '{1}', Name = '{2}', Height = {3}, Weight = {4}, Gender = '{5}', Birthday = '{6}', Surname = '{7}' WHERE PupilId = {8}",
                Convert.ToString(11 - pupil.Grade + CurrentSchoolYear), pupil.Letter, pupil.Name, pupil.Height,
                pupil.Weight,
                pupil.Gender, pupil.Birthday, pupil.Surname, pupil.Id); //изменение значения в таблице
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
        }
    }

    public static void AddPupil(ref Pupil pupil) //передается со ссылкой, чтобы потом задать ему правильный ID
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "INSERT INTO main.Pupils (Graduation, Letter, Name, Height, Weight, Gender, Birthday, Surname) VALUES ({0}, '{1}', '{2}', {3}, {4}, '{5}', '{6}', '{7}')",
                Convert.ToString(11 - pupil.Grade + CurrentSchoolYear), pupil.Letter, pupil.Name, pupil.Height,
                pupil.Weight,
                pupil.Gender, pupil.Birthday, pupil.Surname); //создаем строку
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
                command.CommandText = "SELECT MAX(PupilId) FROM Pupils";
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    pupil.Id = reader.GetInt32(0);
                }
            }
        }
    }

    public static void DeletePupil(int Id) //удаление ученика
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format("DELETE FROM Pupils WHERE PupilId = {0}", Id);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
        }
    }

    #endregion

    #region Normative

    public static Normative ReturnNormative(int id)
    {
        Normative result = null;
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format("SELECT * FROM Normatives WHERE NormativeId = {0}", id);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    if (reader.HasRows)
                    {
                        result = new Normative(
                            id: reader.GetInt32(0),
                            name: reader.GetString(1),
                            formula: reader.GetString(2),
                            type: reader.GetString(3));
                    }
                }
            }
        }

        return result;
    }

    public static Dictionary<string, Normative> LoadNormatives() //загрузка нормативов
    {
        Dictionary<string, Normative> result = new Dictionary<string, Normative>();
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = "SELECT * FROM Normatives WHERE not (Name is 'BaseTypeName')";
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        result.Add(reader.GetString(1), new Normative(
                            id: reader.GetInt32(0),
                            name: reader.GetString(1),
                            formula: reader.GetString(2),
                            type: reader.GetString(3)
                        ));
                }
            }
        }

        return result;
    }

    public static void EditNormative(Normative normative) //сохранение норматива
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "UPDATE Normatives SET Name = '{0}', Formula = '{1}', Type = '{2}' WHERE NormativeId = {3}",
                normative.Name, normative.Formula, normative.Type, normative.Id);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
        }
    }

    public static int CreateNormative(Normative normative) //создание нового норматива
    {
        int result = 0;
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "INSERT INTO main.Normatives (Name, Formula, Type) VALUES ('{0}', '{1}', '{2}')",
                normative.Name, normative.Formula, normative.Type);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
            result = Convert.ToInt32(connection.LastInsertRowId);
        }
        return result;
    }

    public static void DeleteNormative(Normative normative)
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "DELETE FROM Normatives WHERE NormativeId = {0}", normative.Id);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
            commandText = string.Format(
                "DELETE FROM Tests WHERE NormativeId = {0}", normative.Id);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
            commandText = string.Format(
                "DELETE FROM MarksTable WHERE NormativeId = {0}", normative.Id);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
        }
    }

    public static void AddNortmativeRanges(Normative normative)
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            foreach (string key in normative.Ranges[0].Keys)
            {
                for (int i = 0; i < normative.Ranges.Count - 1; i++)
                {
                    string commandText = string.Format(
                        "INSERT into main.MarksTable(NormativeId, Mark, Age, Result) VALUES({0}, '{1}', {2}, '{3}')",
                        normative.Id, key, i, normative.Ranges[i][key]);
                    using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
                    {
                        command.ExecuteScalar();
                    }
                }
            }
        }
    }

    public static void EditNormativeRanges(Normative normative)
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            foreach (string key in normative.Ranges[0].Keys)
            {
                for (int i = 0; i < normative.Ranges.Count; i++)
                {
                    string commandText = string.Format(
                        "UPDATE main.MarksTable SET Age = '{0}', Result = '{1}', WHERE (NormativeId = {2}) and (Mark = '{3}')",
                        i, normative.Ranges[i][key], normative.Id, key);
                    using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
                    {
                        command.ExecuteScalar();
                    }
                }
            }
        }
    }


    public static List<Dictionary<string, double>> ReturnNormativeRanges(Normative normative)
    {
        List<Dictionary<string, double>> result = new List<Dictionary<string, double>>();
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format("SELECT count(*) FROM MarksTable WHERE (NormativeId = {0}) and (Mark = '5max')", normative.Id); //считаем количетво возраство в таблице
            
            int rowsCount;
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    rowsCount = reader.GetInt32(0); 
                }
            }
            for (int i = 0; i < rowsCount; i++) //создаем нужное количество столбцов
            {
                result.Add(new Dictionary<string, double>()); 
            }
            
            commandText = string.Format("SELECT Mark, Age, Result FROM MarksTable WHERE NormativeId = {0}", normative.Id);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    { 
                        string key = reader.GetString(0);
                        int age = reader.GetInt32(1);
                        double value = Convert.ToDouble(reader.GetString(2)); //костыль, т.к. в SQLite нет double
                        result[age].Add(key, value);
                    }
                }
            }
 
        }

        return result;

    }

    #endregion

    #region Type

    public static void CreateType(string typeName) //создание нового типа нормативов
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "INSERT INTO main.Normatives (Name, Formula, Type) VALUES ('BaseTypeName', 'BaseFormula', '{0}')",
                typeName); //создаем новый норматив с именем, которое не читатется при поиске нормативов, но читается при поиске типов
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
        }
    }

    public static void EditType(string oldName, string newName) //изменение типа
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "UPDATE Normatives SET Type = '{0}' WHERE Type = '{1}'", newName,
                oldName); //заменяем имя в тех строка, где имя равно старому
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
        }
    }

    public static List<string> LoadTypes() //загрузка всех типов
    {
        List<string> result = new List<string>();
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = "SELECT DISTINCT Type FROM Normatives";
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) result.Add(reader.GetString(0));
                }
            }
        }

        return result;
    }

    public static void DeleteType(string typeName)
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "DELETE FROM Normatives WHERE (Name = 'BaseTypeName') and (Type = '{0}')", typeName);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }

            connection.Close();
            EditType(typeName, "Без категории"); //изменяем типы оставшихся нормтаивов
        }
    }

    #endregion

    #region Tests

    public static void DeleteTest(int normativeId, string testDate)
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "DELETE FROM Tests WHERE (NormativeId = {0}) and (Date = '{1}')",
                normativeId, testDate);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
        }
    }

    public static void ApplyNormative(int normativeId, int pupilId, double testResult)
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "INSERT INTO main.Tests (PupilId, Result, NormativeId, Date) VALUES ({0}, '{1}', {2}, '{3}')",
                pupilId, testResult, normativeId, DateTime.Now.Date.ToString().Substring(0, 10));
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                command.ExecuteScalar();
            }
        }
    }

    public static double GetTestResult(int normativeId, int pupilId, string testDate)
    {
        double result = 0;
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "SELECT Result FROM Tests WHERE (NormativeId = {0}) and (PupilId = {1}) and (Date = '{2}')",
                normativeId, pupilId, testDate);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    if (reader.HasRows)
                    {
                        string Test = reader.GetDataTypeName(0);
                        result = Convert.ToDouble(reader.GetString(0)); //костыль, т.к. в SQLite нет double
                    }
                }
            }
            return result;
        }
    }

    public static List<Test> GetSchoolClassTests(SchoolClass schoolClass) //возвращение всех нормативов в классе
    {
        List<Test> result = new List<Test>();
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "SELECT DISTINCT NormativeId, Date FROM Tests test INNER JOIN Pupils pupil ON pupil.PupilId = test.PupilId WHERE Graduation = {0} AND Letter = '{1}'",
                Convert.ToString(11 - schoolClass.Grade + CurrentSchoolYear),
                schoolClass.Letter); //комманда на поиск всех зачетов, которые проводились в классе
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        result.Add(new Test(ReturnNormative(reader.GetInt32(0)),
                            reader.GetString(1))); //добавляет зачет по определеному нормативу и времени
                }
            }
        }

        return result;
    }

    public static void EditTest(int normativeId, int pupilId, string testDate, double newResult)
    {
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format("SELECT count(*) FROM Tests WHERE (NormativeId = {0}) and (PupilId = {1}) and (Date = '{2}')",
                normativeId, pupilId, testDate);
            int rowsCount;
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    rowsCount = reader.GetInt32(0); //проверка на наличие такого зачета у ученикв
                }
            }
            if (rowsCount > 0) //если есть зачет, то редактируем
            {
                commandText = string.Format(
                    "UPDATE Tests SET Result = '{0}' WHERE (NormativeId = {1}) and (PupilId = {2}) and (Date = '{3}')",
                    newResult, normativeId, pupilId, testDate);
                using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
                {
                    command.ExecuteScalar();
                }
            }
            else //если зачет не проведен. то проводим
            {
                commandText = string.Format(
                    "INSERT INTO main.Tests (PupilId, Result, NormativeId, Date) VALUES ({0}, '{1}', {2}, '{3}')",
                    pupilId, newResult, normativeId, testDate);
                using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
                {
                    command.ExecuteScalar();
                }
            }
        }
    }

    public static List<Test> ReturnPupilTests(int pupilId, int normativeId) // возвращает все тесты у ученика по одному нормативу
    {
        List<Test> result = new List<Test>();
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "SELECT Result, Date FROM Tests WHERE PupilId = {0} and NormativeId = {1}",
                pupilId, normativeId);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Test(Convert.ToDouble(reader.GetString(0)), reader.GetString(1)));
                    }
                }
            }
        }
        return result;
    }

    public static List<Test> ReturnPupilTestsByType(int pupilId, string type) // возвращает все тесты у ученика по одному нормативу
    {
        List<Test> result = new List<Test>();
        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string commandText = string.Format(
                "SELECT test.Result, test.Date, test.NormativeId FROM Tests test INNER JOIN Normatives normative ON test.NormativeId = normative.NormativeId WHERE normative.Type = '{0}' and test.PupilId = {1} ",
                type, pupilId);
            using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Test(Convert.ToDouble(reader.GetString(0)), reader.GetString(1), ReturnNormative(reader.GetInt32(2))));
                    }
                }
            }
        }
        return result;
    }


    #endregion

}