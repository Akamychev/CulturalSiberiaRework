using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using Microsoft.EntityFrameworkCore;

namespace CulturalSiberiaDiplom.ViewModels;

public class ActiveTicketsViewModel : NotifyProperty
{
    private CulturalSiberiaContext _context;
    private User _currentUser;
    
    public string UpcomingEvent { get; set; }
    private ObservableCollection<TicketHistoryItemDto> _activeTickets = new();
    public ObservableCollection<TicketHistoryItemDto> ActiveTickets
    {
        get => _activeTickets;
        set
        {
            SetField(ref _activeTickets, value);
            UpdateUpcomingEvent();
        }
    }
    
    public ICommand ReportActiveTicketsCommand { get; }
    
    public ActiveTicketsViewModel(CulturalSiberiaContext context, User user)
    {
        _context = context;
        _currentUser = user;

        LoadActiveTicketsDataAsync();
        
        ReportActiveTicketsCommand = new RelayCommand(async () => await BuyHistoryReportAsync());
    }

    private async void LoadActiveTicketsDataAsync()
    {
        var tickets = await _context.Tickets
            .Include(t => t.Event)
            .Include(t => t.Museum)
            .Where(t => t.UserId == _currentUser.Id && t.StatusId == 1)
            .Select(t => new TicketHistoryItemDto
            {
                TargetName = t.Event != null ? t.Event.Title : t.Museum!.Name,
                Price = t.Event != null ? t.Event.Price : t.Museum!.Price,
                PurchaseDate = t.PurchaseDate,
                EventStartDate = t.Event!.StartDate,
                Type = t.Event != null ? "[Мероприятие]" : "[Музей]"
            }).ToListAsync();

        ActiveTickets = new ObservableCollection<TicketHistoryItemDto>(tickets);
    }
    
    private void UpdateUpcomingEvent()
    {
        var upcoming = ActiveTickets
            .Where(t => t.Type == "[Мероприятие]")
            .Where(t => t.EventStartDate.HasValue && t.EventStartDate.Value >= DateTime.Now)
            .MinBy(t => t.EventStartDate);

        UpcomingEvent = upcoming != null ? $"Ближайшее мероприятие: {upcoming.TargetName}\nДата проведения: {upcoming.EventStartDate:dd.MM.yyyy HH:mm}"
            : "Нет активных билетов";
        
        OnPropertyChanged(nameof(UpcomingEvent));
    }
    
    private async Task BuyHistoryReportAsync()
    {
        var savePath = Reports.GetSaveFilePath(".pdf", "PDF файл (*.pdf)|*.pdf");
        
        if (string.IsNullOrWhiteSpace(savePath))
            return;
        
        await Reports.GenerateBuyHistoryAndActiveTicketsPdfReportAsync(ActiveTickets, savePath);
    }
}