using Simplic.Framework.UI;
using System;

namespace Simplic.User.UI
{
    public partial class CreateNewGroupView : DefaultRibbonWindow
    {
        public CreateNewGroupView()
        {
            InitializeComponent();
            Loaded += (o, e) =>
            {
                AllowSave = true;
                AllowDelete = false;
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
    }
}
