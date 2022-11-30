using Project_Сonfigurator.Models.Settings;
using System.Windows;
using System.Collections.Generic;

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

        #region Данные выбранного модуля
        private int SelectedIndexVendorModuleType
        {
            get => (int)GetValue(SelectedIndexVendorModuleTypeProperty);
            set => SetValue(SelectedIndexVendorModuleTypeProperty, value);
        }
        /// <summary>
        /// Данные выбранного модуля
        /// </summary>
        private static readonly DependencyProperty SelectedIndexVendorModuleTypeProperty = DependencyProperty.Register(
            nameof(SelectedIndexVendorModuleType),
            typeof(int),
            typeof(WindowEditVendor),
            new PropertyMetadata(default(int)));
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
                SelectedModule = new VendorModule(),
                Modules = new List<VendorModule>()
            });

            SelectedVendorModuleTypeData = VendorData.ModuleTypes[^1];
            DataGridTypeModule.Items.Refresh();
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
            var index = VendorData.ModuleTypes.IndexOf(SelectedVendorModuleTypeData);


            VendorData.ModuleTypes.Remove(SelectedVendorModuleTypeData);
            if (VendorData.ModuleTypes.Count > 0)
            {
                if (index > 0)
                    SelectedVendorModuleTypeData = VendorData.ModuleTypes[index - 1];
                else
                    SelectedVendorModuleTypeData = VendorData.ModuleTypes[index];
            }
            DataGridTypeModule.Items.Refresh();
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
            SelectedVendorModuleTypeData.SelectedModule = SelectedVendorModuleTypeData.Modules[^1];
            DataGridModules.Items.Refresh();
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
            var index = SelectedVendorModuleTypeData.Modules.IndexOf(SelectedVendorModuleData);


            SelectedVendorModuleTypeData.Modules.Remove(SelectedVendorModuleData);
            if (SelectedVendorModuleTypeData.Modules.Count > 0)
            {
                if (index > 0)
                    SelectedVendorModuleData = SelectedVendorModuleTypeData.Modules[index - 1];
                else
                    SelectedVendorModuleData = SelectedVendorModuleTypeData.Modules[index];
            }
            DataGridModules.Items.Refresh();
        }
        #endregion

    }
}
