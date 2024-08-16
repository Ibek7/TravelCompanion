using Microsoft.Maui.Controls;
using TravelCompanion.MAUI.ViewModels;
using TravelCompanion.MAUI.Views;
using TravelCompanion.MAUI.Models;
using TravelCompanion.ViewModels;

namespace TravelCompanion.MAUI.Views
{
    public partial class ProfilePage : BasePage
{
    private readonly ProfileViewModel _viewModel;

    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadUserAsync(1); // Load user with ID 1
    }
}

}
