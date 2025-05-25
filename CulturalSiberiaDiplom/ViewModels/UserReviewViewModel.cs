using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class UserReviewViewModel : NotifyProperty
{
    private readonly Window _window;
    private readonly CulturalSiberiaContext _context;
    private readonly User _user;
    private readonly Event _event;

    public event EventHandler? AddedReview;

    private string? _commentProperty;
    public string? CommentProperty
    {
        get => _commentProperty;
        set
        {
            if (_commentProperty == value) return;

            if (value?.Length > 50) return;
            
            _commentProperty = value;
            OnPropertyChanged(nameof(CommentProperty));
            OnPropertyChanged(nameof(CommentCharacterCount));
        }
    }
    
    public string CommentCharacterCount => $"{CommentProperty?.Length ?? 0}/50";
    
    private int _selectedRating;
    public int SelectedRating
    {
        get => _selectedRating;
        set => SetField(ref _selectedRating, value);
    }
    
    public ICommand SetRatingCommand { get; }
    public ICommand AddReviewCommand { get; }

    public UserReviewViewModel(User user, Event @event, CulturalSiberiaContext context, Window window)
    {
        _window = window;
        _context = context;
        _user = user;
        _event = @event;

        SetRatingCommand = new RelayCommand<object?>(OnSetRating);
        AddReviewCommand = new RelayCommand(OnAddReviewAsync);
    }

    private void OnSetRating(object? parameter)
    {
        if (parameter is string str && int.TryParse(str, out var rating))
        {
            SelectedRating = rating;
            Console.WriteLine("Значение рейтинга: " + SelectedRating);
        }
    }
    
    private async void OnAddReviewAsync()
    {
        try
        {
            if (!InputValidator.ValidateReviewRating(SelectedRating))
                return;
            
            var review = new Review
            {
                EventId = _event.Id,
                Rating = SelectedRating,
                UserId = _user.Id,
                Comment = CommentProperty,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            
            AddedReview?.Invoke(this, EventArgs.Empty);
            MessageService.ShowSuccess("Отзыв успешно сохранен");
            
            _window.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при сохранении отзыва: " + ex.Message);
            
            MessageService.ShowError("Ошибка при сохранении отзыва, повторите попытку позже");
        }
    }
}