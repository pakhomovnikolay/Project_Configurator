using Project_Сonfigurator.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Project_Сonfigurator.Services
{
    public class EncryptorService : IEncryptorService
    {
        #region Соль шифрования
        private static readonly byte[] __Salt =
        {
            0x26, 0xdc, 0xff, 0x00,
            0xad, 0xed, 0x7a, 0xee,
            0xc5, 0xfe, 0x07, 0xaf,
            0x4d, 0x08, 0x22, 0x3c
        };
        #endregion

        #region Шифрование
        private static ICryptoTransform GetEncryptor(string password, byte[] salt = null)
        {
            var pdb = new Rfc2898DeriveBytes(password, salt ?? __Salt);
#pragma warning disable SYSLIB0022 // Тип или член устарел
            var algoritm = Rijndael.Create();
#pragma warning restore SYSLIB0022 // Тип или член устарел
            algoritm.Key = pdb.GetBytes(32);
            algoritm.IV = pdb.GetBytes(16);
            return algoritm.CreateEncryptor();
        }
        #endregion

        #region Расшифровка
        private static ICryptoTransform GetDecryptor(string password, byte[] salt = null)
        {
            var pdb = new Rfc2898DeriveBytes(password, salt ?? __Salt);
#pragma warning disable SYSLIB0022 // Тип или член устарел
            var algoritm = Rijndael.Create();
#pragma warning restore SYSLIB0022 // Тип или член устарел
            algoritm.Key = pdb.GetBytes(32);
            algoritm.IV = pdb.GetBytes(16);
            return algoritm.CreateDecryptor();
        }
        #endregion

        #region Синхронное шифрование
        public void Encryptor(string sourcePath, string destinationPath, string password, int bufferLegth = 102400)
        {
            var encryptor = GetEncryptor(password/*, Encoding.UTF8.GetBytes(sourcePath)*/);

            using var destination_encrypted = File.Create(destinationPath, bufferLegth);
            using var destination = new CryptoStream(destination_encrypted, encryptor, CryptoStreamMode.Write);
            using var source = File.OpenRead(sourcePath);

            int readed;
            var buffer = new byte[bufferLegth];
            do
            {
                readed = source.Read(buffer, 0, bufferLegth);
                destination.Write(buffer, 0, readed);
            } while (readed > 0);

            destination.FlushFinalBlock();
        }
        #endregion

        #region Синхронная расшифровка
        public bool Decryptor(string sourcePath, string destinationPath, string password, int bufferLegth = 102400)
        {
            var decryptor = GetDecryptor(password/*, Encoding.UTF8.GetBytes(sourcePath)*/);

            using var destination_decrypted = File.Create(destinationPath, bufferLegth);
            using var destination = new CryptoStream(destination_decrypted, decryptor, CryptoStreamMode.Write);
            using var source = File.OpenRead(sourcePath);

            int readed;
            var buffer = new byte[bufferLegth];
            do
            {
                readed = source.Read(buffer, 0, bufferLegth);
                destination.Write(buffer, 0, readed);
            } while (readed > 0);

            try
            {
                destination.FlushFinalBlock();
            }
            catch (CryptographicException)
            {

                return false;
            }
            return true;
        }
        #endregion

        #region Асинхронное шифрование
        public async Task EncryptorAsync(
            string sourcePath,
            string destinationPath,
            string password,
            int bufferLegth = 102400,
            IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            if (!File.Exists(sourcePath)) throw new FileNotFoundException("Файл-источник для процесса шифрования не найден", sourcePath);
            if (bufferLegth <= 0) throw new ArgumentOutOfRangeException(nameof(bufferLegth), bufferLegth, "Размер буфера чтения должен быть больше 0");

            cancel.ThrowIfCancellationRequested();

            var encryptor = GetEncryptor(password/*, Encoding.UTF8.GetBytes(sourcePath)*/);

            try
            {
                await using var destination_encrypted = File.Create(destinationPath, bufferLegth);
                await using var destination = new CryptoStream(destination_encrypted, encryptor, CryptoStreamMode.Write);
                await using var source = File.OpenRead(sourcePath);

                var file_lenght = source.Length;

                int readed;
                var buffer = new byte[bufferLegth];
                var last_percent = 0.0;
                do
                {
                    readed = await source.ReadAsync(buffer.AsMemory(0, bufferLegth), cancel).ConfigureAwait(false);
                    await destination.WriteAsync(buffer.AsMemory(0, readed), cancel).ConfigureAwait(false);

                    var position = source.Position;
                    var percent = (double)position / file_lenght;

                    if ((percent - last_percent) >= 0.001)
                    {
                        progress?.Report(percent);
                        last_percent = percent;
                    }

                    if (cancel.IsCancellationRequested)
                        cancel.ThrowIfCancellationRequested();

                } while (readed > 0);

                destination.FlushFinalBlock();

                progress?.Report(1);
            }
            catch (OperationCanceledException)
            {
                File.Delete(destinationPath);
                progress?.Report(0);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in EncryptorAsync:\r\n{0}", e);
                throw;
            }
        }
        #endregion

        #region Асинхронная расшифровка
        public async Task<bool> DecryptorAsync(
            string sourcePath,
            string destinationPath,
            string password,
            int bufferLegth = 102400,
            IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            if (!File.Exists(sourcePath)) throw new FileNotFoundException("Файл-источник для процесса шифрования не найден", sourcePath);
            if (bufferLegth <= 0) throw new ArgumentOutOfRangeException(nameof(bufferLegth), bufferLegth, "Размер буфера чтения должен быть больше 0");

            cancel.ThrowIfCancellationRequested();

            var decryptor = GetDecryptor(password/*, Encoding.UTF8.GetBytes(sourcePath)*/);

            try
            {
                await using var destination_decrypted = File.Create(destinationPath, bufferLegth);
                await using var destination = new CryptoStream(destination_decrypted, decryptor, CryptoStreamMode.Write);
                await using var source = File.OpenRead(sourcePath);

                var file_lenght = source.Length;

                int readed;
                var buffer = new byte[bufferLegth];
                var last_percent = 0.0;
                do
                {
                    readed = await source.ReadAsync(buffer.AsMemory(0, bufferLegth), cancel).ConfigureAwait(false);
                    await destination.WriteAsync(buffer.AsMemory(0, readed), cancel).ConfigureAwait(false);

                    var position = source.Position;
                    var percent = (double)position / file_lenght;

                    if ((percent - last_percent) >= 0.001)
                    {
                        progress?.Report(percent);
                        last_percent = percent;
                    }


                    cancel.ThrowIfCancellationRequested();
                } while (readed > 0);

                try
                {
                    destination.FlushFinalBlock();
                }
                catch (CryptographicException)
                {
                    return false;
                }
                progress?.Report(1);
            }
            catch (OperationCanceledException)
            {
                File.Delete(destinationPath);
                progress?.Report(0);
                throw;
            }
            catch (Exception e)
            {

                Debug.WriteLine("Error in EncryptorAsync:\r\n{0}", e);
                throw;
            }

            return true;
        }
        #endregion

    }
}
