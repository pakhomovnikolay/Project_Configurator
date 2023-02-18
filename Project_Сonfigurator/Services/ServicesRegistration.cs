using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Export.SU;
using Project_Сonfigurator.Services.Export.SU.Interfaces;
using Project_Сonfigurator.Services.Export.VU;
using Project_Сonfigurator.Services.Export.VU.Interfaces;
using Project_Сonfigurator.Services.Interfaces;

namespace Project_Сonfigurator.Services
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddTransient<IUserDialogService, UserDialogService>()
            .AddTransient<ILogSerivece, LogSerivece>()
            .AddTransient<ILayotRackService, LayotRackService>()
            .AddTransient<IEditService, EditService>()
            .AddTransient<ISUExportRedefineService, SUExportRedefineService>()
            .AddTransient<IEncryptorService, EncryptorService>()
            .AddTransient<IVUNamespaceASExportRedefineService, VUNamespaceASExportRedefineService>()
            .AddTransient<IVUAppPLCASExportRedefineService, VUAppPLCASExportRedefineService>()
            .AddTransient<IVUAppIOSASExportRedefineService, VUAppIOSASExportRedefineService>()
            .AddTransient<IVUExportModbusMapService, VUExportModbusMapService>()
            .AddTransient<IVUExportOPCMapService, VUExportOPCMapService>()
            .AddTransient<ICyrillicSymbolService, CyrillicSymbolService>()
            .AddTransient<IRequestToDataBaseService, RequestToDataBaseService>()

            .AddSingleton<IDBService, DBService>()
            .AddSingleton<ISignalService, SignalService>()
            .AddSingleton<ISettingService, SettingService>()
            ;
    }
}
