using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Export.VU.Interfaces;
using Project_Сonfigurator.ViewModels.AS;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Xml;

namespace Project_Сonfigurator.Services.Export.VU
{
    public class VUExportModbusMap : BaseService, IVUExportModbusMap
    {
        private const string Namespace = "format-version";
        private const string HR = "Holding Registers";
        private const string IR = "Input Registers";

        private static Dictionary<string, string> Elements;
        private static Dictionary<string, string> Parametrs;
        private static List<Dictionary<string, string>> ListParametrs;

        private static XmlDocument Doc = new();
        private static XmlNode RootNode;
        private static XmlNode ElementNode;
        private static XmlNode ParametrNode;

        #region Создание корневого узла
        /// <summary>
        /// Создание корневого узла
        /// </summary>
        private static void CreateRootNode(string NodeName)
        {
            Doc = new();
            XmlDeclaration xmlDeclaration = Doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = Doc.DocumentElement;
            Doc.InsertBefore(xmlDeclaration, root);

            RootNode = Doc.CreateElement(NodeName);
            XmlAttribute attribute = Doc.CreateAttribute(Namespace);
            attribute.Value = "1";
            RootNode.Attributes.SetNamedItem(attribute);

            Doc.AppendChild(RootNode);
        }
        #endregion

        #region Создание параметров элемента
        /// <summary>
        /// Создание параметров элемента
        /// </summary>
        private static void CreateParametrsElementNode(string NodeName,
            Dictionary<string, string> Elements = null, Dictionary<string, string> Parametrs = null)
        {
            ElementNode = Doc.CreateElement(NodeName);
            if (Elements is not null)
            {
                foreach (var _Element in Elements)
                {
                    XmlAttribute attribute = Doc.CreateAttribute(_Element.Key);
                    attribute.Value = _Element.Value;
                    ElementNode.Attributes.SetNamedItem(attribute);
                }
                RootNode.AppendChild(ElementNode);
            }

            if (Parametrs is not null)
            {
                foreach (var _Parametr in Parametrs)
                {
                    ParametrNode = Doc.CreateElement(_Parametr.Key);
                    ParametrNode.InnerText = _Parametr.Value;
                    ElementNode.AppendChild(ParametrNode);
                }
            }
        }
        #endregion

        #region Добавление параметра
        /// <summary>
        /// Добавление параметра
        /// </summary>
        private static long AddListParametrs(List<Dictionary<string, string>> _ListParametrs, string NodePath, string Table, long Address, long Size)
        {
            _ListParametrs.Add(new() { { "node-path", NodePath }, { "table", Table }, { "address", $"{Address}" } });
            return Address += Size;

        }
        #endregion

        #region Сохранение документа
        /// <summary>
        /// Сохранение документа
        /// </summary>
        private static void SaveDoc(string FileName)
        {
            var Path = string.IsNullOrWhiteSpace(App.Settings.Config.PathExportVU) ? App.Settings.Config.PathProject : App.Settings.Config.PathExportVU;
            Doc.Save($"{Path}{FileName}{App.__XMLExportFileSuffix}");
        }
        #endregion

