using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Export.VU.Interfaces;
using Project_Сonfigurator.ViewModels;
using Project_Сonfigurator.ViewModels.UserControls;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using Project_Сonfigurator.ViewModels.UserControls.Signals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace Project_Сonfigurator.Services.Export.VU
{
    public class VUNamespaceASExportRedefineService : BaseService, IVUNamespaceASExportRedefineService
    {
        private const string Namespace = "system";
        private const string ValueAttributeTypeHistory = "Enable=\"True\"; ServerTime=\"False\";";
        private const string TypeAttributeInitialValue = "unit.System.Attributes.InitialValue";
        private const string TypeAttributeHistory = "unit.Server.Attributes.History";
        private const string TypeAttributeComment = "unit.System.Attributes.Comment";
        private const string TypeAttributeAlarm = "unit.Server.Attributes.Alarm";
        //private const string TypeAttributeIsReadOnly = "unit.Server.Attributes.IsReadOnly";

        private static Dictionary<string, string> Nodes;
        private static Dictionary<string, string> Attributes;
        private static List<Dictionary<string, string>> ListNodes;
        private static List<Dictionary<string, string>> ListAttributes;
        private static string SourceCodeHandler = "";
        private static string ExceptionSystemName = "";

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

        #region Создание параметра для узла сокета с атрибутами для сообщений
        /// <summary>
        /// Создание параметра для узла сокета с атрибутами для сообщений
        /// </summary>
        private static void CreateSocketParametrNodeWithAttributeMessage(string NodeName, string AttributeNodeName = "",
            Dictionary<string, string> Nodes = null, List<Dictionary<string, string>> Attributes = null)
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

                foreach (var _Attribute in Attributes)
                {
                    AttributeParametrNode = Doc.CreateNode(XmlNodeType.Element, AttributeNodeName, Namespace);
                    foreach (var item in _Attribute)
                    {
                        XmlAttribute attribute = Doc.CreateAttribute(item.Key);
                        attribute.Value = item.Value;
                        AttributeParametrNode.Attributes.SetNamedItem(attribute);
                    }
                    SocketParametrNode.AppendChild(AttributeParametrNode);
                }
            }
        }
        #endregion

        #region Преобразование цвета сообщения в приоритет сообщения
        /// <summary>
        /// Преобразование цвета сообщения в приоритет сообщения
        /// </summary>
        private static string ConverMessageSeverity(string Severity)
        {
            switch (Severity)
            {
                case "зеленый":
                    return "200";
                case "желтый":
                    return "500";
                case "красный":
                    return "800";
                default:
                    return "1";

            }
        }
        #endregion

        #region Создание условий выдачи сообщений
        /// <summary>
        /// Создание условий выдачи сообщений
        /// </summary>
        private static string CreateConditionMessage(string AckStrategy, string IsEnabled, string Message, string Severity, string Sound, string Value)
        {
            _ = int.TryParse(AckStrategy, out int _AckStrategy);
            return $"{{\"AckStrategy\":{_AckStrategy}," +
                $"\"IsEnabled\":{IsEnabled}," +
                $"\"Message\":{FormatingMessage(Message)}," +
                $"\"Severity\":{ConverMessageSeverity(Severity.ToLower())}," +
                $"\"Sound\":{FormatingPathSound(Sound)}," +
                $"\"Value\":{Value}}},";
        }
        #endregion

        #region Финальное создание условий выдачи сообщений
        /// <summary>
        /// Финальное создание условий выдачи сообщений
        /// </summary>
        private static string FinalyConditionMessage(string ConditionValue)
        {
            return $"{{\"Condition\":{{\"IsEnabled\":\"true\",\"Subconditions\":[{ConditionValue.TrimEnd(',')}]}}}}";
        }
        #endregion

        #region Форматирование текста сообщения
        /// <summary>
        /// Форматирование текста сообщения
        /// </summary>
        private static string FormatingMessage(string Message, bool WriteDotAtEnd = false)
        {
            var Dot = WriteDotAtEnd ? ". " : "";
            var MessageResult = $"\"{Message.Trim().ToUpper().Replace("\\", "\\\\").Replace("\"", "\\\"")}{Dot}\"";
            return MessageResult;
        }
        #endregion

        #region Форматирование пути воспроизведения звукового оповещения
        /// <summary>
        /// Форматирование пути воспроизведения звукового оповещения
        /// </summary>
        private static string FormatingPathSound(string PathSound)
        {
            var PathSoundResult = "\"\"";
            if (!string.IsNullOrWhiteSpace(PathSound.Trim()))
                PathSoundResult = $"\"{PathSound.Replace("\\", "\\\\")}\"";

            return PathSoundResult;
        }
        #endregion

        #region Форматирование пути воспроизведения звукового оповещения
        /// <summary>
        /// Форматирование пути воспроизведения звукового оповещения
        /// </summary>
        private static bool EqualTextList(string Equal, List<string> ListEqual)
        {
            var Result = false;
            foreach (var _List in ListEqual)
                if (Equal.Contains(_List, StringComparison.CurrentCultureIgnoreCase))
                    Result = true;

            return Result;
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
                "Сообщения" => Messages(
                    UserDialog.SearchControlViewModel("Сообщения").GetParams<BaseSystemMessage>(),
                    App.Services.GetRequiredService<MessageWindowViewModel>().GetParams<CollectionMessage>()),

                "Диагностика" => ExportDiagnostics(
                    UserDialog.SearchControlViewModel("Компоновка корзин").GetParams<USO>(),
                    UserDialog.SearchControlViewModel("Таблица сигналов").GetParams<USO>(),
                    UserDialog.SearchControlViewModel("Сигнализация").GetParams<BaseSignaling>()),

                "Сигналы AI" => ExportSignalsAI(
                    UserDialog.SearchControlViewModel("Сигналы AI").GetParams<SignalAI>(),
                    UserDialog.SearchControlViewModel("Компоновка корзин").GetParams<USO>()),

                "Регистры формируемые" => ExportUserReg(UserDialog.SearchControlViewModel("Регистры формируемые").GetParams<BaseParam>()),

                "Карта готовностей агрегатов (Лист 1)" => ExportKGMPNA(UserDialog.SearchControlViewModel("Настройки МПНА").GetParams<BaseUMPNA>()),
                "Общестанционные защиты (Лист 2)" => ExportKTPR(UserDialog.SearchControlViewModel("Общестанционные защиты").GetParams<BaseKTPR>()),
                "Агрегатные защиты (Лист 3)" => ExportKTPRA(UserDialog.SearchControlViewModel("Настройки МПНА").GetParams<BaseUMPNA>()),
                "Предельные параметры (Лист 4)" => ExportKTPRS(UserDialog.SearchControlViewModel("Передельные параметры").GetParams<BaseKTPRS>()),
                "Сигнализация (Лист 5)" => ExportLIST5(UserDialog.SearchControlViewModel("Сигнализация").GetParams<BaseSignaling>()),

                "Состояние НА" => ExportStateUMPNA(UserDialog.SearchControlViewModel("Настрйоки МПНА").GetParams<BaseUMPNA>()),
                "Состояние ЗД" => ExportStateUZD(UserDialog.SearchControlViewModel("Настрйоки задвижек").GetParams<BaseUZD>()),
                "Состояние ВС" => ExportStateUVS(UserDialog.SearchControlViewModel("Настрйоки вспомсистем").GetParams<BaseUVS>()),
                "Состояние ТС" => ExportStateUTS(UserDialog.SearchControlViewModel("DO остальные").GetParams<BaseUTS>()),

                "Карта ручного ввода" => ExportHandMap(UserDialog.SearchControlViewModel("Карта ручн. ввода").GetParams<BaseParam>()),
                "Команды" => ExportCommands(UserDialog.SearchControlViewModel("Команды").GetParams<BaseParam>()),

                _ => throw new NotSupportedException($"Экспорт данного типа \"{TypeExport}\" не поддерживается"),
            };
        }
        #endregion

        #region Экспорт сообщений
        /// <summary>
        /// Экспорт сообщений
        /// </summary>
        /// <returns></returns>
        private bool Messages(ObservableCollection<BaseSystemMessage> Params, ObservableCollection<CollectionMessage> SubParams)
        {
            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "Messages" }, { "uuid", "0" } };
                CreateSocketNode("namespace", Nodes);

                #region StructPLC
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "StructPLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter" с атрибутом - "TypeAttributeComment"
                ListAttributes = new()
                {
                    new() { { "type", TypeAttributeComment }, { "value", "Текущее значение" } },
                    new() { { "type", TypeAttributeComment }, { "value", "Предыдущее значение" } },
                    new() { { "type", TypeAttributeComment }, { "value", "Код сообщения - Система, подсистема, код" } },
                    new() { { "type", TypeAttributeComment }, { "value", "Время и дата сообщения - год, месяц, день" } },
                    new() { { "type", TypeAttributeComment }, { "value", "Время и дата сообщения - Часы, минуты, секунды, милисекунды" } }
                };
                ListNodes = new()
                {
                    new (){ { "name", "Val_1" },    { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "Val_2" },    { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "MsgCode" },  { "type", "uint32" },   { "uuid", "0" } },
                    new (){ { "name", "YMD" },      { "type", "uint32" },   { "uuid", "0" } },
                    new (){ { "name", "HMSmS" },    { "type", "uint32" },   { "uuid", "0" } }
                };
                for (int i = 0; i < ListNodes.Count; i++)
                {
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: ListNodes[i], Attributes: ListAttributes[i]);
                }
                #endregion

                #region StructIOS
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "StructIOS" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                // Добавляем узел параметра сокета "ct:socket-parameter" с атрибутом - "TypeAttributeComment"
                ListNodes = new()
                {
                    new (){ { "name", "Val_1" },        { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "Val_2" },        { "type", "float32" },  { "uuid", "0" } },
                    new (){ { "name", "System" },       { "type", "uint16" },   { "uuid", "0" } },
                    new (){ { "name", "SubSystem" },    { "type", "uint16" },   { "uuid", "0" } },
                    new (){ { "name", "NaIndex" },      { "type", "uint16" },   { "uuid", "0" } },
                    new (){ { "name", "Code" },         { "type", "uint16" },   { "uuid", "0" } }
                };
                foreach (var _Nodes in ListNodes)
                    CreateSocketParametrNode("ct:socket-parameter", Nodes: _Nodes);
                #endregion

                #region StructSystem
                // Добавляем узел типа сокета "ct:socket-type"
                Nodes = new() { { "name", "StructSystem" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:socket-type", Nodes);

                ExceptionSystemName = "";
                foreach (var _SubParam in SubParams)
                {
                    SourceCodeHandler = "";
                    foreach (var _Message in _SubParam.Messages)
                    {
                        if (string.IsNullOrWhiteSpace(_Message.Description)) continue;
                        SourceCodeHandler += CreateConditionMessage(_Message.NeedAck, "true", _Message.Description, _Message.Color, _Message.PathSound, _Message.Index);

                    }

                    if (string.IsNullOrWhiteSpace(SourceCodeHandler)) { ExceptionSystemName += _SubParam.NameSystem; continue; }
                    Nodes = new() { { "name", _SubParam.NameSystem }, { "type", "uint16" }, { "uuid", "0" } };
                    ListAttributes = new()
                    {
                        new() { { "type", TypeAttributeHistory },   { "value", ValueAttributeTypeHistory } },
                        new() { { "type", TypeAttributeAlarm },     { "value", FinalyConditionMessage(SourceCodeHandler) } }
                    };
                    CreateSocketParametrNodeWithAttributeMessage("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: ListAttributes);
                }
                #endregion

                #region PLC Device
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "PLC Device" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                for (int i = 0; i < App.Settings.Config.BufferSize; i++)
                {
                    var Name = $"Msg_{i + 1}";
                    Nodes = new()
                    {
                        { "name", Name },
                        { "access-level", "public" },
                        { "access-scope", "global" },
                        { "direction", "out" },
                        { "type", "StructPLC" },
                        { "uuid", "0" }
                    };
                    CreateSocketParametrNode("ct:socket", Nodes: Nodes);
                }
                #endregion

                #region IO Server
                // Добавляем узел типа сокета "ct:type"
                Nodes = new() { { "name", "IO Server" }, { "aspect", "Aspects.IOS" }, { "original", "PLC Device" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:type", Nodes);

                // Добавляем узел параметра сокета "r:ref"
                Nodes = new() { { "name", "_PLC Device" }, { "type", "PLC Device" }, { "const-access", "false" }, { "aspected", "true" }, { "uuid", "0" } };
                CreateSocketParametrNode("r:ref", Nodes: Nodes);

                // Добавляем узел параметра сокета "ct:socket"
                ListNodes = new()
                {
                    new()
                    {
                        { "name", "Msg" }, { "access-level", "public" }, { "access-scope", "global" },
                        { "direction", "none" }, { "type", "StructIOS" }, { "uuid", "0" }
                    },
                    new()
                    {
                        { "name", "System" }, { "access-level", "public" }, { "access-scope", "global" },
                        { "direction", "none" }, { "type", "StructSystem" }, { "uuid", "0" }
                    }
                };
                foreach (var _Nodes in ListNodes)
                    CreateSocketParametrNode("ct:socket", Nodes: _Nodes);

                // Добавляем узел параметра сокета "socket-parameter"
                SourceCodeHandler = $"{{\"Condition\":{{\"IsEnabled\":\"true\",\"Subconditions\":[{{\"AckStrategy\":1,\"IsEnabled\":true,\"Message\":\"Dynamic\",\"Severity\":1,\"Type\":1}}],\"Type\":1}}}}";
                Nodes = new()
                {
                    { "name", "VUMessage" }, { "access-level", "public" }, { "access-scope", "global" },
                    { "direction", "none" }, { "type", "string" }, { "uuid", "0" }
                };
                ListAttributes = new()
                {
                    new() { { "type", TypeAttributeHistory },   { "value", ValueAttributeTypeHistory } },
                    new() { { "type", TypeAttributeAlarm },     { "value", SourceCodeHandler } }
                };
                CreateSocketParametrNodeWithAttributeMessage("ct:parameter", "attribute", Nodes: Nodes, Attributes: ListAttributes);

                // Добавляем узел параметра сокета "ct:handler"
                for (int i = 0; i < App.Settings.Config.BufferSize; i++)
                {
                    #region Код обработчика события изменения кода сообщения
                    SourceCodeHandler =
                        $"// Получаем метку времени\n" +
                        $"Year:  uint2       = TypeConvert.ToUint2($\"_PLC Device\".Msg_{i + 1}.YMD >> 16);\n" +
                        $"Month: uint1       = TypeConvert.ToUint1(($\"_PLC Device\".Msg_{i + 1}.YMD >> 8) & 255);\n" +
                        $"Day:   uint1       = TypeConvert.ToUint1($\"_PLC Device\".Msg_{i + 1}.YMD & 255);\n" +
                        $"Hour:  uint1       = TypeConvert.ToUint1(($\"_PLC Device\".Msg_{i + 1}.HMSmS >> 28) & 15);\n" +
                        $"Min:   uint1       = TypeConvert.ToUint1(($\"_PLC Device\".Msg_{i + 1}.HMSmS >> 22) & 63);\n" +
                        $"Sec:   uint1       = TypeConvert.ToUint1(($\"_PLC Device\".Msg_{i + 1}.HMSmS >> 16) & 63);\n" +
                        $"mSec:  uint2       = TypeConvert.ToUint1($\"_PLC Device\".Msg_{i + 1}.HMSmS & 1023);\n\r" +

                        $"// Разбираем структуру сообщения\n" +
                        $"Msg.Val_1          = $\"_PLC Device\".Msg_{i + 1}.Val_1;\n" +
                        $"Msg.Val_2          = $\"_PLC Device\".Msg_{i + 1}.Val_2;\n" +
                        $"Msg.System         = TypeConvert.ToUint2($\"_PLC Device\".Msg_{i + 1}.MsgCode >> 24);\n" +
                        $"Msg.SubSystem      = TypeConvert.ToUint2(($\"_PLC Device\".Msg_{i + 1}.MsgCode >> 12) & 4095);\n" +
                        $"Msg.NaIndex        = TypeConvert.ToUint2(($\"_PLC Device\".Msg_{i + 1}.MsgCode & 3968) >> 7);\n" +
                        $"Msg.Code           = TypeConvert.ToUint2($\"_PLC Device\".Msg_{i + 1}.MsgCode & 4095);\n" +
                        $"Msg.Code.Timestamp = DateTime.Create(Year, Month, Day, Hour, Min, Sec, mSec);"
                        ;
                    #endregion

                    var Name = $"ParseMsg_{i + 1}";
                    Nodes = new()
                    {
                        { "name", Name },
                        { "source-code", SourceCodeHandler },
                        { "uuid", "0" }
                    };
                    Attributes = new() { { "on", $"_PLC Device.Msg_{i + 1}.HMSmS" }, { "cause", "change" } };
                    CreateSocketParametrNode("ct:handler", "ct:trigger", Nodes: Nodes, Attributes: Attributes);
                }

                // Добавляем узел параметра сокета "ct:handler"
                var Operator = "";
                SourceCodeHandler = "";
                foreach (var _Param in Params)
                {
                    if (string.IsNullOrWhiteSpace(_Param.SystemMessage)) continue;
                    if (ExceptionSystemName.Contains(_Param.SystemMessage, StringComparison.CurrentCultureIgnoreCase)) continue;
                    Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                    SourceCodeHandler += $"{Operator} (Msg.System == {_Param.Index}) {{System.{_Param.SystemMessage} = Msg.Code;}}\n";
                }
                Nodes = new()
                {
                    { "name", "SortingSystem" },
                    { "source-code", SourceCodeHandler },
                    { "uuid", "0" }
                };
                Attributes = new() { { "on", "Msg.Code" }, { "cause", "update" } };
                CreateSocketParametrNode("ct:handler", "ct:trigger", Nodes: Nodes, Attributes: Attributes);

                if (Params is not null && Params.Count > 0)
                {
                    foreach (var _Param in Params)
                    {
                        if (string.IsNullOrWhiteSpace(_Param.SystemMessage)) continue;
                        if (ExceptionSystemName.Contains(_Param.SystemMessage, StringComparison.CurrentCultureIgnoreCase)) continue;

                        SourceCodeHandler = "";
                        Operator = "";
                        var _System = _Param.SystemMessage;
                        var _SubSystem = _Param.NameTabList;
                        var _DescriptionSystem = _Param.DescriptionSystem;
                        var _HandlerName = $"PrepareMsg_{_System}";
                        var Message = $"System.{_System}.Messages.Selected.Message";
                        var Severity = $"System.{_System}.Messages.Selected.Severity";

                        #region Код обработчика события изменения кода сообщения
                        SourceCodeHandler = $"{Message} = String.Replace({Message}, \"$\", String.ToString(Msg.Val_1));\n";
                        SourceCodeHandler += $"{Message} = String.Replace({Message}, \"&\", String.ToString(Msg.Val_2));\n";
                        SourceCodeHandler += $"msg: string = \"\";\n\r";
                        if (_System == "LIST5") SourceCodeHandler += $"Severity: uint4 = 0;\n";

                        if (_DescriptionSystem == _SubSystem)
                        {
                            SourceCodeHandler += $"{Message} = String.Concat(msg, {Message});";
                            Nodes = new()
                            {
                                { "name", _HandlerName },
                                { "source-code", SourceCodeHandler },
                                { "uuid", "0" }
                            };
                            Attributes = new() { { "on", $"System.{_System}" }, { "cause", "message-prepare" } };
                            CreateSocketParametrNode("ct:handler", "ct:trigger", Nodes: Nodes, Attributes: Attributes);
                            continue;
                        }

                        #region Сигналы AI
                        if (_SubSystem == "Сигналы AI")
                        {
                            if (UserDialog.SearchControlViewModel(_SubSystem) is not SignalsAIUserControlViewModel _ViewModel) continue;
                            foreach (var item in _ViewModel.Params)
                            {
                                if (string.IsNullOrWhiteSpace(item.Signal.Description)) continue;
                                var Description = FormatingMessage(item.Signal.Description, true);
                                var Index = item.Signal.Index;
                                Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                SourceCodeHandler += $"{Operator} (Msg.SubSystem == {Index}) {{ msg = {Description}; }}\n";
                            }
                        }
                        #endregion

                        #region Настройки МПНА
                        if (_SubSystem == "Настройки МПНА")
                        {
                            if (UserDialog.SearchControlViewModel(_SubSystem) is not UMPNAUserControlViewModel _ViewModel) continue;

                            #region Данные
                            var ListEqual = new List<string>()
                                {
                                    "UMPNA", "KTPRAS_1", "KTPRAS_2", "CMNA", "NARAB"
                                };
                            if (EqualTextList(_System, ListEqual))
                            {
                                foreach (var item in _ViewModel.Params)
                                {
                                    var Description = FormatingMessage(item.Description, true);
                                    _ = int.TryParse(item.Index, out int Index);
                                    Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                    SourceCodeHandler += $"{Operator} (Msg.SubSystem == {Index}) {{ msg = {Description}; }}\n";
                                }
                            }
                            #endregion

                            #region Параметры
                            else
                            {
                                ListEqual = new List<string>()
                                {
                                    "KGMPNA", "KTPRA", "KTPRAS"
                                };
                                if (EqualTextList(_System, ListEqual))
                                {
                                    foreach (var item in _ViewModel.Params)
                                    {
                                        var SubOperator = "";
                                        _ = int.TryParse(item.Index, out int Index);
                                        Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                        SourceCodeHandler += $"{Operator} (Msg.NaIndex == {Index - 1})\n{{\n";
                                        foreach (var __Param in item.KGMPNA)
                                        {
                                            if (string.IsNullOrWhiteSpace(__Param.Param.Description)) continue;
                                            var SubIndex = __Param.Param.Index;
                                            var Description = FormatingMessage($"{__Param.Param.Description} {item.Description}", true);
                                            SubOperator = string.IsNullOrWhiteSpace(SubOperator) ? "if" : "else if";
                                            SourceCodeHandler += $"\t{SubOperator} (Msg.SubSystem == {SubIndex}) {{ msg = {Description}; }}\n";
                                        }
                                        SourceCodeHandler += $"}}\n";
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region Общестанционные защиты
                        if (_SubSystem == "Общестанционные защиты")
                        {
                            if (UserDialog.SearchControlViewModel(_SubSystem) is not KTPRUserControlViewModel _ViewModel) continue;
                            foreach (var item in _ViewModel.Params)
                            {
                                if (string.IsNullOrWhiteSpace(item.Param.Description)) continue;
                                var Description = FormatingMessage(item.Param.Description, true);
                                var Index = item.Param.Index;
                                Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                SourceCodeHandler += $"{Operator} (Msg.SubSystem == {Index}) {{ msg = {Description}; }}\n";
                            }
                        }
                        #endregion

                        #region Предельные параметры
                        if (_SubSystem == "Предельные параметры")
                        {
                            if (UserDialog.SearchControlViewModel(_SubSystem) is not KTPRSUserControlViewModel _ViewModel) continue;
                            foreach (var item in _ViewModel.Params)
                            {
                                if (string.IsNullOrWhiteSpace(item.Param.Description)) continue;
                                var Description = FormatingMessage(item.Param.Description, true);
                                var Index = item.Param.Index;
                                Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                SourceCodeHandler += $"{Operator} (Msg.SubSystem == {Index}) {{ msg = {Description}; }}\n";
                            }
                        }
                        #endregion

                        #region Сигнализация
                        if (_SubSystem == "Сигнализация")
                        {
                            if (UserDialog.SearchControlViewModel(_SubSystem) is not SignalingUserControlViewModel _ViewModel) continue;
                            foreach (var item in _ViewModel.Params)
                            {
                                if (string.IsNullOrWhiteSpace(item.Param.Description)) continue;
                                var Description = FormatingMessage(item.Param.Description, true);
                                var Index = item.Param.Index;
                                Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                SourceCodeHandler += $"{Operator} (Msg.SubSystem == {Index}) {{ msg = {Description}; }}\n";
                            }
                        }
                        #endregion

                        #region Настройки задвижек
                        if (_SubSystem == "Настройки задвижек")
                        {
                            if (UserDialog.SearchControlViewModel(_SubSystem) is not UZDUserControlViewModel _ViewModel) continue;
                            foreach (var item in _ViewModel.Params)
                            {
                                var Description = FormatingMessage(item.Description, true);
                                var Index = item.Index;
                                Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                SourceCodeHandler += $"{Operator} (Msg.SubSystem == {Index}) {{ msg = {Description}; }}\n";
                            }
                        }
                        #endregion

                        #region Настройки вспомсистем
                        if (_SubSystem == "Настройки вспомсистем" && _Param.SystemMessage == "UVSGRP")
                        {
                            if (UserDialog.SearchControlViewModel(_SubSystem) is not UVSUserControlViewModel _ViewModel) continue;
                            foreach (var item in _ViewModel.Params)
                            {
                                var Description = FormatingMessage(item.Description, true);
                                var Index = item.Index;
                                Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                SourceCodeHandler += $"{Operator} (Msg.SubSystem == {Index}) {{ msg = {Description}; }}\n";
                            }
                        }
                        #endregion

                        #region DO остальные
                        if (_SubSystem == "DO остальные")
                        {
                            if (UserDialog.SearchControlViewModel(_SubSystem) is not UTSUserControlViewModel _ViewModel) continue;
                            foreach (var item in _ViewModel.Params)
                            {
                                if (string.IsNullOrWhiteSpace(item.Param.Description)) continue;
                                var Description = FormatingMessage(item.Param.Description, true);
                                var Index = item.Param.Index;
                                Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                SourceCodeHandler += $"{Operator} (Msg.SubSystem == {Index}) {{ msg = {Description}; }}\n";
                            }
                        }
                        #endregion

                        #region Карта ручн. ввода
                        if (_SubSystem == "Карта ручн. ввода")
                        {
                            if (UserDialog.SearchControlViewModel(_SubSystem) is not HandMapUserControlViewModel _ViewModel) continue;
                            foreach (var item in _ViewModel.Params)
                            {
                                if (string.IsNullOrWhiteSpace(item.Description)) continue;
                                var Description = FormatingMessage(item.Description, true);
                                var Index = item.Index;
                                Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                SourceCodeHandler += $"{Operator} (Msg.SubSystem == {Index}) {{ msg = {Description}; }}\n";
                            }
                        }
                        #endregion

                        #region Компоновка корзин
                        if (_SubSystem == "Компоновка корзин")
                        {
                            if (UserDialog.SearchControlViewModel(_SubSystem) is not LayotRackUserControlViewModel _ViewModel) continue;

                            #region Диагностика ПЛК
                            if (_System == "DiagPLC")
                            {
                                if (App.Settings.Config.PLC_List is not null && App.Settings.Config.PLC_List.Count > 0)
                                {
                                    var PLC_List = App.Settings.Config.PLC_List;
                                    var Index = 0;
                                    foreach (var _PLC in PLC_List)
                                    {
                                        Index++;
                                        var Description = FormatingMessage(_PLC.Text, true);
                                        Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                        SourceCodeHandler += $"{Operator} (Msg.SubSystem == {Index}) {{ msg = {Description}; }}\n";
                                    }
                                }
                            }
                            #endregion

                            else
                            {
                                #region Диагностика корзин
                                if (_System == "DiagRack" || _System == "DiagLink")
                                {
                                    if (_System == "DiagLink") SourceCodeHandler += $"if (Msg.Code > 9)\n{{\n";

                                    var Index = 0;
                                    foreach (var item in _ViewModel.Params)
                                    {
                                        foreach (var _Rack in item.Racks)
                                        {
                                            Index++;
                                            var Description = FormatingMessage($"{item.Name} корзина {_Rack.Name}", true);
                                            Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                            SourceCodeHandler += $"{Operator} (Msg.SubSystem == {Index}) {{ msg = {Description}; }}\n";
                                        }
                                    }
                                    if (_System == "DiagLink") SourceCodeHandler += $"}}\n";
                                }
                                #endregion


                                #region Диагностика модулей
                                else if (_System == "DiagModule")
                                {
                                    var Index = 0;
                                    foreach (var item in _ViewModel.Params)
                                    {
                                        foreach (var _Rack in item.Racks)
                                        {
                                            Index++;
                                            foreach (var _Module in _Rack.Modules)
                                            {
                                                if (string.IsNullOrWhiteSpace(_Module.Name)) continue;
                                                _ = int.TryParse(_Module.Index.Replace($"{_Rack.Name}.", ""), out int IndexModule);
                                                var SubSystem = (Index - 1) * 16 + IndexModule;
                                                var Description = FormatingMessage($"{item.Name} модуль {_Module.Index} {_Module.Name}", true);
                                                Operator = string.IsNullOrWhiteSpace(Operator) ? "if " : "else if ";
                                                SourceCodeHandler += $"{Operator} (Msg.SubSystem == {SubSystem}) {{ msg = {Description}; }}\n";
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion

                        if (_System == "LIST5") { SourceCodeHandler += $"if (Msg.Code == 1) {{ {Severity} = Severity; }}\n"; }
                        SourceCodeHandler += $"{Message} = String.Concat(msg, {Message});";
                        #endregion

                        Nodes = new()
                        {
                            { "name", _HandlerName },
                            { "source-code", SourceCodeHandler },
                            { "uuid", "0" }
                        };
                        Attributes = new() { { "on", $"System.{_System}" }, { "cause", "message-prepare" } };
                        CreateSocketParametrNode("ct:handler", "ct:trigger", Nodes: Nodes, Attributes: Attributes);
                    }
                }
                #endregion

                SaveDoc("Messages");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog($"Экспорт сообщений - {e}", App.NameApp);
                return false;
            }


        }
        #endregion

        #region Экспорт диагностики
        /// <summary>
        /// Экспорт диагностики
        /// </summary>
        /// <returns></returns>
        private bool ExportDiagnostics(ObservableCollection<USO> Params, ObservableCollection<USO> SubParams, ObservableCollection<BaseSignaling> SubSubParams)
        {
            #region Объявление
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
                SourceCodeHandler =
                    "if (COUNTER_LOCAL != $\"_PLC Device\".COUNTER) { COUNTER_STATE = 0; }\n" +
                    "else if (COUNTER_STATE < 5) { COUNTER_STATE += 1; }\n" +
                    "LinkOk = (COUNTER_STATE < 5);\n" +
                    "COUNTER_LOCAL = $\"_PLC Device\".COUNTER;";

                Nodes = new()
                {
                    { "name", "Handler" },
                    { "source-code", $"{SourceCodeHandler}" },
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
                    Nodes = new() { { "name", $"Value_{i + 1}" }, { "type", "float32" }, { "uuid", "0" } };
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
                        if (!_Rack.IsEnable) { index_rack++; continue; }
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
                            var Desc = $"{_Param.Name} {_Module.Index} {_Module.Name.Replace(_Module.Index, "")}";

                            // Добавляем узел типа сокета "ct:nested-socket"
                            Nodes = new() { { "name", $"Module_{Index}" }, { "type", StructType }, { "uuid", "0" } };
                            CreateSocketTypeSubNode("ct:nested-socket", Nodes);

                            Nodes = new() { { "name", "Info" }, { "uuid", "0" } };
                            CreateSocketTypeSubSubNode("ct:nested-socket", Nodes);

                            // Добавляем узел параметра сокета "ct:socket-parameter"
                            Nodes = new() { { "name", "Desc" }, { "type", "string" }, { "uuid", "0" } };
                            Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Module.Index} {_Module.Name.Replace(_Module.Index, "")}" } };
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
                        if (!_Rack.IsEnable) { index_rack++; continue; }
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
                        if (!_Rack.IsEnable) { index_rack++; continue; }
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

                    // Добавляем узел типа сокета "ct:socket-parameter"
                    Nodes = new() { { "name", $"CountButton" }, { "type", "uint16" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{(qty_signaling - 1) / 32 + 1}"  } };
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
                Logger.WriteLog($"Экспорт диагностики - {e}", App.NameApp);
                return false;
            }
        }
        #endregion

        #region Экспорт аналоговых сигналов (AI)
        /// <summary>
        /// Экспорт аналоговых сигналов (AI)
        /// </summary>
        /// <returns></returns>
        private bool ExportSignalsAI(ObservableCollection<SignalAI> Params, ObservableCollection<USO> ParParams)
        {
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
        private bool ExportUserReg(ObservableCollection<BaseParam> Params)
        {
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
                    if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
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
        private bool ExportKGMPNA(ObservableCollection<BaseUMPNA> Params)
        {
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
                    if (_Param.KGMPNA.Count / 16 > qty)
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
        private bool ExportKTPR(ObservableCollection<BaseKTPR> Params)
        {
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
        private bool ExportKTPRA(ObservableCollection<BaseUMPNA> Params)
        {
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
                    if (_Param.KTPRA.Count / 16 > qty)
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
        private bool ExportKTPRS(ObservableCollection<BaseKTPRS> Params)
        {
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
        private bool ExportLIST5(ObservableCollection<BaseSignaling> Params)
        {
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
                    var Color = string.IsNullOrWhiteSpace(_Param.Color) ? "\"\"" : _Param.Color;
                    Nodes = new() { { "name", $"Desc_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Param.Description}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    Nodes = new() { { "name", $"Tag_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{_Param.Param.Id}" } };
                    CreateSocketParametrNode("ct:socket-parameter", "attribute", Nodes: Nodes, Attributes: Attributes);

                    Nodes = new() { { "name", $"Color_{_Param.Param.Index}" }, { "type", "string" }, { "uuid", "0" } };
                    Attributes = new() { { "type", TypeAttributeInitialValue }, { "value", $"{Color}" } };
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
        private bool ExportStateUMPNA(ObservableCollection<BaseUMPNA> Params)
        {
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
        private bool ExportStateUZD(ObservableCollection<BaseUZD> Params)
        {
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
        private bool ExportStateUVS(ObservableCollection<BaseUVS> Params)
        {
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
        private bool ExportStateUTS(ObservableCollection<BaseUTS> Params)
        {
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
                    if (string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
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
                    if (string.IsNullOrWhiteSpace(_Param.Param.Description)) continue;
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
        private bool ExportHandMap(ObservableCollection<BaseParam> Params)
        {
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
                    Nodes = new() { { "name", $"HandMap_{i + 1}" }, { "type", "uint16" }, { "uuid", "0" } };
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
                    if (string.IsNullOrWhiteSpace(_Param.Description)) continue;
                    Nodes = new() { { "name", $"HandMap_{_Param.Index}" }, { "type", "string" } };
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
        private bool ExportCommands(ObservableCollection<BaseParam> Params)
        {
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
                Logger.WriteLog($"Экспорт команд - {e}", App.NameApp);
                return false;
            }
        }
        #endregion
    }
}
