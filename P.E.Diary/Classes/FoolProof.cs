using System;
using System.Windows;

namespace P.E._Helper;

public class FoolProof
{
    public static void SetInt(ref int target, string value)
    {
        int oldValue = target;
        if (!int.TryParse(value, out target))
        {
            MessageBox.Show("Неверный формат ввода", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Stop);
            target = oldValue;
        }
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
    
    

}