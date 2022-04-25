using System;
using System.Windows;

namespace P.E.Diary;

public class FoolProof
{
    public static bool SetInt(ref int target, string value)
    {
        int oldValue = target;
        if (!int.TryParse(value, out target))
        {
            MessageBox.Show("Неверный формат ввода", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Stop);
            target = oldValue;
            return false;
        }
        return true;
    }

    public static bool SetDouble(ref double target, string value)
    {
        double oldValue = target;
        if (!double.TryParse(value, System.Globalization.NumberStyles.Any, new System.Globalization.CultureInfo("en-US"), out target))
        {
            MessageBox.Show("Неверный формат ввода", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Stop);
            target = oldValue;
            return false;
        }
        return true;
    }

    public static string ReturnDate(string target, string value)
    {
        if (!DateTime.TryParse(value, out DateTime result))
        {
            MessageBox.Show("Неверный формат ввода", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Stop); 
            return "-1";
        }
        if (result > DateTime.Today)
        {
            MessageBox.Show("Слишком поздняя дата", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Stop);
            return "-1";
        }
        return result.ToString();
    }
    
    public static void UniversalProtection(string message)
    {
        MessageBox.Show(message, "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Stop);
    }

    public static bool DeletionProtection(string deletedObjectName)
    {
        string messageText = string.Format("Вы уверены, что хотите удалить {0}?",deletedObjectName);
        return (MessageBox.Show(messageText, "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
            == MessageBoxResult.Yes);
    }

    public static bool MessageBoxProtection(string messageText, string header)
    {
        return (MessageBox.Show(messageText, header, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
            == MessageBoxResult.Yes);
    }



}