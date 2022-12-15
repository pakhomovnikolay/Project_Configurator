using System.Windows;

namespace Project_Сonfigurator.Views.DialogControl
{
    public partial class WindowTextEditor
    {
        #region Данные редактируемого вендора
        public string TextView
        {
            get => (string)GetValue(TextViewProperty);
            set => SetValue(TextViewProperty, value);
        }
        /// <summary>
        /// Данные редактируемого вендора
        /// </summary>
        public static readonly DependencyProperty TextViewProperty = DependencyProperty.Register(
            nameof(TextView),
            typeof(string),
            typeof(WindowTextEditor),
            new PropertyMetadata(default(string)));
        #endregion

        public WindowTextEditor() => InitializeComponent();
    }
}
