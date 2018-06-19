using Simplic.Framework.UI;
using System;
using Telerik.Windows.Controls;

namespace Simplic.Authorization.UI
{
    /// <summary>
    /// Interaction logic for AccessRightsEditor.xaml
    /// </summary>
    public partial class AccessRightsEditor : DefaultRibbonWindow
    {
        /// <summary>
        /// Constructor for the AccessRightsEditor. These parameters are needed to load access rights
        /// </summary>
        /// <param name="tableName">The table to get the access rights from</param>
        /// <param name="rowId">Actual row id</param>
        public AccessRightsEditor(string tableName, string idColName, object rowId)
        {
            InitializeComponent();
            this.DataContext = new AccessRightsEditorViewModel(tableName, idColName, rowId);

            StyleManager.SetTheme(this.availableListBox, new Windows8Theme());
            StyleManager.SetTheme(this.toListBox, new Windows8Theme());

            availableListBox.Items.IsLiveSorting = true;
            toListBox.Items.IsLiveSorting = true;                        
        }

        /// <summary>
        /// Is called when save button is clicked
        /// </summary>
        /// <param name="e"></param>
        public override void OnSave(WindowSaveEventArg e)
        {
            (this.DataContext as AccessRightsEditorViewModel).Save();
            base.OnSave(e);
        }

    }
}
