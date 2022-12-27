using MySqlConnector;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using Project_Сonfigurator.ViewModels.UserControls.Signals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Project_Сonfigurator.Services
{
    public class DBService : IDBService
    {
        #region Данные
        /// <summary>
        /// Данные
        /// </summary>
        public DBData AppData { get; set; }
        #endregion

        #region Записи данных в БД

        #region Запрос записи данных
        /// <summary>
        /// Запрос записи данных
        /// </summary>
        /// <returns></returns>
        public bool RequestSetData(List<object> _ViewModels)
        {
            #region Формируем данные приложения перед сохранением
            FormingAppDataBeforeSaving(_ViewModels);
            #endregion

            MySqlConnection _MySqlConnection = new();
            MySqlCommand _MySqlCommand;
            string Query;
            bool Success;

            var Log = new LogSerivece();
            var DialogService = new UserDialogService();
            var Config = Program.Settings.Config;

            try
            {
                #region Сохраняем данные
                foreach (var ConnectDB in Config.ServerDB)
                {
                    ConnectDB.SuccessUpdate = false;
                    if (ConnectDB.IsSelection)
                    {
                        Success = false;
                        var ip_address = Array.Empty<string>();
                        if (ConnectDB.IPAddress is not null)
                            ip_address = ConnectDB.IPAddress.Split(',');

                        foreach (var ip in ip_address)
                        {
                            if (_MySqlConnection.State == System.Data.ConnectionState.Open)
                                _MySqlConnection.Close();


                            if (Success) continue;

                            #region Создаем БД, при ее отсутствии
                            var СonnectionString =
                                        $"SERVER={ip};" +
                                        $"PORT={ConnectDB.Port};" +
                                        //$"DATABASE=mariadb;" +
                                        $"UID={ConnectDB.UserName};" +
                                        $"PASSWORD={ConnectDB.Password};" +
                                        $"Connection Timeout = {ConnectDB.TimeoutConnect}";

                            _MySqlConnection = new(СonnectionString);
                            try
                            {
                                _MySqlConnection.Open();

                                Query = $"CREATE DATABASE IF NOT EXISTS `{ConnectDB.NameDB}` COLLATE = 'utf8mb4_general_ci';";
                                _MySqlCommand = new MySqlCommand(Query, _MySqlConnection);
                                _MySqlCommand.ExecuteNonQuery();
                                _MySqlConnection.Close();
                            }
                            catch (Exception e)
                            {
                                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
                                continue;
                            }
                            #endregion

                            #region Настройки подключения
                            try
                            {
                                СonnectionString =
                                           $"SERVER={ip};" +
                                           $"PORT={ConnectDB.Port};" +
                                           $"DATABASE={ConnectDB.NameDB};" +
                                           $"UID={ConnectDB.UserName};" +
                                           $"PASSWORD={ConnectDB.Password};" +
                                           $"Connection Timeout = {ConnectDB.TimeoutConnect}";
                                _MySqlConnection = new(СonnectionString);
                                _MySqlConnection.Open();
                            }
                            catch (Exception e)
                            {
                                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
                                continue;
                            }

                            #endregion

                            #region Формируем данные для БД
                            foreach (var item in _ViewModels)
                            {
                                List<string> FieldValue = new();
                                switch (item)
                                {
                                    #region Компоновка корзин
                                    case LayotRackUserControlViewModel:

                                        List<string> Field = new() {
                                            "`USO_NAME`", "`RACK_NAME`", "`MODULE_TYPE`", "`MODULE_NAME`", "`CHANNEL_INDEX`", "`CHANNEL_ID`",
                                            "`CHANNEL_DESCRIPTION`", "`VAR_NAME_SU`", "`BIT`", "`ADDRESS`", "`VAR_NAME_VU`" };

                                        foreach (var _USO in AppData.USOList)
                                        {
                                            if (string.IsNullOrWhiteSpace(_USO.Name)) continue;

                                            foreach (var _Rack in _USO.Racks)
                                            {
                                                if (string.IsNullOrWhiteSpace(_Rack.Name)) continue;

                                                foreach (var _Module in _Rack.Modules)
                                                {
                                                    if (string.IsNullOrWhiteSpace(_Module.Name)) continue;
                                                    if (_Module.Channels.Count <= 0)
                                                    {
                                                        var _FieldValue =
                                                            $"('{_USO.Name}', '{_Rack.Name}', '{_Module.Type}', '{_Module.Name}', " +
                                                            $"'{"-"}', '{"-"}', '{"-"}', '{"-"}', '{"-"}', '{"-"}', '{"-"}'),";
                                                        FieldValue.Add(_FieldValue);
                                                    }
                                                    foreach (var _Channel in _Module.Channels)
                                                    {
                                                        var _FieldValue =
                                                            $"('{_USO.Name}', '{_Rack.Name}', '{_Module.Type}', '{_Module.Name}', " +
                                                            $"'{_Channel.Index}', '{_Channel.Id}', '{_Channel.Description}', '{_Channel.VarName}', " +
                                                            $"'{_Channel.Bit}', '{_Channel.Address}', '{_Channel.VarNameVU}'),";
                                                        FieldValue.Add(_FieldValue);
                                                    }
                                                }
                                            }
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "USO", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region Сигналы DI
                                    case SignalsDIUserControlViewModel:

                                        Field = new() {
                                            "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`AREA`", "`ADDRESS`", "`LINK_VALUE`", "`ADDRESS_SIG`" };

                                        foreach (var _Signal in AppData.SignalDI)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Signal.Signal.Id) && string.IsNullOrWhiteSpace(_Signal.Signal.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Signal.Signal.Id}', '{_Signal.Signal.Description}', " +
                                                $"'{_Signal.Signal.VarName}', '{_Signal.Signal.Area}', " +
                                                $"'{_Signal.Signal.Address}', '{_Signal.Signal.LinkValue}', '{_Signal.Signal.Index}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "SIGNAL_DI", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region Сигналы AI
                                    case SignalsAIUserControlViewModel:

                                        Field = new()
                                        {
                                            "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`AREA`", "`ADDRESS`", "`LINK_VALUE`", "`ADDRESS_SIG`",
                                            "`INDEX_UMPNA`", "`UNIT`", "`TYPE_VIBRATION`", "`INDEX_PZ`", "`TYPE_PI`", "`INDEX_BD`",
                                            "`LEVEL_RPP`", "`ADDRESS_UTS`", "`CONVERTER_KGS`"
                                        };

                                        foreach (var _Signal in AppData.SignalAI)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Signal.Signal.Id) && string.IsNullOrWhiteSpace(_Signal.Signal.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Signal.Signal.Id}', '{_Signal.Signal.Description}', '{_Signal.Signal.VarName}', '{_Signal.Signal.Area}', " +
                                                $"'{_Signal.Signal.Address}', '{_Signal.Signal.LinkValue}', '{_Signal.Signal.Index}', " +
                                                $"'{_Signal.IndexNA}', '{_Signal.Unit}', '{_Signal.TypeVibration}', '{_Signal.IndexPZ}', " +
                                                $"'{_Signal.TypePI}', '{_Signal.IndexBD}', '{_Signal.LevelRPP}', '{_Signal.AddresUTS}', '{_Signal.ConverterKgs}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "SIGNAL_AI", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region Сигналы DO
                                    case SignalsDOUserControlViewModel:

                                        Field = new() { "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`AREA`", "`ADDRESS`", "`LINK_VALUE`", "`ADDRESS_SIG`" };

                                        foreach (var _Signal in AppData.SignalDO)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Signal.Signal.Id) && string.IsNullOrWhiteSpace(_Signal.Signal.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Signal.Signal.Id}', '{_Signal.Signal.Description}', '{_Signal.Signal.VarName}', '{_Signal.Signal.Area}', " +
                                                $"'{_Signal.Signal.Address}', '{_Signal.Signal.LinkValue}', '{_Signal.Signal.Index}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "SIGNAL_DO", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region Сигналы AO
                                    case SignalsAOUserControlViewModel:

                                        Field = new() { "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`AREA`", "`ADDRESS`", "`LINK_VALUE`", "`ADDRESS_SIG`" };

                                        foreach (var _Signal in AppData.SignalAO)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Signal.Signal.Id) && string.IsNullOrWhiteSpace(_Signal.Signal.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Signal.Signal.Id}', '{_Signal.Signal.Description}', '{_Signal.Signal.VarName}', '{_Signal.Signal.Area}', " +
                                                $"'{_Signal.Signal.Address}', '{_Signal.Signal.LinkValue}', '{_Signal.Signal.Index}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "SIGNAL_AO", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region Секции шин
                                    case ECUserControlViewModel:

                                        Field = new() { "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`INV`", "`TYPE_SIGNAL`", "`LINK_ADDRESSVALUE`" };

                                        foreach (var _Param in AppData.ECParam)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', '{_Param.Inv}', " +
                                                $"'{_Param.TypeSignal}', '{_Param.Address}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "PARAM_EC", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region DI формируемые
                                    case UserDIUserControlViewModel:

                                        Field = new() { "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`BIT`", "`ADDRESS`" };

                                        foreach (var _Param in AppData.UserDI)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', " +
                                                $"'{(int.Parse(_Param.Index) - 1) % 16}', '{_Param.Address}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "USER_DI", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region AI формируемые
                                    case UserAIUserControlViewModel:

                                        Field = new() { "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`ADDRESS`" };

                                        foreach (var _Param in AppData.UserAI)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                            var _FieldValue = $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', '{_Param.Address}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "USER_AI", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region Регистры формируемые
                                    case UserRegUserControlViewModel:

                                        Field = new() { "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`ADDRESS_VU`" };

                                        foreach (var _Param in AppData.UserReg)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                            var _FieldValue = $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', '{_Param.Address}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "USER_REG", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region Сигналы групп
                                    case SignalsGroupUserControlViewModel:

                                        Field = new() { "`ID`", "`DESCRIPTION`", "`INV`", "`TYPE_SIGNAL`", "`ADDRESS`", "`ADDRESS_SIG`" };

                                        foreach (var _Param in AppData.SignalGroup)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Id}', '{_Param.Description}', '{_Param.Inv}', " +
                                                $"'{_Param.TypeSignal}', '{_Param.Address}', '{_Param.Index}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "SIGNAL_GROUP", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region Группы сигналов
                                    case GroupsSignalUserControlViewModel:

                                        Field = new() { "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`QTY`", "`ADDRESS_START`", "`ADDRESS_END`", "`ADDRESS_SIG`" };

                                        foreach (var _Param in AppData.GroupSignals)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', '{_Param.QtyInGroup}', " +
                                                $"'{_Param.AddressStart}', '{_Param.AddressEnd}', '{_Param.Param.Index}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "GROUP_SIGNAL", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region UZD
                                    case UZDUserControlViewModel:

                                        Field = new()
                                        {
                                            "`DESCRIPTION`", "`VAR_NAME`", "`SHORT_NAME`", "`INDEX_EC`", "`INDEX_GROUP`", "`NAME_GROUP`",
                                            "`DIST`", "`DOUBLE_STOP`", "`BUR`", "`COz`", "`CZz`", "`EC`", "`CHECK_STATE`",
                                            "`RS_OFF`", "`TYPE`", "`INDEX_PZ`", "`INDEX_BD`"
                                        };


                                        foreach (var _Param in AppData.UZD)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Description}', '{_Param.VarName}', '{_Param.ShortDescription}', '{_Param.IndexEC}', '{_Param.IndexGroup}', " +
                                                $"'{_Param.DescriptionGroup}', '{_Param.Dist}', '{_Param.DoubleStop}', '{_Param.Bur}', '{_Param.COz}', '{_Param.CZz}', " +
                                                $"'{_Param.EC}', '{_Param.CheckState}', '{_Param.RsOff}', '{_Param.TypeZD}', '{_Param.IndexPZ}', '{_Param.IndexBD}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "UZD", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region UVS
                                    case UVSUserControlViewModel:

                                        Field = new()
                                        {
                                            "`DESCRIPTION`", "`VAR_NAME`", "`SHORT_NAME`", "`INDEX_EC`", "`INDEX_GROUP`", "`NAME_GROUP`",
                                            "`RESERVABLE`", "`TYPE_PRESSURE`", "`COz`", "`ONE_PRESSURE_SENSOR_GROUP`", "`TYPE`"
                                        };

                                        foreach (var _Param in AppData.UVS)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Description}', '{_Param.VarName}', '{_Param.ShortDescription}', '{_Param.IndexEC}', " +
                                                $"'{_Param.IndexGroup}', '{_Param.DescriptionGroup}', '{_Param.Reservable}', '{_Param.TypePressure}', " +
                                                $"'{_Param.COz}', '{_Param.OnePressureSensorGroup}', '{_Param.TypePressure}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "UVS", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region UMPNA
                                    case UMPNAUserControlViewModel:

                                        #region Данные UMPNA
                                        Field = new()
                                        {
                                            "`DESCRIPTION`", "`VAR_NAME`", "`SHORT_NAME`", "`INDEX_PZ`", "`INDEX_VZ`", "`TYPE`",
                                            "`INDEX_GROUP_MS`", "`USED_MCP`", "`USED_KPD`", "`QTY_BUTTON_STOP`"
                                        };

                                        foreach (var _Param in AppData.UMPNA)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Description}', '{_Param.VarName}', '{_Param.ShortDescription}', '{_Param.IndexPZ}', " +
                                                $"'{_Param.IndexVZ}', '{_Param.TypeUMPNA}', '{_Param.IndexGroupMS}', '{_Param.UsedMCP}', " +
                                                $"'{_Param.UsedKPD}', '{_Param.CountButtonStop}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "UMPNA", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        #endregion

                                        #region Данные KGMPNA
                                        FieldValue = new();
                                        Field = new()
                                        {
                                            "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`INV`", "`TYPE_SIGNAL`", "`ADDRESS`", "`NO_MASKED`"
                                        };

                                        foreach (var _UMPNA in AppData.UMPNA)
                                        {
                                            foreach (var _Param in _UMPNA.KGMPNA)
                                            {
                                                if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                                var _FieldValue =
                                                    $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', '{_Param.Param.Inv}', " +
                                                    $"'{_Param.Param.TypeSignal}', '{_Param.Param.Address}', '{_Param.NoMasked}'),";
                                                FieldValue.Add(_FieldValue);
                                            }
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "KGMPNA", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        #endregion

                                        #region Данные KTPRA
                                        FieldValue = new();
                                        Field = new()
                                        {
                                            "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`INV`", "`TYPE_SIGNAL`", "`ADDRESS`", "`STATE`", "`NO_MASKED`",
                                            "`AVR`", "`TYPE`", "`STOP_TYPE`"
                                        };

                                        foreach (var _UMPNA in AppData.UMPNA)
                                        {
                                            foreach (var _Param in _UMPNA.KTPRA)
                                            {
                                                if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                                var _FieldValue =
                                                    $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', '{_Param.Param.Inv}', " +
                                                    $"'{_Param.Param.TypeSignal}', '{_Param.Param.Address}', '{_Param.StateUMPNA}', '{_Param.NoMasked}', " +
                                                    $"'{_Param.AVR}', '{_Param.Type}', '{_Param.StopType}'),";
                                                FieldValue.Add(_FieldValue);
                                            }
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "KTPRA", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        #endregion

                                        #region Данные KTPRAS
                                        FieldValue = new();
                                        Field = new()
                                        {
                                            "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`INV`", "`TYPE_SIGNAL`", "`ADDRESS`", "`STATE`", "`TYPE_WARNING`", "`TYPE`"
                                        };

                                        foreach (var _UMPNA in AppData.UMPNA)
                                        {
                                            foreach (var _Param in _UMPNA.KTPRAS)
                                            {
                                                if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                                var _FieldValue =
                                                    $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', " +
                                                    $"'{_Param.Param.Inv}', '{_Param.Param.TypeSignal}', '{_Param.Param.Address}', " +
                                                    $"'{_Param.StateUMPNA}', '{_Param.TypeWarning}', '{_Param.Type}'),";
                                                FieldValue.Add(_FieldValue);
                                            }
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "KTPRAS", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        #endregion

                                        break;
                                    #endregion

                                    #region KTPR
                                    case KTPRUserControlViewModel:

                                        Field = new()
                                        {
                                            "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`INV`", "`TYPE_SIGNAL`", "`ADDRESS`",
                                            "`STATE_STATION`", "`SHOULDER`", "`SUB_SHOULDER`", "`AUTODEBLOCK`", "`NO_MASKED`",
                                            "`STOP_TYPE_STATION`", "`STOP_TYPE_NA`", "`TYPE`"
                                        };

                                        foreach (var _Param in AppData.KTPR)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', " +
                                                $"'{_Param.Param.Inv}', '{_Param.Param.TypeSignal}', '{_Param.Param.Address}', " +
                                                $"'{_Param.StateStation}', '{_Param.Shoulder}', '{_Param.SubShoulder}', " +
                                                $"'{_Param.Autodeblok}', '{_Param.NoMasked}', '{_Param.StopTypeNS}', '{_Param.StopTypeUMPNA}', '{_Param.Type}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "KTPR", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region KTPRS
                                    case KTPRSUserControlViewModel:

                                        Field = new()
                                        {
                                            "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`INV`", "`TYPE_SIGNAL`", "`ADDRESS`", "`TYPE_WARNING`", "`TYPE`"
                                        };

                                        foreach (var _Param in AppData.KTPRS)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', " +
                                                $"'{_Param.Param.Inv}', '{_Param.Param.TypeSignal}', '{_Param.Param.Address}', " +
                                                $"'{_Param.TypeWarning}', '{_Param.Type}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "KTPRS", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region Signaling
                                    case SignalingUserControlViewModel:

                                        Field = new()
                                        {
                                            "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`INV`", "`TYPE_SIGNAL`", "`ADDRESS`", "`VAR_NAME_VU`", "`BIT`",
                                            "`COLOR`", "`USO_INDEX`", "`TYPE_WARNING`"
                                        };

                                        foreach (var _Param in AppData.Signaling)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', " +
                                                $"'{_Param.Param.Inv}', '{_Param.Param.TypeSignal}', '{_Param.Param.Address}', " +
                                                $"'{_Param.VarNameVU}', '{(int.Parse(_Param.Param.Index) - 1) % 16}', " +
                                                $"'{_Param.Color}', '{_Param.IndexUSO}', '{_Param.TypeWarning}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "Signaling", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region UTS
                                    case UTSUserControlViewModel:

                                        Field = new()
                                        {
                                            "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`TYPE_SIGNAL`", "`ADDRESS`", "`LOCK_ENABLE`", "`INDEX_PZ`", "`APT_OFF`",
                                            "`INDEX_GROUP`", "`TYPE_KCO`", "`ADDRESS_KCO`", "`TYPE_SIGNAL_SOD`", "`ADDRESS_SOD`", "`TYPE_SIGNALERR_SOD`", "`ADDRESS_ERR_SOD`"
                                        };

                                        foreach (var _Param in AppData.UTS)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', '{_Param.Type}', '{_Param.Param.Address}', " +
                                                $"'{_Param.LockEnable}', '{_Param.IndexPZ}', '{_Param.AptOff}', '{_Param.IndexGroup}', " +
                                                $"'{_Param.KCO.TypeSignal}', '{_Param.KCO.Address}', '{_Param.SignalSOD.TypeSignal}', '{_Param.SignalSOD.Address}', " +
                                                $"'{_Param.SignalErrSOD.TypeSignal}', '{_Param.SignalErrSOD.Address}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "UTS", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region SETPOINTS_REAL
                                    case UstRealUserControlViewModel:

                                        Field = new()
                                        {
                                            "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`ADDRESS`", "`VALUE`", "`UNIT`", "`QTY_SIMBOLS_COMMA`"
                                        };

                                        foreach (var _Param in AppData.SetpointsReal)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Setpoints.Id) && string.IsNullOrWhiteSpace(_Param.Setpoints.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Setpoints.Id}', '{_Param.Setpoints.Description}', '{_Param.Setpoints.VarName}', " +
                                                $"'{_Param.Setpoints.Address}', '{_Param.Setpoints.Value}', '{_Param.Setpoints.Unit}', '{_Param.QtySimbolsComma}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "SETPOINTS_REAL", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region Врем. уставки общие
                                    case UstCommonUserControlViewModel:

                                        Field = new()
                                        {
                                            "`ID`", "`DESCRIPTION`", "`VAR_NAME`", "`ADDRESS`", "`VALUE`", "`UNIT`"
                                        };

                                        foreach (var _Param in AppData.SetpointsCommon)
                                        {
                                            if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', " +
                                                $"'{_Param.Address}', '{_Param.Value}', '{_Param.Unit}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "SETPOINTS_COMMON", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                    #endregion

                                    #region Карта ручн ввода
                                    case HandMapUserControlViewModel:

                                        Field = new()
                                        {
                                            "`DESCRIPTION`", "`VAR_NAME`", "`BIT`"
                                        };

                                        foreach (var _Param in AppData.HandMap)
                                        {
                                            var iSh = int.TryParse(_Param.Index, out int _Index);
                                            if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                            var _FieldValue =
                                                $"('{_Param.Description}', '{_Param.VarName}', '{_Index % 16}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

                                        if (FieldValue is null || FieldValue.Count <= 0) continue;
                                        _MySqlCommand = new MySqlCommand(FormingData(_MySqlConnection, "HAND_MAP", Field, FieldValue), _MySqlConnection);
                                        _MySqlCommand.ExecuteNonQuery();
                                        break;
                                        #endregion

                                }
                            }
                            #endregion

                            ConnectDB.SuccessUpdate = true;
                            Success = Success || ConnectDB.SuccessUpdate;
                        }
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
                Success = false;
                return false;
            }
            finally
            {
                if (_MySqlConnection.State == System.Data.ConnectionState.Open)
                    _MySqlConnection.Close();
            }

            return true;
        }
        #endregion

        #region Формируем данные приложения перед сохранением
        /// <summary>
        /// Формируем данные приложения перед сохранением
        /// </summary>
        public void FormingAppDataBeforeSaving(List<object> _ViewModels)
        {
            var Log = new LogSerivece();
            try
            {
                foreach (var item in _ViewModels)
                {
                    object _Data = item switch
                    {
                        LayotRackUserControlViewModel Data => AppData.USOList = Data.USOList is null ? new() : Data.USOList,
                        SignalsDIUserControlViewModel Data => AppData.SignalDI = Data.SignalsDI is null ? new() : Data.SignalsDI,
                        SignalsAIUserControlViewModel Data => AppData.SignalAI = Data.SignalsAI is null ? new() : Data.SignalsAI,
                        SignalsDOUserControlViewModel Data => AppData.SignalDO = Data.SignalsDO is null ? new() : Data.SignalsDO,
                        SignalsAOUserControlViewModel Data => AppData.SignalAO = Data.SignalsAO is null ? new() : Data.SignalsAO,
                        ECUserControlViewModel Data => AppData.ECParam = Data.BaseParams is null ? new() : Data.BaseParams,
                        UserDIUserControlViewModel Data => AppData.UserDI = Data.BaseSignals is null ? new() : Data.BaseSignals,
                        UserAIUserControlViewModel Data => AppData.UserAI = Data.BaseSignals is null ? new() : Data.BaseSignals,
                        UserRegUserControlViewModel Data => AppData.UserReg = Data.BaseParams is null ? new() : Data.BaseParams,
                        SignalsGroupUserControlViewModel Data => AppData.SignalGroup = Data.BaseParams is null ? new() : Data.BaseParams,
                        GroupsSignalUserControlViewModel Data => AppData.GroupSignals = Data.GroupSignals is null ? new() : Data.GroupSignals,
                        UZDUserControlViewModel Data => AppData.UZD = Data.UZD is null ? new() : Data.UZD,
                        UVSUserControlViewModel Data => AppData.UVS = Data.UVS is null ? new() : Data.UVS,
                        UMPNAUserControlViewModel Data => AppData.UMPNA = Data.UMPNA is null ? new() : Data.UMPNA,
                        KTPRUserControlViewModel Data => AppData.KTPR = Data.KTPR is null ? new() : Data.KTPR,
                        KTPRSUserControlViewModel Data => AppData.KTPRS = Data.KTPRS is null ? new() : Data.KTPRS,
                        SignalingUserControlViewModel Data => AppData.Signaling = Data.Signaling is null ? new() : Data.Signaling,
                        UTSUserControlViewModel Data => AppData.UTS = Data.UTS is null ? new() : Data.UTS,
                        UstRealUserControlViewModel Data => AppData.SetpointsReal = Data.Setpoints is null ? new() : Data.Setpoints,
                        UstCommonUserControlViewModel Data => AppData.SetpointsCommon = Data.Setpoints is null ? new() : Data.Setpoints,
                        HandMapUserControlViewModel Data => AppData.HandMap = Data.BaseParams is null ? new() : Data.BaseParams,
                        _ => null
                    };
                }

            }
            catch (Exception e)
            {
                Log.WriteLog($"Не удалось сохранить данные приложения - {e}", App.NameApp);
            }

        }
        #endregion

        #region Формируем данные для БД
        /// <summary>
        /// Формируем данные УСО
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private static string FormingData(MySqlConnection mySqlConnection, string NameTable, List<string> Field, List<string> FieldValue)
        {
            var Log = new LogSerivece();

            #region Удаляем таблицу
            var Query = $"DROP TABLE IF EXISTS `{NameTable}`";
            var _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
            _MySqlCommand.ExecuteNonQuery();
            #endregion

            #region Создаем таблицу
            try
            {
                Query = $"CREATE TABLE `{NameTable}`(`INDEX` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,";
                foreach (var item in Field)
                {
                    Query += $"{item} TEXT,";
                }
                Query += $"PRIMARY KEY(`INDEX`));";

                _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
                return "";
            }
            #endregion

            #region Наполняем таблицу данными
            try
            {
                Query = $"INSERT INTO {NameTable} (";
                foreach (var item in Field)
                {
                    var _item = item.Replace("`", "");
                    Query += $"{_item},";
                }
                Query = Query.TrimEnd(',');
                Query += $") VALUES";

                var QueryPar = "";
                foreach (var item in FieldValue)
                {
                    QueryPar += $"{item}";
                }
                QueryPar = QueryPar.TrimEnd(',');

                return Query + QueryPar;
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
                return "";
            }
            #endregion

        }
        #endregion

        #endregion

        #region Обновление данных во ViewModels

        #region Обновление данных во ViewModels
        /// <summary>
        /// Обновление данных во ViewModels
        /// </summary>
        /// <param name="Item"></param>
        public bool RefreshDataViewModel(object Item, bool CreateNewProject)
        {
            if (AppData is null && !CreateNewProject)
            {
                var path = string.IsNullOrWhiteSpace(Program.Settings.Config.PathProject) ? Program.PathConfig + "\\ProjectData.xml" : Program.Settings.Config.PathProject;
                AppData = LoadData(path);
            }


            if (Item is null) throw new ArgumentNullException(nameof(Item));
            return Item switch
            {
                LayotRackUserControlViewModel Data => RefreshUSO(Data),
                TableSignalsUserControlViewModel Data => RefreshTableSignals(Data),

                SignalsDIUserControlViewModel Data => RefreshSignalsDI(Data),
                SignalsAIUserControlViewModel Data => RefreshSignalsAI(Data),
                SignalsDOUserControlViewModel Data => RefreshSignalsDO(Data),
                SignalsAOUserControlViewModel Data => RefreshSignalsAO(Data),
                ECUserControlViewModel Data => RefreshEC(Data),
                UserDIUserControlViewModel Data => RefreshUserDI(Data),
                UserAIUserControlViewModel Data => RefreshUserAI(Data),
                UserRegUserControlViewModel Data => RefreshUserReg(Data),
                SignalsGroupUserControlViewModel Data => RefreshSignalsGroup(Data),
                GroupsSignalUserControlViewModel Data => RefreshGroupsSignal(Data),
                UZDUserControlViewModel Data => RefreshUZD(Data),
                UVSUserControlViewModel Data => RefreshUVS(Data),
                UMPNAUserControlViewModel Data => RefreshUMPNA(Data),
                KTPRUserControlViewModel Data => RefreshKTPR(Data),
                KTPRSUserControlViewModel Data => RefreshKTPRS(Data),
                SignalingUserControlViewModel Data => RefreshSignaling(Data),
                UTSUserControlViewModel Data => RefreshUTS(Data),
                UstRealUserControlViewModel Data => RefreshUstReal(Data),
                UstCommonUserControlViewModel Data => RefreshUstReal(Data),
                HandMapUserControlViewModel Data => RefreshHandMap(Data),
                _ => throw new NotSupportedException($"Редактирование объекта типа {Item.GetType().Name} не поддерживается")
            };
        }
        #endregion

        #region Обновляем данные Компоновки корзин
        /// <summary>
        /// Обновляем данные Компоновки корзин
        /// </summary>
        /// <returns></returns>
        private bool RefreshUSO(LayotRackUserControlViewModel Data)
        {
            Data.USOList = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.USOList.Count > 0)
            {
                foreach (var _USO in AppData.USOList)
                    Data.USOList.Add(_USO);

                Data.SelectedUSO = Data.USOList[0];
                if (Data.USOList[0].Racks.Count > 0)
                    Data.SelectedRack = Data.USOList[0].Racks[0];
            }
            #endregion

            Data.GeneratedData();
            return true;

        }
        #endregion

        #region Обновляем данные Таблицы сигналов
        /// <summary>
        /// Обновляем данные Таблицы сигналов
        /// </summary>
        /// <returns></returns>
        private bool RefreshTableSignals(TableSignalsUserControlViewModel Data)
        {
            Data.USOList = new();
            Data.SelectedUSO = new USO();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.USOList.Count > 0)
            {
                foreach (var _USO in AppData.USOList)
                {
                    var need_add_uso = false;
                    foreach (var _Rack in _USO.Racks)
                    {
                        foreach (var _Module in _Rack.Modules)
                        {
                            switch (_Module.Type)
                            {
                                case TypeModule.AI:
                                case TypeModule.DI:
                                case TypeModule.AO:
                                case TypeModule.DO:
                                case TypeModule.DA:
                                    need_add_uso = true;
                                    break;
                            }
                        }
                    }
                    if (need_add_uso)
                        Data.USOList.Add(_USO);
                }
                Data.SelectedUSO = Data.USOList[0];
            }
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные Сигналы DI
        /// <summary>
        /// Обновляем данные Сигналы DI
        /// </summary>
        /// <returns></returns>
        private bool RefreshSignalsDI(SignalsDIUserControlViewModel Data)
        {
            Data.SignalsDI = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.SignalDI.Count > 0)
            {
                foreach (var signal in AppData.SignalDI)
                    Data.SignalsDI.Add(signal);

                Data.SelectedSignalDI = Data.SignalsDI[0];
            }
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные Сигналы AI
        /// <summary>
        /// Обновляем данные Сигналы AI
        /// </summary>
        /// <returns></returns>
        private bool RefreshSignalsAI(SignalsAIUserControlViewModel Data)
        {
            Data.SignalsAI = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.SignalAI.Count > 0)
            {
                foreach (var signal in AppData.SignalAI)
                    Data.SignalsAI.Add(signal);

                Data.SelectedSignalAI = Data.SignalsAI[0];

            }
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные Сигналы DO
        /// <summary>
        /// Обновляем данные Сигналы DO
        /// </summary>
        /// <returns></returns>
        private bool RefreshSignalsDO(SignalsDOUserControlViewModel Data)
        {
            Data.SignalsDO = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.SignalDO.Count > 0)
            {
                foreach (var signal in AppData.SignalDO)
                    Data.SignalsDO.Add(signal);

                Data.SelectedSignalDO = Data.SignalsDO[0];
            }
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные Сигналы AO
        /// <summary>
        /// Обновляем данные Сигналы AO
        /// </summary>
        /// <returns></returns>
        private bool RefreshSignalsAO(SignalsAOUserControlViewModel Data)
        {
            Data.SignalsAO = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.SignalAO.Count > 0)
            {
                foreach (var signal in AppData.SignalAO)
                    Data.SignalsAO.Add(signal);

                Data.SelectedSignalAO = Data.SignalsAO[0];
            }
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные Секции шин
        /// <summary>
        /// Обновляем данные Секции шин
        /// </summary>
        /// <returns></returns>
        private bool RefreshEC(ECUserControlViewModel Data)
        {
            Data.BaseParams = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.ECParam.Count > 0)
            {
                foreach (var signal in AppData.ECParam)
                    Data.BaseParams.Add(signal);

                Data.SelectedSignal = Data.BaseParams[0];
                Data.GeneratedData();
                return true;
            }
            #endregion

            #region Генерируем сигналы DI
            for (int i = 0; i < 100; i++)
            {
                var signal = new BaseParam
                {
                    Index = $"{Data.BaseParams.Count + 1}",
                    Id = "",
                    Description = "",
                    VarName = $"EC_KTP_P[{Data.BaseParams.Count + 1}]",
                    Inv = "",
                    TypeSignal = "",
                    Address = ""
                };
                Data.BaseParams.Add(signal);
            }
            Data.SelectedSignal = Data.BaseParams[0];
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные DI формируемые
        /// <summary>
        /// Обновляем данные DI формируемые
        /// </summary>
        /// <returns></returns>
        private bool RefreshUserDI(UserDIUserControlViewModel Data)
        {
            Data.BaseSignals = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.UserDI.Count > 0)
            {
                foreach (var signal in AppData.UserDI)
                    Data.BaseSignals.Add(signal);

                Data.SelectedSignal = Data.BaseSignals[0];
                Data.GeneratedData();
                return true;
            }
            #endregion

            #region Генерируем сигналы DI
            for (int i = 0; i < 75; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    var signal = new BaseSignal
                    {
                        Index = $"{Data.BaseSignals.Count + 1}",
                        Id = "",
                        Description = "",
                        VarName = $"user_di[{i + 1}]",
                        Area = "",
                        Address = $"{Data.BaseSignals.Count + 1}",
                        LinkValue = ""
                    };
                    Data.BaseSignals.Add(signal);
                }
            }
            Data.SelectedSignal = Data.BaseSignals[0];
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные AI формируемые
        /// <summary>
        /// Обновляем данные AI формируемые
        /// </summary>
        /// <returns></returns>
        private bool RefreshUserAI(UserAIUserControlViewModel Data)
        {
            Data.BaseSignals = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.UserAI.Count > 0)
            {
                foreach (var signal in AppData.UserAI)
                    Data.BaseSignals.Add(signal);

                Data.SelectedSignal = Data.BaseSignals[0];
                Data.GeneratedData();
                return true;
            }

            #endregion

            #region Генерируем сигналы AI
            while (Data.BaseSignals.Count < 500)
            {
                var signal = new BaseSignal
                {
                    Index = $"{Data.BaseSignals.Count + 1}",
                    Id = "",
                    Description = "",
                    VarName = $"user_ai[{Data.BaseSignals.Count + 1}]",
                    Area = "",
                    Address = $"{Data.BaseSignals.Count + 1}",
                    LinkValue = ""
                };
                Data.BaseSignals.Add(signal);
            }
            Data.SelectedSignal = Data.BaseSignals[0];
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные Регистры формируемые
        /// <summary>
        /// Обновляем данные Регистры формируемые
        /// </summary>
        /// <returns></returns>
        private bool RefreshUserReg(UserRegUserControlViewModel Data)
        {
            Data.BaseParams = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.UserReg.Count > 0)
            {
                foreach (var signal in AppData.UserReg)
                    Data.BaseParams.Add(signal);

                Data.SelectedSignal = Data.BaseParams[0];
                Data.GeneratedData();
                return true;
            }

            #endregion

            #region Генерируем регистры формируемые
            while (Data.BaseParams.Count < 1000)
            {
                var signal = new BaseParam
                {
                    Index = $"{Data.BaseParams.Count + 1}",
                    Id = "",
                    Description = "",
                    VarName = $"user_reg[{Data.BaseParams.Count + 1}]",
                    Address = $"{Data.BaseParams.Count + 1}"
                };
                Data.BaseParams.Add(signal);
            }
            Data.SelectedSignal = Data.BaseParams[0];
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные Сигналы групп
        /// <summary>
        /// Обновляем данные Сигналы групп
        /// </summary>
        /// <returns></returns>
        private bool RefreshSignalsGroup(SignalsGroupUserControlViewModel Data)
        {
            Data.BaseParams = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.SignalGroup.Count > 0)
            {
                foreach (var signal in AppData.SignalGroup)
                    Data.BaseParams.Add(signal);

                Data.SelectedParam = Data.BaseParams[0];
                Data.GeneratedData();
                return true;
            }

            #endregion

            #region Генерируем данные
            while (Data.BaseParams.Count < 256)
            {
                var signal = new BaseParam()
                {
                    Index = $"{Data.BaseParams.Count + 1}",
                    Id = "",
                    Description = "",
                    VarName = "",
                    Inv = "",
                    TypeSignal = "",
                    Address = ""
                };
                Data.BaseParams.Add(signal);
            }
            Data.SelectedParam = Data.BaseParams[0];
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные Группы сигналов
        /// <summary>
        /// Обновляем данные Группы сигналов
        /// </summary>
        /// <returns></returns>
        private bool RefreshGroupsSignal(GroupsSignalUserControlViewModel Data)
        {
            Data.GroupSignals = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.GroupSignals.Count > 0)
            {
                foreach (var signal in AppData.GroupSignals)
                    Data.GroupSignals.Add(signal);

                Data.SelectedParam = Data.GroupSignals[0];
                Data.GeneratedData();
                return true;
            }

            #endregion

            #region Генерируем сигналы
            while (Data.GroupSignals.Count < 128)
            {
                var signal = new GroupSignal()
                {
                    AddressEnd = "",
                    AddressStart = "",
                    QtyInGroup = "",
                    Param = new BaseParam
                    {
                        Index = $"{Data.GroupSignals.Count + 1}",
                        Id = "",
                        Description = "",
                        VarName = $"sig_grp[{Data.GroupSignals.Count + 1}]",
                        Address = "",
                        Inv = "",
                        TypeSignal = ""
                    }
                };
                Data.GroupSignals.Add(signal);
            }
            Data.SelectedParam = Data.GroupSignals[0];
            #endregion 

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные UZD
        /// <summary>
        /// Обновляем данные UZD
        /// </summary>
        /// <returns></returns>
        private bool RefreshUZD(UZDUserControlViewModel Data)
        {
            Data.UZD = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.UZD.Count > 0)
            {
                foreach (var signal in AppData.UZD)
                    Data.UZD.Add(signal);

                Data.SelectedUZD = Data.UZD[0];
            }
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные UVS
        /// <summary>
        /// Обновляем данные UVS
        /// </summary>
        /// <returns></returns>
        private bool RefreshUVS(UVSUserControlViewModel Data)
        {
            Data.UVS = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.UVS.Count > 0)
            {
                foreach (var signal in AppData.UVS)
                    Data.UVS.Add(signal);

                Data.SelectedUVS = Data.UVS[0];
            }
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные UMPNA
        /// <summary>
        /// Обновляем данные UMPNA
        /// </summary>
        /// <returns></returns>
        private bool RefreshUMPNA(UMPNAUserControlViewModel Data)
        {
            Data.UMPNA = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.UMPNA.Count > 0)
            {
                foreach (var signal in AppData.UMPNA)
                    Data.UMPNA.Add(signal);

                Data.SelectedUMPNA = Data.UMPNA[0];
            }
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные KTPR
        /// <summary>
        /// Обновляем данные KTPR
        /// </summary>
        /// <returns></returns>
        private bool RefreshKTPR(KTPRUserControlViewModel Data)
        {
            Data.KTPR = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.KTPR.Count > 0)
            {
                foreach (var signal in AppData.KTPR)
                    Data.KTPR.Add(signal);

                Data.SelectedParam = Data.KTPR[0];
                Data.GeneratedData();
                return true;
            }
            #endregion

            #region Генерируем регистры формируемые
            var DefualtMapKTPR = Program.Settings.Config.DefualtMapKTPR;

            for (int i = 0; i < 256; i++)
            {
                var param = new BaseKTPR()
                {
                    Param = new BaseParam
                    {
                        Index = $"{Data.KTPR.Count + 1}",
                        Id = "",
                        Description = i > DefualtMapKTPR.Count ? "" : $"{DefualtMapKTPR[i].Param.Description}",
                        VarName = $"ktpr_param[{Data.KTPR.Count + 1}]",
                        Inv = i > DefualtMapKTPR.Count ? "" : $"{DefualtMapKTPR[i].Param.Inv}",
                        TypeSignal = i > DefualtMapKTPR.Count ? "" : $"{DefualtMapKTPR[i].Param.TypeSignal}",
                        Address = ""
                    },
                    StateStation = "",
                    Shoulder = "",
                    SubShoulder = "",
                    Autodeblok = "",
                    NoMasked = "",
                    StopTypeNS = "",
                    StopTypeUMPNA = "",
                    Type = "",
                    ControlUTS = new BaseControlUTS(),
                    ControlUVS = new BaseControlUVS(),
                    ControlUZD = new BaseControlUZD(),
                    Setpoints = new BaseSetpoints
                    {
                        Index = $"{Data.KTPR.Count + 1}",
                        Value = i > DefualtMapKTPR.Count ? "" : $"{DefualtMapKTPR[i].Setpoints.Value}",
                        Unit = i > DefualtMapKTPR.Count ? "" : $"{DefualtMapKTPR[i].Setpoints.Unit}",
                        Id = $"H{2000 + Data.KTPR.Count}",
                        VarName = $"SP_STAT_PROT[{Data.KTPR.Count + 1}]",
                        Address = $"%MW{4800 + Data.KTPR.Count}",
                        Description = ""
                    }
                };

                Data.KTPR.Add(param);
            }
            Data.SelectedParam = Data.KTPR[0];
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные KTPRS
        /// <summary>
        /// Обновляем данные KTPRS
        /// </summary>
        /// <returns></returns>
        private bool RefreshKTPRS(KTPRSUserControlViewModel Data)
        {
            Data.KTPRS = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.KTPRS.Count > 0)
            {
                foreach (var signal in AppData.KTPRS)
                    Data.KTPRS.Add(signal);

                Data.SelectedParam = Data.KTPRS[0];
                Data.GeneratedData();
                return true;
            }
            #endregion

            #region Генерируем регистры формируемые
            var DefualtMapKTPRS = Program.Settings.Config.DefualtMapKTPRS;

            for (int i = 0; i < 256; i++)
            {
                var param = new BaseKTPRS()
                {
                    Param = new BaseParam
                    {
                        Index = $"{Data.KTPRS.Count + 1}",
                        Id = "",
                        Description = i > DefualtMapKTPRS.Count ? "" : $"{DefualtMapKTPRS[i].Param.Description}",
                        VarName = $"ktprs_param[{Data.KTPRS.Count + 1}]",
                        Inv = i > DefualtMapKTPRS.Count ? "" : $"{DefualtMapKTPRS[i].Param.Inv}",
                        TypeSignal = i > DefualtMapKTPRS.Count ? "" : $"{DefualtMapKTPRS[i].Param.TypeSignal}",
                        Address = ""
                    },
                    TypeWarning = "",
                    Type = "",
                    ControlUTS = new BaseControlUTS(),
                    ControlUVS = new BaseControlUVS(),
                    ControlUZD = new BaseControlUZD(),
                    Setpoints = new BaseSetpoints
                    {
                        Index = $"{Data.KTPRS.Count + 1}",
                        Value = i > DefualtMapKTPRS.Count ? "" : $"{DefualtMapKTPRS[i].Setpoints.Value}",
                        Unit = i > DefualtMapKTPRS.Count ? "" : $"{DefualtMapKTPRS[i].Setpoints.Unit}",
                        Id = $"H{5000 + Data.KTPRS.Count}",
                        VarName = $"SP_CRIT_PROT[{Data.KTPRS.Count + 1}]",
                        Address = $"%MW{4600 + Data.KTPRS.Count}",
                        Description = ""
                    }
                };
                Data.KTPRS.Add(param);
            }

            Data.SelectedParam = Data.KTPRS[0];
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные Сигнализации
        /// <summary>
        /// Обновляем данные Сигнализации
        /// </summary>
        /// <returns></returns>
        private bool RefreshSignaling(SignalingUserControlViewModel Data)
        {
            Data.Signaling = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.Signaling.Count > 0)
            {
                foreach (var signal in AppData.Signaling)
                    Data.Signaling.Add(signal);

                Data.SelectedParam = Data.Signaling[0];
                Data.GeneratedData();
                return true;
            }
            #endregion

            #region Генерируем регистры формируемые
            for (int i = 0; i < 58; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    var param = new BaseSignaling
                    {
                        Param = new BaseParam
                        {
                            Index = $"{Data.Signaling.Count + 1}",
                            Id = "",
                            Description = "",
                            VarName = $"list5_param[{Data.Signaling.Count + 1}]",
                            Inv = "",
                            TypeSignal = "",
                            Address = ""
                        },
                        Color = "",
                        IndexUSO = "",
                        TypeWarning = "1",
                        VarNameVU = $"LIST5[{i + 1}]"
                    };
                    Data.Signaling.Add(param);
                }
            }
            Data.SelectedParam = Data.Signaling[0];
            #endregion

            Data.GeneratedData();
            return true;
        }
        #endregion

        #region Обновляем данные UTS
        /// <summary>
        /// Обновляем данные UTS
        /// </summary>
        /// <returns></returns>
        private bool RefreshUTS(UTSUserControlViewModel Data)
        {
            Data.UTS = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.UTS.Count > 0)
            {
                foreach (var signal in AppData.UTS)
                    Data.UTS.Add(signal);

                Data.SelectedUTS = Data.UTS[0];
                Data.GeneratedData();
                return true;
            }
            #endregion

            #region Генерируем регистры формируемые
            for (int i = 0; i < 256; i++)
            {
                var param = new BaseUTS
                {
                    Type = "",
                    LockEnable = "",
                    TypeCOz = "",
                    IndexPZ = "",
                    AptOff = "",
                    IndexGroup = "",
                    Param = new BaseParam
                    {
                        Index = $"{i + 1}",
                        Id = "",
                        Description = "",
                        VarName = $"uts_param[{i + 1}]",
                        Inv = "",
                        TypeSignal = "",
                        Address = ""
                    },
                    KCO = new BaseParam
                    {
                        Index = $"{i + 1}",
                        Id = "",
                        Description = "",
                        VarName = $"uts_param[{i + 1}]",
                        Inv = "",
                        TypeSignal = "",
                        Address = ""
                    },
                    SignalSOD = new BaseParam
                    {
                        Index = $"{i + 1}",
                        Id = "",
                        Description = "",
                        VarName = $"uts_param[{i + 1}]",
                        Inv = "",
                        TypeSignal = "",
                        Address = ""
                    },
                    SignalErrSOD = new BaseParam
                    {
                        Index = $"{i + 1}",
                        Id = "",
                        Description = "",
                        VarName = $"uts_param[{i + 1}]",
                        Inv = "",
                        TypeSignal = "",
                        Address = ""
                    }
                };
                Data.UTS.Add(param);
            }
            Data.SelectedUTS = Data.UTS[0];
            Data.GeneratedData();
            return true;
            #endregion
        }
        #endregion

        #region Обновляем данные Уставки Real
        /// <summary>
        /// Обновляем данные Уставки Real
        /// </summary>
        /// <returns></returns>
        private bool RefreshUstReal(UstRealUserControlViewModel Data)
        {
            Data.Setpoints = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.SetpointsReal.Count > 0)
            {
                foreach (var signal in AppData.SetpointsReal)
                    Data.Setpoints.Add(signal);

                Data.SelectedParam = Data.Setpoints[0];
                Data.GeneratedData();
                return true;
            }
            #endregion

            #region Генерируем регистры формируемые
            for (int i = 0; i < 100; i++)
            {
                var param = new BaseSetpointsReal
                {
                    QtySimbolsComma = "",
                    Setpoints = new()
                    {
                        Index = $"{i + 1}",
                        Id = $"FL{(i + 1):000}",
                        Description = "",
                        VarName = $"SP_REAL[{i + 1}]",
                        Address = $"{1000 + i}",
                        Value = "",
                        Unit = "",
                    }
                };
                Data.Setpoints.Add(param);
            }
            Data.SelectedParam = Data.Setpoints[0];
            Data.GeneratedData();
            return true;
            #endregion
        }
        #endregion

        #region Обновляем данные общих уставок
        /// <summary>
        /// Обновляем данные общих уставок
        /// </summary>
        /// <returns></returns>
        private bool RefreshUstReal(UstCommonUserControlViewModel Data)
        {
            Data.Setpoints = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.SetpointsCommon.Count > 0)
            {
                foreach (var signal in AppData.SetpointsCommon)
                    Data.Setpoints.Add(signal);

                Data.SelectedParam = Data.Setpoints[0];
                Data.GeneratedData();
                return true;
            }
            #endregion

            #region Генерируем регистры формируемые
            for (int i = 0; i < 400; i++)
            {
                var param = new BaseSetpoints
                {
                    Index = $"{i + 1}",
                    Id = $"H{(1 + i):000}",
                    Description = "",
                    VarName = $"SP_TM_COMMON[{i + 1}]",
                    Address = $"{1000 + i}",
                    Value = "",
                    Unit = "",
                };
                Data.Setpoints.Add(param);
            }
            Data.SelectedParam = Data.Setpoints[0];
            Data.GeneratedData();
            return true;
            #endregion
        }
        #endregion

        #region Обновляем карту ручного ввода
        /// <summary>
        /// Обновляем карту ручного ввода
        /// </summary>
        /// <returns></returns>
        private bool RefreshHandMap(HandMapUserControlViewModel Data)
        {
            Data.BaseParams = new();

            #region При наличии данных генерируем данные
            if (AppData is not null && AppData.HandMap.Count > 0)
            {
                foreach (var signal in AppData.HandMap)
                    Data.BaseParams.Add(signal);

                Data.SelectedSignal = Data.BaseParams[0];
                Data.GeneratedData();
                return true;
            }
            #endregion

            #region Генерируем регистры формируемые
            for (int i = 0; i < 256; i++)
            {
                var iSh = i / 16 + 1;
                var param = new BaseParam
                {
                    Index = $"{i + 1}",
                    Id = "",
                    Description = "",
                    VarName = $"HAND_MAP[{iSh}]",
                    Address = "",
                    Inv = "",
                    TypeSignal = ""
                };
                Data.BaseParams.Add(param);
            }
            Data.SelectedSignal = Data.BaseParams[0];
            Data.GeneratedData();
            return true;
            #endregion
        }
        #endregion

        #endregion

        #region Сохранение файла приложения
        /// <summary>
        /// Сохранение файла приложения
        /// </summary>
        /// <returns></returns>
        public bool SaveData()
        {
            var path = Program.Settings.Config.PathProject;
            try
            {
                var SettingsAppSerializer = new XmlSerializer(typeof(DBData));
                var xmlWriterSettings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
                using XmlWriter xmlWriter = XmlWriter.Create(path, xmlWriterSettings);
                SettingsAppSerializer.Serialize(xmlWriter, AppData);
                var UserDialog = new UserDialogService();
                UserDialog.SendMessage("Управление приложением", "Документ успешно сохранен.",
                    MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }
        #endregion

        #region Загрузка файла приложения
        /// <summary>
        /// Загрузка файла приложения
        /// </summary>
        /// <returns></returns>
        public DBData LoadData(string SelectedPath = "")
        {
            var path = string.IsNullOrWhiteSpace(SelectedPath) ? Program.PathConfig + "\\ProjectData.xml" : SelectedPath;
            var SettingsAppSerializer = new XmlSerializer(typeof(DBData));
            try
            {
                using FileStream fs = new(path, FileMode.OpenOrCreate);
                AppData = SettingsAppSerializer.Deserialize(fs) as DBData;
                Program.Settings.Config.PathProject = path;
                return AppData;
            }
            catch (Exception)
            {
                return AppData = null;
            }
        }
        #endregion

        #region Обнуление данных, при созданиии нового проекта
        /// <summary>
        /// Обнуление данных, при созданиии нового проекта
        /// </summary>
        /// <returns></returns>
        public bool ClearDataBase()
        {
            AppData = null;
            return true;
        }
        #endregion
    }
}
