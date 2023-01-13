using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Models.Settings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Project_Сonfigurator.Views.DialogControl
{
    public partial class WindowEditDefaultMapDefense
    {
        #region Карта параметров
        /// <summary>
        /// Карта параметров
        /// </summary>
        public ObservableCollection<SettingDefualtDefenseMap> DefaultMap
        {
            get => (ObservableCollection<SettingDefualtDefenseMap>)GetValue(DefaultMapProperty);
            set => SetValue(DefaultMapProperty, value);
        }
        public static readonly DependencyProperty DefaultMapProperty = DependencyProperty.Register(
            nameof(DefaultMap),
            typeof(ObservableCollection<SettingDefualtDefenseMap>), 
            typeof(WindowEditDefaultMapDefense),
            new PropertyMetadata(default(ObservableCollection<SettingDefualtDefenseMap>)));
        #endregion

        #region Выбранный параметр
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        private SettingDefualtDefenseMap SelectedDefaultMapLocal
        {
            get => (SettingDefualtDefenseMap)GetValue(SelectedDefaultMapLocalProperty);
            set => SetValue(SelectedDefaultMapLocalProperty, value);
        }
        private static readonly DependencyProperty SelectedDefaultMapLocalProperty = DependencyProperty.Register(
            nameof(SelectedDefaultMapLocal),
            typeof(SettingDefualtDefenseMap),
            typeof(WindowEditDefaultMapDefense),
            new PropertyMetadata(default(SettingDefualtDefenseMap)));
        #endregion

        #region Создать параметр
        /// <summary>
        /// Создать параметр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdCreateNewParam(object sender, RoutedEventArgs e)
        {
            DefaultMap ??= new ObservableCollection<SettingDefualtDefenseMap> ();
            DefaultMap.Add(new SettingDefualtDefenseMap
            {
                Param = new BaseParam
                {
                    Index = $"{DefaultMap.Count + 1}",
                    Id = "",
                    Description = "",
                    VarName = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = ""
                },
                Setpoints = new BaseSetpoints
                {
                    Index = $"{DefaultMap.Count + 1}",
                    Id = "",
                    Description = "",
                    VarName = "",
                    Address = "",
                    Unit = "сек.",
                    Value = ""
                }
            });
            DataGridInputParam.Items.Refresh();
            SelectedDefaultMapLocal = DefaultMap[^1];
        }
        #endregion

        #region Удалить выбранный параметр
        /// <summary>
        /// Удалить выбранный параметр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdDeleteSelectedParam(object sender, RoutedEventArgs e)
        {
            var index = DefaultMap.IndexOf(SelectedDefaultMapLocal);
            if (index < 0) return;
            DefaultMap.Remove(SelectedDefaultMapLocal);

            if (DefaultMap.Count > 0)
            {
                if (index > 0)
                    SelectedDefaultMapLocal = DefaultMap[index - 1];
                else
                    SelectedDefaultMapLocal = DefaultMap[index];
            }
            DataGridInputParam.Items.Refresh();
        }
        #endregion

        #region Обновить нередактируемые данные
        /// <summary>
        /// Обновить нередактируемые данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdUpdateData(object sender, RoutedEventArgs e)
        {
            var index = 0;
            foreach (var _Map in DefaultMap)
            {
                if (VarName is not null && !string.IsNullOrWhiteSpace(VarName))
                    _Map.Param.VarName = $"{VarName}[{++index}]";

                if (SetpointsVarName is not null && !string.IsNullOrWhiteSpace(SetpointsVarName))
                    _Map.Setpoints.VarName = $"{SetpointsVarName}[{index}]";

                if (SetpointsAddress is not null && !string.IsNullOrWhiteSpace(SetpointsAddress))
                    _Map.Setpoints.Address = $"%MW{int.Parse(SetpointsAddress.Replace("%MW", "")) + (index - 1)}";

                if (SetpointsId is not null && !string.IsNullOrWhiteSpace(SetpointsId))
                    _Map.Setpoints.Id = $"H{int.Parse(SetpointsId.Replace("H", "")) + (index - 1)}";
            }
            DataGridInputParam.Items.Refresh();
        }
        #endregion

        #region Имя переменной параметра
        /// <summary>
        /// Имя переменной параметра
        /// </summary>
        public string VarName
        {
            get => (string)GetValue(VarNameProperty);
            set => SetValue(VarNameProperty, value);
        }
        public static readonly DependencyProperty VarNameProperty = DependencyProperty.Register(
            nameof(VarName),
            typeof(string),
            typeof(WindowEditDefaultMapDefense),
            new PropertyMetadata(default(string)));
        #endregion

        #region Адрес уставки
        /// <summary>
        /// Адрес уставки
        /// </summary>
        public string SetpointsAddress
        {
            get => (string)GetValue(SetpointsAddressProperty);
            set => SetValue(SetpointsAddressProperty, value);
        }
        public static readonly DependencyProperty SetpointsAddressProperty = DependencyProperty.Register(
            nameof(SetpointsAddress),
            typeof(string),
            typeof(WindowEditDefaultMapDefense),
            new PropertyMetadata(default(string)));
        #endregion

        #region Имя переменной уставки
        /// <summary>
        /// Имя переменной уставки
        /// </summary>
        public string SetpointsVarName
        {
            get => (string)GetValue(SetpointsVarNameProperty);
            set => SetValue(SetpointsVarNameProperty, value);
        }
        public static readonly DependencyProperty SetpointsVarNameProperty = DependencyProperty.Register(
            nameof(SetpointsVarName),
            typeof(string),
            typeof(WindowEditDefaultMapDefense),
            new PropertyMetadata(default(string)));
        #endregion

        #region Идентификатор уставки
        /// <summary>
        /// Идентификатор уставки
        /// </summary>
        public string SetpointsId
        {
            get => (string)GetValue(SetpointsIdProperty);
            set => SetValue(SetpointsIdProperty, value);
        }
        public static readonly DependencyProperty SetpointsIdProperty = DependencyProperty.Register(
            nameof(SetpointsId),
            typeof(string),
            typeof(WindowEditDefaultMapDefense),
            new PropertyMetadata(default(string)));
        #endregion

        public WindowEditDefaultMapDefense() => InitializeComponent();
    }
}
