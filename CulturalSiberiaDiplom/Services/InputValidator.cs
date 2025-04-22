using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace CulturalSiberiaDiplom.Services;

public static class InputValidator
{
    public static bool ValidateAuthorization(string login, string password)
    {
        if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password)) return true;

        MessageBox.Show("Необходимо заполнить все поля авторизции", 
            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        return false;
    }

    public static bool ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            MessageBox.Show("Электронная почта не может быть пустой",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            MessageBox.Show("Некорректный ввод электронной почты", 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }

    public static bool ValidateFirstNameAndLastName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            MessageBox.Show("Имя и фамилия не могут быть пустыми", 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        
        const string namePattern = @"^[А-Яа-яA-Za-zёЁ]+$";
        if (!Regex.IsMatch(firstName, namePattern) || !Regex.IsMatch(lastName, namePattern))
        {
            MessageBox.Show("Имя и фамилия может содержать только буквы",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }

    public static bool ValidateMiddleName(string middleName)
    {
        if (string.IsNullOrEmpty(middleName))
        {
            return true;
        }
        
        const string namePattern = @"^[А-Яа-яA-Za-zёЁ]+$";
        if (!Regex.IsMatch(middleName, namePattern))
        {
            MessageBox.Show("Отчество может содержать только буквы.",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }

    public static bool ValidateUsernameFormat(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            MessageBox.Show("Логин не может быть пустым", 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        const string usernamePattern = @"^[A-Za-z0-9_]+$";
        if (!Regex.IsMatch(username, usernamePattern))
        {
            MessageBox.Show("Логин может содержать только буквы, цифры и подчеркивания",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }

    public static bool ValidateUsernameUniqueness(string username)
    {
        if (Service.GetDbContext().Users.Any(u => u.Username == username))
        {
            MessageBox.Show("Пользователь с таким логином уже существует, попробуйте другой",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }

    public static bool ValidatePasswordFormat(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Пароль не может быть пустым", 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        
        bool hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
        bool hasDigit = Regex.IsMatch(password, @"[0-9]");

        if (!hasUpperCase || !hasDigit)
        {
            MessageBox.Show("Пароль должен содержать минимум 1 заглавную букву и 1 цифру",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }

    public static bool ValidatePasswordLength(string password)
    {
        if (password.Length is < 6 or > 32)
        {
            MessageBox.Show("Пароль должен содержать от 6-ти до 32-ух символов",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }
}