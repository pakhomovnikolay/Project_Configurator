using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace Project_Сonfigurator.Views.DialogControl
{
    public partial class WindowEditVendor
    {
        #region Данные редактируемого вендора
        public Vendor VendorData
        {
            get => (Vendor)GetValue(VendorDataProperty);
            set => SetValue(VendorDataProperty, value);
        }
        /// <summary>
        /// Данные редактируемого вендора
        /// </summary>
        public static readonly DependencyProperty VendorDataProperty = DependencyProperty.Register(
            nameof(VendorData),
            typeof(Vendor),
            typeof(WindowEditVendor),
            new PropertyMetadata(new Vendor()));
        #endregion

        #region Данные выбранного типа модуля
        private VendorModuleType SelectedVendorModuleTypeData
        {
            get => (VendorModuleType)GetValue(SelectedVendorModuleTypeDataProperty);
            set => SetValue(SelectedVendorModuleTypeDataProperty, value);
        }
        /// <summary>
        /// Данные выбранного типа модуля
        /// </summary>
        private static readonly DependencyProperty SelectedVendorModuleTypeDataProperty = DependencyProperty.Register(
            nameof(SelectedVendorModuleTypeData),
            typeof(VendorModuleType),
            typeof(WindowEditVendor),
            new PropertyMetadata(default(VendorModuleType)));
        #endregion

        #region Данные выбранного модуля
        private VendorModule SelectedVendorModuleData
        {
            get => (VendorModule)GetValue(SelectedVendorModuleDataProperty);
            set => SetValue(SelectedVendorModuleDataProperty, value);
        }
        /// <summary>
        /// Данные выбранного модуля
        /// </summary>
        private static readonly DependencyProperty SelectedVendorModuleDataProperty = DependencyProperty.Register(
            nameof(SelectedVendorModuleData),
            typeof(VendorModule),
            typeof(WindowEditVendor),
            new PropertyMetadata(default(VendorModule)));
        #endregion

        #region Конструктор
        public WindowEditVendor() => InitializeComponent();
        #endregion

        #region Создать новый тип модуля
        /// <summary>
        /// Создать новый тип модуля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdCreateTypeModule(object sender, RoutedEventArgs e)
        {

            VendorData.ModuleTypes.Add(new VendorModuleType()
            {
                Name = $"Новый тип модуля {VendorData.ModuleTypes.Count + 1}",
                Modules = new ObservableCollection<VendorModule>()
            });

            SelectedVendorModuleTypeData = VendorData.ModuleTypes[^1];
        }
        #endregion

        #region Удалить выбранный тип модуля
        /// <summary>
        /// Удалить выбранный тип модуля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdDeleteTypeModule(object sender, RoutedEventArgs e)
        {
            IUserDialogService UserDialog = new UserDialogService();
            if (!UserDialog.SendMessage(Title, "Вы действительно хотите\nудалить выбранный тип моудля?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.Yes,
                MessageBoxOptions.None))
                return;

            var index = VendorData.ModuleTypes.IndexOf(SelectedVendorModuleTypeData);
            index = index == 0 ? index : index - 1;

            VendorData.ModuleTypes.Remove(SelectedVendorModuleTypeData);
            if (VendorData.ModuleTypes.Count > 0)
                SelectedVendorModuleTypeData = VendorData.ModuleTypes[index];
        }
        #endregion

        #region Создать новый модуль
        /// <summary>
        /// Создать новый модуль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdCreateModule(object sender, RoutedEventArgs e)
        {
            SelectedVendorModuleTypeData.Modules.Add(new VendorModule() { Name = $"Новый модуль {SelectedVendorModuleTypeData.Modules.Count + 1}" });
            SelectedVendorModuleData = SelectedVendorModuleTypeData.Modules[^1];
        }
        #endregion

        #region Удалить выбранный тип модуля
        /// <summary>
        /// Удалить выбранный тип модуля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdDeleteModule(object sender, RoutedEventArgs e)
        {
            IUserDialogService UserDialog = new UserDialogService();
            if (!UserDialog.SendMessage(Title, "Вы действительно хотите\nудалить выбранный моудль?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.Yes,
                MessageBoxOptions.None))
                return;

            var index = SelectedVendorModuleTypeData.Modules.IndexOf(SelectedVendorModuleData);
            index = index == 0 ? index : index - 1;

            SelectedVendorModuleTypeData.Modules.Remove(SelectedVendorModuleData);
            if (SelectedVendorModuleTypeData.Modules.Count > 0)
                SelectedVendorModuleData = SelectedVendorModuleTypeData.Modules[index];
        }
        #endregion

    }
}
