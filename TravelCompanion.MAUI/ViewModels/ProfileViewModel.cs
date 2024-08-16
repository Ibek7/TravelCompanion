using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TravelCompanion.Domain.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TravelCompanion.SDK.Clients;

namespace TravelCompanion.ViewModels
{
    public class ProfileViewModel : ObservableObject
    {
    private readonly AppUserClient _appUserClient;

    public ProfileViewModel(AppUserClient appUserClient)
    {
        _appUserClient = appUserClient;
    }

    private AppUserDto _user;
    public AppUserDto User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }

    public async Task LoadUserAsync(int userId)
    {
        User = await _appUserClient.GetUser();
    }
    }

}
