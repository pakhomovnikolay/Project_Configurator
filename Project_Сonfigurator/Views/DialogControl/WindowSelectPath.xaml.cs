using Project_Сonfigurator.Services;
using System.Windows;

namespace Project_Сonfigurator.Views.DialogControl
{
    public partial class WindowSelectPath
    {
        #region Выбранный путь для сохранения проекта
        public string SelectPath
        {
            get => (string)GetValue(SelectPathProperty);
            set => SetValue(SelectPathProperty, value);
        }
        /// <summary>
        /// Выбранный путь для сохранения проекта
        /// </summary>
        public static readonly DependencyProperty SelectPathProperty = DependencyProperty.Register(
            nameof(SelectPath),
            typeof(string),
            typeof(WindowSelectPath),
            new PropertyMetadata("По умолчанию"));
        #endregion

        #region Конструктор
        public WindowSelectPath() => InitializeComponent();
        #endregion

        #region Выбрать путь для сохранения файла
        /// <summary>
        /// Выбрать путь для сохранения файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdSelectePath(object sender, RoutedEventArgs e)
        {
            UserDialogService DialogService = new();
            if (!DialogService.SelectFolder(Title, out string path, SelectPath)) return;
            SelectPath = path;
        }
        #endregion

    }
}
