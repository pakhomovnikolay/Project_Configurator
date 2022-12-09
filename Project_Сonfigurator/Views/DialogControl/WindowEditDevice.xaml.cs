using Project_Сonfigurator.Models;
using System.Collections.Generic;
using System.Windows;

namespace Project_Сonfigurator.Views.DialogControl
{
    public partial class WindowEditDevice
    {
        #region Заголовок окна
        public string _Title
        {
            get => (string)GetValue(_TitleProperty);
            set => SetValue(_TitleProperty, value);
        }
        /// <summary>
        /// Заголовок окна
        /// </summary>
        public static readonly DependencyProperty _TitleProperty = DependencyProperty.Register(
            nameof(_Title),
            typeof(string),
            typeof(WindowEditDevice),
            new PropertyMetadata(""));
        #endregion

        #region Входные параметры
        public List<BaseText> InputParam
        {
            get => (List<BaseText>)GetValue(InputParamProperty);
            set => SetValue(InputParamProperty, value);
        }
        /// <summary>
        /// Входные параметры
        /// </summary>
        public static readonly DependencyProperty InputParamProperty = DependencyProperty.Register(
            nameof(InputParam),
            typeof(List<BaseText>),
            typeof(WindowEditDevice),
            new PropertyMetadata(default(List<BaseText>)));
        #endregion

        #region Выбранный входной параметр
        public BaseText SelectedInputParam
        {
            get => (BaseText)GetValue(SelectedInputParamProperty);
            set => SetValue(SelectedInputParamProperty, value);
        }
        /// <summary>
        /// Выбранный входной параметр
        /// </summary>
        public static readonly DependencyProperty SelectedInputParamProperty = DependencyProperty.Register(
            nameof(SelectedInputParam),
            typeof(BaseText),
            typeof(WindowEditDevice),
            new PropertyMetadata(default(BaseText)));
        #endregion

        #region Выходные параметры
        public List<BaseText> OutputParam
        {
            get => (List<BaseText>)GetValue(OutputParamProperty);
            set => SetValue(OutputParamProperty, value);
        }
        /// <summary>
        /// Выходные параметры
        /// </summary>
        public static readonly DependencyProperty OutputParamProperty = DependencyProperty.Register(
            nameof(OutputParam),
            typeof(List<BaseText>),
            typeof(WindowEditDevice),
            new PropertyMetadata(default(List<BaseText>)));
        #endregion

        #region Выбранный выходной параметр
        public BaseText SelectedOutputParam
        {
            get => (BaseText)GetValue(SelectedOutputParamProperty);
            set => SetValue(SelectedOutputParamProperty, value);
        }
        /// <summary>
        /// Выбранный выходной параметр
        /// </summary>
        public static readonly DependencyProperty SelectedOutputParamProperty = DependencyProperty.Register(
            nameof(SelectedOutputParam),
            typeof(BaseText),
            typeof(WindowEditDevice),
            new PropertyMetadata(default(BaseText)));
        #endregion

        #region Уставки
        public List<BaseText> Setpoints
        {
            get => (List<BaseText>)GetValue(SetpointsProperty);
            set => SetValue(SetpointsProperty, value);
        }
        /// <summary>
        /// Уставки
        /// </summary>
        public static readonly DependencyProperty SetpointsProperty = DependencyProperty.Register(
            nameof(Setpoints),
            typeof(List<BaseText>),
            typeof(WindowEditDevice),
            new PropertyMetadata(default(List<BaseText>)));
        #endregion

        #region Выбранная уставка
        public BaseText SelectedSetpoint
        {
            get => (BaseText)GetValue(SelectedSetpointProperty);
            set => SetValue(SelectedSetpointProperty, value);
        }
        /// <summary>
        /// Выбранная уставка
        /// </summary>
        public static readonly DependencyProperty SelectedSetpointProperty = DependencyProperty.Register(
            nameof(SelectedSetpoint),
            typeof(BaseText),
            typeof(WindowEditDevice),
            new PropertyMetadata(default(BaseText)));
        #endregion

        #region Создать входный параметр
        /// <summary>
        /// Создать входный параметр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdCreateInputParam(object sender, RoutedEventArgs e)
        {
            InputParam.Add(new BaseText { Text = $"Новый параметр {InputParam.Count + 1}" });
            DataGridInputParam.Items.Refresh();
            SelectedInputParam = InputParam[^1];

        }
        #endregion

        #region Удалить выбранный входный параметр
        /// <summary>
        /// Удалить выбранный входный параметр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdDeleteSelectedInputParam(object sender, RoutedEventArgs e)
        {
            var index = InputParam.IndexOf(SelectedInputParam);
            if (index < 0) return;
            InputParam.Remove(SelectedInputParam);

            if (InputParam.Count > 0)
            {
                if (index > 0)
                    SelectedInputParam = InputParam[index - 1];
                else
                    SelectedInputParam = InputParam[index];
            }
            DataGridInputParam.Items.Refresh();
        }
        #endregion

        #region Создать выходный параметр
        /// <summary>
        /// Создать выходный параметр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdCreateOutputParam(object sender, RoutedEventArgs e)
        {
            OutputParam.Add(new BaseText { Text = $"Новый параметр {OutputParam.Count + 1}" });
            DataGridOutputParam.Items.Refresh();
            SelectedOutputParam = OutputParam[^1];

        }
        #endregion

        #region Удалить выбранный выходный параметр
        /// <summary>
        /// Удалить выбранный выходный параметр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdDeleteSelectedOutputParam(object sender, RoutedEventArgs e)
        {
            var index = OutputParam.IndexOf(SelectedOutputParam);
            if (index < 0) return;
            OutputParam.Remove(SelectedOutputParam);

            if (OutputParam.Count > 0)
            {
                if (index > 0)
                    SelectedOutputParam = OutputParam[index - 1];
                else
                    SelectedOutputParam = OutputParam[index];
            }
            DataGridOutputParam.Items.Refresh();
        }
        #endregion

        #region Создать уставку
        /// <summary>
        /// Создать уставку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdCreateSetpoints(object sender, RoutedEventArgs e)
        {
            Setpoints.Add(new BaseText { Text = $"Новый параметр {Setpoints.Count + 1}" });
            DataGridSetpoints.Items.Refresh();
            SelectedSetpoint = Setpoints[^1];

        }
        #endregion

        #region Удалить выбранную уставку
        /// <summary>
        /// Удалить выбранную уставку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdDeleteSelectedSetpoints(object sender, RoutedEventArgs e)
        {
            var index = Setpoints.IndexOf(SelectedSetpoint);
            Setpoints.Remove(SelectedSetpoint);
            if (index < 0) return;

            if (Setpoints.Count > 0)
            {
                if (index > 0)
                    SelectedSetpoint = Setpoints[index - 1];
                else
                    SelectedSetpoint = Setpoints[index];
            }
            DataGridSetpoints.Items.Refresh();
        }
        #endregion

        #region Конструктор
        public WindowEditDevice() => InitializeComponent();
        #endregion
    }
}
