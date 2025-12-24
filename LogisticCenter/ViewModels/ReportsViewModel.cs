using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace LogisticCenter.ViewModels;

public partial class ReportsViewModel : ObservableObject
{
    public ObservableCollection<ReportItemModel> Reports { get; } = new();

    public ReportsViewModel()
    {
        Reports.Add(new ReportItemModel
        {
            Title = "Отчёт по остаткам",
            Description = "Актуальные данные по складам"
        });

        Reports.Add(new ReportItemModel
        {
            Title = "Отчёт по отгрузкам",
            Description = "История доставок"
        });

        Reports.Add(new ReportItemModel
        {
            Title = "Финансовый отчёт",
            Description = "Доходы и расходы"
        });
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("//main");
    }
}

public class ReportItemModel
{
    public string Title { get; set; }
    public string Description { get; set; }
}
