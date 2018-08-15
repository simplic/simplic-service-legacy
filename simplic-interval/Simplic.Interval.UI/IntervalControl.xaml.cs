using Simplic.Framework.UI;
using System.Windows.Controls;

namespace Simplic.Interval.UI
{
    /// <summary>
    /// Interaktionslogik für IntervalControl.xaml
    /// </summary>
    public partial class IntervalControl : UserControl
    {
        #region Fields

        private IntervalViewModel context;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public IntervalControl()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Sets the datacontext 
        /// </summary>
        /// <param name="interval"></param>
        public void InitContext(Interval interval)
        {
            DataContext = new IntervalViewModel(interval);
            context = (IntervalViewModel)DataContext;
        }

        #endregion
    }
}