        #region Экспорт карты Modbus адресов, для проектов AS
        /// <summary>
        /// Экспорт карты Modbus адресов, для проектов AS
        /// </summary>
        /// <returns></returns>
        public bool ASExprot()
        {
            #region Объявление
            var CheckBoxs = App.Services.GetRequiredService<ExportNamespaceASWindowViewModel>().GetParams<CheckBox>();
            var TypeSystem = App.Settings.Config.TypeSystem;
            var ModbusTCP_HR = App.Settings.Config.ModbusTCP_HR;
            var ModbusTCP_IR = App.Settings.Config.ModbusTCP_IR;
            string VariableName;
            long MBAddress;
            var Result = false;

            var MBAddress_AIChState = long.Parse(ModbusTCP_HR[5].AddressStart);
            var MBAddress_AIMask = long.Parse(ModbusTCP_HR[6].AddressStart);
            var MBAddress_AI = long.Parse(ModbusTCP_HR[7].AddressStart);
            var MBAddress_AOChState = long.Parse(ModbusTCP_HR[8].AddressStart);
            var MBAddress_AOMask = long.Parse(ModbusTCP_HR[9].AddressStart);
            var MBAddress_AO = long.Parse(ModbusTCP_HR[10].AddressStart);
            var MBAddress_DI = long.Parse(ModbusTCP_HR[11].AddressStart);
            var MBAddress_DO = long.Parse(ModbusTCP_HR[12].AddressStart);
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("root");

                foreach (var _CheckBox in CheckBoxs)
                {
                    if (_CheckBox.IsChecked != true) continue;
                    Result = true;

                    #region Сообщения
                    if (_CheckBox.Content.ToString() == "Сообщения")
                    {
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_IR[3].AddressStart);
                        for (int i = 0; i < App.Settings.Config.BufferSize; i++)
                        {
                            VariableName = $"{TypeSystem}.Messages.Msg_{i + 1}.Val_1";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, IR, MBAddress, 2);

                            VariableName = $"{TypeSystem}.Messages.Msg_{i + 1}.Val_2";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, IR, MBAddress, 2);

                            VariableName = $"{TypeSystem}.Messages.Msg_{i + 1}.MsgCode";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, IR, MBAddress, 2);

                            VariableName = $"{TypeSystem}.Messages.Msg_{i + 1}.YMD";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, IR, MBAddress, 2);

