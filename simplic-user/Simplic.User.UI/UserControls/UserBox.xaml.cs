using System.Windows;
using System.Windows.Input;

namespace Simplic.User.UI
{
    public partial class UserBox 
    {
        public UserBox()
        {
            InitializeComponent();
        }

        #region dependency properties
        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register("EditCommand", typeof(ICommand), typeof(UserBox));
        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register("RemoveCommand", typeof(ICommand), typeof(UserBox));
        public ICommand RemoveCommand
        {
            get { return (ICommand)GetValue(RemoveCommandProperty); }
            set { SetValue(RemoveCommandProperty, value); }
        }

        public static readonly DependencyProperty UserProperty = DependencyProperty.Register("User", typeof(UserViewModel), typeof(UserBox));
        public UserViewModel User
        {
            get { return (UserViewModel)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        #endregion

        private void OnEdit(object sender, RoutedEventArgs e)
        {
            if (EditCommand != null)
                EditCommand.Execute(User);
        }

        private void OnRemove(object sender, RoutedEventArgs e)
        {
            if (RemoveCommand != null)
                RemoveCommand.Execute(User);
        }
    }
}
