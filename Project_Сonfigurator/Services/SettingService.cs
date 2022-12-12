using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Project_Сonfigurator.Services
{
    public class SettingService : ISettingService
    {
        #region Параметры настроек
        /// <summary>
        /// Параметры настроек
        /// </summary>
        public SettingApp Config { get; set; } = new();
        #endregion

        #region Сохранение настроек
        /// <summary>
        /// Сохранение настроек
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            try
            {
                var SettingsAppSerializer = new XmlSerializer(typeof(SettingApp));
                var xmlWriterSettings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
                using XmlWriter xmlWriter = XmlWriter.Create(Program.PathConfig + "\\Config.xml", xmlWriterSettings);

                //Config.SelectedPassword = Encryption(Config.SelectedPassword);
                SettingsAppSerializer.Serialize(xmlWriter, Config);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Загрузка настроек
        /// <summary>
        /// Загрузка настроек
        /// </summary>
        /// <returns></returns>
        public SettingApp Load()
        {
            var SettingsAppSerializer = new XmlSerializer(typeof(SettingApp));
            try
            {
                using FileStream fs = new(Program.PathConfig + "\\Config.xml", FileMode.OpenOrCreate);
                Config = SettingsAppSerializer.Deserialize(fs) as SettingApp;
                return Config;
            }
            catch (Exception)
            {
                return Config = new();
            }
        }
        #endregion
    }
}
