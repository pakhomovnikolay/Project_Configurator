using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.RequestToDataBase;
using Project_Сonfigurator.Models;
using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels;
using Project_Сonfigurator.ViewModels.Base;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using Project_Сonfigurator.ViewModels.UserControls.Signals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Project_Сonfigurator.Services
{
    public class DBService : IDBService
    {
        #region Данные для сохранения
        /// <summary>
        /// Данные для сохранения
        /// </summary>
        public ProjectDataToSave DataToSave { get; set; } = new();
        #endregion

        #region Запрос на получение данных проекта
        /// <summary>
        /// Запрос на получение данных проекта
        /// </summary>
        public void ProjectDataRequest()
        {
            IEncryptorService _Encryptor = new EncryptorService();
            IUserDialogService UserDialog = new UserDialogService();
            ILogSerivece Log = new LogSerivece();

            var path = App.Settings.Config.PathProject;
            if (!File.Exists(path))
                return;


            var FileNameEncrypt = path;
            var FileNameEncrypted = path.Replace(App.__EncryptedProjectFileSuffix, ".xml");
            _Encryptor.Decryptor(FileNameEncrypt, FileNameEncrypted, "");

            try
            {
                var SettingsAppSerializer = new XmlSerializer(typeof(ProjectDataToSave));
                using FileStream fs = new(FileNameEncrypted, FileMode.OpenOrCreate);
                DataToSave = SettingsAppSerializer.Deserialize(fs) as ProjectDataToSave;
                IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
                App.Services.GetRequiredService<MessageWindowViewModel>().Params = DataToSave.Messages is null ? new() : DataToSave.Messages;

                try
                {
                    foreach (var item in _ViewModelsUserControl)
                    {
                        object _Data = item switch
                        {
                            LayotRackUserControlViewModel Data => Data.Params = DataToSave.LayotRack is null ? new() : DataToSave.LayotRack,
                            TableSignalsUserControlViewModel Data => Data.Params = DataToSave.TableSignals is null ? new() : DataToSave.TableSignals,
                            SignalsDIUserControlViewModel Data => Data.Params = DataToSave.SignalDI is null ? new() : DataToSave.SignalDI,
                            SignalsAIUserControlViewModel Data => Data.Params = DataToSave.SignalAI is null ? new() : DataToSave.SignalAI,
                            SignalsDOUserControlViewModel Data => Data.Params = DataToSave.SignalDO is null ? new() : DataToSave.SignalDO,
                            SignalsAOUserControlViewModel Data => Data.Params = DataToSave.SignalAO is null ? new() : DataToSave.SignalAO,
                            ECUserControlViewModel Data => Data.Params = DataToSave.ECParam is null ? new() : DataToSave.ECParam,
                            UserDIUserControlViewModel Data => Data.Params = DataToSave.UserDI is null ? new() : DataToSave.UserDI,
                            UserAIUserControlViewModel Data => Data.Params = DataToSave.UserAI is null ? new() : DataToSave.UserAI,
                            UserRegUserControlViewModel Data => Data.Params = DataToSave.UserReg is null ? new() : DataToSave.UserReg,
                            SignalsGroupUserControlViewModel Data => Data.Params = DataToSave.SignalGroup is null ? new() : DataToSave.SignalGroup,
                            GroupsSignalUserControlViewModel Data => Data.Params = DataToSave.GroupSignals is null ? new() : DataToSave.GroupSignals,
                            UZDUserControlViewModel Data => Data.Params = DataToSave.UZD is null ? new() : DataToSave.UZD,
                            UVSUserControlViewModel Data => Data.Params = DataToSave.UVS is null ? new() : DataToSave.UVS,
                            UMPNAUserControlViewModel Data => Data.Params = DataToSave.UMPNA is null ? new() : DataToSave.UMPNA,
                            KTPRUserControlViewModel Data => Data.Params = DataToSave.KTPR is null ? new() : DataToSave.KTPR,
                            KTPRSUserControlViewModel Data => Data.Params = DataToSave.KTPRS is null ? new() : DataToSave.KTPRS,
                            SignalingUserControlViewModel Data => Data.Params = DataToSave.Signaling is null ? new() : DataToSave.Signaling,
                            UTSUserControlViewModel Data => Data.Params = DataToSave.UTS is null ? new() : DataToSave.UTS,
                            UstRealUserControlViewModel Data => Data.Params = DataToSave.SetpointsReal is null ? new() : DataToSave.SetpointsReal,
                            UstCommonUserControlViewModel Data => Data.Params = DataToSave.SetpointsCommon is null ? new() : DataToSave.SetpointsCommon,
                            HandMapUserControlViewModel Data => Data.Params = DataToSave.HandMap is null ? new() : DataToSave.HandMap,
                            _ => null
                        };
                    }
                }
                catch (Exception e)
                {
                    Log.WriteLog($"Не удалось загрузить данные приложения - {e}", App.NameApp);
                }
            }
            catch (Exception e)
            {
                Log.WriteLog($"Не удалось загрузить данные приложения - {e}", App.NameApp);
            }
            finally
            {
                UserDialog.DeleteFile(FileNameEncrypted);
            }

        }
        #endregion

        #region Запрос на запись данных проекта
        /// <summary>
        /// Запрос на запись данных проекта
        /// </summary>
        public void RequestToWriteProjectData()
        {
            var Log = new LogSerivece();
            var path = App.Settings.Config.PathProject.Replace(App.__EncryptedProjectFileSuffix, ".xml");
            IUserDialogService UserDialog = new UserDialogService();
            IEncryptorService _Encryptor = new EncryptorService();
            var SettingsAppSerializer = new XmlSerializer(typeof(ProjectDataToSave));
            var xmlWriterSettings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            var _Params = App.Services.GetRequiredService<MessageWindowViewModel>().Params;
            DataToSave.Messages = _Params is null ? new() : _Params;

            try
            {
                foreach (var item in _ViewModelsUserControl)
                {
                    object _Data = item switch
                    {
                        LayotRackUserControlViewModel Data => DataToSave.LayotRack = Data.Params is null ? new() : Data.Params,
                        TableSignalsUserControlViewModel Data => DataToSave.TableSignals = Data.Params is null ? new() : Data.Params,
                        SignalsDIUserControlViewModel Data => DataToSave.SignalDI = Data.Params is null ? new() : Data.Params,
                        SignalsAIUserControlViewModel Data => DataToSave.SignalAI = Data.Params is null ? new() : Data.Params,
                        SignalsDOUserControlViewModel Data => DataToSave.SignalDO = Data.Params is null ? new() : Data.Params,
                        SignalsAOUserControlViewModel Data => DataToSave.SignalAO = Data.Params is null ? new() : Data.Params,
                        ECUserControlViewModel Data => DataToSave.ECParam = Data.Params is null ? new() : Data.Params,
                        UserDIUserControlViewModel Data => DataToSave.UserDI = Data.Params is null ? new() : Data.Params,
                        UserAIUserControlViewModel Data => DataToSave.UserAI = Data.Params is null ? new() : Data.Params,
                        UserRegUserControlViewModel Data => DataToSave.UserReg = Data.Params is null ? new() : Data.Params,
                        SignalsGroupUserControlViewModel Data => DataToSave.SignalGroup = Data.Params is null ? new() : Data.Params,
                        GroupsSignalUserControlViewModel Data => DataToSave.GroupSignals = Data.Params is null ? new() : Data.Params,
                        UZDUserControlViewModel Data => DataToSave.UZD = Data.Params is null ? new() : Data.Params,
                        UVSUserControlViewModel Data => DataToSave.UVS = Data.Params is null ? new() : Data.Params,
                        UMPNAUserControlViewModel Data => DataToSave.UMPNA = Data.Params is null ? new() : Data.Params,
                        KTPRUserControlViewModel Data => DataToSave.KTPR = Data.Params is null ? new() : Data.Params,
                        KTPRSUserControlViewModel Data => DataToSave.KTPRS = Data.Params is null ? new() : Data.Params,
                        SignalingUserControlViewModel Data => DataToSave.Signaling = Data.Params is null ? new() : Data.Params,
                        UTSUserControlViewModel Data => DataToSave.UTS = Data.Params is null ? new() : Data.Params,
                        UstRealUserControlViewModel Data => DataToSave.SetpointsReal = Data.Params is null ? new() : Data.Params,
                        UstCommonUserControlViewModel Data => DataToSave.SetpointsCommon = Data.Params is null ? new() : Data.Params,
                        HandMapUserControlViewModel Data => DataToSave.HandMap = Data.Params is null ? new() : Data.Params,
                        _ => null
                    };
                }
            }
            catch (Exception e)
            {
                Log.WriteLog($"Не удалось сохранить данные приложения - {e}", App.NameApp);
            }

            try
            {
                using XmlWriter xmlWriter = XmlWriter.Create(path, xmlWriterSettings);
                SettingsAppSerializer.Serialize(xmlWriter, DataToSave);
                xmlWriter.Close();
                var FileNameEncrypt = path;
                var FileNameEncrypted = path.Replace(".xml", App.__EncryptedProjectFileSuffix);

                try
                {
                    _Encryptor.Encryptor(FileNameEncrypt, FileNameEncrypted, "");
                }
                catch (OperationCanceledException e)
                {
                    Debug.WriteLine("Error in EncryptorAsync:\r\n{0}", e);
                }
                finally
                {
                    UserDialog.DeleteFile(FileNameEncrypt);
                }


                UserDialog.SendMessage("Управление приложением", "Документ успешно сохранен.",
                    MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
            }
            catch (Exception)
            {
                UserDialog.SendMessage("Управление приложением", "Ошибка сохраненеия документа.\nПроверьте настройки",
                    MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
            }
        }
        #endregion

        #region Запрос на запись данных в БД
        /// <summary>
        /// Запрос на запись данных в БД
        /// </summary>
        public void RequestToWriteDataToTheDataBase()
        {
            SettingApp Config = App.Settings.Config;
            ILogSerivece Logger = new LogSerivece();
            IUserDialogService UserDialog = new UserDialogService();
            IEnumerable<IViewModelUserControls> _ViewModels = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            var Log = new LogSerivece();

            try
            {
                foreach (var ConnectSetting in Config.ServerDB)
                {
                    ConnectSetting.SuccessUpdate = false;
                    if (ConnectSetting.IsSelection)
                    {
                        string IPResult = "";
                        var ip_address = ConnectSetting.IPAddress is not null ? ConnectSetting.IPAddress.Split(',') : Array.Empty<string>();

                        foreach (var ip in ip_address)
                        {
                            if (Request.SetConnection(ConnectSetting, ip))
                            {
                                ConnectSetting.SuccessUpdate = true;
                                IPResult = ip;
                                break;
                            }
                        }
                        if (string.IsNullOrWhiteSpace(IPResult)) continue;
                        if (!Request.OpenConnection(ConnectSetting, IPResult))
                        {
                            ConnectSetting.SuccessUpdate = false;
                            continue;
                        }

                        foreach (var _ViewModel in _ViewModels)
                        {
                            List<string> FieldValue = new();

                            #region Формируем данные

                            #region Таблица сигналов
                            if (_ViewModel is TableSignalsUserControlViewModel)
                            {
                                var Data = _ViewModel as TableSignalsUserControlViewModel;
                                foreach (var _USO in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_USO.Name)) continue;
                                    if (_USO.Racks.Count <= 0)
                                    {
                                        var _FieldValue =
                                            $"('{_USO.Name}', '{"-"}', '{"-"}', '{"-"}', " +
                                            $"'{"-"}', '{"-"}', '{"-"}', '{"-"}', '{"-"}', '{"-"}', '{"-"}'),";
                                        FieldValue.Add(_FieldValue);
                                    }

                                    foreach (var _Rack in _USO.Racks)
                                    {
                                        if (string.IsNullOrWhiteSpace(_Rack.Name)) continue;
                                        if (_Rack.Modules.Count <= 0)
                                        {
                                            var _FieldValue =
                                                $"('{_USO.Name}', '{_Rack.Name}', '{"-"}', '{"-"}', " +
                                                $"'{"-"}', '{"-"}', '{"-"}', '{"-"}', '{"-"}', '{"-"}', '{"-"}'),";
                                            FieldValue.Add(_FieldValue);
                                        }

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
                                if (!Request.SetData("TableSignals", Request.TableFieldTableSignals, Request.FieldTableSignals, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Сигналы DI
                            if (_ViewModel is SignalsDIUserControlViewModel)
                            {
                                var Data = _ViewModel as SignalsDIUserControlViewModel;

                                foreach (var _Signal in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Signal.Signal.Id) && string.IsNullOrWhiteSpace(_Signal.Signal.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Signal.Signal.Id}', '{_Signal.Signal.Description}', " +
                                        $"'{_Signal.Signal.VarName}', '{_Signal.Signal.Area}', " +
                                        $"'{_Signal.Signal.Address}', '{_Signal.Signal.LinkValue}', '{_Signal.Signal.Index}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("SIGNAL_DI", Request.TableFieldSignalsDI, Request.FieldSignalsDI, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Сигналы AI
                            if (_ViewModel is SignalsAIUserControlViewModel)
                            {
                                var Data = _ViewModel as SignalsAIUserControlViewModel;

                                foreach (var _Signal in Data.Params)
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
                                if (!Request.SetData("SIGNAL_AI", Request.TableFieldSignalsAI, Request.FieldSignalsAI, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Сигналы DO
                            if (_ViewModel is SignalsDOUserControlViewModel)
                            {
                                var Data = _ViewModel as SignalsDOUserControlViewModel;

                                foreach (var _Signal in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Signal.Signal.Id) && string.IsNullOrWhiteSpace(_Signal.Signal.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Signal.Signal.Id}', '{_Signal.Signal.Description}', '{_Signal.Signal.VarName}', '{_Signal.Signal.Area}', " +
                                        $"'{_Signal.Signal.Address}', '{_Signal.Signal.LinkValue}', '{_Signal.Signal.Index}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("SIGNAL_DO", Request.TableFieldSignalsDO, Request.FieldSignalsDO, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Сигналы AO
                            if (_ViewModel is SignalsAOUserControlViewModel)
                            {
                                var Data = _ViewModel as SignalsAOUserControlViewModel;

                                foreach (var _Signal in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Signal.Signal.Id) && string.IsNullOrWhiteSpace(_Signal.Signal.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Signal.Signal.Id}', '{_Signal.Signal.Description}', '{_Signal.Signal.VarName}', '{_Signal.Signal.Area}', " +
                                        $"'{_Signal.Signal.Address}', '{_Signal.Signal.LinkValue}', '{_Signal.Signal.Index}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("SIGNAL_AO", Request.TableFieldSignalsAO, Request.FieldSignalsAO, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Секции шин
                            if (_ViewModel is ECUserControlViewModel)
                            {
                                var Data = _ViewModel as ECUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', '{_Param.Inv}', " +
                                        $"'{_Param.TypeSignal}', '{_Param.Address}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("PARAM_EC", Request.TableFieldEC, Request.FieldEC, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region DI формируемые
                            if (_ViewModel is UserDIUserControlViewModel)
                            {
                                var Data = _ViewModel as UserDIUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', " +
                                        $"'{(int.Parse(_Param.Index) - 1) % 16}', '{_Param.Address}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("USER_DI", Request.TableFieldUserDI, Request.FieldUserDI, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region AI формируемые
                            if (_ViewModel is UserAIUserControlViewModel)
                            {
                                var Data = _ViewModel as UserAIUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue = $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', '{_Param.Address}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("USER_AI", Request.TableFieldUserAI, Request.FieldUserAI, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Регистры формируемые
                            if (_ViewModel is UserRegUserControlViewModel)
                            {
                                var Data = _ViewModel as UserRegUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue = $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', '{_Param.Address}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("USER_REG", Request.TableFieldUserReg, Request.FieldUserReg, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Сигналы групп
                            if (_ViewModel is SignalsGroupUserControlViewModel)
                            {
                                var Data = _ViewModel as SignalsGroupUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Id}', '{_Param.Description}', '{_Param.Inv}', " +
                                        $"'{_Param.TypeSignal}', '{_Param.Address}', '{_Param.Index}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("SIGNAL_GROUP", Request.TableFieldSignalsGroup, Request.FieldSignalsGroup, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Группы сигналов
                            if (_ViewModel is GroupsSignalUserControlViewModel)
                            {
                                var Data = _ViewModel as GroupsSignalUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', '{_Param.QtyInGroup}', " +
                                        $"'{_Param.AddressStart}', '{_Param.AddressEnd}', '{_Param.Param.Index}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("GROUP_SIGNAL", Request.TableFieldGroupsSignal, Request.FieldGroupsSignal, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Задвижки
                            if (_ViewModel is UZDUserControlViewModel)
                            {
                                var Data = _ViewModel as UZDUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Description}', '{_Param.VarName}', '{_Param.ShortDescription}', '{_Param.IndexEC}', '{_Param.IndexGroup}', " +
                                        $"'{_Param.DescriptionGroup}', '{_Param.Dist}', '{_Param.DoubleStop}', '{_Param.Bur}', '{_Param.COz}', '{_Param.CZz}', " +
                                        $"'{_Param.EC}', '{_Param.CheckState}', '{_Param.RsOff}', '{_Param.TypeZD}', '{_Param.IndexPZ}', '{_Param.IndexBD}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("UZD", Request.TableFieldUZD, Request.FieldUZD, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Вспомсистемы
                            if (_ViewModel is UVSUserControlViewModel)
                            {
                                var Data = _ViewModel as UVSUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Description}', '{_Param.VarName}', '{_Param.ShortDescription}', '{_Param.IndexEC}', " +
                                        $"'{_Param.IndexGroup}', '{_Param.DescriptionGroup}', '{_Param.Reservable}', '{_Param.TypePressure}', " +
                                        $"'{_Param.COz}', '{_Param.OnePressureSensorGroup}', '{_Param.TypePressure}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("UVS", Request.TableFieldUVS, Request.FieldUVS, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Насосные агрегаты
                            if (_ViewModel is UMPNAUserControlViewModel)
                            {
                                var Data = _ViewModel as UMPNAUserControlViewModel;

                                #region Данные насосных агрегатов
                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Description}', '{_Param.VarName}', '{_Param.ShortDescription}', '{_Param.IndexPZ}', " +
                                        $"'{_Param.IndexVZ}', '{_Param.TypeUMPNA}', '{_Param.IndexGroupMS}', '{_Param.UsedMCP}', " +
                                        $"'{_Param.UsedKPD}', '{_Param.CountButtonStop}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("UMPNA", Request.TableFieldUMPNA, Request.FieldUMPNA, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                                #endregion

                                #region Готовности насосных агрегатов
                                foreach (var _UMPNA in Data.Params)
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
                                if (!Request.SetData("KGMPNA", Request.TableFieldKGMPNA, Request.FieldKGMPNA, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                                #endregion

                                #region Агрегатные защиты
                                foreach (var _UMPNA in Data.Params)
                                {
                                    foreach (var _Param in _UMPNA.KTPRA)
                                    {
                                        if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                        var _FieldValue =
                                            $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', '{_Param.Param.Inv}', " +
                                            $"'{_Param.Param.TypeSignal}', '{_Param.Param.Address}', '{_Param.NoMasked}'),";
                                        FieldValue.Add(_FieldValue);
                                    }
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("KTPRA", Request.TableFieldKTPRA, Request.FieldKTPRA, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                                #endregion

                                #region Предельные параметры агрегатных защит
                                foreach (var _UMPNA in Data.Params)
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
                                if (!Request.SetData("KTPRAS", Request.TableFieldKTPRAS, Request.FieldKTPRAS, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                                #endregion
                            }
                            #endregion

                            #region Общестанционные защиты
                            if (_ViewModel is KTPRUserControlViewModel)
                            {
                                var Data = _ViewModel as KTPRUserControlViewModel;

                                foreach (var _Param in Data.Params)
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
                                if (!Request.SetData("KTPR", Request.TableFieldKTPR, Request.FieldKTPR, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Предельные параметры общестанционых защит
                            if (_ViewModel is KTPRSUserControlViewModel)
                            {
                                var Data = _ViewModel as KTPRSUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', " +
                                        $"'{_Param.Param.Inv}', '{_Param.Param.TypeSignal}', '{_Param.Param.Address}', " +
                                        $"'{_Param.TypeWarning}', '{_Param.Type}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("KTPRS", Request.TableFieldKTPRS, Request.FieldKTPRS, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region LIST5
                            if (_ViewModel is SignalingUserControlViewModel)
                            {
                                var Data = _ViewModel as SignalingUserControlViewModel;

                                foreach (var _Param in Data.Params)
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
                                if (!Request.SetData("Signaling", Request.TableFieldSignaling, Request.FieldSignaling, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Табло и сирены
                            if (_ViewModel is UTSUserControlViewModel)
                            {
                                var Data = _ViewModel as UTSUserControlViewModel;

                                foreach (var _Param in Data.Params)
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
                                if (!Request.SetData("UTS", Request.TableFieldUTS, Request.FieldUTS, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Уставки типа REAL
                            if (_ViewModel is UstRealUserControlViewModel)
                            {
                                var Data = _ViewModel as UstRealUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Setpoints.Id) && string.IsNullOrWhiteSpace(_Param.Setpoints.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Setpoints.Id}', '{_Param.Setpoints.Description}', '{_Param.Setpoints.VarName}', " +
                                        $"'{_Param.Setpoints.Address}', '{_Param.Setpoints.Value}', '{_Param.Setpoints.Unit}', '{_Param.QtySimbolsComma}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("SETPOINTS_REAL", Request.TableFieldUstReal, Request.FieldUstReal, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Временные уставки общие
                            if (_ViewModel is UstCommonUserControlViewModel)
                            {
                                var Data = _ViewModel as UstCommonUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', " +
                                        $"'{_Param.Address}', '{_Param.Value}', '{_Param.Unit}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("SETPOINTS_COMMON", Request.TableFieldUstCommon, Request.FieldUstCommon, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #region Карта ручного ввода
                            if (_ViewModel is HandMapUserControlViewModel)
                            {
                                var Data = _ViewModel as HandMapUserControlViewModel;

                                foreach (var _Param in Data.Params)
                                {
                                    var iSh = int.TryParse(_Param.Index, out int _Index);
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Description}', '{_Param.VarName}', '{_Index % 16}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!Request.SetData("HAND_MAP", Request.TableFieldHandMap, Request.FieldHandMap, FieldValue))
                                    ConnectSetting.SuccessUpdate = false;
                            }
                            #endregion

                            #endregion
                        }

                        #region Сообщения
                        List<string> MessageFieldValue = new();
                        var MessageViewModel = App.Services.GetRequiredService<MessageWindowViewModel>();
                        var Params = MessageViewModel.Params;

                        foreach (var _Param in Params)
                        {
                            
                            foreach (var _Message in _Param.Messages)
                            {
                                if (string.IsNullOrWhiteSpace(_Message.Description)) continue;

                                var _FieldValue =
                                    $"('{_Param.Description}', '{_Param.IndexSystem}', " +
                                    $"'{_Message.Description}', '{_Message.Color}', '{_Message.NeedAck}', '{_Message.PathSound}', " +
                                    $"'{_Message.TypeSound}', '{_Message.NeedPlay}', '{_Message.Hide}', '{_Message.LevelAccess}'),";

                                MessageFieldValue.Add(_FieldValue);
                            }
                        }
                        if (MessageFieldValue is not null && MessageFieldValue.Count > 0)
                            if (!Request.SetData("MESSAGES", Request.TableFieldMessages, Request.FieldMessages, MessageFieldValue))
                                ConnectSetting.SuccessUpdate = false;
                        #endregion
                    }
                }
            }
            catch (Exception e)
            {
                Log.WriteLog($"Не удалось сохранить данные приложения - {e}", App.NameApp);
                UserDialog.SendMessage("Управление приложением", "Не удалось сохранить данные приложения\nСм. лог.",
                    MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
            }
        }
        #endregion

        #region Создать новый проект
        /// <summary>
        /// Создать новый проект
        /// </summary>
        public void CreateNewProject()
        {
            ILogSerivece Log = new LogSerivece();
            IEnumerable<IViewModelUserControls> _ViewModels = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            try
            {
                foreach (var item in _ViewModels)
                {
                    object _Data = item switch
                    {
                        LayotRackUserControlViewModel Data => Data.Params = new(),
                        TableSignalsUserControlViewModel Data => Data.Params = new(),
                        SignalsDIUserControlViewModel Data => Data.Params = new(),
                        SignalsAIUserControlViewModel Data => Data.Params = new(),
                        SignalsDOUserControlViewModel Data => Data.Params = new(),
                        SignalsAOUserControlViewModel Data => Data.Params = new(),
                        ECUserControlViewModel Data => Data.Params = new(),
                        UserDIUserControlViewModel Data => Data.Params = new(),
                        UserAIUserControlViewModel Data => Data.Params = new(),
                        UserRegUserControlViewModel Data => Data.Params = new(),
                        SignalsGroupUserControlViewModel Data => Data.Params = new(),
                        GroupsSignalUserControlViewModel Data => Data.Params = new(),
                        UZDUserControlViewModel Data => Data.Params = new(),
                        UVSUserControlViewModel Data => Data.Params = new(),
                        UMPNAUserControlViewModel Data => Data.Params = new(),
                        KTPRUserControlViewModel Data => Data.Params = new(),
                        KTPRSUserControlViewModel Data => Data.Params = new(),
                        SignalingUserControlViewModel Data => Data.Params = new(),
                        UTSUserControlViewModel Data => Data.Params = new(),
                        UstRealUserControlViewModel Data => Data.Params = new(),
                        UstCommonUserControlViewModel Data => Data.Params = new(),
                        HandMapUserControlViewModel Data => Data.Params = new(),
                        MessageWindowViewModel Data => Data.Params = new(),
                        _ => null
                    };
                }

                App.Settings.Config.PathProject = "";

            }
            catch (Exception e)
            {
                Log.WriteLog($"Не удалось создать новый проект - {e}", App.NameApp);
            }
        }
        #endregion
    }
}