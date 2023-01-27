using Project_Сonfigurator.Infrastructures.Commands;
using Project_Сonfigurator.Services.Export.VU.Interfaces;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project_Сonfigurator.ViewModels.AS
{
    public class IOSExportASWindowViewModel : ViewModel
    {
        #region Конструктор
        private readonly IVUAppIOSASExportRedefineService VUAppIOSASExportRedefineServices;
        private readonly IUserDialogService UserDialog;

        public IOSExportASWindowViewModel()
        {
            Title = "Экспорт приложения IOS";
        }

        public IOSExportASWindowViewModel(IVUAppIOSASExportRedefineService _VUAppIOSASExportRedefineServices, IUserDialogService _UserDialog) : this()
        {
            VUAppIOSASExportRedefineServices = _VUAppIOSASExportRedefineServices;
            UserDialog = _UserDialog;

            #region Создаем CheckBox'ы
            CheckBoxs = new()
            {
                new CheckBox{ Command = CmdSelectParam, Content = "Сообщения" },
                new CheckBox{ Command = CmdSelectParam, Content = "Диагностика" },
                new CheckBox{ Command = CmdSelectParam, Content = "Сигналы AI" },
                new CheckBox{ Command = CmdSelectParam, Content = "Регистры формируемые" },
                new CheckBox{ Command = CmdSelectParam, Content = "Карта готовностей агрегатов (Лист 1)" },
                new CheckBox{ Command = CmdSelectParam, Content = "Общестанционные защиты (Лист 2)" },
                new CheckBox{ Command = CmdSelectParam, Content = "Агрегатные защиты (Лист 3)" },
                new CheckBox{ Command = CmdSelectParam, Content = "Предельные параметры (Лист 4)" },
                new CheckBox{ Command = CmdSelectParam, Content = "Лист 5" },
                new CheckBox{ Command = CmdSelectParam, Content = "Состояние НА" },
                new CheckBox{ Command = CmdSelectParam, Content = "Состояние ЗД" },
                new CheckBox{ Command = CmdSelectParam, Content = "Состояние ВС" },
                new CheckBox{ Command = CmdSelectParam, Content = "Состояние ТС" },
                new CheckBox{ Command = CmdSelectParam, Content = "Карта ручного ввода" },
                new CheckBox{ Command = CmdSelectParam, Content = "Команды" }
            };
            #endregion
        }
        #endregion

        #region Параметры

        #region Коллекция параметров
        private ObservableCollection<CheckBox> _CheckBoxs;
        /// <summary>
        /// Коллекция параметров
        /// </summary>
        public ObservableCollection<CheckBox> CheckBoxs
        {
            get => _CheckBoxs;
            set => Set(ref _CheckBoxs, value);
        }
        #endregion

        #region Текущее состояние флажка "Выбрать все"
        private bool _IsSelectedAll;
        /// <summary>
        /// Текущее состояние флажка "Выбрать все"
        /// </summary>
        public bool IsSelectedAll
        {
            get => _IsSelectedAll;
            set => Set(ref _IsSelectedAll, value);
        }
        #endregion

        #region Описание состояния выбранных CheckBox'сов
        private string _DescriptionSelectedAll;
        /// <summary>
        /// Описание состояния выбранных CheckBox'сов
        /// </summary>
        public string DescriptionSelectedAll
        {
            get => _DescriptionSelectedAll;
            set => Set(ref _DescriptionSelectedAll, value);
        }
        #endregion

        #region Наличие выбранного хотя бы одного из параметров экспорта
        private bool _IsSelectedOne;
        /// <summary>
        /// Наличие выбранного хотя бы одного из параметров экспорта
        /// </summary>
        public bool IsSelectedOne
        {
            get => _IsSelectedOne;
            set => Set(ref _IsSelectedOne, value);
        }
        #endregion

        #endregion

        #region Команды

        #region Команда - Выбрать\Снять все параметры
        private ICommand _CmdSelectAllParam;
        /// <summary>
        /// Команда - Выбрать\Снять все параметры
        /// </summary>
        public ICommand CmdSelectAllParam => _CmdSelectAllParam ??= new RelayCommand(OnCmdSelectAllParamExecuted);
        private void OnCmdSelectAllParamExecuted()
        {
            IsSelectedAll = !IsSelectedAll;
            foreach (var _CheckBox in CheckBoxs)
            {
                _CheckBox.IsChecked = IsSelectedAll;
                IsSelectedOne = IsSelectedAll;
            }

        }
        #endregion

        #region Команда - Выбрать\Снять один из параметров
        private ICommand _CmdSelectParam;
        /// <summary>
        /// Команда - Выбрать\Снять один из параметров
        /// </summary>
        public ICommand CmdSelectParam => _CmdSelectParam ??= new RelayCommand(OnCmdSelectParamExecuted);

        private void OnCmdSelectParamExecuted()
        {
            IsSelectedAll = true;
            IsSelectedOne = false;
            foreach (var _CheckBox in CheckBoxs)
            {
                IsSelectedAll = IsSelectedAll && _CheckBox.IsChecked == true;
                IsSelectedOne = IsSelectedOne || _CheckBox.IsChecked == true;
            }

        }
        #endregion

        #region Команда - Экспорт параметров
        private ICommand _CmdExportParams;
        /// <summary>
        /// Команда - Экспорт параметров
        /// </summary>
        public ICommand CmdExportParams => _CmdExportParams ??= new RelayCommand(OnCmdExportParamsExecuted, CanCmdExportParamsExecute);
        private bool CanCmdExportParamsExecute(object p) => IsSelectedOne;
        private void OnCmdExportParamsExecuted(object p)
        {
            if (p is not Window window) return;
            if (!VUAppIOSASExportRedefineServices.Export(CheckBoxs))
                if (UserDialog.SendMessage("Внимание!", $"Экспорт выполнен c ошибками.\nСм. лог", ImageType: MessageBoxImage.Warning)) return;

            if (UserDialog.SendMessage(Title, $"Экпорт выполнен успешно.\nДанные сохранены - {App.Settings.Config.PathExportVU}"))
                window.Close();
        }
        #endregion

        #endregion
    }
}
