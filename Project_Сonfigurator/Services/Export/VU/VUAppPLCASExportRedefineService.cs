using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Export.VU.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Xml;

namespace Project_Сonfigurator.Services.Export.VU
{
    public class VUAppPLCASExportRedefineService : BaseService, IVUAppPLCASExportRedefineService
    {
        private const string Namespace = "system";

        private static Dictionary<string, string> Nodes;
        private static List<Dictionary<string, string>> ListNodes;

        private static XmlDocument Doc = new();
        private static XmlNode RootNode;
        private static XmlNode SocketNode;
        private static XmlNode SocketTypeNode;
        private static XmlNode SocketParametrNode;
        private static XmlNode AttributeParametrNode;

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

            RootNode = Doc.CreateNode(XmlNodeType.Element, NodeName, Namespace);
            XmlAttribute attribute = Doc.CreateAttribute("xmlns:a");
            attribute.Value = "automation";
            RootNode.Attributes.SetNamedItem(attribute);

            attribute = Doc.CreateAttribute("xmlns:ct");
            attribute.Value = "automation.control";
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
        /// <param name="CheckBoxs"></param>
        /// <returns></returns>
        public bool Export(ObservableCollection<CheckBox> CheckBoxs)
        {
            #region Объявление
            var qty_plc = 0;
            if (App.Settings is not null && App.Settings.Config is not null && App.Settings.Config.PLC_List is not null && App.Settings.Config.PLC_List.Count > 0)
                qty_plc = App.Settings.Config.PLC_List.Count;
            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "a:application-type"
                Nodes = new() { { "name", "APP_PLC" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketNode("a:application-type", Nodes);

                // Добавляем узел типа сокета "ct:object"
                Nodes = new() { { "name", App.Settings.Config.TypeSystem }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                CreateSocketTypeNode("ct:object", Nodes);

                foreach (var _CheckBox in CheckBoxs)
                {
                    ListNodes = null;
                    if (_CheckBox.IsChecked == true)
                    {
                        string TypeExport = _CheckBox.Content.ToString();
                        var name = "";
                        var base_type = "";

                        if (TypeExport == "Состояние НА") { name = "UMPNA"; base_type = "UMPNA.PLC Device"; }
                        else if (TypeExport == "Состояние ЗД") { name = "UZD"; base_type = "UZD.PLC Device"; }
                        else if (TypeExport == "Состояние ВС") { name = "UVS"; base_type = "UVS.PLC Device"; }
                        else if (TypeExport == "Состояние ТС") { name = "UTS"; base_type = "UTS.PLC Device"; }
                        else if (TypeExport == "Карта готовностей агрегатов (Лист 1)") { name = "LIST1"; base_type = "LIST1.PLC Device"; }
                        else if (TypeExport == "Общестанционные защиты (Лист 2)") { name = "LIST2"; base_type = "LIST2.PLC Device"; }
                        else if (TypeExport == "Агрегатные защиты (Лист 3)") { name = "LIST3"; base_type = "LIST3.PLC Device"; }
                        else if (TypeExport == "Предельные параметры (Лист 4)") { name = "LIST4"; base_type = "LIST4.PLC Device"; }
                        else if (TypeExport == "Сигнализация (Лист 5)") { name = "LIST5"; base_type = "LIST5.PLC Device"; }
                        else if (TypeExport == "Карта ручного ввода") { name = "HandMap"; base_type = "HandMap.PLC Device"; }
                        else if (TypeExport == "Команды") { name = "CMD"; base_type = "CMD.PLC Device"; }
                        else if (TypeExport == "Сигналы AI") { name = "OIP"; base_type = "OIP.PLC Device"; }
                        else if (TypeExport == "Регистры формируемые") { name = "UserReg"; base_type = "UserReg.PLC Device"; }
                        else if (TypeExport == "Сообщения") { name = "Messages"; base_type = "Messages.PLC Device"; }
                        else if (TypeExport == "Диагностика")
                        {
                            ListNodes = new();

                            Nodes = new() { { "name", "DiagLink" }, { "base-type", "DiagLink.PLC Device" },
                                    { "access-level", "public" }, { "access-scope", "global" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                            ListNodes.Add(Nodes);

                            if (qty_plc > 0)
                            {
                                Nodes = new() { { "name", "DiagPLC" }, { "base-type", "DiagPLC.PLC Device" },
                                    { "access-level", "public" }, { "access-scope", "global" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                                ListNodes.Add(Nodes);
                            }

                            Nodes = new() { { "name", "DiagRack" }, { "base-type", "DiagRack.PLC Device" },
                                    { "access-level", "public" }, { "access-scope", "global" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                            ListNodes.Add(Nodes);

                            Nodes = new() { { "name", "DiagUSO" }, { "base-type", "DiagUSO.PLC Device" },
                                    { "access-level", "public" }, { "access-scope", "global" }, { "aspect", "Aspects.PLC" }, { "uuid", "0" } };
                            ListNodes.Add(Nodes);
                        }

                        // Добавляем узел параметра сокета "ct:object"
                        if (ListNodes is not null && ListNodes.Count > 0)
                        {
                            foreach (var _Node in ListNodes)
                                CreateSocketParametrNode("ct:object", Nodes: _Node);
                        }
                        else
                        {
                            Nodes = new()
                            {
                                { "name", name },
                                { "base-type", base_type },
                                { "access-level", "public" },
                                { "access-scope", "global" },
                                { "aspect", "Aspects.PLC" },
                                { "uuid", "0" }
                            };
                            CreateSocketParametrNode("ct:object", Nodes: Nodes);
                        }
                    }
                }

                SaveDoc("AppPLC");
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
