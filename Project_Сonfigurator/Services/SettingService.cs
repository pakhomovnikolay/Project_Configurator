using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Project_Сonfigurator.Services
{
    public class SettingService : ISettingService
    {
        private const string __EncryptedFileSuffix = ".configproject";

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
            IEncryptorService _Encryptor = new EncryptorService();
            IUserDialogService UserDialog = new UserDialogService();
            var PathConfig = App.PathConfig + "\\Config.xml";
            try
            {
                var SettingsAppSerializer = new XmlSerializer(typeof(SettingApp));
                var xmlWriterSettings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
                using XmlWriter xmlWriter = XmlWriter.Create(PathConfig, xmlWriterSettings);

                SettingsAppSerializer.Serialize(xmlWriter, Config);
                xmlWriter.Close();

                var FileNameEncrypt = PathConfig;
                var FileNameEncrypted = PathConfig.Replace(".xml", __EncryptedFileSuffix);
                try
                {
                    _Encryptor.Encryptor(FileNameEncrypt, FileNameEncrypted, "");
                    UserDialog.DeleteFile(FileNameEncrypt);
                }
                catch (OperationCanceledException e)
                {
                    Debug.WriteLine("Error in EncryptorAsync:\r\n{0}", e);
                    return false;
                }

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
            IEncryptorService _Encryptor = new EncryptorService();
            IUserDialogService UserDialog = new UserDialogService();

            var FileNameEncrypt = App.PathConfig + "\\Config.xml";
            var FileNameEncrypted = App.PathConfig + "\\Config" + __EncryptedFileSuffix;

            _Encryptor.Decryptor(FileNameEncrypted, FileNameEncrypt, "");
            var SettingsAppSerializer = new XmlSerializer(typeof(SettingApp));
            try
            {
                using FileStream fs = new(App.PathConfig + "\\Config.xml", FileMode.OpenOrCreate);
                Config = SettingsAppSerializer.Deserialize(fs) as SettingApp;
                return Config;
            }
            catch (Exception)
            {
                return Config = new();
            }
            finally
            {
                UserDialog.DeleteFile(FileNameEncrypt);
            }
        }
        #endregion
    }
}
