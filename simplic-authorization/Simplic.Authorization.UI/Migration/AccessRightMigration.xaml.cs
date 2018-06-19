using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Simplic.Authorization.UI.Migration
{
    /// <summary>
    /// Interaction logic for AccessRightMigration.xaml
    /// </summary>
    public partial class AccessRightMigration : Window
    {
        public AccessRightMigration()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                var worker = new BackgroundWorker();
                worker.DoWork += WorkerEventHandler;
                worker.RunWorkerAsync();
            };
        }

        private void WorkerEventHandler(object sender, DoWorkEventArgs e)
        {
            var service = CommonServiceLocator.ServiceLocator.Current.GetInstance<IAuthorizationService>();
            service.Migrate();

            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Migration abgeschlossen.", "Abgeschlossen", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            });
        }
    }
}
