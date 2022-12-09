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

namespace Project_Сonfigurator.Services
{
    public class DBService : IDBService
    {
        #region Функция получения данных
        /// <summary>
        /// Функция получения данных
        /// </summary>
        /// <returns></returns>
        public DBData GetData()
        {
            var connection = new MySqlConnection();
            var data_table = new DBData();
            var Log = new LogSerivece();
            var DialogService = new UserDialogService();
            var Config = Program.Settings.Config.ConnectDB;

            try
            {
                var connectionString =
                    $"SERVER={Config.IPAddress};" +
                    $"PORT={Config.Port};" +
                    //$"DATABASE=mariadb;" +
                    $"UID={Config.UserName};" +
                    $"PASSWORD={Config.Password};" +
                    $"Connection Timeout = {Config.TimeoutConnect}";

                connection = new(connectionString);
                try
                {
                    connection.Open();
                    string Query = $"CREATE DATABASE IF NOT EXISTS `{Config.NameDB}`;";
                    var mySqlCommand = new MySqlCommand(Query, connection);
                    mySqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception e)
                {
                    Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return data_table;
        }
        #endregion

        #region Обновление данных во ViewModels

        #region Обновление данных во ViewModels
        /// <summary>
        /// Обновление данных во ViewModels
        /// </summary>
        /// <param name="Item"></param>
        public bool RefreshDataViewModel(object Item)
        {
            if (Item is null) throw new ArgumentNullException(nameof(Item));
            return Item switch
            {
                LayotRackUserControlViewModel Data => RefreshUSO(Data),
                TableSignalsUserControlViewModel Data => RefreshTableSignals(Data),

                SignalsDIUserControlViewModel Data => RefreshSignalsDI(Data),
                SignalsAIUserControlViewModel Data => RefreshSignalsAI(Data),
                SignalsDOUserControlViewModel Data => RefreshSignalsDO(Data),
                SignalsAOUserControlViewModel Data => RefreshSignalsAO(Data),
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
        private static bool RefreshUSO(LayotRackUserControlViewModel Data)
        {
            Data.USOList = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.USOList.Count > 0)
                foreach (var _USO in Program.Settings.AppData.USOList)
                    Data.USOList.Add(_USO);
            #endregion

            if (Data.USOList.Count > 0)
            {
                Data.SelectedUSO = Data.USOList[^1];
                Data.SelectedRack = Data.USOList[^1].Racks[0];
            }
            return true;
        }
        #endregion

        #region Обновляем данные Таблицы сигналов
        /// <summary>
        /// Обновляем данные Таблицы сигналов
        /// </summary>
        /// <returns></returns>
        private static bool RefreshTableSignals(TableSignalsUserControlViewModel Data)
        {
            Data.USOList = new();
            Data.SelectedUSO = new USO();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.USOList.Count > 0)
                foreach (var _USO in Program.Settings.AppData.USOList)
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
            #endregion

            if (Data.USOList.Count > 0)
                Data.SelectedUSO = Data.USOList[0];

            return true;
        }
        #endregion

        #region Обновляем данные Сигналы DI
        /// <summary>
        /// Обновляем данные Сигналы DI
        /// </summary>
        /// <returns></returns>
        private static bool RefreshSignalsDI(SignalsDIUserControlViewModel Data)
        {
            Data.SignalsDI = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.SignalDI.Count > 0)
                foreach (var signal in Program.Settings.AppData.SignalDI)
                    Data.SignalsDI.Add(signal);
            #endregion

            if (Data.SignalsDI.Count > 0)
                Data.SelectedSignalDI = Data.SignalsDI[0];

            return true;
        }
        #endregion

        #region Обновляем данные Сигналы AI
        /// <summary>
        /// Обновляем данные Сигналы AI
        /// </summary>
        /// <returns></returns>
        private static bool RefreshSignalsAI(SignalsAIUserControlViewModel Data)
        {
            Data.SignalsAI = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.SignalAI.Count > 0)
                foreach (var signal in Program.Settings.AppData.SignalAI)
                    Data.SignalsAI.Add(signal);
            #endregion

            if (Data.SignalsAI.Count > 0)
                Data.SelectedSignalAI = Data.SignalsAI[0];

            return true;
        }
        #endregion

