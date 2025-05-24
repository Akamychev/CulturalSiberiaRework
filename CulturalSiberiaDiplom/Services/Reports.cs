using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.ViewModels;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Win32;

namespace CulturalSiberiaDiplom.Services;

public static class Reports
{
    public static async Task GenerateAllEventsPdfReportAsync(List<Event> events, string filePath)
    {
        try
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    MessageBox.Show("Сохранение отменено",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string dest = filePath;
                PdfWriter writer = new PdfWriter(dest);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                Table table = new Table(8);
                table.AddHeaderCell("Название");
                table.AddHeaderCell("Описание");
                table.AddHeaderCell("Тип");
                table.AddHeaderCell("Дата начала");
                table.AddHeaderCell("Дата конца");
                table.AddHeaderCell("Местоположение");
                table.AddHeaderCell("Цена");
                table.AddHeaderCell("Вместимость");

                foreach (var @event in events)
                {
                    table.AddCell(@event.Title);
                    table.AddCell(@event.Description ?? "Не указано");
                    table.AddCell(@event.Type.TypeName);
                    table.AddCell(@event.StartDate.ToString(CultureInfo.CurrentCulture));
                    table.AddCell(@event.EndDate.ToString(CultureInfo.CurrentCulture));
                    table.AddCell(@event.Location ?? "Не указано");
                    table.AddCell(@event.Price.ToString());
                    table.AddCell(@event.Capacity.ToString() ?? "Не указано");
                }

                var font = EmbeddedFontService.GetFont("pt-astra-serif_regular.ttf");

                document.SetFont(font);
                document.Add(table);
                document.Close();
            });

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            });
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Console.WriteLine($"Ошибка генерации отчета мероприятий: {ex.Message}");
                
                MessageService.ShowError("Ошибка генерации отчета, повторите попытку позже");
            });
        }
    }
    
    public static async Task GenerateAllMuseumsPdfReportAsync(List<Museum> museums, string filePath)
    {
        try
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    MessageBox.Show("Сохранение отменено",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string dest = filePath;
                PdfWriter writer = new PdfWriter(dest);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                Table table = new Table(7);
                table.AddHeaderCell("Название");
                table.AddHeaderCell("Местоположение");
                table.AddHeaderCell("Дата основания");
                table.AddHeaderCell("Тип");
                table.AddHeaderCell("Архитектор");
                table.AddHeaderCell("Начало рабочего дня");
                table.AddHeaderCell("Конец рабочего дня");

                foreach (var museum in museums)
                {
                    table.AddCell(museum.Name);
                    table.AddCell(museum.Location);
                    table.AddCell(museum.DateOfFoundation?.ToString() ?? "Не указана");
                    table.AddCell(museum.Type.TypeName);
                    table.AddCell(museum.Architects ?? "Не указан");
                    table.AddCell(museum.StartWorkingTime.ToString());
                    table.AddCell(museum.EndWorkingTime.ToString());
                }

                var font = EmbeddedFontService.GetFont("pt-astra-serif_regular.ttf");

                document.SetFont(font);
                document.Add(table);
                document.Close();
            });

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            });
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Console.WriteLine($"Ошибка генерации отчета музеев: {ex.Message}");
                
                MessageService.ShowError("Ошибка генерации отчета, повторите попытку позже");
            });
        }
    }
    
    public static async Task GenerateBuyHistoryAndActiveTicketsPdfReportAsync(ObservableCollection<TicketHistoryItemDto> tickets, string filePath)
    {
        try
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    MessageBox.Show("Сохранение отменено",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string dest = filePath;
                PdfWriter writer = new PdfWriter(dest);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                Table table = new Table(4);
                table.AddHeaderCell("Тип");
                table.AddHeaderCell("Наименование");
                table.AddHeaderCell("Цена");
                table.AddHeaderCell("Дата приобретения");

                foreach (var ticket in tickets)
                {
                    table.AddCell(ticket.Type);
                    table.AddCell(ticket.TargetName);
                    table.AddCell(ticket.Price.ToString());
                    table.AddCell(ticket.PurchaseDate.ToString());
                }

                var font = EmbeddedFontService.GetFont("pt-astra-serif_regular.ttf");

                document.SetFont(font);
                document.Add(table);
                document.Close();
            });

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            });
        }
        catch (Exception ex)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Console.WriteLine($"Ошибка генерации отчета истории покупок: {ex.Message}");
                
                MessageService.ShowError("Ошибка генерации отчета, повторите попытку позже");
            });
        }
    }

    public static string? GetSaveFilePath(string defaultExtension, string filter)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Сохранить отчет",
            Filter = filter,
            DefaultExt = defaultExtension,
            AddExtension = true
        };

        bool? result = dialog.ShowDialog();

        return result == true ? dialog.FileName : null;
    }
}