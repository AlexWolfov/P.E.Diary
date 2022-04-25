namespace P.E.Diary;

public class Test
{
    public readonly Normative Normative;
    public readonly string Date;
    public readonly double Result; // необязательный параметр, необходимый для отрисовки статистики

    public Test(Normative normative, string date)
    {
        Normative = normative;
        Date = date;
    }
    
    public Test(double result, string date)
    {
        Date = date;
        Result = result;
    }
    
    public Test(double result, string date, Normative normative)
    {
        Date = date;
        Result = result;
        Normative = normative;
    }
}