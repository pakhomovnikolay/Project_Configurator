using Project_Сonfigurator.Models.Settings;
using System.Windows;

namespace Project_Сonfigurator.Views.DialogControl
{
    public partial class WindowEditServerDB
    {
        #region Данные редактируемого вендора
        public SettingServerDB SelectedServer
        {
            get => (SettingServerDB)GetValue(SelectedServerProperty);
            set => SetValue(SelectedServerProperty, value);
        }
        /// <summary>
        /// Данные редактируемого вендора
        /// </summary>
        public static readonly DependencyProperty SelectedServerProperty = DependencyProperty.Register(
            nameof(SelectedServer),
            typeof(SettingServerDB),
            typeof(WindowEditServerDB),
            new PropertyMetadata(new SettingServerDB()));
        #endregion

        public WindowEditServerDB() => InitializeComponent();
    }
}
