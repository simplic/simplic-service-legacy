using Simplic.Framework.UI;
using System;

namespace Simplic.User.UI
{
    public partial class UserDetailsView : DefaultRibbonWindow
    {
        public UserDetailsView()
        {
            InitializeComponent();
            Loaded += (o, e) =>
            {
                AllowSave = true;
                AllowDelete = true;
                AllowPaging = false;
            };
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="e"></param>
        public override void OnSave(WindowSaveEventArg e)
        {
            if (DataContext is ISaveableViewModel vm)
                vm.SaveCommand.Execute(EventArgs.Empty);
            base.OnSave(e);
        }

        public override void OnDelete(WindowDeleteEventArg e)
        {
            if (DataContext is IDeletableViewModel vm)
                vm.DeleteCommand.Execute(EventArgs.Empty);
            base.OnDelete(e);
        }
    }
}