        #region Обновляем данные Сигналы DO
        /// <summary>
        /// Обновляем данные Сигналы DO
        /// </summary>
        /// <returns></returns>
        private static bool RefreshSignalsDO(SignalsDOUserControlViewModel Data)
        {
            Data.SignalsDO = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.SignalDO.Count > 0)
                foreach (var signal in Program.Settings.AppData.SignalDO)
                    Data.SignalsDO.Add(signal);
            #endregion

            if (Data.SignalsDO.Count > 0)
                Data.SelectedSignalDO = Data.SignalsDO[0];

            return true;
        }
        #endregion

        #region Обновляем данные Сигналы AO
        /// <summary>
        /// Обновляем данные Сигналы AO
        /// </summary>
        /// <returns></returns>
        private static bool RefreshSignalsAO(SignalsAOUserControlViewModel Data)
        {
            Data.SignalsAO = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.SignalAO.Count > 0)
                foreach (var signal in Program.Settings.AppData.SignalAO)
                    Data.SignalsAO.Add(signal);
            #endregion

            if (Data.SignalsAO.Count > 0)
                Data.SelectedSignalAO = Data.SignalsAO[0];

            return true;
        }
        #endregion

        #region Обновляем данные DI формируемые
        /// <summary>
        /// Обновляем данные DI формируемые
        /// </summary>
        /// <returns></returns>
        private static bool RefreshUserDI(UserDIUserControlViewModel Data)
        {
            Data.BaseSignals = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.UserDI.Count > 0)
                foreach (var signal in Program.Settings.AppData.UserDI)
                    Data.BaseSignals.Add(signal);
            #endregion

            #region Генерируем сигналы DI
            var index_reg = 0;
            while (Data.BaseSignals.Count < 75)
            {
                for (int i = 0; i < 16; i++)
                {
                    var signal = new BaseSignal
                    {
                        Index = $"{Data.BaseSignals.Count + 1}",
                        Id = "",
                        Description = "",
                        VarName = $"user_di[{++index_reg}]",
                        Area = "",
                        Address = $"{Data.BaseSignals.Count + 1}",
                        LinkValue = ""
                    };
                    Data.BaseSignals.Add(signal);
                }
            }
            Data.SelectedSignal = Data.BaseSignals[0];
            #endregion

            return true;
        }
        #endregion

        #region Обновляем данные AI формируемые
        /// <summary>
        /// Обновляем данные AI формируемые
        /// </summary>
        /// <returns></returns>
        private static bool RefreshUserAI(UserAIUserControlViewModel Data)
        {
            Data.BaseSignals = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.UserAI.Count > 0)
                foreach (var signal in Program.Settings.AppData.UserAI)
                    Data.BaseSignals.Add(signal);
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

            return true;
        }
        #endregion

        #region Обновляем данные Регистры формируемые
        /// <summary>
        /// Обновляем данные Регистры формируемые
        /// </summary>
        /// <returns></returns>
        private static bool RefreshUserReg(UserRegUserControlViewModel Data)
        {
            Data.BaseParams = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.UserReg.Count > 0)
                foreach (var signal in Program.Settings.AppData.UserReg)
                    Data.BaseParams.Add(signal);
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

            return true;
        }
        #endregion

        #region Обновляем данные Сигналы групп
        /// <summary>
        /// Обновляем данные Сигналы групп
        /// </summary>
        /// <returns></returns>
        private static bool RefreshSignalsGroup(SignalsGroupUserControlViewModel Data)
        {
            Data.BaseParams = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.SignalGroup.Count > 0)
                foreach (var signal in Program.Settings.AppData.SignalGroup)
                    Data.BaseParams.Add(signal);
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

            return true;
        }
        #endregion

        #region Обновляем данные Группы сигналов
        /// <summary>
        /// Обновляем данные Группы сигналов
        /// </summary>
        /// <returns></returns>
        private static bool RefreshGroupsSignal(GroupsSignalUserControlViewModel Data)
        {
            Data.GroupSignals = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.GroupSignals.Count > 0)
                foreach (var signal in Program.Settings.AppData.GroupSignals)
                    Data.GroupSignals.Add(signal);
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

            return true;
        }
        #endregion

        #region Обновляем данные UZD
        /// <summary>
        /// Обновляем данные UZD
        /// </summary>
        /// <returns></returns>
        private static bool RefreshUZD(UZDUserControlViewModel Data)
        {
            Data.UZD = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.UZD.Count > 0)
                foreach (var signal in Program.Settings.AppData.UZD)
                    Data.UZD.Add(signal);

            if (Data.UZD.Count > 0)
                Data.SelectedUZD = Data.UZD[0];
            #endregion

            return true;
        }
        #endregion

        #region Обновляем данные UVS
        /// <summary>
        /// Обновляем данные UVS
        /// </summary>
        /// <returns></returns>
        private static bool RefreshUVS(UVSUserControlViewModel Data)
        {
            Data.UVS = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.UVS.Count > 0)
                foreach (var signal in Program.Settings.AppData.UVS)
                    Data.UVS.Add(signal);

            if (Data.UVS.Count > 0)
                Data.SelectedUVS = Data.UVS[0];
            #endregion

            return true;
        }
        #endregion

        #region Обновляем данные UMPNA
        /// <summary>
        /// Обновляем данные UMPNA
        /// </summary>
        /// <returns></returns>
        private static bool RefreshUMPNA(UMPNAUserControlViewModel Data)
        {
            Data.UMPNA = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.UMPNA.Count > 0)
                foreach (var signal in Program.Settings.AppData.UMPNA)
                    Data.UMPNA.Add(signal);

            if (Data.UMPNA.Count > 0)
                Data.SelectedUMPNA = Data.UMPNA[0];
            #endregion

            return true;
        }
        #endregion

        #region Обновляем данные KTPR
        /// <summary>
        /// Обновляем данные KTPR
        /// </summary>
        /// <returns></returns>
        private static bool RefreshKTPR(KTPRUserControlViewModel Data)
        {
            Data.KTPR = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.KTPR.Count > 0)
                foreach (var signal in Program.Settings.AppData.KTPR)
                    Data.KTPR.Add(signal);
            #endregion

            #region Генерируем регистры формируемые
            while (Data.KTPR.Count < 256)
            {
                var param = new BaseKTPR()
                {
                    Param = new BaseParam
                    {
                        Index = $"{Data.KTPR.Count + 1}",
                        Id = "",
                        Description = "",
                        VarName = $"ktpr_param[{Data.KTPR.Count + 1}]",
                        Inv = "",
                        TypeSignal = "",
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
                        Value = "",
                        Unit = "",
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

            return true;
        }
        #endregion

        #region Обновляем данные KTPRS
        /// <summary>
        /// Обновляем данные KTPRS
        /// </summary>
        /// <returns></returns>
        private static bool RefreshKTPRS(KTPRSUserControlViewModel Data)
        {
            Data.KTPRS = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.KTPRS.Count > 0)
                foreach (var signal in Program.Settings.AppData.KTPRS)
                    Data.KTPRS.Add(signal);
            #endregion

            #region Генерируем регистры формируемые
            while (Data.KTPRS.Count < 256)
            {
                var param = new BaseKTPRS()
                {
                    Param = new BaseParam
                    {
                        Index = $"{Data.KTPRS.Count + 1}",
                        Id = "",
                        Description = "",
                        VarName = $"ktprs_param[{Data.KTPRS.Count + 1}]",
                        Inv = "",
                        TypeSignal = "",
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
                        Value = "",
                        Unit = "",
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

            return true;
        }
        #endregion

        #region Обновляем данные Сигнализации
        /// <summary>
        /// Обновляем данные Сигнализации
        /// </summary>
        /// <returns></returns>
        private static bool RefreshSignaling(SignalingUserControlViewModel Data)
        {
            Data.Signaling = new();

            #region При наличии данных генерируем данные
            if (Program.Settings.AppData is not null && Program.Settings.AppData.Signaling.Count > 0)
                foreach (var signal in Program.Settings.AppData.Signaling)
                    Data.Signaling.Add(signal);
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

            return true;
        }
        #endregion

        #endregion
    }
}
