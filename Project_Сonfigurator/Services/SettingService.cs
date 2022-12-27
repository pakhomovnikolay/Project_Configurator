using Project_Сonfigurator.Models.Settings;
using Project_Сonfigurator.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Project_Сonfigurator.Services
{
    public class SettingService : ISettingService
    {
        private const string __EncryptedFileSuffix = ".etd";

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

            try
            {
                var SettingsAppSerializer = new XmlSerializer(typeof(SettingApp));
                var xmlWriterSettings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
                using XmlWriter xmlWriter = XmlWriter.Create(Program.PathConfig + "\\Config.xml", xmlWriterSettings);

                SettingsAppSerializer.Serialize(xmlWriter, Config);
                xmlWriter.Close();

                var FileNameEncrypt = Program.PathConfig + "\\Config.xml";
                var FileNameEncrypted = Program.PathConfig + "\\Config" + __EncryptedFileSuffix;
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

            var FileNameEncrypt = Program.PathConfig + "\\Config.xml";
            var FileNameEncrypted = Program.PathConfig + "\\Config" + __EncryptedFileSuffix;


            _Encryptor.Decryptor(FileNameEncrypted, FileNameEncrypt, "");
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
            finally
            {
                UserDialog.DeleteFile(FileNameEncrypt);
            }
        }
        #endregion
    }
}
