using Project_Сonfigurator.Services.Interfaces;

namespace Project_Сonfigurator.Services.Base
{
    public class BaseService
    {
        public readonly IUserDialogService UserDialog = new UserDialogService();
        public readonly ILogSerivece Logger = new LogSerivece();
    }
}
