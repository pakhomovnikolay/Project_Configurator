using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Export.VU.Interfaces;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using Project_Сonfigurator.ViewModels.UserControls.Signals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace Project_Сonfigurator.Services.Export.VU
{
    public class VUNamespaceASExportRedefineService : IVUNamespaceASExportRedefineService
    {
        private static readonly ILogSerivece Logger = new LogSerivece();
        private const string Namespace = "system";
        private const string ValueAttributeTypeHistory = "Enable=\"True\"; ServerTime=\"False\";";
        private const string TypeAttributeInitialValue = "unit.System.Attributes.InitialValue";
        private const string TypeAttributeHistory = "unit.Server.Attributes.History";
        //private const string TypeAttributeAlarm = "unit.Server.Attributes.Alarm";
        //private const string TypeAttributeIsReadOnly = "unit.Server.Attributes.IsReadOnly";

        private static Dictionary<string, string> Nodes;
        private static Dictionary<string, string> Attributes;
        private static List<Dictionary<string, string>> ListNodes;
        private static string CourceCode = "";

        private static XmlDocument Doc = new();
        private static XmlNode RootNode;
        private static XmlNode SocketNode;
        private static XmlNode SocketTypeNode;
        private static XmlNode SocketTypeSubNode;
        private static XmlNode SocketTypeSubSubNode;
        private static XmlNode SocketParametrNode;
        private static XmlNode AttributeParametrNode;

        #region Создание корневого узла
        /// <summary>
        /// Создание корневого узла
        /// </summary>
        private static void CreateRootNode(string NodeName)
        {
            Doc = new();
            RootNode = Doc.CreateNode(XmlNodeType.Element, NodeName, Namespace);
            XmlAttribute attribute = Doc.CreateAttribute("xmlns:ct");
            attribute.Value = "automation.control";
            RootNode.Attributes.SetNamedItem(attribute);

            attribute = Doc.CreateAttribute("xmlns:r");
            attribute.Value = "automation.reference";
            RootNode.Attributes.SetNamedItem(attribute);

            Doc.AppendChild(RootNode);
        }
        #endregion

        #region Создание корневого узла для сокета
        /// <summary>
        /// Создание корневого узла для сокета
        /// </summary>
        private static void CreateSocketNode(string NodeName, Dictionary<string, string> Nodes = null)
        {
            SocketNode = Doc.CreateNode(XmlNodeType.Element, NodeName, Namespace);
            if (Nodes is not null)
            {
                foreach (var _Nodes in Nodes)
                {
                    XmlAttribute attribute = Doc.CreateAttribute(_Nodes.Key);
                    attribute.Value = _Nodes.Value;
                    SocketNode.Attributes.SetNamedItem(attribute);
                }
                RootNode.AppendChild(SocketNode);
            }
        }
        #endregion

        #region Создание корневого узла типа сокета
        /// <summary>
        /// Создание корневого узла типа сокета
        /// </summary>
        private static void CreateSocketTypeNode(string NodeName, Dictionary<string, string> Nodes = null)
        {
            SocketTypeNode = Doc.CreateNode(XmlNodeType.Element, NodeName, Namespace);
            if (Nodes is not null)
            {
                foreach (var _Node in Nodes)
                {
                    XmlAttribute attribute = Doc.CreateAttribute(_Node.Key);
                    attribute.Value = _Node.Value;
                    SocketTypeNode.Attributes.SetNamedItem(attribute);
                }
                SocketNode.AppendChild(SocketTypeNode);
            }
        }
        #endregion

        #region Создание корневого подузла типа сокета
        /// <summary>
        /// Создание корневого подузла типа сокета
        /// </summary>
        private static void CreateSocketTypeSubNode(string NodeName, Dictionary<string, string> Nodes = null)
        {
            SocketTypeSubNode = Doc.CreateNode(XmlNodeType.Element, NodeName, Namespace);
            if (Nodes is not null)
            {
                foreach (var _Node in Nodes)
                {
                    XmlAttribute attribute = Doc.CreateAttribute(_Node.Key);
                    attribute.Value = _Node.Value;
                    SocketTypeSubNode.Attributes.SetNamedItem(attribute);
                }
                SocketTypeNode.AppendChild(SocketTypeSubNode);
            }
        }
        #endregion

        #region Создание корневого подподузла типа сокета
        /// <summary>
        /// Создание корневого подподузла типа сокета
        /// </summary>
        private static void CreateSocketTypeSubSubNode(string NodeName, Dictionary<string, string> Nodes = null)
        {
            SocketTypeSubSubNode = Doc.CreateNode(XmlNodeType.Element, NodeName, Namespace);
            if (Nodes is not null)
            {
                foreach (var _Node in Nodes)
                {
                    XmlAttribute attribute = Doc.CreateAttribute(_Node.Key);
                    attribute.Value = _Node.Value;
                    SocketTypeSubSubNode.Attributes.SetNamedItem(attribute);
                }
                SocketTypeSubNode.AppendChild(SocketTypeSubSubNode);
            }
        }
        #endregion

        #region Создание параметра для узла сокета 
        /// <summary>
        /// Создание параметра для узла сокета 
        /// </summary>
        private static void CreateSocketParametrNode(string NodeName, string AttributeNodeName = "",
            Dictionary<string, string> Nodes = null, Dictionary<string, string> Attributes = null)
        {
            SocketParametrNode = Doc.CreateNode(XmlNodeType.Element, NodeName, Namespace);
            if (Nodes is not null)
            {
                foreach (var _Node in Nodes)
                {
                    XmlAttribute attribute = Doc.CreateAttribute(_Node.Key);
                    attribute.Value = _Node.Value;
                    SocketParametrNode.Attributes.SetNamedItem(attribute);
                }
                SocketTypeNode.AppendChild(SocketParametrNode);
            }

            if (!string.IsNullOrWhiteSpace(AttributeNodeName))
            {
                AttributeParametrNode = Doc.CreateNode(XmlNodeType.Element, AttributeNodeName, Namespace);
                foreach (var _Attribute in Attributes)
                {
                    XmlAttribute attribute = Doc.CreateAttribute(_Attribute.Key);
                    attribute.Value = _Attribute.Value;
                    AttributeParametrNode.Attributes.SetNamedItem(attribute);
                }
                SocketParametrNode.AppendChild(AttributeParametrNode);
            }
        }
        #endregion

        #region Создание параметра для подузла сокета 
        /// <summary>
        /// Создание параметра для подузла сокета 
        /// </summary>
        private static void CreateSocketParametrSubNode(string NodeName, string AttributeNodeName = "",
            Dictionary<string, string> Nodes = null, Dictionary<string, string> Attributes = null)
        {
            SocketParametrNode = Doc.CreateNode(XmlNodeType.Element, NodeName, Namespace);
            if (Nodes is not null)
            {
                foreach (var _Node in Nodes)
                {
                    XmlAttribute attribute = Doc.CreateAttribute(_Node.Key);
                    attribute.Value = _Node.Value;
                    SocketParametrNode.Attributes.SetNamedItem(attribute);
                }
                SocketTypeSubNode.AppendChild(SocketParametrNode);
            }

            if (!string.IsNullOrWhiteSpace(AttributeNodeName))
            {
                AttributeParametrNode = Doc.CreateNode(XmlNodeType.Element, AttributeNodeName, Namespace);
                foreach (var _Attribute in Attributes)
                {
                    XmlAttribute attribute = Doc.CreateAttribute(_Attribute.Key);
                    attribute.Value = _Attribute.Value;
                    AttributeParametrNode.Attributes.SetNamedItem(attribute);
                }
                SocketParametrNode.AppendChild(AttributeParametrNode);
            }
        }
        #endregion

        #region Создание параметра для подподузла сокета 
        /// <summary>
        /// Создание параметра для подподузла сокета 
        /// </summary>
        private static void CreateSocketParametrSubSubNode(string NodeName, string AttributeNodeName = "",
            Dictionary<string, string> Nodes = null, Dictionary<string, string> Attributes = null)
        {
            SocketParametrNode = Doc.CreateNode(XmlNodeType.Element, NodeName, Namespace);
            if (Nodes is not null)
            {
                foreach (var _Node in Nodes)
                {
                    XmlAttribute attribute = Doc.CreateAttribute(_Node.Key);
                    attribute.Value = _Node.Value;
                    SocketParametrNode.Attributes.SetNamedItem(attribute);
                }
                SocketTypeSubSubNode.AppendChild(SocketParametrNode);
            }

            if (!string.IsNullOrWhiteSpace(AttributeNodeName))
            {
                AttributeParametrNode = Doc.CreateNode(XmlNodeType.Element, AttributeNodeName, Namespace);
                foreach (var _Attribute in Attributes)
                {
                    XmlAttribute attribute = Doc.CreateAttribute(_Attribute.Key);
                    attribute.Value = _Attribute.Value;
                    AttributeParametrNode.Attributes.SetNamedItem(attribute);
                }
                SocketParametrNode.AppendChild(AttributeParametrNode);
            }
        }
        #endregion

        #region Сохранение документа
        /// <summary>
        /// Сохранение документа
        /// </summary>
        private static void SaveDoc(string FileName)
        {
            var Path = string.IsNullOrWhiteSpace(App.Settings.Config.PathExportVU) ? App.Settings.Config.PathProject : App.Settings.Config.PathExportVU;
            Doc.Save($"{Path}{FileName}{App.__SocketsExportFileSuffix}");
        }
        #endregion

        #region Экспорт данных ВУ
        /// <summary>
        /// Экспорт данных ВУ
        /// </summary>
        /// <param name="TypeExport"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public bool Export(string TypeExport)
        {
            if (TypeExport is null) throw new ArgumentNullException(nameof(TypeExport));

            Doc = new();
            XmlDeclaration xmlDeclaration = Doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = Doc.DocumentElement;
            Doc.InsertBefore(xmlDeclaration, root);

            return TypeExport switch
            {
                "Сообщения" => Messages(),
                "Диагностика" => ExportDiagnostics(),
                "Сигналы AI" => ExportSignalsAI(),
                "Регистры формируемые" => ExportUserReg(),

                "Карта готовностей агрегатов (Лист 1)" => ExportKGMPNA(),
                "Общестанционные защиты (Лист 2)" => ExportKTPR(),
                "Агрегатные защиты (Лист 3)" => ExportKTPRA(),
                "Предельные параметры (Лист 4)" => ExportKTPRS(),
                "Лист 5" => ExportLIST5(),

                "Состояние НА" => ExportStateUMPNA(),
                "Состояние ЗД" => ExportStateUZD(),
                "Состояние ВС" => ExportStateUVS(),
                "Состояние ТС" => ExportStateUTS(),

                "Карта ручного ввода" => ExportHandMap(),
                "Команды" => ExportCommands(),

                _ => throw new NotSupportedException($"Экспорт данного типа \"{TypeExport}\" не поддерживается"),
            };
        }
        #endregion

        #region Экспорт сообщений
        /// <summary>
        /// Экспорт сообщений
        /// </summary>
        /// <returns></returns>
        private static bool Messages()
        {
            return false;
        }
        #endregion

        #region Экспорт диагностики
        /// <summary>
        /// Экспорт диагностики
        /// </summary>
        /// <returns></returns>
        private static bool ExportDiagnostics()
        {
            #region Объявление
            ObservableCollection<USO> Params = new();
            ObservableCollection<USO> SubParams = new();
            ObservableCollection<BaseSignaling> SubSubParams = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as LayotRackUserControlViewModel
                                     where _TabItem is LayotRackUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as TableSignalsUserControlViewModel
                                     where _TabItem is TableSignalsUserControlViewModel
                                     select _TabItem)
                SubParams = _TabItem.Params;

            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as SignalingUserControlViewModel
                                     where _TabItem is SignalingUserControlViewModel
                                     select _TabItem)
                SubSubParams = _TabItem.Params;

            var qty_plc = 0;
            foreach (var _Param in Params)
                foreach (var _Rack in _Param.Racks)
                    foreach (var _Module in _Rack.Modules)
                        if (_Module.Type == TypeModule.PLC)
                            qty_plc++;
            #endregion

            try
            {
                #region DiagLink
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "DiagLink" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                {
                    { "name", "COUNTER" },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "direction", "out" },
                    { "type", "uint16" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:parameter", Nodes: Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:sparameter"
                Nodes = new()
                {
                    { "name", "COUNTER_LOCAL" },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "direction", "none" },
                    { "type", "uint16" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:parameter", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:timer"
                Nodes = new()
                {
                    { "name", "Timer" },
                    { "period", "1000" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:timer", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:sparameter"
                Nodes = new()
                {
                    { "name", "COUNTER_STATE" },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "direction", "none" },
                    { "type", "uint16" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:parameter", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:sparameter"
                Nodes = new()
                {
                    { "name", "LinkOk" },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "direction", "out" },
                    { "type", "bool" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:parameter", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:handler"
                CourceCode =
                    "if (COUNTER_LOCAL != $\"_PLC Device\".COUNTER) { COUNTER_STATE = 0; }\n" +
                    "else if (COUNTER_STATE < 5) { COUNTER_STATE += 1; }\n" +
                    "LinkOk = (COUNTER_STATE < 5);\n" +
                    "COUNTER_LOCAL = $\"_PLC Device\".COUNTER;";

                Nodes = new()
                {
                    { "name", "Handler" },
                    { "source-code", $"{CourceCode}" },
                    { "uuid", "0" }
                };
                Attributes = new() { { "on", "Timer" }, { "cause", "update" } };
                CreateSocketParametrNode("ct:handler", "ct:trigger", Nodes: Nodes, Attributes: Attributes);
                #endregion

                SaveDoc("DiagLink");

                #endregion

                #region DiagPLC
                if (qty_plc > 0)
                {
                    // Добавляем корневой узел "omx"
                    CreateRootNode("omx");

                    // Добавляем узел сокета "namespace"
                    Nodes = new() { { "name", "DiagPLC" }, { "uuid", "0" } };
                    CreateSocketNode("namespace", Nodes);

                    #region Struct
                    // Добавляем узел типа сокета "ct:socket-type"
                    Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                    CreateSocketTypeNode("ct:socket-type", Nodes);

                    // Добавляем узел параметра сокета "ct:socket-parameter"
                    ListNodes = new()
                    {
                        new() { { "name", "Status" },               { "type", "uint16" },   { "uuid", "0" } },
                        new() { { "name", "Mode" },                 { "type", "int16" },    { "uuid", "0" } },
                        new() { { "name", "CurrDT" },               { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "Version_1" },            { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "Version_2" },            { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "Version_3" },            { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "Version_4" },            { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "StandardVersion_1" },    { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "StandardVersion_2" },    { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "StandardVersion_3" },    { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "StandardVersion_4" },    { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "BuildTime" },            { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "StandardBuildTime" },    { "type", "uint32" },   { "uuid", "0" } }

                    };
                    foreach (var _Node in ListNodes)
                        CreateSocketParametrNode("ct:socket-parameter", Nodes: _Node);

                    // Добавляем узел типа сокета "ct:socket-type"
                    Nodes = new() { { "name", "Data" }, { "uuid", "0" } };
                    CreateSocketTypeNode("ct:socket-type", Nodes);
                    for (int i = 0; i < qty_plc; i++)
                    {
                        Nodes = new() { { "name", $"PLC_{i + 1}" }, { "type", "Struct" }, { "uuid", "0" } };
                        CreateSocketParametrNode("ct:nested-socket", Nodes: Nodes);
                    }
                    #endregion

                    #region PLC Device
                    // Добавляем узел типа сокета "ct:type"
                    Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                    CreateSocketTypeNode("ct:type", Nodes);

                    // Добавляем узел параметра сокета "ct:socket"
                    Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "Data" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                    #endregion

                    #region IO Server
                    // Добавляем узел типа сокета "ct:type"
                    Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                    CreateSocketTypeNode("ct:type", Nodes);

                    // Добавляем узел параметра сокета "r:ref"
                    Nodes = new()
                    {
                        { "name", "_PLC Device" },
                        { "type", "PLC Device" },
                        { "const-access", "false" },
                        { "aspected", "true" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("r:ref", Nodes: Nodes);

                    // Добавляем узел параметра сокета "ct:socket-bind"
                    Nodes = new()
                    {
                        { "source", "_PLC Device.Data" },
                        { "target", "Data" }
                    };
                    CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                    // Добавляем узел параметра сокета "ct:socket"
                    Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Data" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                    #endregion

                    SaveDoc("DiagPLC");
                }
                #endregion

                #region DiagRack
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "DiagRack" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region StructAnalogs
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "StructAnalogs" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                ListNodes = new()
                {
                    new() { { "name", "ChannelHealth" },    { "type", "uint16" }, { "uuid", "0" } },
                    new() { { "name", "ChannelMask" },      { "type", "uint16" }, { "uuid", "0" } }
                };
                foreach (var _Node in ListNodes)
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: _Node);

                for (int i = 0; i < 8; i++)
                {
                    Nodes = new() { { "name", $"Value_{i + 1}" }, { "type", "uint16" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);
                }
                #endregion

                #region StructDiscrets
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "StructDiscrets" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                Nodes = new() { { "name", "Value" }, { "type", "uint32" }, { "uuid", "0" } };
                CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);
                #endregion

                #region StructRack
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "StructRack" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                ListNodes = new()
                {
                    new() { { "name", "Status" },       { "type", "uint16" }, { "uuid", "0" } },
                    new() { { "name", "ModHealth" },    { "type", "uint32" }, { "uuid", "0" } },
                    new() { { "name", "ErrBusA" },      { "type", "uint16" }, { "uuid", "0" } },
                    new() { { "name", "ErrBusB" },      { "type", "uint16" }, { "uuid", "0" } },
                    new() { { "name", "TotalErrBusA" }, { "type", "uint16" }, { "uuid", "0" } },
                    new() { { "name", "TotalErrBusB" }, { "type", "uint16" }, { "uuid", "0" } }
                };
                foreach (var _Node in ListNodes)
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: _Node);
                #endregion

                #region Данные корзины
                var index_rack = 0;
                foreach (var _Param in Params)
                {
                    foreach (var _Rack in _Param.Racks)
                    {
                        // Добавляем узел типа сокета "ct:socket-type"
                        Nodes = new() { { "name", $"Rack_{++index_rack}" }, { "uuid", "0" } };
                        CreateSocketTypeNode("ct:socket-type", Nodes);

                        // Добавляем узел параметра сокета "ct:nested-socket"
                        Nodes = new() { { "name", "Data" }, { "type", "StructRack" }, { "uuid", "0" } };
                        CreateSocketParametrNode("ct:nested-socket", Nodes: Nodes);

                        // Добавляем узел параметра сокета "ct:socket-parameter"
                        Nodes = new() { { "name", "Desc" }, { "type", "string" }, { "uuid", "0" } };
                        Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", _Rack.Name } };
                        CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                        var ModuleEnabled = 0;
                        foreach (var _Module in _Rack.Modules)
                        {
                            if (string.IsNullOrWhiteSpace(_Module.Name)) continue;
                            var Index = int.Parse(_Module.Index.Replace($"A{_Rack.Index}.", ""));
                            ModuleEnabled += (int)Math.Pow(2, Index - 1);
                            var StructType = _Module.Type == TypeModule.AI || _Module.Type == TypeModule.AO ? "StructAnalogs" : "StructDiscrets";
                            var Desc = $"{_Param.Name} {_Module.Index} {_Module.Name}";

                            // Добавляем узел типа сокета "ct:nested-socket"
                            Nodes = new() { { "name", $"Module_{Index}" }, { "type", StructType }, { "uuid", "0" } };
                            CreateSocketTypeSubNode("ct:nested-socket", Nodes);

                            Nodes = new() { { "name", "Info" }, { "uuid", "0" } };
                            CreateSocketTypeSubSubNode("ct:nested-socket", Nodes);

                            // Добавляем узел параметра сокета "ct:socket-parameter"
                            Nodes = new() { { "name", "Desc" }, { "type", "string" }, { "uuid", "0" } };
                            Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", _Module.Name } };
                            CreateSocketParametrSubSubNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                            // Добавляем узел параметра сокета "ct:socket-parameter"
                            Nodes = new() { { "name", "DescForm" }, { "type", "string" }, { "uuid", "0" } };
                            Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", Desc } };
                            CreateSocketParametrSubSubNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                            // Добавляем узел параметра сокета "ct:socket-parameter"
                            Nodes = new() { { "name", "ModType" }, { "type", "string" }, { "uuid", "0" } };
                            Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", _Module.Type.ToString() } };
                            CreateSocketParametrSubSubNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                            switch (_Module.Type)
                            {
                                case TypeModule.AI:
                                case TypeModule.DI:
                                case TypeModule.AO:
                                case TypeModule.DO:
                                    foreach (var _SubParam in SubParams)
                                    {
                                        if (_SubParam.Name != _Param.Name) continue;
                                        foreach (var _SubRack in _SubParam.Racks)
                                        {
                                            if (_SubRack.Name != _Rack.Name) continue;
                                            foreach (var _SubModule in _SubRack.Modules)
                                            {
                                                TypeModule _Type = _SubModule.Type;
                                                if (!(_Type == TypeModule.AI || _Type == TypeModule.DI || _Type == TypeModule.AO || _Type == TypeModule.DO)) continue;

                                                if (_SubModule.StartAddress != _Module.StartAddress || _SubModule.Type != _Module.Type) continue;
                                                foreach (var _SubChannel in _SubModule.Channels)
                                                {
                                                    // Добавляем узел параметра сокета "ct:socket-parameter"
                                                    var TagChannel = string.IsNullOrWhiteSpace(_SubChannel.Id) ? "-" : _SubChannel.Id;
                                                    Nodes = new() { { "name", $"Tag_{_SubChannel.Index}" }, { "type", "string" }, { "uuid", "0" } };
                                                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", TagChannel } };
                                                    CreateSocketParametrSubSubNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                                                    // Добавляем узел параметра сокета "ct:socket-parameter"
                                                    var DescChannel = string.IsNullOrWhiteSpace(_SubChannel.Description) ? "Резерв" : _SubChannel.Description;
                                                    Nodes = new() { { "name", $"Desc_{_SubChannel.Index}" }, { "type", "string" }, { "uuid", "0" } };
                                                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", DescChannel } };
                                                    CreateSocketParametrSubSubNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        // Добавляем узел параметра сокета "ct:socket-parameter"
                        Nodes = new() { { "name", "ModuleEnabled" }, { "type", "uint16" }, { "uuid", "0" } };
                        Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{ModuleEnabled}" } };
                        CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                    }
                }

                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                var AIIndex = 0;
                var DIIndex = 0;
                var AOIndex = 0;
                var DOIndex = 0;
                index_rack = 0;
                foreach (var _Param in Params)
                {
                    foreach (var _Rack in _Param.Racks)
                    {
                        // Добавляем узел параметра сокета "ct:socket"
                        Nodes = new()
                        {
                            { "name", $"Rack_{++index_rack}" },
                            { "access-level", "public" },
                            { "access-scope", "global" },
                            { "direction", "out" },
                            { "type", "StructRack" },
                            { "uuid", "0" }
                        };
                        CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                        foreach (var _Module in _Rack.Modules)
                        {
                            if (string.IsNullOrWhiteSpace(_Module.Name)) continue;
                            if (_Module.Type != TypeModule.AI && _Module.Type != TypeModule.DI && _Module.Type != TypeModule.AO && _Module.Type != TypeModule.DO) continue;

                            var NameResult = "";
                            var IndexResult = -1;
                            var StructResult = "";

                            switch (_Module.Type)
                            {
                                case TypeModule.AI:
                                    StructResult = "StructAnalogs";
                                    IndexResult = ++AIIndex;
                                    NameResult = $"HW_AI_{IndexResult}";
                                    break;

                                case TypeModule.DI:
                                    StructResult = "StructDiscrets";
                                    IndexResult = ++DIIndex;
                                    NameResult = $"HW_DI_{IndexResult}";
                                    break;

                                case TypeModule.AO:
                                    StructResult = "StructAnalogs";
                                    IndexResult = ++AOIndex;
                                    NameResult = $"HW_AO_{IndexResult}";
                                    break;

                                case TypeModule.DO:
                                    StructResult = "StructDiscrets";
                                    IndexResult = ++DOIndex;
                                    NameResult = $"HW_DO_{IndexResult}";
                                    break;
                            }

                            // Добавляем узел параметра сокета "ct:socket"
                            Nodes = new()
                                    {
                                        { "name", NameResult },
                                        { "access-level", "public" },
                                        { "access-scope", "global" },
                                        { "direction", "out" },
                                        { "type", StructResult },
                                        { "uuid", "0" }
                                    };
                            CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                        }
                    }
                }
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                AIIndex = 0;
                DIIndex = 0;
                AOIndex = 0;
                DOIndex = 0;
                index_rack = 0;
                foreach (var _Param in Params)
                {
                    foreach (var _Rack in _Param.Racks)
                    {
                        // Добавляем узел параметра сокета "ct:socket"
                        Nodes = new()
                        {
                            { "name", $"Rack_{++index_rack}" },
                            { "access-level", "public" },
                            { "access-scope", "global" },
                            { "direction", "none" },
                            { "type", $"Rack_{index_rack}" },
                            { "uuid", "0" }
                        };
                        CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                        // Добавляем узел параметра сокета "ct:socket-bind"
                        Nodes = new()
                        {
                            { "source", $"_PLC Device.Rack_{index_rack}" },
                            { "target", $"Rack_{index_rack}.Data" }
                        };
                        CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                        foreach (var _Module in _Rack.Modules)
                        {
                            if (string.IsNullOrWhiteSpace(_Module.Name)) continue;
                            if (_Module.Type != TypeModule.AI && _Module.Type != TypeModule.DI && _Module.Type != TypeModule.AO && _Module.Type != TypeModule.DO) continue;

                            var NameResult = "";
                            var IndexResult = -1;
                            var StructResult = "";
                            var Index = int.Parse(_Module.Index.Replace($"A{_Rack.Index}.", ""));

                            switch (_Module.Type)
                            {
                                case TypeModule.AI:
                                    StructResult = $"Rack_{index_rack}.Module_{Index}";
                                    IndexResult = ++AIIndex;
                                    NameResult = $"_PLC Device.HW_AI_{IndexResult}";
                                    break;

                                case TypeModule.DI:
                                    StructResult = $"Rack_{index_rack}.Module_{Index}";
                                    IndexResult = ++DIIndex;
                                    NameResult = $"_PLC Device.HW_DI_{IndexResult}";
                                    break;

                                case TypeModule.AO:
                                    StructResult = $"Rack_{index_rack}.Module_{Index}";
                                    IndexResult = ++AOIndex;
                                    NameResult = $"_PLC Device.HW_AO_{IndexResult}";
                                    break;

                                case TypeModule.DO:
                                    StructResult = $"Rack_{index_rack}.Module_{Index}";
                                    IndexResult = ++DOIndex;
                                    NameResult = $"_PLC Device.HW_DO_{IndexResult}";
                                    break;
                            }

                            // Добавляем узел параметра сокета "ct:socket-bind"
                            Nodes = new()
                                    {
                                        { "source", NameResult },
                                        { "target", StructResult }
                                    };
                            CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);
                        }
                    }
                }
                #endregion

                SaveDoc("DiagRack");

                #endregion

                #region DiagUSO
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "DiagUSO" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region StructDiag
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "StructDiag" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                ListNodes = new()
                    {
                        new() { { "name", "Red_1" },            { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "Red_2" },            { "type", "uint32" },    { "uuid", "0" } },
                        new() { { "name", "RedBlink_1" },       { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "RedBlink_2" },       { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "Yellow_1" },         { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "Yellow_2" },         { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "YellowBlink_1" },    { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "YellowBlink_2" },    { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "Common" },           { "type", "uint16" },   { "uuid", "0" } }

                    };
                foreach (var _Node in ListNodes)
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: _Node);
                #endregion

                #region StructDiag
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "StructServiceSignals" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                ListNodes = new()
                    {
                        new() { { "name", "ServiceSignal_1" },  { "type", "uint32" },   { "uuid", "0" } },
                        new() { { "name", "ServiceSignal_2" },  { "type", "uint32" },   { "uuid", "0" } }
                    };
                foreach (var _Node in ListNodes)
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: _Node);
                #endregion

                #region Данные УСО
                foreach (var _Param in Params)
                {
                    var qty_signaling = 0;

                    // Добавляем узел типа сокета "ct:socket-type"
                    Nodes = new() { { "name", $"USO_{_Param.Index}" }, { "uuid", "0" } };
                    CreateSocketTypeNode("ct:socket-type", Nodes);

                    // Добавляем узел типа сокета "ct:nested-socket"
                    Nodes = new() { { "name", "ServiceSignals" }, { "type", "StructServiceSignals" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:nested-socket", Nodes: Nodes);

                    // Добавляем узел типа сокета "ct:socket-parameter"
                    Nodes = new() { { "name", "Desc" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", _Param.Name } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    foreach (var _SubSubParam in SubSubParams)
                    {
                        if (string.IsNullOrWhiteSpace(_SubSubParam.IndexUSO)) continue;
                        if (_SubSubParam.IndexUSO == _Param.Index)
                        {
                            qty_signaling++;

                            // Добавляем узел типа сокета "ct:socket-parameter"
                            Nodes = new() { { "name", $"Tag_{qty_signaling}" }, { "type", "string" }, { "uuid", "0" } };
                            Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", _SubSubParam.Param.Id } };
                            CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                            // Добавляем узел типа сокета "ct:socket-parameter"
                            Nodes = new() { { "name", $"Desc_{qty_signaling}" }, { "type", "string" }, { "uuid", "0" } };
                            Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", _SubSubParam.Param.Description } };
                            CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                            // Добавляем узел типа сокета "ct:socket-parameter"
                            Nodes = new() { { "name", $"Severity_{qty_signaling}" }, { "type", "string" }, { "uuid", "0" } };
                            Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", _SubSubParam.Color } };
                            CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                        }
                    }
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "StructDiag" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                foreach (var _Param in Params)
                {
                    // Добавляем узел параметра сокета "ct:socket"
                    Nodes = new()
                    {
                        { "name", $"USO_{_Param.Index}" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "StructServiceSignals" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                }
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                    {
                        { "name", "_PLC Device" },
                        { "type", "PLC Device" },
                        { "const-access", "false" },
                        { "aspected", "true" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket-bind"
                Nodes = new()
                    {
                        { "source", "_PLC Device.Data" },
                        { "target", "Data" }
                    };
                CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "StructDiag" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                foreach (var _Param in Params)
                {
                    // Добавляем узел параметра сокета "ct:socket-bind"
                    Nodes = new()
                    {
                        { "source", $"_PLC Device.USO_{_Param.Index}" },
                        { "target", $"USO_{_Param.Index}.ServiceSignals" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                    // Добавляем узел параметра сокета "ct:socket-bind"
                    Nodes = new()
                    {
                        { "name", $"USO_{_Param.Index}" },
                        { "target", $"USO_{_Param.Index}.ServiceSignals" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", $"USO_{_Param.Index}" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                }

                #endregion

                SaveDoc("DiagUSO");

                #endregion

                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт регистров формируемых - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт аналоговых сигналов (AI)
        /// <summary>
        /// Экспорт аналоговых сигналов (AI)
        /// </summary>
        /// <returns></returns>
        private static bool ExportSignalsAI()
        {
            #region Объявление
            ObservableCollection<SignalAI> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as SignalsAIUserControlViewModel
                                     where _TabItem is SignalsAIUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            ObservableCollection<USO> ParParams = new();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as LayotRackUserControlViewModel
                                     where _TabItem is LayotRackUserControlViewModel
                                     select _TabItem)
                ParParams = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "OIP" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter" с атрибутом - "attributeTypeHistory"
                Nodes = new() { { "name", "Value" }, { "type", "float32" }, { "uuid", "0" } };
                Attributes = new() { { "type", TypeAttributeHistory }, { "value", ValueAttributeTypeHistory } };
                CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                ListNodes = new()
                {
                    new (){ { "name", "ElValue" },      { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "VisualValue" },  { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "SimValue" },     { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "CurrLevel" },    { "type", "int16" },    { "uuid", "0" } },
                    new (){ { "name", "Status" },       { "type", "uint16" },   { "uuid", "0" } },
                    new (){ { "name", "UnitsVF" },      { "type", "uint16" },   { "uuid", "0" } }
                };
                foreach (var _Nodes in ListNodes)
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: _Nodes);
                #endregion

                #region setpoints_index_struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "setpoints_index_struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                ListNodes = new()
                {
                    new (){ { "name", "request_index" },    { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "conf_index" },       { "type", "uint16" },  { "uuid", "0" } }
                };
                foreach (var _Nodes in ListNodes)
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: _Nodes);
                #endregion

                #region oip_setpoints_struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "oip_setpoints_struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                ListNodes = new()
                {
                    new (){ { "name", "link" },             { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "t_max" },            { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "l_max_1" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "l_max_2" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "l_max_3" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "l_max_4" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "l_max_5" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "l_max_6" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "l_min_1" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "l_min_2" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "l_min_3" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "l_min_4" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "l_min_5" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "l_min_6" },          { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "t_min" },            { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "ks" },               { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "agrNsigType" },      { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "mask_msg" },         { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "mask_sig" },         { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "allowed_t_min" },    { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "allowed_t_max" },    { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "T01" },              { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "hyst_level" },       { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "max_speed" },        { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "mask_level" },       { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "r_max" },            { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "r_min" },            { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "units" },            { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "visual_format" },    { "type", "uint16" },  { "uuid", "0" } },

                    new (){ { "name", "res_03" },           { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "res_04" },           { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "res_05" },           { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "res_06" },           { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "res_07" },           { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "res_08" },           { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "res_09" },           { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "res_10" },           { "type", "uint16" },  { "uuid", "0" } },

                };
                foreach (var _Nodes in ListNodes)
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: _Nodes);
                #endregion

                #region Setpoints
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "SETPOINTS" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                ListNodes = new()
                {
                    new (){ { "name", "index_r" },      { "type", "setpoints_index_struct" },  { "uuid", "0" } },
                    new (){ { "name", "index_w" },      { "type", "setpoints_index_struct" },  { "uuid", "0" } },
                    new (){ { "name", "setpoints_w" },  { "type", "oip_setpoints_struct" },  { "uuid", "0" } },
                    new (){ { "name", "setpoints_r" },  { "type", "oip_setpoints_struct" },  { "uuid", "0" } }
                };
                foreach (var _Nodes in ListNodes)
                    CreateSocketParametrNode("ct:nested-socket", Nodes: _Nodes);
                #endregion

                #region Info
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Info" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                foreach (var _Param in Params)
                {
                    if (string.IsNullOrWhiteSpace(_Param.Signal.Description)) continue;

                    Nodes = new() { { "name", $"OIP_Tag_{_Param.Signal.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", _Param.Signal.Id } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    Nodes = new() { { "name", $"OIP_Desc_{_Param.Signal.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", _Param.Signal.Description } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    int.TryParse(_Param.Signal.Address, out int Address);
                    int.TryParse(_Param.Signal.Area, out int Area);

                    if (Area > 0)
                    {
                        Nodes = new() { { "name", $"Index_RackMod_{_Param.Signal.Index}" }, { "type", "uint16" }, { "uuid", "0" } };
                        Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", "-1" } };
                        CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                        Nodes = new() { { "name", $"Index_Module_{_Param.Signal.Index}" }, { "type", "uint16" }, { "uuid", "0" } };
                        Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", "-1" } };
                        CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                    }
                    else
                    {
                        foreach (var _ParParam in ParParams)
                        {
                            foreach (var _Rack in _ParParam.Racks)
                            {
                                foreach (var _Module in _Rack.Modules)
                                {
                                    if (_Module.Type == TypeModule.AI)
                                    {
                                        int.TryParse(_Module.StartAddress, out int StartAddress);
                                        int.TryParse(_Module.EndAddress, out int EndAddress);

                                        if (Address >= StartAddress && Address <= EndAddress)
                                        {
                                            Nodes = new() { { "name", $"Index_Rack_{_Param.Signal.Index}" }, { "type", "uint16" }, { "uuid", "0" } };
                                            Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{Address / 16 / 8 + 1}" } };
                                            CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                                            var Index = _Module.Index.Replace($"A{_Rack.Index}.", "");
                                            Nodes = new() { { "name", $"Index_Module_{_Param.Signal.Index}" }, { "type", "uint16" }, { "uuid", "0" } };
                                            Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{Index}" } };
                                            CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Data
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Data" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                foreach (var _Param in Params)
                {
                    if (string.IsNullOrWhiteSpace(_Param.Signal.Description)) continue;

                    Nodes = new() { { "name", $"OIP_{_Param.Signal.Index}" }, { "type", "Struct" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:nested-socket", Nodes: Nodes);
                }
                #endregion

                #region Sim
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Sim" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                foreach (var _Param in Params)
                {
                    if (string.IsNullOrWhiteSpace(_Param.Signal.Description)) continue;

                    Nodes = new() { { "name", $"OIP_{_Param.Signal.Index}" }, { "type", "float32" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                ListNodes = new()
                {
                    new (){ { "name", "Data" }, { "access-level", "public" }, { "access-scope", "global" }, { "direction", "out" }, { "type", "Data" }, { "uuid", "0" } },
                    new (){ { "name", "Sim" }, { "access-level", "public" }, { "access-scope", "global" }, { "direction", "In" }, { "type", "Sim" }, { "uuid", "0" } },
                    new (){ { "name", "SETPOINTS" }, { "access-level", "public" }, { "access-scope", "global" }, { "direction", "in-out" }, { "type", "SETPOINTS" }, { "uuid", "0" } }
                };
                foreach (var _Nodes in ListNodes)
                    CreateSocketParametrNode("ct:socket", Nodes: _Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.IOS" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                Nodes = new() { { "name", "_PLC Device" }, { "type", "PLC Device" }, { "const-access", "false" }, { "aspected", "true" }, { "uuid", "0" } };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                ListNodes = new()
                {
                    new (){ { "source", "_PLC Device.Data" }, { "target", "Data" } },
                    new (){ { "source", "Sim" }, { "target", "_PLC Device.Sim" } },
                    new (){ { "source", "_PLC Device.SETPOINTS" }, { "target", "SETPOINTS" } },
                    new (){ { "source", "SETPOINTS" }, { "target", "_PLC Device.SETPOINTS" } }
                };
                foreach (var _Nodes in ListNodes)
                    CreateSocketParametrNode("ct:socket-bind", Nodes: _Nodes);

                ListNodes = new()
                {
                    new (){ { "name", "Data" }, { "access-level", "public" }, { "access-scope", "global" }, { "direction", "out" }, { "type", "Data" }, { "uuid", "0" } },
                    new (){ { "name", "Sim" }, { "access-level", "public" }, { "access-scope", "global" }, { "direction", "in" }, { "type", "Sim" }, { "uuid", "0" } },
                    new (){ { "name", "SETPOINTS" }, { "access-level", "public" }, { "access-scope", "global" }, { "direction", "in-out" }, { "type", "SETPOINTS" }, { "uuid", "0" } },
                    new (){ { "name", "Info" }, { "access-level", "public" }, { "access-scope", "global" }, { "direction", "none" }, { "type", "Info" }, { "uuid", "0" } }
                };
                foreach (var _Nodes in ListNodes)
                    CreateSocketParametrNode("ct:socket", Nodes: _Nodes);
                #endregion

                SaveDoc("SignalsAI");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт аналоговых сигналов (AI) - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт регистров формируемых
        /// <summary>
        /// Экспорт регистров формируемых
        /// </summary>
        /// <returns></returns>
        private static bool ExportUserReg()
        {
            #region Объявление
            ObservableCollection<BaseParam> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UserRegUserControlViewModel
                                     where _TabItem is UserRegUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "UserReg" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                foreach (var _Param in Params)
                {
                    Nodes = new() { { "name", $"UserReg_{_Param.Index}" }, { "type", "uint16" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                {
                    { "name", "Data" },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "direction", "out" },
                    { "type", "Struct" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket-bind"
                Nodes = new()
                {
                    { "source", "_PLC Device.Data" },
                    { "target", "Data" }
                };
                CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                {
                    { "name", "Data" },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "direction", "none" },
                    { "type", "Struct" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                SaveDoc("UserReg");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт регистров формируемых - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт карты готовностей МПНА
        /// <summary>
        /// Экспорт карты готовностей МПНА
        /// </summary>
        /// <returns></returns>
        private static bool ExportKGMPNA()
        {
            #region Объявление
            ObservableCollection<BaseUMPNA> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UMPNAUserControlViewModel
                                     where _TabItem is UMPNAUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "LIST1" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                Nodes = new() { { "name", "Result" }, { "type", "uint16" }, { "uuid", "0" } };
                CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);

                var qty = App.Settings.Config.DefualtMapKGMPNA.Count / 16;
                foreach (var _Param in Params)
                    if (_Param.KGMPNA.Count > qty)
                        qty = _Param.KGMPNA.Count;

                for (int i = 0; i < qty; i++)
                {
                    ListNodes = new()
                    {
                        new (){ { "name", $"F_{i + 1}" },      { "type", "uint16" },   { "uuid", "0" } },
                        new (){ { "name", $"M_{i + 1}" },      { "type", "uint16" },   { "uuid", "0" } },
                        new (){ { "name", $"P_{i + 1}" },      { "type", "uint16" },   { "uuid", "0" } }
                    };
                    foreach (var _Nodes in ListNodes)
                        CreateSocketParametrNode("ct:socket-parameter", Nodes: _Nodes);
                }
                #endregion

                #region Info
                foreach (var _Param in Params)
                {
                    Nodes = new() { { "name", $"Info_{_Param.Index}" }, { "uuid", "0" } };
                    CreateSocketTypeNode("ct:socket-type", Nodes);

                    Nodes = new() { { "name", "Desc" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Description}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    var i = 1;
                    foreach (var _SubParam in _Param.KGMPNA)
                    {
                        if (string.IsNullOrWhiteSpace(_SubParam.Param.Description)) continue;
                        Nodes = new() { { "name", $"Desc_{_SubParam.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                        Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_SubParam.Param.Description}" } };
                        CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                        Nodes = new() { { "name", $"Index_{_SubParam.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                        Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{i++}" } };
                        CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                    }
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                foreach (var _Param in Params)
                {
                    // Добавляем узел параметра сокета "ct:socket"
                    Nodes = new()
                    {
                        { "name", $"NA_{_Param.Index}" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "Struct" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                }
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                foreach (var _Param in Params)
                {
                    // Добавляем узел параметра сокета "ct:socket-bind"
                    Nodes = new()
                    {
                        { "source", $"_PLC Device.NA_{_Param.Index}" },
                        { "target", $"NA_{_Param.Index}" }
                    };
                    CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                    // Добавляем узел параметра сокета "ct:socket"
                    Nodes = new()
                    {
                        { "name", $"NA_{_Param.Index}" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Struct" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                    // Добавляем узел параметра сокета "ct:socket"
                    Nodes = new()
                    {
                        { "name", $"Info_{_Param.Index}" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", $"Info_{_Param.Index}" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                }
                #endregion

                SaveDoc("LIST1");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт карты готовностей МПНА - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт карты общестанционных защит
        /// <summary>
        /// Экспорт карты общестанционных защит
        /// </summary>
        /// <returns></returns>
        private static bool ExportKTPR()
        {
            #region Объявление
            ObservableCollection<BaseKTPR> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as KTPRUserControlViewModel
                                     where _TabItem is KTPRUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "LIST2" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                Nodes = new() { { "name", "Result" }, { "type", "uint16" }, { "uuid", "0" } };
                CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);

                var qty = Params.Count / 16;
                for (int i = 0; i < qty; i++)
                {
                    ListNodes = new()
                    {
                        new (){ { "name", $"F_{i + 1}" },      { "type", "uint16" },   { "uuid", "0" } },
                        new (){ { "name", $"M_{i + 1}" },      { "type", "uint16" },   { "uuid", "0" } },
                        new (){ { "name", $"P_{i + 1}" },      { "type", "uint16" },   { "uuid", "0" } }
                    };
                    foreach (var _Nodes in ListNodes)
                        CreateSocketParametrNode("ct:socket-parameter", Nodes: _Nodes);
                }
                #endregion

                #region Info
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Info" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                var j = 1;
                foreach (var _Param in Params)
                {
                    if (string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                    Nodes = new() { { "name", $"Desc_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Param.Description}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    Nodes = new() { { "name", $"Index_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{j++}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "Struct" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket-bind"
                Nodes = new()
                    {
                        { "source", $"_PLC Device.Data" },
                        { "target", $"Data" }
                    };
                CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Struct" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Info" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Info" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                SaveDoc("LIST2");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт карты общестанционных защит - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт карты агрегатных защит
        /// <summary>
        /// Экспорт карты агрегатных защит
        /// </summary>
        /// <returns></returns>
        private static bool ExportKTPRA()
        {
            #region Объявление
            ObservableCollection<BaseUMPNA> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UMPNAUserControlViewModel
                                     where _TabItem is UMPNAUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "LIST3" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                Nodes = new() { { "name", "Result" }, { "type", "uint16" }, { "uuid", "0" } };
                CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);

                var qty = App.Settings.Config.DefualtMapKTPRA.Count / 16;
                foreach (var _Param in Params)
                    if (_Param.KTPRA.Count > qty)
                        qty = _Param.KTPRA.Count;

                for (int i = 0; i < qty; i++)
                {
                    ListNodes = new()
                    {
                        new (){ { "name", $"F_{i + 1}" },      { "type", "uint16" },   { "uuid", "0" } },
                        new (){ { "name", $"M_{i + 1}" },      { "type", "uint16" },   { "uuid", "0" } },
                        new (){ { "name", $"P_{i + 1}" },      { "type", "uint16" },   { "uuid", "0" } }
                    };
                    foreach (var _Nodes in ListNodes)
                        CreateSocketParametrNode("ct:socket-parameter", Nodes: _Nodes);
                }
                #endregion

                #region Info
                foreach (var _Param in Params)
                {
                    Nodes = new() { { "name", $"Info_{_Param.Index}" }, { "uuid", "0" } };
                    CreateSocketTypeNode("ct:socket-type", Nodes);

                    Nodes = new() { { "name", "Desc" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Description}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    var i = 1;
                    foreach (var _SubParam in _Param.KTPRA)
                    {
                        if (string.IsNullOrWhiteSpace(_SubParam.Param.Description)) continue;
                        Nodes = new() { { "name", $"Desc_{_SubParam.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                        Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_SubParam.Param.Description}" } };
                        CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                        Nodes = new() { { "name", $"Index_{_SubParam.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                        Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{i++}" } };
                        CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                    }
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                foreach (var _Param in Params)
                {
                    // Добавляем узел параметра сокета "ct:socket"
                    Nodes = new()
                    {
                        { "name", $"NA_{_Param.Index}" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "Struct" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                }
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                foreach (var _Param in Params)
                {
                    // Добавляем узел параметра сокета "ct:socket-bind"
                    Nodes = new()
                    {
                        { "source", $"_PLC Device.NA_{_Param.Index}" },
                        { "target", $"NA_{_Param.Index}" }
                    };
                    CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                    // Добавляем узел параметра сокета "ct:socket"
                    Nodes = new()
                    {
                        { "name", $"NA_{_Param.Index}" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Struct" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                    // Добавляем узел параметра сокета "ct:socket"
                    Nodes = new()
                    {
                        { "name", $"Info_{_Param.Index}" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", $"Info_{_Param.Index}" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                }
                #endregion

                SaveDoc("LIST3");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт карты агрегатных защит - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт карты предельных параметров общестанционных защит
        /// <summary>
        /// Экспорт карты предельных параметров общестанционных защит
        /// </summary>
        /// <returns></returns>
        private static bool ExportKTPRS()
        {
            #region Объявление
            ObservableCollection<BaseKTPRS> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as KTPRSUserControlViewModel
                                     where _TabItem is KTPRSUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "LIST4" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                Nodes = new() { { "name", "Result" }, { "type", "uint16" }, { "uuid", "0" } };
                CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);

                var qty = Params.Count / 16;
                for (int i = 0; i < qty; i++)
                {
                    ListNodes = new()
                    {
                        new (){ { "name", $"F_{i + 1}" },      { "type", "uint16" },   { "uuid", "0" } },
                        new (){ { "name", $"P_{i + 1}" },      { "type", "uint16" },   { "uuid", "0" } }
                    };
                    foreach (var _Nodes in ListNodes)
                        CreateSocketParametrNode("ct:socket-parameter", Nodes: _Nodes);
                }
                #endregion

                #region Info
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Info" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                var j = 1;
                foreach (var _Param in Params)
                {
                    if (string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                    Nodes = new() { { "name", $"Desc_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Param.Description}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    Nodes = new() { { "name", $"Index_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{j++}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "Struct" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket-bind"
                Nodes = new()
                    {
                        { "source", $"_PLC Device.Data" },
                        { "target", $"Data" }
                    };
                CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Struct" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Info" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Info" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                SaveDoc("LIST4");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт карты предельных параметров общестанционных защит - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт карты общесистемной сигнализации
        /// <summary>
        /// Экспорт карты общесистемной сигнализации
        /// </summary>
        /// <returns></returns>
        private static bool ExportLIST5()
        {
            #region Объявление
            ObservableCollection<BaseSignaling> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as SignalingUserControlViewModel
                                     where _TabItem is SignalingUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "LIST5" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                var qty = Params.Count / 16;
                for (int i = 0; i < qty; i++)
                {
                    Nodes = new() { { "name", $"P_{i + 1}" }, { "type", "uint16" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);
                }
                #endregion

                #region Info
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Info" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                var j = 1;
                foreach (var _Param in Params)
                {
                    if (string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
                    Nodes = new() { { "name", $"Desc_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Param.Description}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    Nodes = new() { { "name", $"Tag_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Param.Id}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    Nodes = new() { { "name", $"Color_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Color}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    Nodes = new() { { "name", $"Index_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{j++}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                }

                Nodes = new() { { "name", "CountButton" }, { "type", "uint16" }, { "uuid", "0" } };
                Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{j / 16}" } };
                CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "Struct" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket-bind"
                Nodes = new()
                    {
                        { "source", $"_PLC Device.Data" },
                        { "target", $"Data" }
                    };
                CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Struct" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Info" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Info" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:parameter"
                Nodes = new()
                    {
                        { "name", "RefreshData" },
                        { "target", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "bool" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:parameter", Nodes: Nodes);

                qty = Params.Count / 16;
                for (int i = 0; i < qty; i++)
                {
                    Nodes = new() { { "name", $"Handler_{i + 1}" }, { "source-code", "RefreshData = true;" }, { "uuid", "0" } };
                    Attributes = new() { { "on", $"Data.P_{i + 1}" }, { "cause", "update" } };
                    CreateSocketParametrNode("ct:handler", "ct:trigger", Nodes: Nodes, Attributes: Attributes);
                }

                #endregion

                SaveDoc("LIST5");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт карты общесистемной сигнализации - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт данных МПНА
        /// <summary>
        /// Экспорт данных МПНА
        /// </summary>
        /// <returns></returns>
        private static bool ExportStateUMPNA()
        {
            #region Объявление
            ObservableCollection<BaseUMPNA> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UMPNAUserControlViewModel
                                     where _TabItem is UMPNAUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "UMPNA" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                ListNodes = new()
                {
                    new (){ { "name", "State_1" },      { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "State_2" },      { "type", "uint16" },  { "uuid", "0" } }
                };
                foreach (var _Nodes in ListNodes)
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: _Nodes);
                #endregion

                #region Data
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Data" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                foreach (var _Param in Params)
                {
                    // Добавляем узел типа сокета "ct:nested-socket"
                    Nodes = new() { { "name", $"NA_{_Param.Index}" }, { "type", "Struct" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:nested-socket", Nodes: Nodes);
                }
                #endregion

                #region Info
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Info" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                foreach (var _Param in Params)
                {
                    Nodes = new() { { "name", $"NA_{_Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Description}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "Data" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket-bind"
                Nodes = new()
                    {
                        { "source", $"_PLC Device.Data" },
                        { "target", $"Data" }
                    };
                CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Data" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Info" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Info" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                SaveDoc("UMPNA");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт данных МПНА - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт данных ЗД
        /// <summary>
        /// Экспорт данных ЗД
        /// </summary>
        /// <returns></returns>
        private static bool ExportStateUZD()
        {
            #region Объявление
            ObservableCollection<BaseUZD> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UZDUserControlViewModel
                                     where _TabItem is UZDUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "UZD" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                ListNodes = new()
                {
                    new (){ { "name", "State_1" },      { "type", "uint16" },  { "uuid", "0" } },
                    new (){ { "name", "State_2" },      { "type", "uint16" },  { "uuid", "0" } }
                };
                foreach (var _Nodes in ListNodes)
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: _Nodes);
                #endregion

                #region Data
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Data" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                foreach (var _Param in Params)
                {
                    // Добавляем узел типа сокета "ct:nested-socket"
                    Nodes = new() { { "name", $"UZD_{_Param.Index}" }, { "type", "Struct" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:nested-socket", Nodes: Nodes);
                }
                #endregion

                #region Info
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Info" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                foreach (var _Param in Params)
                {
                    Nodes = new() { { "name", $"UZD_{_Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Description}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "Data" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket-bind"
                Nodes = new()
                    {
                        { "source", $"_PLC Device.Data" },
                        { "target", $"Data" }
                    };
                CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Data" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Info" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Info" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                SaveDoc("UZD");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт данных ЗД - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт данных ВС
        /// <summary>
        /// Экспорт данных ВС
        /// </summary>
        /// <returns></returns>
        private static bool ExportStateUVS()
        {
            #region Объявление
            ObservableCollection<BaseUVS> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UVSUserControlViewModel
                                     where _TabItem is UVSUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "UVS" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                Nodes = new() { { "name", "State_1" }, { "type", "uint32" }, { "uuid", "0" } };
                CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);
                #endregion

                #region Data
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Data" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                foreach (var _Param in Params)
                {
                    // Добавляем узел типа сокета "ct:nested-socket"
                    Nodes = new() { { "name", $"UVS_{_Param.Index}" }, { "type", "Struct" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:nested-socket", Nodes: Nodes);
                }
                #endregion

                #region Info
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Info" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                foreach (var _Param in Params)
                {
                    Nodes = new() { { "name", $"UVS_{_Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Description}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "Data" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket-bind"
                Nodes = new()
                    {
                        { "source", $"_PLC Device.Data" },
                        { "target", $"Data" }
                    };
                CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Data" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Info" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Info" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                SaveDoc("UVS");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт данных ВС - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт данных ТС
        /// <summary>
        /// Экспорт данных ТС
        /// </summary>
        /// <returns></returns>
        private static bool ExportStateUTS()
        {
            #region Объявление
            ObservableCollection<BaseUTS> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UTSUserControlViewModel
                                     where _TabItem is UTSUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "UTS" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                Nodes = new() { { "name", "State_1" }, { "type", "uint16" }, { "uuid", "0" } };
                CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);
                #endregion

                #region Data
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Data" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                foreach (var _Param in Params)
                {
                    // Добавляем узел типа сокета "ct:nested-socket"
                    Nodes = new() { { "name", $"UTS_{_Param.Param.Index}" }, { "type", "Struct" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:nested-socket", Nodes: Nodes);
                }
                #endregion

                #region Info
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Info" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                foreach (var _Param in Params)
                {
                    Nodes = new() { { "name", $"UTS_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Param.Description}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "Data" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket-bind"
                Nodes = new()
                    {
                        { "source", $"_PLC Device.Data" },
                        { "target", $"Data" }
                    };
                CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Data" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Data" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                    {
                        { "name", "Info" },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "none" },
                        { "type", "Info" },
                        { "uuid", "0" }
                    };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                SaveDoc("UTS");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт данных ТС - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт данных карты ручного ввода
        /// <summary>
        /// Экспорт данных карты ручного ввода
        /// </summary>
        /// <returns></returns>
        private static bool ExportHandMap()
        {
            #region Объявление
            ObservableCollection<BaseParam> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as HandMapUserControlViewModel
                                     where _TabItem is HandMapUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "HandMap" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                var qty = Params.Count / 16;
                for (int i = 0; i < qty; i++)
                {
                    Nodes = new() { { "name", $"HandMap_{i}" }, { "type", "uint16" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);
                }
                #endregion

                #region Info
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Info" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                foreach (var _Param in Params)
                {
                    if (!string.IsNullOrWhiteSpace(_Param.Description))
                    {
                        Nodes = new() { { "name", $"HandMap_{_Param.Index}" }, { "type", "string" } };
                        Attributes = new() { { TypeAttributeInitialValue, _Param.Description } };
                        CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);
                    }
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                {
                    { "name", "Data" },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "direction", "out" },
                    { "type", "Struct" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket-bind"
                Nodes = new()
                {
                    { "source", "_PLC Device.Data" },
                    { "target", "Data" }
                };
                CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                {
                    { "name", "Data" },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "direction", "out" },
                    { "type", "Struct" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                {
                    { "name", "Info" },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "direction", "none" },
                    { "type", "Info" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                SaveDoc("HandMap");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт данных карты ручного ввода - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт команд
        /// <summary>
        /// Экспорт команд
        /// </summary>
        /// <returns></returns>
        private static bool ExportCommands()
        {
            #region Объявление
            ObservableCollection<BaseParam> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as CommandUserControlViewModel
                                     where _TabItem is CommandUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "CMD" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region Struct
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "Struct" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter"
                foreach (var _Param in Params)
                {
                    if (string.IsNullOrWhiteSpace(_Param.VarName)) continue;
                    Nodes = new() { { "name", _Param.VarName }, { "type", "uint32" }, { "uuid", "0" } };
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: Nodes);
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                {
                    { "name", "Cmd" },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "direction", "in" },
                    { "type", "Struct" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.PLC" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_PLC Device" },
                    { "type", "PLC Device" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket-bind"
                Nodes = new()
                {
                    { "source", "Cmd" },
                    { "target", "_PLC Device.Cmd" }
                };
                CreateSocketParametrNode("ct:socket-bind", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                Nodes = new()
                {
                    { "name", "Cmd" },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "direction", "in" },
                    { "type", "Struct" },
                    { "uuid", "0" }
                };
                CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                #endregion

                SaveDoc("CMD");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт данных карты ручного ввода - {e}", App.NameApp);
                return false;
            }
        }
        #endregion
    }
}
