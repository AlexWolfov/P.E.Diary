namespace P.E.Diary;

public class Test
{
    public readonly Normative Normative;
    public readonly string Date;
    public readonly int Result; // необязательный параметр, необходимый для отрисовки статистики

    public Test(Normative normative, string date)
    {
        Normative = normative;
        Date = date;
    }
    
    public Test(int result, string date)
    {
        Date = date;
        Result = result;
    }
    
    public Test(int result, string date, Normative normative)
    {
        Date = date;
        Result = result;
        Normative = normative;
    }
}