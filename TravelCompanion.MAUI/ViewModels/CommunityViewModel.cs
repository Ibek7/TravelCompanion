using System.Collections.ObjectModel;
using TravelCompanion.MAUI.Models;
using TravelCompanion.MAUI.ViewModels;

namespace TravelCompanion.MAUI.ViewModels
{
    public class CommunityViewModel : BaseViewModel
    {
        public ObservableCollection<Post> Posts { get; set; }

        public CommunityViewModel()
        {
            Posts = new ObservableCollection<Post>
            {
                new Post { Content = "First post", User = "User1" },
                new Post { Content = "Second post", User = "User2" }
            };
        }
    }
}
