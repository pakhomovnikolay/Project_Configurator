using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Services.Export.VU.Interfaces;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.ViewModels.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Xml;

namespace Project_Сonfigurator.Services.Export.VU
{
    public class VUAppIOSASExportRedefineService : IVUAppIOSASExportRedefineService
    {
        private static readonly ILogSerivece Logger = new LogSerivece();
        private const string Namespace = "system";

        private static Dictionary<string, string> Nodes;
        private static Dictionary<string, string> Attributes;

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
        /// <param name="TypeExport"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public bool Export(ObservableCollection<CheckBox> CheckBoxs)
        {
            #region Объявление
            var TypeSystem = App.Settings.Config.TypeSystem;
            ObservableCollection<USO> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as LayotRackUserControlViewModel
                                     where _TabItem is LayotRackUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var NoCheck = true;
            foreach (var CheckBox in CheckBoxs)
                if (CheckBox.IsChecked == true)
                    NoCheck = false;

            if (CheckBoxs is null || CheckBoxs.Count <= 0) return false;
            if (NoCheck) return false;

            var qty_plc = 0;
            foreach (var _Param in Params)
                foreach (var _Rack in _Param.Racks)
                    foreach (var _Module in _Rack.Modules)
                        if (_Module.Type == TypeModule.PLC)
                            qty_plc++;

            #endregion

            try
            {
                // Добавляем корневой узел "omx"
                CreateRootNode("omx");

                // Добавляем узел сокета "namespace"
                Nodes = new() { { "name", "APP_IOS" }, { "aspect", "Aspects.IOS" }, { "original", "APP_PLC" }, { "uuid", "0" } };
                CreateSocketNode("a:application-type", Nodes);

                // Добавляем узел типа сокета "r:ref"
                Nodes = new()
                {
                    { "name", "_APP_PLC" },
                    { "type", "APP_PLC" },
                    { "const-access", "false" },
                    { "aspected", "true" },
                    { "uuid", "0" }
                };
                CreateSocketTypeNode("r:ref", Nodes);

                // Добавляем узел типа сокета "ct:object"
                Nodes = new()
                {
                    { "name", TypeSystem },
                    { "access-level", "public" },
                    { "access-scope", "global" },
                    { "aspect", "Aspects.IOS" },
                    { "original", $"_APP_PLC.{TypeSystem}" },
                    { "uuid", "0" }
                };
                CreateSocketTypeNode("ct:object", Nodes);

                foreach (var _CheckBox in CheckBoxs)
                {
                    var need_add = true;
                    if (_CheckBox.IsChecked == true)
                    {
                        string TypeExport = _CheckBox.Content.ToString();
                        var name = "";
                        var base_type = "";
                        var original_type = "";

                        if (TypeExport == "Состояние НА") { name = "UMPNA"; base_type = "UMPNA.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.UMPNA"; }
                        else if (TypeExport == "Состояние ЗД") { name = "UZD"; base_type = "UZD.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.UZD"; }
                        else if (TypeExport == "Состояние ВС") { name = "UVS"; base_type = "UVS.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.UVS"; }
                        else if (TypeExport == "Состояние ТС") { name = "UTS"; base_type = "UTS.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.UTS"; }
                        else if (TypeExport == "Карта готовностей агрегатов (Лист 1)") { name = "LIST1"; base_type = "LIST1.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.LIST1"; }
                        else if (TypeExport == "Общестанционные защиты (Лист 2)") { name = "LIST2"; base_type = "LIST2.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.LIST2"; }
                        else if (TypeExport == "Агрегатные защиты (Лист 3)") { name = "LIST3"; base_type = "LIST3.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.LIST3"; }
                        else if (TypeExport == "Предельные параметры (Лист 4)") { name = "LIST4"; base_type = "LIST4.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.LIST4"; }
                        else if (TypeExport == "Лист 5") { name = "LIST5"; base_type = "LIST5.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.LIST5"; }
                        else if (TypeExport == "Карта ручного ввода") { name = "HandMap"; base_type = "HandMap.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.HandMap"; }
                        else if (TypeExport == "Команды") { name = "CMD"; base_type = "CMD.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.CMD"; }
                        else if (TypeExport == "Сигналы AI") { name = "OIP"; base_type = "OIP.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.OIP"; }
                        else if (TypeExport == "Регистры формируемые") { name = "UserReg"; base_type = "UserReg.IO Server"; original_type = $"_APP_PLC.{TypeSystem}.UserReg"; }

                        else if (TypeExport == "Диагностика")
                        {
                            need_add = false;
                            Nodes = new()
                            {
                                { "name", "DiagLink" },
                                { "base-type", "DiagLink.IO Server" },
                                { "access-level", "public" },
                                { "access-scope", "global" },
                                { "aspect", "Aspects.IOS" },
                                { "original", $"_APP_PLC.{TypeSystem}.DiagLink" },
                                { "uuid", "0" }
                            };
                            Attributes = new() { { "ref", "_PLC Device" }, { "target", $"_APP_PLC.{TypeSystem}.DiagLink" } };
                            CreateSocketParametrNode("ct:object", "r:init-ref", Nodes: Nodes, Attributes: Attributes);

                            if (qty_plc > 0)
                            {
                                Nodes = new()
                                {
                                    { "name", "DiagPLC" },
                                    { "base-type", "DiagPLC.IO Server" },
                                    { "access-level", "public" },
                                    { "access-scope", "global" },
                                    { "aspect", "Aspects.IOS" },
                                    { "original", $"_APP_PLC.{TypeSystem}.DiagPLC" },
                                    { "uuid", "0" }
                                };
                                Attributes = new() { { "ref", "_PLC Device" }, { "target", $"_APP_PLC.{TypeSystem}.DiagPLC" } };
                                CreateSocketParametrNode("ct:object", "r:init-ref", Nodes: Nodes, Attributes: Attributes);
                            }

                            Nodes = new()
                                {
                                    { "name", "DiagRack" },
                                    { "base-type", "DiagRack.IO Server" },
                                    { "access-level", "public" },
                                    { "access-scope", "global" },
                                    { "aspect", "Aspects.IOS" },
                                    { "original", $"_APP_PLC.{TypeSystem}.DiagRack" },
                                    { "uuid", "0" }
                                };
                            Attributes = new() { { "ref", "_PLC Device" }, { "target", $"_APP_PLC.{TypeSystem}.DiagRack" } };
                            CreateSocketParametrNode("ct:object", "r:init-ref", Nodes: Nodes, Attributes: Attributes);

                            Nodes = new()
                                {
                                    { "name", "DiagUSO" },
                                    { "base-type", "DiagUSO.IO Server" },
                                    { "access-level", "public" },
                                    { "access-scope", "global" },
                                    { "aspect", "Aspects.IOS" },
                                    { "original", $"_APP_PLC.{TypeSystem}.DiagUSO" },
                                    { "uuid", "0" }
                                };
                            Attributes = new() { { "ref", "_PLC Device" }, { "target", $"_APP_PLC.{TypeSystem}.DiagUSO" } };
                            CreateSocketParametrNode("ct:object", "r:init-ref", Nodes: Nodes, Attributes: Attributes);
                        }

                        // Добавляем узел параметра сокета "ct:object"
                        if (need_add)
                        {
                            Nodes = new()
                            {
                                { "name", name },
                                { "base-type", base_type },
                                { "access-level", "public" },
                                { "access-scope", "global" },
                                { "aspect", "Aspects.IOS" },
                                { "original", original_type },
                                { "uuid", "0" }
                            };
                            Attributes = new() { { "ref", "_PLC Device" }, { "target", original_type } };
                            CreateSocketParametrNode("ct:object", "r:init-ref", Nodes: Nodes, Attributes: Attributes);
                        }

                        //"Сообщения" => Messages()
                    }
                }

                SaveDoc("AppIOS");
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
