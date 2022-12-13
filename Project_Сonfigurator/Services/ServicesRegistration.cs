﻿using Microsoft.Extensions.DependencyInjection;
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
            .AddSingleton<IDBService, DBService>()

            .AddSingleton<ISignalService, SignalService>()
            .AddSingleton<ISettingService, SettingService>()
            ;
    }
}
