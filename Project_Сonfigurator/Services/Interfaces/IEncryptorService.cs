using System;
using System.Threading;
using System.Threading.Tasks;

namespace Project_Сonfigurator.Services.Interfaces
{
    public interface IEncryptorService
    {
        void Encryptor(string sourcePath, string destinationPath, string password, int bufferLegth = 102400);

        bool Decryptor(string sourcePath, string destinationPath, string password, int bufferLegth = 102400);

        Task EncryptorAsync(string sourcePath, string destinationPath, string password, int bufferLegth = 102400,
            IProgress<double> progress = null, CancellationToken cancel = default);

        Task<bool> DecryptorAsync(string sourcePath, string destinationPath, string password, int bufferLegth = 102400,
            IProgress<double> progress = null, CancellationToken cancel = default);
    }
}
