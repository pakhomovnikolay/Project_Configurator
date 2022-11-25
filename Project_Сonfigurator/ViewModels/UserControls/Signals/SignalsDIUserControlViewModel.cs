using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;

namespace Project_Сonfigurator.ViewModels.UserControls.Signals
{
    public class SignalsDIUserControlViewModel : ViewModel
    {
        #region Конструктор
        ILayotRackService _LayotRackService;

        public SignalsDIUserControlViewModel(ILayotRackService iLayotRackService)
        {
            _LayotRackService = iLayotRackService;
        }
        #endregion

        #region Параметры

        #region Заголовок вкладки
        private string _Title = "Компоновка корзин";
        /// <summary>
        /// Заголовок вкладки
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region Описание вкладки
        private string _Description = "Компоновка корзин НПС-1 \"Сызрань\"";
        /// <summary>
        /// Описание вкладки
        /// </summary>
        public string Description
        {
            get => _Description;
            set => Set(ref _Description, value);
        }
        #endregion

        #region Высота окна
        private int _WindowHeight = 800;
        /// <summary>
        /// Высота окна
        /// </summary>
        public int WindowHeight
        {
            get => _WindowHeight;
            set => Set(ref _WindowHeight, value);
        }
        #endregion

        #region Ширина окна
        private int _WindowWidth = 1740;
        /// <summary>
        /// Ширина окна
        /// </summary>
        public int WindowWidth
        {
            get => _WindowWidth;
            set => Set(ref _WindowWidth, value);
        }
        #endregion

        #region Состояние активной вкладки
        private bool _IsSelected = false;
        /// <summary>
        /// Состояние активной вкладки
        /// </summary>
        public bool IsSelected
        {
            get => _IsSelected;
            set => Set(ref _IsSelected, value);
        }
        #endregion

        #endregion
    }
}
