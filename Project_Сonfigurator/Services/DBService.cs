using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Models;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Setpoints;
using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using Project_Сonfigurator.ViewModels.UserControls.Signals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Project_Сonfigurator.Services
{
    public class DBService : BaseService, IDBService
    {
        #region Сервис шифрования данных
        /// <summary>
        /// Сервис шифрования данных
        /// </summary>
        private readonly IEncryptorService EncryptorServices = new EncryptorService();
        #endregion

        #region Сервис запросов к БД
        /// <summary>
        /// Сервис запросов к БД
        /// </summary>
        private readonly IRequestToDataBaseService RequestToDataBaseServices = new RequestToDataBaseService();
        #endregion

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
            var path = App.Settings.Config.PathProject;
            if (!File.Exists(path))
            {
                CreateNewProject();
                return;
            }

            var FileNameEncrypt = path;
            var FileNameEncrypted = path.Replace(App.__EncryptedProjectFileSuffix, ".xml");
            EncryptorServices.Decryptor(FileNameEncrypt, FileNameEncrypted, "");

            try
            {
                var SettingsAppSerializer = new XmlSerializer(typeof(ProjectDataToSave));
                using FileStream fs = new(FileNameEncrypted, FileMode.OpenOrCreate);
                DataToSave = SettingsAppSerializer.Deserialize(fs) as ProjectDataToSave;

                App.Services.GetRequiredService<MessageWindowViewModel>().SetParams(DataToSave.Messages);
                var _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();

                try
                {
                    foreach (var _ViewModel in _ViewModelsUserControl)
                    {
                        if (_ViewModel is LayotRackUserControlViewModel) _ViewModel.SetParams(DataToSave.LayotRack);
                        if (_ViewModel is TableSignalsUserControlViewModel) _ViewModel.SetParams(DataToSave.TableSignals);
                        if (_ViewModel is SignalsDIUserControlViewModel) _ViewModel.SetParams(DataToSave.SignalDI);
                        if (_ViewModel is SignalsAIUserControlViewModel) _ViewModel.SetParams(DataToSave.SignalAI);
                        if (_ViewModel is SignalsDOUserControlViewModel) _ViewModel.SetParams(DataToSave.SignalDO);
                        if (_ViewModel is SignalsAOUserControlViewModel) _ViewModel.SetParams(DataToSave.SignalAO);
                        if (_ViewModel is ECUserControlViewModel) _ViewModel.SetParams(DataToSave.ECParam);
                        if (_ViewModel is UserDIUserControlViewModel) _ViewModel.SetParams(DataToSave.UserDI);
                        if (_ViewModel is UserAIUserControlViewModel) _ViewModel.SetParams(DataToSave.UserAI);
                        if (_ViewModel is UserRegUserControlViewModel) _ViewModel.SetParams(DataToSave.UserReg);
                        if (_ViewModel is SignalsGroupUserControlViewModel) _ViewModel.SetParams(DataToSave.SignalGroup);
                        if (_ViewModel is GroupsSignalUserControlViewModel) _ViewModel.SetParams(DataToSave.GroupSignals);
                        if (_ViewModel is UZDUserControlViewModel) _ViewModel.SetParams(DataToSave.UZD);
                        if (_ViewModel is UVSUserControlViewModel) _ViewModel.SetParams(DataToSave.UVS);
                        if (_ViewModel is UMPNAUserControlViewModel) _ViewModel.SetParams(DataToSave.UMPNA);
                        if (_ViewModel is KTPRUserControlViewModel) _ViewModel.SetParams(DataToSave.KTPR);
                        if (_ViewModel is KTPRSUserControlViewModel) _ViewModel.SetParams(DataToSave.KTPRS);
                        if (_ViewModel is SignalingUserControlViewModel) _ViewModel.SetParams(DataToSave.Signaling);
                        if (_ViewModel is UTSUserControlViewModel) _ViewModel.SetParams(DataToSave.UTS);
                        if (_ViewModel is UstRealUserControlViewModel) _ViewModel.SetParams(DataToSave.SetpointsReal);
                        if (_ViewModel is UstCommonUserControlViewModel) _ViewModel.SetParams(DataToSave.SetpointsCommon);
                        if (_ViewModel is HandMapUserControlViewModel) _ViewModel.SetParams(DataToSave.HandMap);
                        if (_ViewModel is CommandUserControlViewModel) _ViewModel.SetParams(DataToSave.Commands);
                        if (_ViewModel is MessagesUserControlViewModel) _ViewModel.SetParams(DataToSave.SystemMessages);
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLog($"Не удалось загрузить данные приложения - {e}", App.NameApp);
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Не удалось загрузить данные приложения - {e}", App.NameApp);
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
            var path = App.Settings.Config.PathProject.Replace(App.__EncryptedProjectFileSuffix, ".xml");
            var SettingsAppSerializer = new XmlSerializer(typeof(ProjectDataToSave));
            var xmlWriterSettings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };

            DataToSave.Messages = App.Services.GetRequiredService<MessageWindowViewModel>().GetParams<CollectionMessage>();
            var _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();

            try
            {
                foreach (var _ViewModel in _ViewModelsUserControl)
                {
                    if (_ViewModel is LayotRackUserControlViewModel) DataToSave.LayotRack = _ViewModel.GetParams<USO>();
                    if (_ViewModel is TableSignalsUserControlViewModel) DataToSave.TableSignals = _ViewModel.GetParams<USO>();
                    if (_ViewModel is SignalsDIUserControlViewModel) DataToSave.SignalDI = _ViewModel.GetParams<SignalDI>();
                    if (_ViewModel is SignalsAIUserControlViewModel) DataToSave.SignalAI = _ViewModel.GetParams<SignalAI>();
                    if (_ViewModel is SignalsDOUserControlViewModel) DataToSave.SignalDO = _ViewModel.GetParams<SignalDO>();
                    if (_ViewModel is SignalsAOUserControlViewModel) DataToSave.SignalAO = _ViewModel.GetParams<SignalAO>();
                    if (_ViewModel is ECUserControlViewModel) DataToSave.ECParam = _ViewModel.GetParams<BaseParam>();
                    if (_ViewModel is UserDIUserControlViewModel) DataToSave.UserDI = _ViewModel.GetParams<BaseSignal>();
                    if (_ViewModel is UserAIUserControlViewModel) DataToSave.UserAI = _ViewModel.GetParams<BaseSignal>();
                    if (_ViewModel is UserRegUserControlViewModel) DataToSave.UserReg = _ViewModel.GetParams<BaseParam>();
                    if (_ViewModel is SignalsGroupUserControlViewModel) DataToSave.SignalGroup = _ViewModel.GetParams<BaseParam>();
                    if (_ViewModel is GroupsSignalUserControlViewModel) DataToSave.GroupSignals = _ViewModel.GetParams<GroupSignal>();
                    if (_ViewModel is UZDUserControlViewModel) DataToSave.UZD = _ViewModel.GetParams<BaseUZD>();
                    if (_ViewModel is UVSUserControlViewModel) DataToSave.UVS = _ViewModel.GetParams<BaseUVS>();
                    if (_ViewModel is UMPNAUserControlViewModel) DataToSave.UMPNA = _ViewModel.GetParams<BaseUMPNA>();
                    if (_ViewModel is KTPRUserControlViewModel) DataToSave.KTPR = _ViewModel.GetParams<BaseKTPR>();
                    if (_ViewModel is KTPRSUserControlViewModel) DataToSave.KTPRS = _ViewModel.GetParams<BaseKTPRS>();
                    if (_ViewModel is SignalingUserControlViewModel) DataToSave.Signaling = _ViewModel.GetParams<BaseSignaling>();
                    if (_ViewModel is UTSUserControlViewModel) DataToSave.UTS = _ViewModel.GetParams<BaseUTS>();
                    if (_ViewModel is UstRealUserControlViewModel) DataToSave.SetpointsReal = _ViewModel.GetParams<BaseSetpointsReal>();
                    if (_ViewModel is UstCommonUserControlViewModel) DataToSave.SetpointsCommon = _ViewModel.GetParams<BaseSetpoints>();
                    if (_ViewModel is HandMapUserControlViewModel) DataToSave.HandMap = _ViewModel.GetParams<BaseParam>();
                    if (_ViewModel is CommandUserControlViewModel) DataToSave.Commands = _ViewModel.GetParams<BaseParam>();
                    if (_ViewModel is MessagesUserControlViewModel) DataToSave.SystemMessages = _ViewModel.GetParams<BaseSystemMessage>();
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Не удалось сохранить данные приложения - {e}", App.NameApp);
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
                    EncryptorServices.Encryptor(FileNameEncrypt, FileNameEncrypted, "");
                }
                catch (OperationCanceledException e)
                {
                    Logger.WriteLog($"Error in EncryptorAsync:\r\n{0} - {e}", App.NameApp);
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
            var _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            string TablesNotLoaded;
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
                            if (RequestToDataBaseServices.SetConnection(ConnectSetting, ip))
                            {
                                IPResult = ip;
                                break;
                            }
                        }
                        if (string.IsNullOrWhiteSpace(IPResult)) continue;
                        if (!RequestToDataBaseServices.OpenConnection(ConnectSetting, IPResult))
                            continue;

                        ConnectSetting.SuccessUpdate = true;

                        #region Формируем данные
                        TablesNotLoaded = "";
                        foreach (var _ViewModel in _ViewModelsUserControl)
                        {
                            List<string> FieldValue = new();

                            #region Таблица сигналов
                            if (_ViewModel is TableSignalsUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<USO>();
                                foreach (var _USO in Params)
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
                                if (!RequestToDataBaseServices.SetData("TableSignals",
                                    RequestToDataBaseServices.TableFieldTableSignals,
                                    RequestToDataBaseServices.FieldTableSignals,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "TableSignals; ";
                                }
                                    
                            }
                            #endregion

                            #region Сигналы DI
                            if (_ViewModel is SignalsDIUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<SignalDI>();
                                foreach (var _Signal in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Signal.Signal.Id) && string.IsNullOrWhiteSpace(_Signal.Signal.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Signal.Signal.Id}', '{_Signal.Signal.Description}', " +
                                        $"'{_Signal.Signal.VarName}', '{_Signal.Signal.Area}', " +
                                        $"'{_Signal.Signal.Address}', '{_Signal.Signal.LinkValue}', '{_Signal.Signal.Index}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "SIGNAL_DI",
                                    RequestToDataBaseServices.TableFieldSignalsDI,
                                    RequestToDataBaseServices.FieldSignalsDI,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "SIGNAL_DI; ";
                                }
                            }
                            #endregion

                            #region Сигналы AI
                            if (_ViewModel is SignalsAIUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<SignalAI>();
                                foreach (var _Signal in Params)
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
                                if (!RequestToDataBaseServices.SetData(
                                    "SIGNAL_AI",
                                    RequestToDataBaseServices.TableFieldSignalsAI,
                                    RequestToDataBaseServices.FieldSignalsAI,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "SIGNAL_AI; ";
                                }
                            }
                            #endregion

                            #region Сигналы DO
                            if (_ViewModel is SignalsDOUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<SignalDO>();
                                foreach (var _Signal in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Signal.Signal.Id) && string.IsNullOrWhiteSpace(_Signal.Signal.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Signal.Signal.Id}', '{_Signal.Signal.Description}', '{_Signal.Signal.VarName}', '{_Signal.Signal.Area}', " +
                                        $"'{_Signal.Signal.Address}', '{_Signal.Signal.LinkValue}', '{_Signal.Signal.Index}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "SIGNAL_DO",
                                    RequestToDataBaseServices.TableFieldSignalsDO,
                                    RequestToDataBaseServices.FieldSignalsDO,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "SIGNAL_DO; ";
                                }
                            }
                            #endregion

                            #region Сигналы AO
                            if (_ViewModel is SignalsAOUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<SignalAO>();
                                foreach (var _Signal in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Signal.Signal.Id) && string.IsNullOrWhiteSpace(_Signal.Signal.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Signal.Signal.Id}', '{_Signal.Signal.Description}', '{_Signal.Signal.VarName}', '{_Signal.Signal.Area}', " +
                                        $"'{_Signal.Signal.Address}', '{_Signal.Signal.LinkValue}', '{_Signal.Signal.Index}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "SIGNAL_AO",
                                    RequestToDataBaseServices.TableFieldSignalsAO,
                                    RequestToDataBaseServices.FieldSignalsAO,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "SIGNAL_AO; ";
                                }
                            }
                            #endregion

                            #region Секции шин
                            if (_ViewModel is ECUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseParam>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', '{_Param.Inv}', " +
                                        $"'{_Param.TypeSignal}', '{_Param.Address}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "PARAM_EC",
                                    RequestToDataBaseServices.TableFieldEC,
                                    RequestToDataBaseServices.FieldEC,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "PARAM_EC; ";
                                }
                            }
                            #endregion

                            #region DI формируемые
                            if (_ViewModel is UserDIUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseSignal>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', " +
                                        $"'{(int.Parse(_Param.Index) - 1) % 16}', '{_Param.Address}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "USER_DI",
                                    RequestToDataBaseServices.TableFieldUserDI,
                                    RequestToDataBaseServices.FieldUserDI,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "USER_DI; ";
                                }
                            }
                            #endregion

                            #region AI формируемые
                            if (_ViewModel is UserAIUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseSignal>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue = $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', '{_Param.Address}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "USER_AI",
                                    RequestToDataBaseServices.TableFieldUserAI,
                                    RequestToDataBaseServices.FieldUserAI,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "USER_AI; ";
                                }
                            }
                            #endregion

                            #region Регистры формируемые
                            if (_ViewModel is UserRegUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseParam>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue = $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', '{_Param.Address}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "USER_REG",
                                    RequestToDataBaseServices.TableFieldUserReg,
                                    RequestToDataBaseServices.FieldUserReg,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "USER_REG; ";
                                }
                            }
                            #endregion

                            #region Сигналы групп
                            if (_ViewModel is SignalsGroupUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseParam>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Id}', '{_Param.Description}', '{_Param.Inv}', " +
                                        $"'{_Param.TypeSignal}', '{_Param.Address}', '{_Param.Index}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "SIGNAL_GROUP",
                                    RequestToDataBaseServices.TableFieldSignalsGroup,
                                    RequestToDataBaseServices.FieldSignalsGroup,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "SIGNAL_GROUP; ";
                                }
                            }
                            #endregion

                            #region Группы сигналов
                            if (_ViewModel is GroupsSignalUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<GroupSignal>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', '{_Param.QtyInGroup}', " +
                                        $"'{_Param.AddressStart}', '{_Param.AddressEnd}', '{_Param.Param.Index}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "GROUP_SIGNAL",
                                    RequestToDataBaseServices.TableFieldGroupsSignal,
                                    RequestToDataBaseServices.FieldGroupsSignal,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "GROUP_SIGNAL; ";
                                }
                            }
                            #endregion

                            #region Задвижки
                            if (_ViewModel is UZDUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseUZD>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Description}', '{_Param.VarName}', '{_Param.ShortDescription}', '{_Param.IndexEC}', '{_Param.IndexGroup}', " +
                                        $"'{_Param.DescriptionGroup}', '{_Param.Dist}', '{_Param.DoubleStop}', '{_Param.Bur}', '{_Param.COz}', '{_Param.CZz}', " +
                                        $"'{_Param.EC}', '{_Param.CheckState}', '{_Param.RsOff}', '{_Param.TypeZD}', '{_Param.IndexPZ}', '{_Param.IndexBD}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "UZD",
                                    RequestToDataBaseServices.TableFieldUZD,
                                    RequestToDataBaseServices.FieldUZD,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "UZD; ";
                                }
                            }
                            #endregion

                            #region Вспомсистемы
                            if (_ViewModel is UVSUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseUVS>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Description}', '{_Param.VarName}', '{_Param.ShortDescription}', '{_Param.IndexEC}', " +
                                        $"'{_Param.IndexGroup}', '{_Param.DescriptionGroup}', '{_Param.Reservable}', '{_Param.TypePressure}', " +
                                        $"'{_Param.COz}', '{_Param.OnePressureSensorGroup}', '{_Param.TypePressure}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "UVS", RequestToDataBaseServices.TableFieldUVS,
                                    RequestToDataBaseServices.FieldUVS,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "UVS; ";
                                }
                            }
                            #endregion

                            #region Насосные агрегаты
                            if (_ViewModel is UMPNAUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseUMPNA>();

                                #region Данные насосных агрегатов
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Description}', '{_Param.VarName}', '{_Param.ShortDescription}', '{_Param.IndexPZ}', " +
                                        $"'{_Param.IndexVZ}', '{_Param.TypeUMPNA}', '{_Param.IndexGroupMS}', '{_Param.UsedMCP}', " +
                                        $"'{_Param.UsedKPD}', '{_Param.CountButtonStop}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "UMPNA",
                                    RequestToDataBaseServices.TableFieldUMPNA,
                                    RequestToDataBaseServices.FieldUMPNA,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "UMPNA; ";
                                }
                                #endregion

                                #region Готовности насосных агрегатов
                                FieldValue = new();
                                foreach (var _UMPNA in Params)
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
                                if (!RequestToDataBaseServices.SetData(
                                    "KGMPNA",
                                    RequestToDataBaseServices.TableFieldKGMPNA,
                                    RequestToDataBaseServices.FieldKGMPNA,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "KGMPNA; ";
                                }
                                #endregion

                                #region Агрегатные защиты
                                FieldValue = new();
                                foreach (var _UMPNA in Params)
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
                                if (!RequestToDataBaseServices.SetData(
                                    "KTPRA",
                                    RequestToDataBaseServices.TableFieldKTPRA,
                                    RequestToDataBaseServices.FieldKTPRA,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "KTPRA; ";
                                }
                                #endregion

                                #region Предельные параметры агрегатных защит
                                FieldValue = new();
                                foreach (var _UMPNA in Params)
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
                                if (!RequestToDataBaseServices.SetData(
                                    "KTPRAS",
                                    RequestToDataBaseServices.TableFieldKTPRAS,
                                    RequestToDataBaseServices.FieldKTPRAS,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "KTPRAS; ";
                                }
                                #endregion
                            }
                            #endregion

                            #region Общестанционные защиты
                            if (_ViewModel is KTPRUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseKTPR>();
                                foreach (var _Param in Params)
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
                                if (!RequestToDataBaseServices.SetData(
                                    "KTPR",
                                    RequestToDataBaseServices.TableFieldKTPR,
                                    RequestToDataBaseServices.FieldKTPR,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "KTPR; ";
                                }
                            }
                            #endregion

                            #region Предельные параметры общестанционых защит
                            if (_ViewModel is KTPRSUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseKTPRS>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Param.Id) && string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Param.Id}', '{_Param.Param.Description}', '{_Param.Param.VarName}', " +
                                        $"'{_Param.Param.Inv}', '{_Param.Param.TypeSignal}', '{_Param.Param.Address}', " +
                                        $"'{_Param.TypeWarning}', '{_Param.Type}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "KTPRS",
                                    RequestToDataBaseServices.TableFieldKTPRS,
                                    RequestToDataBaseServices.FieldKTPRS,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "KTPRS; ";
                                }
                            }
                            #endregion

                            #region LIST5
                            if (_ViewModel is SignalingUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseSignaling>();
                                foreach (var _Param in Params)
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
                                if (!RequestToDataBaseServices.SetData(
                                    "Signaling",
                                    RequestToDataBaseServices.TableFieldSignaling,
                                    RequestToDataBaseServices.FieldSignaling,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "Signaling; ";
                                }
                            }
                            #endregion

                            #region Табло и сирены
                            if (_ViewModel is UTSUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseUTS>();
                                foreach (var _Param in Params)
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
                                if (!RequestToDataBaseServices.SetData(
                                    "UTS",
                                    RequestToDataBaseServices.TableFieldUTS,
                                    RequestToDataBaseServices.FieldUTS,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "UTS; ";
                                }
                            }
                            #endregion

                            #region Уставки типа REAL
                            if (_ViewModel is UstRealUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseSetpointsReal>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Setpoints.Id) && string.IsNullOrWhiteSpace(_Param.Setpoints.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Setpoints.Id}', '{_Param.Setpoints.Description}', '{_Param.Setpoints.VarName}', " +
                                        $"'{_Param.Setpoints.Address}', '{_Param.Setpoints.Value}', '{_Param.Setpoints.Unit}', '{_Param.QtySimbolsComma}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "SETPOINTS_REAL",
                                    RequestToDataBaseServices.TableFieldUstReal,
                                    RequestToDataBaseServices.FieldUstReal,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "SETPOINTS_REAL; ";
                                }
                            }
                            #endregion

                            #region Карта ручного ввода
                            if (_ViewModel is HandMapUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseParam>();
                                foreach (var _Param in Params)
                                {
                                    var iSh = int.TryParse(_Param.Index, out int _Index);
                                    if (string.IsNullOrWhiteSpace(_Param.Id) && string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Description}', '{_Param.VarName}', '{_Index % 16}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "HAND_MAP",
                                    RequestToDataBaseServices.TableFieldHandMap,
                                    RequestToDataBaseServices.FieldHandMap,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "HAND_MAP; ";
                                }
                            }
                            #endregion

                            #region Команды
                            if (_ViewModel is CommandUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseParam>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.VarName)) continue;
                                    var _FieldValue =
                                        $"('{_Param.VarName}', '{_Param.Id}', '{_Param.Description}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "COMMANDS",
                                    RequestToDataBaseServices.TableFieldCommand,
                                    RequestToDataBaseServices.FieldCommand,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "COMMANDS; ";
                                }
                            }
                            #endregion

                            #region Системы сообщений
                            if (_ViewModel is MessagesUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseSystemMessage>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.SystemMessage)) continue;
                                    var _FieldValue =
                                        $"('{_Param.SystemMessage}', '{_Param.DescriptionMessage}', '{_Param.DescriptionSystem}', '{_Param.NameTabList}'),";
                                    FieldValue.Add(_FieldValue);
                                }
                                if (FieldValue is null || FieldValue.Count <= 0) continue;
                                if (!RequestToDataBaseServices.SetData(
                                    "MESSAGE_SYSTEMS",
                                    RequestToDataBaseServices.TableFieldSyatemMessages,
                                    RequestToDataBaseServices.FieldSyatemMessages,
                                    FieldValue))
                                {
                                    ConnectSetting.SuccessUpdate = false;
                                    TablesNotLoaded += "MESSAGE_SYSTEMS; ";
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region Уставки
                        List<string> UstFieldValue = new();
                        foreach (var _ViewModel in _ViewModelsUserControl)
                        {
                            #region Временные уставки общие
                            if (_ViewModel is UstCommonUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseSetpoints>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Value)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Id}', '{_Param.Description}', '{_Param.VarName}', " +
                                        $"'{_Param.Address}', '{_Param.Value}', '{_Param.Unit}'),";
                                    UstFieldValue.Add(_FieldValue);
                                }
                            }
                            #endregion

                            #region Временные уставки UZD
                            if (_ViewModel is UZDUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseUZD>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    foreach (var _Setpoint in _Param.Setpoints)
                                    {
                                        if (string.IsNullOrWhiteSpace(_Setpoint.Value)) continue;
                                        var _FieldValue =
                                            $"('{_Setpoint.Id}', '{_Param.Description}. {_Setpoint.Description}', '{_Setpoint.VarName}', " +
                                            $"'{_Setpoint.Address}', '{_Setpoint.Value}', '{_Setpoint.Unit}'),";
                                        UstFieldValue.Add(_FieldValue);
                                    }
                                }
                            }
                            #endregion

                            #region Временные уставки UVS
                            if (_ViewModel is UVSUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseUVS>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                                    foreach (var _Setpoint in _Param.Setpoints)
                                    {
                                        if (string.IsNullOrWhiteSpace(_Setpoint.Value)) continue;
                                        var _FieldValue =
                                            $"('{_Setpoint.Id}', '{_Param.Description}. {_Setpoint.Description}', '{_Setpoint.VarName}', " +
                                            $"'{_Setpoint.Address}', '{_Setpoint.Value}', '{_Setpoint.Unit}'),";
                                        UstFieldValue.Add(_FieldValue);
                                    }
                                }
                            }
                            #endregion

                            #region Временные уставки UMPNA
                            if (_ViewModel is UMPNAUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseUMPNA>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Description)) continue;

                                    #region Уставки МПНА
                                    foreach (var _Setpoint in _Param.Setpoints)
                                    {
                                        if (string.IsNullOrWhiteSpace(_Setpoint.Value)) continue;
                                        var _FieldValue =
                                            $"('{_Setpoint.Id}', '{_Param.Description}. {_Setpoint.Description}', '{_Setpoint.VarName}', " +
                                            $"'{_Setpoint.Address}', '{_Setpoint.Value}', '{_Setpoint.Unit}'),";
                                        UstFieldValue.Add(_FieldValue);
                                    }
                                    #endregion

                                    #region Уставки агрегатный готовностей
                                    foreach (var _SubParam in _Param.KGMPNA)
                                    {
                                        if (string.IsNullOrWhiteSpace(_SubParam.Setpoints.Value)) continue;
                                        var _FieldValue =
                                            $"('{_SubParam.Setpoints.Id}', '{_Param.Description}. {_SubParam.Param.Description}', '{_SubParam.Setpoints.VarName}', " +
                                            $"'{_SubParam.Setpoints.Address}', '{_SubParam.Setpoints.Value}', '{_SubParam.Setpoints.Unit}'),";
                                        UstFieldValue.Add(_FieldValue);
                                    }
                                    #endregion

                                    #region Уставки агрегатных защит
                                    foreach (var _SubParam in _Param.KTPRA)
                                    {
                                        if (string.IsNullOrWhiteSpace(_SubParam.Setpoints.Value)) continue;
                                        var _FieldValue =
                                            $"('{_SubParam.Setpoints.Id}', '{_Param.Description}. {_SubParam.Param.Description}', '{_SubParam.Setpoints.VarName}', " +
                                            $"'{_SubParam.Setpoints.Address}', '{_SubParam.Setpoints.Value}', '{_SubParam.Setpoints.Unit}'),";
                                        UstFieldValue.Add(_FieldValue);
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region Временные уставки обществнционных защит
                            if (_ViewModel is KTPRUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseKTPR>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Setpoints.Value)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Setpoints.Id}', '{_Param.Param.Description}', '{_Param.Setpoints.VarName}', " +
                                        $"'{_Param.Setpoints.Address}', '{_Param.Setpoints.Value}', '{_Param.Setpoints.Unit}'),";
                                    UstFieldValue.Add(_FieldValue);
                                }
                            }
                            #endregion

                            #region Временные уставки предельных параметров обществнционных защит
                            if (_ViewModel is KTPRSUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<BaseKTPRS>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Setpoints.Value)) continue;
                                    var _FieldValue =
                                        $"('{_Param.Setpoints.Id}', '{_Param.Param.Description}', '{_Param.Setpoints.VarName}', " +
                                        $"'{_Param.Setpoints.Address}', '{_Param.Setpoints.Value}', '{_Param.Setpoints.Unit}'),";
                                    UstFieldValue.Add(_FieldValue);
                                }
                            }
                            #endregion
                        }
                        if (UstFieldValue is not null && UstFieldValue.Count > 0)
                            if (!RequestToDataBaseServices.SetData(
                                "SETPOINTS",
                                RequestToDataBaseServices.TableFieldUstCommon,
                                RequestToDataBaseServices.FieldUstCommon,
                                UstFieldValue))
                            {
                                ConnectSetting.SuccessUpdate = false;
                                TablesNotLoaded += "SETPOINTS; ";
                            }
                        #endregion

                        #region Уставки аналоговых параметров
                        UstFieldValue = new();
                        foreach (var _ViewModel in _ViewModelsUserControl)
                        {
                            if (_ViewModel is SignalsAIUserControlViewModel)
                            {
                                var Params = _ViewModel.GetParams<SignalAI>();
                                foreach (var _Param in Params)
                                {
                                    if (string.IsNullOrWhiteSpace(_Param.Signal.Description)) continue;
                                    var _FieldValue =
                                            $"('{_Param.Signal.Id}', '{_Param.Signal.Description}', '{_Param.Setpoints.Unit}', " +
                                            $"'{_Param.Setpoints.TMin}', '{_Param.Setpoints.TMax}', " +
                                            $"'{_Param.Setpoints.TMin_1}', '{_Param.Setpoints.TMin_2}', '{_Param.Setpoints.TMin_3}', '{_Param.Setpoints.TMin_4}', '{_Param.Setpoints.TMin_5}', '{_Param.Setpoints.TMin_6}', " +
                                            $"'{_Param.Setpoints.TMax_1}', '{_Param.Setpoints.TMax_2}', '{_Param.Setpoints.TMax_3}', '{_Param.Setpoints.TMax_4}', '{_Param.Setpoints.TMax_5}', '{_Param.Setpoints.TMax_6}', " +
                                            $"'{_Param.Setpoints.AMin}', '{_Param.Setpoints.AMax}', '{_Param.Setpoints.Hyst}', " +
                                            $"'{_Param.Setpoints.HystNPD}', '{_Param.Setpoints.HystVPD}', '{_Param.Setpoints.KS}', " +
                                            $"'{_Param.Setpoints.PDDelay}', '{_Param.Signal.Address}', " +
                                            $"'\"\"', '\"\"', '\"\"', '\"\"', '\"\"', '\"\"', '\"\"', '\"\"', '\"\"', '\"\"', '\"\"', '\"\"', '\"\"', '\"\"'),";

                                    UstFieldValue.Add(_FieldValue);
                                }

                                if (UstFieldValue is not null && UstFieldValue.Count > 0)
                                    if (!RequestToDataBaseServices.SetData(
                                        "SETPOINTS_AI",
                                        RequestToDataBaseServices.TableFieldSetpointsAI,
                                        RequestToDataBaseServices.FieldSetpointsAI,
                                        UstFieldValue))
                                    {
                                        ConnectSetting.SuccessUpdate = false;
                                        TablesNotLoaded += "SETPOINTS_AI; ";
                                    }
                            }
                        }
                        #endregion

                        #region Сообщения
                        List<string> MessageFieldValue = new();
                        var _Params = App.Services.GetRequiredService<MessageWindowViewModel>().GetParams<CollectionMessage>();
                        foreach (var _Param in _Params)
                        {
                            foreach (var _Message in _Param.Messages)
                            {
                                if (string.IsNullOrWhiteSpace(_Message.Description)) continue;

                                var _FieldValue =
                                    $"('{_Message.Index}', '{_Param.Description}', '{_Param.IndexSystem}', '{_Param.NameSystem}', " +
                                    $"'{_Message.Description}', '{_Message.Color}', '{_Message.NeedAck}', '{_Message.PathSound}', " +
                                    $"'{_Message.TypeSound}', '{_Message.NeedPlay}', '{_Message.Hide}', '{_Message.LevelAccess}'),";

                                MessageFieldValue.Add(_FieldValue);
                            }
                        }
                        if (MessageFieldValue is not null && MessageFieldValue.Count > 0)
                            if (!RequestToDataBaseServices.SetData(
                                "MESSAGES",
                                RequestToDataBaseServices.TableFieldMessages,
                                RequestToDataBaseServices.FieldMessages,
                                MessageFieldValue))
                            {
                                ConnectSetting.SuccessUpdate = false;
                                TablesNotLoaded += "MESSAGES; ";
                            }
                        #endregion

                        if (!ConnectSetting.SuccessUpdate)
                        {
                            UserDialog.SendMessage("Управление приложением", $"Таблицы из указанного списка не были загружены в БД - {TablesNotLoaded}",
                                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
                            
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Не удалось сохранить данные приложения - {e}", App.NameApp);
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
            App.Services.GetRequiredService<MessageWindowViewModel>().SetParams(new ObservableCollection<CollectionMessage>());
            var _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();

            try
            {
                foreach (var _ViewModel in _ViewModelsUserControl)
                {
                    if (_ViewModel is LayotRackUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<USO>());
                    if (_ViewModel is TableSignalsUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<USO>());
                    if (_ViewModel is SignalsDIUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<SignalDI>());
                    if (_ViewModel is SignalsAIUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<SignalAI>());
                    if (_ViewModel is SignalsDOUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<SignalDO>());
                    if (_ViewModel is SignalsAOUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<SignalAO>());
                    if (_ViewModel is ECUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseParam>());
                    if (_ViewModel is UserDIUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseSignal>());
                    if (_ViewModel is UserAIUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseSignal>());
                    if (_ViewModel is UserRegUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseParam>());
                    if (_ViewModel is SignalsGroupUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseParam>());
                    if (_ViewModel is GroupsSignalUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<GroupSignal>());
                    if (_ViewModel is UZDUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseUZD>());
                    if (_ViewModel is UVSUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseUVS>());
                    if (_ViewModel is UMPNAUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseUMPNA>());
                    if (_ViewModel is KTPRUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseKTPR>());
                    if (_ViewModel is KTPRSUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseKTPRS>());
                    if (_ViewModel is SignalingUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseSignaling>());
                    if (_ViewModel is UTSUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseUTS>());
                    if (_ViewModel is UstRealUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseSetpointsReal>());
                    if (_ViewModel is UstCommonUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseSetpoints>());
                    if (_ViewModel is HandMapUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseParam>());
                    if (_ViewModel is CommandUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseParam>());
                    if (_ViewModel is MessagesUserControlViewModel) _ViewModel.SetParams(new ObservableCollection<BaseSystemMessage>());
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