                            VariableName = $"{TypeSystem}.Messages.Msg_{i + 1}.HMSmS";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, IR, MBAddress, 2);
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }
                    #endregion

                    #region Диагностика
                    if (_CheckBox.Content.ToString() == "Диагностика")
                    {
                        #region DiagLink
                        MBAddress = long.Parse(ModbusTCP_HR[0].AddressStart);
                        VariableName = $"{TypeSystem}.DiagLink.COUNTER";
                        Elements = new() { { "Binding", "Introduced" } };
                        Parametrs = new() { { "node-path", VariableName }, { "table", HR }, { "address", $"{MBAddress}" } };
                        CreateParametrsElementNode("item", Elements, Parametrs);
                        #endregion

                        #region Diagnostics
                        var _Params = UserDialog.SearchControlViewModel("Компоновка корзин").GetParams<USO>();
                        var _ParParams = UserDialog.SearchControlViewModel("Таблица сигналов").GetParams<USO>();

                        #region Корзины
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[2].AddressStart);
                        var index = 1;
                        foreach (var _Param in _Params)
                        {
                            foreach (var _Racks in _Param.Racks)
                            {
                                VariableName = $"{TypeSystem}.DiagRack.Rack_{index}.Status";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                                VariableName = $"{TypeSystem}.DiagRack.Rack_{index}.ModHealth";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                                VariableName = $"{TypeSystem}.DiagRack.Rack_{index}.ErrBusA";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                                VariableName = $"{TypeSystem}.DiagRack.Rack_{index}.ErrBusB";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                                VariableName = $"{TypeSystem}.DiagRack.Rack_{index}.TotalErrBusA";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                                VariableName = $"{TypeSystem}.DiagRack.Rack_{index++}.TotalErrBusB";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                            }
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                        #endregion

                        #region ПКЛ
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[3].AddressStart);
                        index = 0;
                        foreach (var _Param in _Params)
                        {
                            foreach (var _Racks in _Param.Racks)
                            {
                                foreach (var _Module in _Racks.Modules)
                                {
                                    if (_Module.Type != TypeModule.PLC) continue;
                                    VariableName = $"{TypeSystem}.DiagPLC.Data.PLC_{++index}.Status";
                                    MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                                    VariableName = $"{TypeSystem}.DiagPLC.Data.PLC_{index}.Mode";
                                    MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                                    VariableName = $"{TypeSystem}.DiagPLC.Data.PLC_{index}.CurrDT";
                                    MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                                    for (int i = 0; i < 4; i++)
                                    {
                                        VariableName = $"{TypeSystem}.DiagPLC.Data.PLC_{index}.Version_{i + 1}";
                                        MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                                    }
                                    for (int i = 0; i < 4; i++)
                                    {
                                        VariableName = $"{TypeSystem}.DiagPLC.Data.PLC_{index}.StandardVersion_{i + 1}";
                                        MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                                    }

                                    VariableName = $"{TypeSystem}.DiagPLC.Data.PLC_{index}.BuildTime";
                                    MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                                    VariableName = $"{TypeSystem}.DiagPLC.Data.PLC_{index}.StandardBuildTime";
                                    MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                                }
                            }
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                        #endregion

                        #region УСО
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[4].AddressStart);
                        for (int i = 0; i < 2; i++)
                        {
                            VariableName = $"{TypeSystem}.DiagUSO.Data.Red_{i + 1}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                        }

                        for (int i = 0; i < 2; i++)
                        {
                            VariableName = $"{TypeSystem}.DiagUSO.Data.Yellow_{i + 1}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                        }

                        for (int i = 0; i < 2; i++)
                        {
                            VariableName = $"{TypeSystem}.DiagUSO.Data.RedBlink_{i + 1}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                        }

                        for (int i = 0; i < 2; i++)
                        {
                            VariableName = $"{TypeSystem}.DiagUSO.Data.YellowBlink_{i + 1}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                        }
                        VariableName = $"{TypeSystem}.DiagUSO.Data.Common";
                        MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                        foreach (var _Param in _Params)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                VariableName = $"{TypeSystem}.DiagUSO.USO_{_Param.Index}.ServiceSignal_{i + 1}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                            }
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                        #endregion

                        #region Модуля В/В
                        ListParametrs = new();
                        var IndexAI = 0;
                        var IndexAO = 0;
                        var IndexDI = 0;
                        var IndexDO = 0;

                        foreach (var _Param in _ParParams)
                        {
                            foreach (var _Rack in _Param.Racks)
                            {
                                foreach (var _Module in _Rack.Modules)
                                {
                                    if (string.IsNullOrWhiteSpace(_Module.Name)) continue;

                                    if (_Module.Type == TypeModule.AI)
                                    {
                                        VariableName = $"{TypeSystem}.DiagRack.HW_AI_{++IndexAI}.ChannelHealth";
                                        MBAddress_AIChState = AddListParametrs(ListParametrs, VariableName, HR, MBAddress_AIChState, 1);

                                        VariableName = $"{TypeSystem}.DiagRack.HW_AI_{IndexAI}.ChannelMask";
                                        MBAddress_AIMask = AddListParametrs(ListParametrs, VariableName, HR, MBAddress_AIMask, 1);

                                        foreach (var _Channel in _Module.Channels)
                                        {
                                            VariableName = $"{TypeSystem}.DiagRack.HW_AI_{IndexAI}.Value_{_Channel.Index}";
                                            MBAddress_AI = AddListParametrs(ListParametrs, VariableName, HR, MBAddress_AI, 1);
                                        }
                                    }

                                    if (_Module.Type == TypeModule.AO)
                                    {
                                        VariableName = $"{TypeSystem}.DiagRack.HW_AO_{++IndexAO}.ChannelHealth";
                                        MBAddress_AOChState = AddListParametrs(ListParametrs, VariableName, HR, MBAddress_AOChState, 1);

                                        VariableName = $"{TypeSystem}.DiagRack.HW_AO_{IndexAO}.ChannelMask";
                                        MBAddress_AOMask = AddListParametrs(ListParametrs, VariableName, HR, MBAddress_AOMask, 1);

                                        foreach (var _Channel in _Module.Channels)
                                        {
                                            VariableName = $"{TypeSystem}.DiagRack.HW_AO_{IndexAO}.Value_{_Channel.Index}";
                                            MBAddress_AO = AddListParametrs(ListParametrs, VariableName, HR, MBAddress_AO, 1);
                                        }
                                    }

                                    if (_Module.Type == TypeModule.DI)
                                    {
                                        VariableName = $"{TypeSystem}.DiagRack.HW_DI_{++IndexDI}.Value";
                                        MBAddress_DI = AddListParametrs(ListParametrs, VariableName, HR, MBAddress_DI, 2);
                                    }

                                    if (_Module.Type == TypeModule.DO)
                                    {
                                        VariableName = $"{TypeSystem}.DiagRack.HW_DO_{++IndexDO}.Value";
                                        MBAddress_DO = AddListParametrs(ListParametrs, VariableName, HR, MBAddress_DO, 2);
                                    }
                                }
                            }
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                        #endregion

                        #endregion
                    }
                    #endregion

                    #region Сигналы AI
                    if (_CheckBox.Content.ToString() == "Сигналы AI")
                    {
                        var Params = UserDialog.SearchControlViewModel("Сигналы AI").GetParams<SignalAI>();

                        #region OIP.Data
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[14].AddressStart);
                        var index = 0;
                        foreach (var _Param in Params)
                        {
                            if (string.IsNullOrWhiteSpace(_Param.Signal.Description)) continue;
                            VariableName = $"{TypeSystem}.OIP.Data.OIP_{++index}.ElValue";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                            VariableName = $"{TypeSystem}.OIP.Data.OIP_{index}.Value";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                            VariableName = $"{TypeSystem}.OIP.Data.OIP_{index}.VisualValue";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                            VariableName = $"{TypeSystem}.OIP.Data.OIP_{index}.SimValue";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                            VariableName = $"{TypeSystem}.OIP.Data.OIP_{index}.CurrLevel";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{TypeSystem}.OIP.Data.OIP_{index}.Status";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{TypeSystem}.OIP.Data.OIP_{index}.UnitsVF";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                        #endregion

                        #region OIP.Sim
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[15].AddressStart);
                        index = 0;
                        foreach (var _Param in Params)
                        {
                            if (string.IsNullOrWhiteSpace(_Param.Signal.Description)) continue;
                            VariableName = $"{TypeSystem}.OIP.Sim.OIP_{++index}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                        #endregion

                        #region OIP.SETPOINTS
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[30].AddressStart);

                        VariableName = $"{TypeSystem}.OIP.SETPOINTS.index_r.request_index";
                        MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                        VariableName = $"{TypeSystem}.OIP.SETPOINTS.index_r.conf_index";
                        MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                        VariableName = $"{TypeSystem}.OIP.SETPOINTS.index_w.request_index";
                        MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                        VariableName = $"{TypeSystem}.OIP.SETPOINTS.index_w.conf_index";
                        MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                        var VariableNamePar = "";
                        for (int i = 0; i < 2; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    VariableNamePar = $"{TypeSystem}.OIP.SETPOINTS.setpoints_w";
                                    break;
                                case 1:
                                    VariableNamePar = $"{TypeSystem}.OIP.SETPOINTS.setpoints_r";
                                    break;
                            }

                            VariableName = $"{VariableNamePar}.link";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{VariableNamePar}.t_max";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            for (int j = 0; j < 6; j++)
                            {
                                VariableName = $"{VariableNamePar}.l_max_{j + 1}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                            }

                            for (int j = 0; j < 6; j++)
                            {
                                VariableName = $"{VariableNamePar}.l_min_{j + 1}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                            }

                            VariableName = $"{VariableNamePar}.t_min";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{VariableNamePar}.ks";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{VariableNamePar}.agrNsigType";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{VariableNamePar}.mask_msg";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{VariableNamePar}.mask_sig";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{VariableNamePar}.allowed_t_min";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{VariableNamePar}.allowed_t_max";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{VariableNamePar}.T01";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{VariableNamePar}.hyst_level";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                            VariableName = $"{VariableNamePar}.max_speed";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                            VariableName = $"{VariableNamePar}.mask_level";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{VariableNamePar}.r_max";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                            VariableName = $"{VariableNamePar}.r_min";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);

                            VariableName = $"{VariableNamePar}.units";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{VariableNamePar}.visual_format";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            for (int j = 0; j < 8; j++)
                            {
                                VariableName = $"{VariableNamePar}.res_{(j + 3):00}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                            }
                        }

                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                        #endregion
                    }
                    #endregion

                    #region Регистры формируемые
                    if (_CheckBox.Content.ToString() == "Регистры формируемые")
                    {
                        var Params = UserDialog.SearchControlViewModel("Регистры формируемые").GetParams<BaseParam>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[26].AddressStart);
                        var index = 0;
                        foreach (var _Param in Params)
                        {
                            if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                            VariableName = $"{TypeSystem}.UserReg.Data.UserReg_{++index}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }
                    #endregion

                    #region Карта готовностей агрегатов (Лист 1)
                    if (_CheckBox.Content.ToString() == "Карта готовностей агрегатов (Лист 1)")
                    {
                        var Params = UserDialog.SearchControlViewModel("Настройки МПНА").GetParams<BaseUMPNA>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[16].AddressStart);
                        foreach (var _Param in Params)
                        {
                            VariableName = $"{TypeSystem}.LIST1.NA_{_Param.Index}.Result";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            var Qty = _Param.KGMPNA.Count / 16;
                            for (int i = 0; i < Qty; i++)
                            {
                                VariableName = $"{TypeSystem}.LIST1.NA_{_Param.Index}.F_{i + 1}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                                VariableName = $"{TypeSystem}.LIST1.NA_{_Param.Index}.M_{i + 1}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                                VariableName = $"{TypeSystem}.LIST1.NA_{_Param.Index}.P_{i + 1}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                            }
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }
                    #endregion

                    #region Общестанционные защиты (Лист 2)
                    if (_CheckBox.Content.ToString() == "Общестанционные защиты (Лист 2)")
                    {
                        var Params = UserDialog.SearchControlViewModel("Общестанционные защиты").GetParams<BaseKTPR>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[17].AddressStart);
                        var index = 0;
                        VariableName = $"{TypeSystem}.LIST2.Data.Result";
                        MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                        foreach (var _Param in Params)
                        {
                            var index_par = (int.Parse(_Param.Param.Index) - 1) % 16;
                            if (index_par != 0) continue;
                            VariableName = $"{TypeSystem}.LIST2.Data.F_{++index}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{TypeSystem}.LIST2.Data.M_{index}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{TypeSystem}.LIST2.Data.P_{index}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }
                    #endregion

                    #region Агрегатные защиты (Лист 3)
                    if (_CheckBox.Content.ToString() == "Агрегатные защиты (Лист 3)")
                    {
                        var Params = UserDialog.SearchControlViewModel("Настройки МПНА").GetParams<BaseUMPNA>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[18].AddressStart);
                        foreach (var _Param in Params)
                        {
                            VariableName = $"{TypeSystem}.LIST3.NA_{_Param.Index}.Result";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            var Qty = _Param.KTPRA.Count / 16;
                            for (int i = 0; i < Qty; i++)
                            {
                                VariableName = $"{TypeSystem}.LIST3.NA_{_Param.Index}.F_{i + 1}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                                VariableName = $"{TypeSystem}.LIST3.NA_{_Param.Index}.M_{i + 1}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                                VariableName = $"{TypeSystem}.LIST3.NA_{_Param.Index}.P_{i + 1}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                            }
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }
                    #endregion

                    #region Предельные параметры (Лист 4)
                    if (_CheckBox.Content.ToString() == "Предельные параметры (Лист 4)")
                    {
                        var Params = UserDialog.SearchControlViewModel("Предельные параметры").GetParams<BaseKTPRS>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[19].AddressStart);
                        var index = 0;
                        VariableName = $"{TypeSystem}.LIST4.Data.Result";
                        MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                        foreach (var _Param in Params)
                        {
                            var index_par = (int.Parse(_Param.Param.Index) - 1) % 16;
                            if (index_par != 0) continue;
                            VariableName = $"{TypeSystem}.LIST4.Data.F_{++index}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);

                            VariableName = $"{TypeSystem}.LIST4.Data.P_{index}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }

                    #endregion

                    #region Лист 5
                    if (_CheckBox.Content.ToString() == "Сигнализация (Лист 5)")
                    {
                        var Params = UserDialog.SearchControlViewModel("Сигнализация").GetParams<BaseSignaling>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[20].AddressStart);
                        var index = 0;
                        foreach (var _Param in Params)
                        {
                            var index_par = (int.Parse(_Param.Param.Index) - 1) % 16;
                            if (index_par != 0) continue;
                            VariableName = $"{TypeSystem}.LIST5.Data.P_{++index}";
                            ListParametrs.Add(new() { { "node-path", VariableName }, { "table", HR }, { "address", $"{MBAddress += 1}" } });
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }

                    #endregion

                    #region Состояние НА
                    if (_CheckBox.Content.ToString() == "Состояние НА")
                    {
                        var Params = UserDialog.SearchControlViewModel("Настройки МПНА").GetParams<BaseUMPNA>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[21].AddressStart);
                        foreach (var _Param in Params)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                VariableName = $"{TypeSystem}.UMPNA.Data.NA_{_Param.Index}.State_{i + 1}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                            }
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }
                    #endregion

                    #region Состояние ЗД
                    if (_CheckBox.Content.ToString() == "Состояние ЗД")
                    {
                        var Params = UserDialog.SearchControlViewModel("Настройки задвижек").GetParams<BaseUZD>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[22].AddressStart);
                        foreach (var _Param in Params)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                VariableName = $"{TypeSystem}.UZD.Data.UZD_{_Param.Index}.State_{i + 1}";
                                MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                            }
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }
                    #endregion

                    #region Состояние ВС
                    if (_CheckBox.Content.ToString() == "Состояние ВС")
                    {
                        var Params = UserDialog.SearchControlViewModel("Настройки вспомсистем").GetParams<BaseUVS>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[23].AddressStart);
                        foreach (var _Param in Params)
                        {
                            VariableName = $"{TypeSystem}.UVS.Data.UVS_{_Param.Index}.State_1";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }
                    #endregion

                    #region Состояние ТС
                    if (_CheckBox.Content.ToString() == "Состояние ТС")
                    {
                        var Params = UserDialog.SearchControlViewModel("DO остальные").GetParams<BaseUTS>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[24].AddressStart);
                        foreach (var _Param in Params)
                        {
                            if (string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                            VariableName = $"{TypeSystem}.UTS.Data.UTS_{_Param.Param.Index}.State_1";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }
                    #endregion

                    #region Карта ручного ввода
                    if (_CheckBox.Content.ToString() == "Карта ручного ввода")
                    {
                        var Params = UserDialog.SearchControlViewModel("Карта ручн. ввода").GetParams<BaseParam>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[25].AddressStart);
                        var qty = Params.Count / 16;
                        for (int i = 0; i < qty; i++)
                        {
                            VariableName = $"{TypeSystem}.HandMap.Data.HandMap_{i + 1}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 1);
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }
                    #endregion

                    #region Команды
                    if (_CheckBox.Content.ToString() == "Команды")
                    {
                        var Params = UserDialog.SearchControlViewModel("Команды").GetParams<BaseParam>();
                        ListParametrs = new();
                        MBAddress = long.Parse(ModbusTCP_HR[13].AddressStart);
                        foreach (var _Param in Params)
                        {
                            if (string.IsNullOrWhiteSpace(_Param.VarName)) continue;
                            VariableName = $"{TypeSystem}.CMD.Cmd.{_Param.VarName}";
                            MBAddress = AddListParametrs(ListParametrs, VariableName, HR, MBAddress, 2);
                        }
                        Elements = new() { { "Binding", "Introduced" } };
                        foreach (var _ListParametr in ListParametrs)
                            CreateParametrsElementNode("item", Elements, _ListParametr);
                    }
                    #endregion
                }

                if (!Result) return false;

                SaveDoc("MB_MAP");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт регистров формируемых - {e}", App.NameApp);
                return false;
            }
        }
        #endregion
    }
}
