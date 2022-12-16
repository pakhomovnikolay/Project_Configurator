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
using System.IO;
using System.Text;
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

        #region Функция записи данных
        /// <summary>
        /// Функция записи данных
        /// </summary>
        /// <returns></returns>
        public bool SetData()
        {
            MySqlConnection _MySqlConnection = new();
            MySqlCommand _MySqlCommand;
            string Query;
            bool Success;

            var Log = new LogSerivece();
            var DialogService = new UserDialogService();
            var Config = Program.Settings.Config;

            try
            {
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

                                Query = $"CREATE DATABASE IF NOT EXISTS `{ConnectDB.NameDB}`;";
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

                            #region Создаем данные УСО
                            FormingDataUSO(_MySqlConnection);
                            #endregion

                            #region Создаем данные DI формируемые
                            FormingDataUserDI(_MySqlConnection);
                            #endregion

                            #region Создаем данные AI формируемые
                            FormingDataUserAI(_MySqlConnection);
                            #endregion

                            #region Формируем данные Регистры формируемые
                            FormingDataUserREG(_MySqlConnection);
                            #endregion

                            #region Формируем данные Задвижек
                            FormingDataUZD(_MySqlConnection);
                            #endregion

                            #region Формируем данные Вспомсистем
                            FormingDataUVS(_MySqlConnection);
                            #endregion

                            #region Формируем данные МПНА
                            FormingDataUMPNA(_MySqlConnection);
                            #endregion

                            #region Формируем данные KTPR
                            FormingDataKTPR(_MySqlConnection);
                            #endregion

                            #region Формируем данные KTPRS
                            FormingDataKTPRS(_MySqlConnection);
                            #endregion

                            #region Формируем данные Signaling
                            FormingDataSignaling(_MySqlConnection);
                            #endregion

                            ConnectDB.SuccessUpdate = true;
                            Success = Success || ConnectDB.SuccessUpdate;
                        }
                    }
                }
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

                Data.SelectedUSO = Data.USOList[^1];
                Data.SelectedRack = Data.USOList[^1].Racks[0];
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
                        TypeWarning = "",
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

        #region Формируем данные для БД

        #region Формируем данные УСО
        /// <summary>
        /// Формируем данные УСО
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private void FormingDataUSO(MySqlConnection mySqlConnection)
        {
            var Log = new LogSerivece();

            #region Удаляем таблицу
            var Query = $"DROP TABLE IF EXISTS `USO`";
            var _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
            _MySqlCommand.ExecuteNonQuery();
            #endregion

            #region Создаем таблицу
            try
            {
                Query = $"CREATE TABLE `USO`(" +
                    $"`ID` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    $"`USO_NAME` TEXT," +
                    $"`RACK_NAME` TEXT," +
                    $"`MODULE_NAME` TEXT," +
                    $"`MODULE_TYPE` TEXT," +
                    $"`CHANNEL_INDEX` TEXT," +
                    $"`CHANNEL_ID` TEXT," +
                    $"`CHANNEL_NAME` TEXT," +
                    $"PRIMARY KEY(`ID`));";

                _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

            #region Наполняем таблицу данными
            try
            {
                Query = $"INSERT INTO USO (USO_NAME, RACK_NAME, MODULE_NAME, MODULE_TYPE, CHANNEL_INDEX, CHANNEL_ID, CHANNEL_NAME) VALUES";
                var QueryPar = "";
                foreach (var _USO in AppData.USOList)
                {
                    foreach (var _Rack in _USO.Racks)
                    {
                        foreach (var _Module in _Rack.Modules)
                        {
                            if (string.IsNullOrWhiteSpace(_Module.Name)) continue;
                            if (_Module.Channels.Count <= 0)
                            {
                                QueryPar += $"('{_USO.Name}', '{_Rack.Name}'," +
                                    $"'{_Module.Name}', '{_Module.Type}'," +
                                    $"'{""}', '{""}', '{""}'),";
                            }
                            foreach (var _Channel in _Module.Channels)
                            {
                                QueryPar += $"('{_USO.Name}', '{_Rack.Name}'," +
                                    $"'{_Module.Name}', '{_Module.Type}'," +
                                    $"'{_Channel.Index}', '{_Channel.Id}', '{_Channel.Description}'),";
                            }
                        }
                    }
                }
                QueryPar = QueryPar.TrimEnd(',') + ";";
                _MySqlCommand = new MySqlCommand(Query + QueryPar, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

        }
        #endregion

        #region Формируем данные DI формируемые
        /// <summary>
        /// Формируем данные DI формируемые
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private void FormingDataUserDI(MySqlConnection mySqlConnection)
        {
            var Log = new LogSerivece();

            #region Удаляем таблицу
            var Query = $"DROP TABLE IF EXISTS `USER_DI`";
            var _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
            _MySqlCommand.ExecuteNonQuery();
            #endregion

            #region Создаем таблицу
            try
            {
                Query = $"CREATE TABLE `USER_DI`(" +
                    $"`ID` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    $"`PARAM_ID` TEXT," +
                    $"`PARAM_NAME` TEXT," +
                    $"PRIMARY KEY(`ID`));";

                _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

            #region Наполняем таблицу данными
            try
            {
                Query = $"INSERT INTO USER_DI (PARAM_ID, PARAM_NAME) VALUES";
                var QueryPar = "";
                foreach (var _UserDI in AppData.UserDI)
                {
                    if (string.IsNullOrWhiteSpace(_UserDI.Id) && string.IsNullOrWhiteSpace(_UserDI.Description)) continue;
                    QueryPar += $"('{_UserDI.Id}', '{_UserDI.Description}'),";

                }

                QueryPar = QueryPar.TrimEnd(',') + ";";
                _MySqlCommand = new MySqlCommand(Query + QueryPar, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

        }
        #endregion

        #region Формируем данные AI формируемые
        /// <summary>
        /// Формируем данные AI формируемые
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private void FormingDataUserAI(MySqlConnection mySqlConnection)
        {
            var Log = new LogSerivece();

            #region Удаляем таблицу
            var Query = $"DROP TABLE IF EXISTS `USER_AI`";
            var _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
            _MySqlCommand.ExecuteNonQuery();
            #endregion

            #region Создаем таблицу
            try
            {
                Query = $"CREATE TABLE `USER_AI`(" +
                    $"`ID` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    $"`PARAM_ID` TEXT," +
                    $"`PARAM_NAME` TEXT," +
                    $"PRIMARY KEY(`ID`));";

                _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

            #region Наполняем таблицу данными
            try
            {
                Query = $"INSERT INTO USER_AI (PARAM_ID, PARAM_NAME) VALUES";
                var QueryPar = "";
                foreach (var _UserAI in AppData.UserAI)
                {
                    if (string.IsNullOrWhiteSpace(_UserAI.Id) && string.IsNullOrWhiteSpace(_UserAI.Description)) continue;
                    QueryPar += $"('{_UserAI.Id}', '{_UserAI.Description}'),";
                }

                QueryPar = QueryPar.TrimEnd(',') + ";";
                _MySqlCommand = new MySqlCommand(Query + QueryPar, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

        }
        #endregion

        #region Формируем данные Регистры формируемые
        /// <summary>
        /// Формируем данные Регистры формируемые
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private void FormingDataUserREG(MySqlConnection mySqlConnection)
        {
            var Log = new LogSerivece();

            #region Удаляем таблицу
            var Query = $"DROP TABLE IF EXISTS `USER_REG`";
            var _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
            _MySqlCommand.ExecuteNonQuery();
            #endregion

            #region Создаем таблицу
            try
            {
                Query = $"CREATE TABLE `USER_REG`(" +
                    $"`ID` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    $"`PARAM_ID` TEXT," +
                    $"`PARAM_NAME` TEXT," +
                    $"PRIMARY KEY(`ID`));";

                _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

            #region Наполняем таблицу данными
            try
            {
                Query = $"INSERT INTO USER_REG (PARAM_ID, PARAM_NAME) VALUES";
                var QueryPar = "";
                foreach (var _UserReg in AppData.UserReg)
                {
                    if (string.IsNullOrWhiteSpace(_UserReg.Id) && string.IsNullOrWhiteSpace(_UserReg.Description)) continue;
                    QueryPar += $"('{_UserReg.Id}', '{_UserReg.Description}'),";
                }

                QueryPar = QueryPar.TrimEnd(',') + ";";
                _MySqlCommand = new MySqlCommand(Query + QueryPar, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

        }
        #endregion

        #region Формируем данные Задвижек
        /// <summary>
        /// Формируем данные Задвижек
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private void FormingDataUZD(MySqlConnection mySqlConnection)
        {
            var Log = new LogSerivece();

            #region Удаляем таблицу
            var Query = $"DROP TABLE IF EXISTS `UZD`";
            var _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
            _MySqlCommand.ExecuteNonQuery();
            #endregion

            #region Создаем таблицу
            try
            {
                Query = $"CREATE TABLE `UZD`(" +
                    $"`ID` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    $"`NAME` TEXT," +
                    $"`SHORT_NAME` TEXT," +
                    $"`DIST` TEXT," +
                    $"`DOUBLE_STOP` TEXT," +
                    $"`BUR` TEXT," +
                    $"`COz` TEXT," +
                    $"`CZz` TEXT," +
                    $"`EC` TEXT," +
                    $"`CHECK_STATE` TEXT," +
                    $"`RS_OFF` TEXT," +
                    $"PRIMARY KEY(`ID`));";

                _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

            #region Наполняем таблицу данными
            try
            {
                Query = $"INSERT INTO UZD (NAME, SHORT_NAME, DIST, DOUBLE_STOP, BUR, COz, CZz, EC, CHECK_STATE, RS_OFF) VALUES";
                var QueryPar = "";
                foreach (var _UZD in AppData.UZD)
                {
                    QueryPar += $"('{_UZD.Description}', '{_UZD.ShortDescription}', '{_UZD.Dist}', '{_UZD.DoubleStop}', '{_UZD.Bur}'," +
                        $"'{_UZD.COz}', '{_UZD.CZz}', '{_UZD.EC}', '{_UZD.CheckState}', '{_UZD.RsOff}'),";
                }

                QueryPar = QueryPar.TrimEnd(',') + ";";
                _MySqlCommand = new MySqlCommand(Query + QueryPar, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

        }
        #endregion

        #region Формируем данные Вспомсистем
        /// <summary>
        /// Формируем данные Вспомсистем
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private void FormingDataUVS(MySqlConnection mySqlConnection)
        {
            var Log = new LogSerivece();

            #region Удаляем таблицу
            var Query = $"DROP TABLE IF EXISTS `UVS`";
            var _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
            _MySqlCommand.ExecuteNonQuery();
            #endregion

            #region Создаем таблицу
            try
            {
                Query = $"CREATE TABLE `UVS`(" +
                    $"`ID` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    $"`NAME` TEXT," +
                    $"`SHORT_NAME` TEXT," +
                    $"`RESERVABLE` TEXT," +
                    $"`TYPE_PRESSURE` TEXT," +
                    $"`COz` TEXT," +
                    $"PRIMARY KEY(`ID`));";

                _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

            #region Наполняем таблицу данными
            try
            {
                Query = $"INSERT INTO UVS (NAME, SHORT_NAME, RESERVABLE, TYPE_PRESSURE, COz) VALUES";
                var QueryPar = "";
                foreach (var _UVS in AppData.UVS)
                {

                    QueryPar += $"('{_UVS.Description}', '{_UVS.ShortDescription}', '{_UVS.Reservable}', '{_UVS.TypePressure}', '{_UVS.COz}'),";
                }

                QueryPar = QueryPar.TrimEnd(',') + ";";
                _MySqlCommand = new MySqlCommand(Query + QueryPar, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

        }
        #endregion

        #region Формируем данные МПНА
        /// <summary>
        /// Формируем данные МПНА
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private void FormingDataUMPNA(MySqlConnection mySqlConnection)
        {
            var Log = new LogSerivece();

            #region Удаляем таблицу
            var Query = $"DROP TABLE IF EXISTS `UMPNA`";
            var _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
            _MySqlCommand.ExecuteNonQuery();
            #endregion

            #region Создаем таблицу
            try
            {
                Query = $"CREATE TABLE `UMPNA`(" +
                    $"`ID` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    $"`NAME` TEXT," +
                    $"`SHORT_NAME` TEXT," +
                    $"PRIMARY KEY(`ID`));";

                _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

            #region Наполняем таблицу данными
            try
            {
                Query = $"INSERT INTO UMPNA (NAME, SHORT_NAME) VALUES";
                var QueryPar = "";
                foreach (var _UMPNA in AppData.UMPNA)
                {

                    QueryPar += $"('{_UMPNA.Description}', '{_UMPNA.ShortDescription}'),";
                }

                QueryPar = QueryPar.TrimEnd(',') + ";";
                _MySqlCommand = new MySqlCommand(Query + QueryPar, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

        }
        #endregion

        #region Формируем данные KTPR
        /// <summary>
        /// Формируем данные KTPR
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private void FormingDataKTPR(MySqlConnection mySqlConnection)
        {
            var Log = new LogSerivece();

            #region Удаляем таблицу
            var Query = $"DROP TABLE IF EXISTS `KTPR`";
            var _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
            _MySqlCommand.ExecuteNonQuery();
            #endregion

            #region Создаем таблицу
            try
            {
                Query = $"CREATE TABLE `KTPR`(" +
                    $"`ID` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    $"`PARAM_ID` TEXT," +
                    $"`PARAM_NAME` TEXT," +
                    $"PRIMARY KEY(`ID`));";

                _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

            #region Наполняем таблицу данными
            try
            {
                Query = $"INSERT INTO KTPR (PARAM_ID, PARAM_NAME) VALUES";
                var QueryPar = "";
                foreach (var _KTPR in AppData.KTPR)
                {
                    QueryPar += $"('{_KTPR.Param.Id}', '{_KTPR.Param.Description}'),";
                }

                QueryPar = QueryPar.TrimEnd(',') + ";";
                _MySqlCommand = new MySqlCommand(Query + QueryPar, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

        }
        #endregion

        #region Формируем данные KTPRS
        /// <summary>
        /// Формируем данные KTPRS
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private void FormingDataKTPRS(MySqlConnection mySqlConnection)
        {
            var Log = new LogSerivece();

            #region Удаляем таблицу
            var Query = $"DROP TABLE IF EXISTS `KTPRS`";
            var _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
            _MySqlCommand.ExecuteNonQuery();
            #endregion

            #region Создаем таблицу
            try
            {
                Query = $"CREATE TABLE `KTPRS`(" +
                    $"`ID` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    $"`PARAM_ID` TEXT," +
                    $"`PARAM_NAME` TEXT," +
                    $"PRIMARY KEY(`ID`));";

                _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

            #region Наполняем таблицу данными
            try
            {
                Query = $"INSERT INTO KTPRS (PARAM_ID, PARAM_NAME) VALUES";
                var QueryPar = "";
                foreach (var _KTPRS in AppData.KTPRS)
                {
                    QueryPar += $"('{_KTPRS.Param.Id}', '{_KTPRS.Param.Description}'),";
                }

                QueryPar = QueryPar.TrimEnd(',') + ";";
                _MySqlCommand = new MySqlCommand(Query + QueryPar, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

        }
        #endregion

        #region Формируем данные сигнализации и общесистемная диагностика
        /// <summary>
        /// Формируем данные сигнализации и общесистемная диагностика
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private void FormingDataSignaling(MySqlConnection mySqlConnection)
        {
            var Log = new LogSerivece();

            #region Удаляем таблицу
            var Query = $"DROP TABLE IF EXISTS `Signaling`";
            var _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
            _MySqlCommand.ExecuteNonQuery();
            #endregion

            #region Создаем таблицу
            try
            {
                Query = $"CREATE TABLE `Signaling`(" +
                    $"`ID` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT," +
                    $"`PARAM_ID` TEXT," +
                    $"`PARAM_NAME` TEXT," +
                    $"`COLOR` TEXT," +
                    $"`USO_INDEX` TEXT," +
                    $"`TYPE_WARNING` TEXT," +
                    $"PRIMARY KEY(`ID`));";

                _MySqlCommand = new MySqlCommand(Query, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

            #region Наполняем таблицу данными
            try
            {
                Query = $"INSERT INTO Signaling (PARAM_ID, PARAM_NAME, COLOR, USO_INDEX, TYPE_WARNING) VALUES";
                var QueryPar = "";
                foreach (var _Signaling in AppData.Signaling)
                {
                    QueryPar += $"('{_Signaling.Param.Id}', '{_Signaling.Param.Description}', '{_Signaling.Color}', '{_Signaling.IndexUSO}', '{_Signaling.TypeWarning}'),";
                }

                QueryPar = QueryPar.TrimEnd(',') + ";";
                _MySqlCommand = new MySqlCommand(Query + QueryPar, mySqlConnection);
                _MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
            }
            #endregion

        }
        #endregion

        #endregion
    }
}
