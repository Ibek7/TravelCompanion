using TravelCompanion.MAUI.Views;
using Syncfusion.Licensing;

namespace TravelCompanion.MAUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF1cWWhAYVVpR2Nbe05xflZOalxVVAciSV9jS3pTfkVkWX5acHFWQ2ZdUg==");

        MainPage = new AppShell();
    }
}
