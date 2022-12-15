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

        #region Новое имя файла
        public string SelectFileName
        {
            get => (string)GetValue(SelectFileNameProperty);
            set => SetValue(SelectFileNameProperty, value);
        }
        /// <summary>
        /// Новое имя файла
        /// </summary>
        public static readonly DependencyProperty SelectFileNameProperty = DependencyProperty.Register(
            nameof(SelectFileName),
            typeof(string),
            typeof(WindowSelectPath),
            new PropertyMetadata(default(string)));
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
            if (!DialogService.SelectFolder(Title, out string path, out string file_name, SelectPath)) return;
            if (file_name == "Выберите путь")
                file_name = "";

            SelectFileName = file_name;
            SelectPath = path + SelectFileName;
        }
        #endregion

    }
}
