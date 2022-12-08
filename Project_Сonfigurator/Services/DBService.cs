using MySqlConnector;
using Project_Сonfigurator.Models;
using Project_Сonfigurator.Services.Interfaces;
using System.Collections.Generic;
using System;
using DocumentFormat.OpenXml.Office.Word;

namespace Project_Сonfigurator.Services
{
    public class DBService : IDBService
    {
        #region Функция получения данных
        /// <summary>
        /// Функция получения данных
        /// </summary>
        /// <returns></returns>
        public DBData GetData()
        {
            var connection = new MySqlConnection();
            var data_table = new DBData();
            var Log = new LogSerivece();
            var DialogService = new UserDialogService();
            var Config = Program.Settings.Config.ConnectDB;

            try
            {
                var connectionString =
                    $"SERVER={Config.IPAddress};" +
                    $"PORT={Config.Port};" +
                    //$"DATABASE=mariadb;" +
                    $"UID={Config.UserName};" +
                    $"PASSWORD={Config.Password};" +
                    $"Connection Timeout = {Config.TimeoutConnect}";

                connection = new(connectionString);
                try
                {
                    connection.Open();
                    string Query = $"CREATE DATABASE IF NOT EXISTS `{Config.NameDB}`;";
                    var mySqlCommand = new MySqlCommand(Query, connection);
                    mySqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception e)
                {
                    Log.WriteLog($"Подключения к БД: {e.Message}", App.NameApp);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return data_table;
        }
        #endregion
    }
}
