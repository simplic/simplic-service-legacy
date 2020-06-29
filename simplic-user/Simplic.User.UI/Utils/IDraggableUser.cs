using Simplic.UI.MVC;

namespace Simplic.User.UI
{
    /// <summary>
    /// Implemented by an entity that can be dragged
    /// </summary>
    public interface IDraggableUser
    {
        /// <summary>
        /// Entity id
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// Joining two entities using D&D
        /// </summary>
        /// <param name="vm">The entity which the draggable entity join to</param>
        void Join(ViewModelBase vm);
    }
}
