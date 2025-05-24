using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using Microsoft.EntityFrameworkCore;

namespace CulturalSiberiaDiplom.ViewModels;

public class BuyHistoryViewModel : NotifyProperty
{
    private CulturalSiberiaContext _context;
    private User _currentUser;

    public string TotalTickets => $"Приобретенных билетов: {_currentUser.Tickets.Count.ToString()}";
    
    private ObservableCollection<TicketHistoryItemDto> _buyHistory = new();
    public ObservableCollection<TicketHistoryItemDto> BuyHistory
    {
        get => _buyHistory;
        set => SetField(ref _buyHistory, value);
    }

    public ICommand ReportBuyHistoryCommand { get; }

    public BuyHistoryViewModel(CulturalSiberiaContext context, User user)
    {
        _context = context;
        _currentUser = user;

        LoadBuyHistory();

        ReportBuyHistoryCommand = new RelayCommand(async () => await BuyHistoryReportAsync());
    }

    private async void LoadBuyHistory()
    {
        var tickets = await _context.Tickets
            .Include(t => t.Event)
            .Include(t => t.Museum)
            .Where(t => t.UserId == _currentUser.Id)
            .Select(t => new TicketHistoryItemDto
            {
                TargetName = t.Event != null ? t.Event.Title : t.Museum!.Name,
                Price = t.Event != null ? t.Event.Price : t.Museum!.Price,
                PurchaseDate = t.PurchaseDate,
                Type = t.Event != null ? "[Мероприятие]" : "[Музей]"
            }).ToListAsync();
        
        BuyHistory = new ObservableCollection<TicketHistoryItemDto>(tickets);
    }
    
    private async Task BuyHistoryReportAsync()
    {
        var savePath = Reports.GetSaveFilePath(".pdf", "PDF файл (*.pdf)|*.pdf");
        
        if (string.IsNullOrWhiteSpace(savePath))
            return;
        
        await Reports.GenerateBuyHistoryAndActiveTicketsPdfReportAsync(BuyHistory, savePath);
    }
}