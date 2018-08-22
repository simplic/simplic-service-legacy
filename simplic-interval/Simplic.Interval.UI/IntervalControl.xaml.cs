using System.Windows;
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
        /// Dependency property for the datacontext
        /// </summary>
        public Interval IntervalContext
        {
            get { return (Interval)GetValue(IntervalContextProperty); }
            set
            {
                context = new IntervalViewModel(value);
                DataContext = context;
                SetValue(IntervalContextProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for IntervalContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntervalContextProperty =
            DependencyProperty.Register("IntervalContext", typeof(Interval), typeof(IntervalControl), new PropertyMetadata(0));

        /// <summary>
        /// Sets the datacontext
        /// </summary>
        /// <param name="interval"></param>
        //todo: dependency property
        public void InitContext(Interval interval)
        {
            DataContext = new IntervalViewModel(interval);
            context = (IntervalViewModel)DataContext;
        }

        #endregion Public methods
    }
}