using Simplic.UI.MVC;

namespace Simplic.User.UI
{
    public interface IDraggableUser
    {
        int UserId { get; set; }
        void Join(ViewModelBase vm);
    }
}
