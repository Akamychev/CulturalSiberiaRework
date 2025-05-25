using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using System.Windows;

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

    public static bool ValidateAvailableSeats(string? availableSeats)
    {
        if (availableSeats is null or "Неизвестно")
        {
            MessageService.ShowInfo("Нет доступных билетов");
            return false;
        }

        if (!int.TryParse(availableSeats, out var seats))
        {
            MessageService.ShowError("Ошибка при обработке количества мест");
            return false;
        }

        if (seats <= 0)
        {
            MessageService.ShowInfo("Все билеты распроданы");
            return false;
        }

        return true;
    }

    public static bool ValidateReviewRating(int? rating)
    {
        if (rating == null)
        {
            MessageService.ShowError("Поставьте оценку мероприятию");
            
            return false;
        }
        
        return true;
    }

    public static bool ValidatePhoneNumbers(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return true;
        }

        if (!Regex.IsMatch(phone, @"^\+7\d{11}$"))
        {
            MessageService.ShowError("Неверный формат ввода телефона. Формат ввода: +7XXXXXXXXXX");
            return false;
        }
        
        return true;
    }
    
    public static bool ValidateNewEvent(string title, string? location, decimal? price, int? capacity, DateTime startDate, DateTime endDate)
    {
        if (!ValidateLocation(location)) return false;
        if (!ValidatePrice(price)) return false;
        if (!ValidateCapacity(capacity)) return false;
        if (!ValidateEventDates(startDate, endDate)) return false;
        if (!ValidateTitle(title)) return false;
        
        return true;
    }

    public static bool ValidateNewMuseum(string title, string location, decimal? price, string? architects,
        TimeOnly startTime, TimeOnly endTime)
    {
        if (!ValidateLocation(location)) return false;
        if (!ValidatePrice(price)) return false;
        if (!ValidateArchitects(architects)) return false;
        if (!ValidateMuseumTimes(startTime, endTime)) return false;
        if (!ValidateTitle(title)) return false;
        
        if (string.IsNullOrWhiteSpace(location))
        {
            MessageService.ShowError("Местоположение не может быть пустым");
            return false;
        }

        return true;
    }

    public static bool ValidateNewExhibit(string title, decimal? price)
    {
        if (!ValidatePrice(price)) return false;
        if (!ValidateTitle(title)) return false;

        return true;
    }

    private static bool ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            MessageService.ShowError("Все обязательные поля должны быть заполнены");
            return false;
        }
        
        if (!Regex.IsMatch(title, @"^[А-Яа-яA-Za-zёЁ0-9, ]+$"))
        {
            MessageService.ShowError("Название не должно содержать ничего кроме букв, цифр, знака ',' и пробелов");
            return false;
        }

        return true;
    }

    private static bool ValidateEventDates(DateTime startDate, DateTime endDate)
    {
        if (startDate == default && endDate == default)
        {
            MessageService.ShowError("Некорректный ввод даты");
            return false;
        }

        if (startDate >= endDate)
        {
            MessageService.ShowError("Дата начала не может быть меньше или равной даты окончания");
            return false;
        }

        if (startDate < DateTime.Now)
        {
            MessageService.ShowError("Дата и время начала мероприятия не могут быть меньше сегодняшней даты");
            return false;
        }

        return true;
    }

    private static bool ValidateMuseumTimes(TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime == default && endTime == default)
        {
            MessageService.ShowError("Некорректный ввод времени");
            return false;
        }

        if (startTime >= endTime)
        {
            MessageService.ShowError("Время начала не может быть меньше или равной времени окончания");
            return false;
        }

        return true;
    }

    private static bool ValidateLocation(string? location)
    {
        if (string.IsNullOrWhiteSpace(location)) return true;

        if (!Regex.IsMatch(location, @"^[А-Яа-яA-Za-zёЁ0-9,/. ]+$"))
        {
            MessageBox.Show(
                "Местоположение не должно содержать ничего кроме букв, цифр, знаков ',' и '/', а также пробелов",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }

    private static bool ValidatePrice(decimal? price)
    {
        switch (price)
        {
            case null:
                return true;
            case < 0:
                MessageBox.Show("Стоимость не может быть отрицательной",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            default:
                return true;
        }
    }

    private static bool ValidateCapacity(int? capacity)
    {
        switch (capacity)
        {
            case null:
                return true;
            case <= 0:
                MessageBox.Show("Вместимость не может быть меньше, либо равной 0",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            default:
                return true;
        }
    }

    private static bool ValidateArchitects(string? architects)
    {
        if (string.IsNullOrWhiteSpace(architects))
            return true;

        if (!Regex.IsMatch(architects, @"^[A-Za-zА-Яа-яёЁ, ]+$"))
        {
            MessageService.ShowError("Поле 'Архитекторы' не должно содержать ничего кроме букв, знака ',', а также пробелов");
            return false;
        }

        return true;
    }
}