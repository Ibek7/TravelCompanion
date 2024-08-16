using TravelCompanion.MAUI.ViewModels;

namespace TravelCompanion.MAUI.Views
{
    public partial class MemoryCreationPage : BasePage
    {
        public MemoryCreationPage()
        {
            InitializeComponent();
            BindingContext = new MemoryCreationViewModel();
        }
    }
}
