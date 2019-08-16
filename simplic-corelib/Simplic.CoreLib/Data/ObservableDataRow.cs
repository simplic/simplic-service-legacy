using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data
{
    /// <summary>
    /// Customized DataRow with notifyproerpty changed support.
    /// </summary>
    public class ObservableDataRow : DataRow, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Event which must be called if the property changed (Content of ItemArray)
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Create datarow
        /// </summary>
        /// <param name="builder">Build instance</param>
        public ObservableDataRow(DataRowBuilder builder) : base(builder)
        {

        }

        /// <summary>
        /// Gets the current row
        /// </summary>
        public ObservableDataRow Row => this;

        /// <summary>
        /// Notify if the ItemsArray has changed
        /// </summary>
        public void NotifyPropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ItemArray"));
            }
        }

        /// <summary>
        /// Notify if the ItemsArray has changed
        /// </summary>
        public void NotifyCellPropertyChanged(string cellName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(cellName));
            }
        }
    }
}
