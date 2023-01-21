using Microsoft.Extensions.DependencyInjection;
using Project_Сonfigurator.Models.Params;
using Project_Сonfigurator.Models.Signals;
using Project_Сonfigurator.Services.Interfaces;
using Project_Сonfigurator.ViewModels.Base.Interfaces;
using Project_Сonfigurator.ViewModels.UserControls.Params;
using Project_Сonfigurator.ViewModels.UserControls.Signals;
using Project_Сonfigurator.Views.DialogControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Project_Сonfigurator.Services
{
    public class SUExportRedefineService : ISUExportRedefineService
    {
        #region Экспорт данных СУ
        /// <summary>
        /// Экспорт данных СУ
        /// </summary>
        /// <param name="TypeExport"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public bool Export(string TypeExport, object item)
        {
            if (TypeExport is null) throw new ArgumentNullException(nameof(TypeExport));
            return TypeExport switch
            {
                "Экспорт всего проекта" => ExportAll(item),
                "Чтение данных с модулей" => ExportReadInputs(item),
                "Запись данных для модулей" => ExportReadOutputs(item),

                "Сигналы AI" => ExportSignalsAI(item),
                "Сигналы DI" => ExportSignalsDI(item),
                "Сигналы DO" => ExportSignalsDO(item),
                "Сигналы AO" => ExportSignalsAO(item),

                "Диагностика" => ExportDiagnostics(item),
                "Секции шин" => ExportEC(item),
                "Группы сигналов" => ExportGroupSignal(item),
                "Рамки УСО" => ExportFrameUSO(item),
                "Рамки" => ExportFrame(item),

                "Карта готовностей агрегатов (Лист 1)" => ExportKGMPNA(item),
                "Общестанционные защиты (Лист 2)" => ExportKTPR(item),
                "Агрегатные защиты (Лист 3)" => ExportKTPRA(item),
                "Агрегатные предупреждения (Лист 3,5)" => ExportKTPRAS(item),
                "Предельные параметры (Лист 4)" => ExportKTPRS(item),
                "Лист 5" => ExportLIST5(item),

                "DI агрегатов" => ExportUMPNA_DI(item),
                "DI задвижек" => ExportUZD_DI(item),
                "DI вспомсистем" => ExportUVS_DI(item),
                "Параметры DO остальных" => ExportDO_Param(item),

                "DO агрегатов" => ExportUMPNA_DO(item),
                "DO задвижек" => ExportUZD_DO(item),
                "DO вспомсистем" => ExportUVS_DO(item),
                "DO остальные" => ExportDO_Others(item),
                _ => throw new NotSupportedException($"Экспорт данного типа \"{TypeExport}\" не поддерживается"),
            };
        }
        #endregion

        #region Проверка значения на целое число, преобразование его в string
        private static string TextToSint(string i)
        {
            return TextToInt(i).ToString();
        }
        #endregion

        #region Проверка значения на целое число
        private static int TextToInt(string i)
        {
            _ = int.TryParse(i, out var value);
            return value;
        }
        #endregion

        #region Конвертация числа в bool, преобразование его в string
        private static string TextToSbool(string i)
        {
            if (TextToBool(i))
                return "TRUE";
            else
                return "FALSE";
        }
        #endregion

        #region Конвертация числа в bool
        private static bool TextToBool(string i)
        {
            _ = int.TryParse(i, out var value);
            return value != 0;
        }
        #endregion

        #region Генерация входных сигналов для параметров
        private static string GenInSignal(int TypeSignal, int Address, string VarName, bool Inv)
        {
            string _VarName = Inv ? $"{VarName} := NOT" : $"{VarName} := ";

            return TypeSignal switch
            {
                //DI
                0 => $"{_VarName}di_shared[{Address}];",

                //Группы сигналов
                1 => $"{_VarName}siggrp[{Address}];",

                //AI
                2 => $"{_VarName}oip_shared[{Address}].OutSignal.LTMin;",                                       /*НПД*/
                3 => $"{_VarName}oip_shared[{Address}].zone < -5;",                                             /*МИН 6*/
                4 => $"{_VarName}oip_shared[{Address}].zone < -4;",                                             /*МИН 5*/
                5 => $"{_VarName}oip_shared[{Address}].zone < -3;",                                             /*МИН 4*/
                6 => $"{_VarName}oip_shared[{Address}].zone < -2;",                                             /*МИН 3*/
                7 => $"{_VarName}oip_shared[{Address}].zone < -1;",                                             /*МИН 2*/
                8 => $"{_VarName}oip_shared[{Address}].zone < -0;",                                             /*МИН 1*/
                9 => $"{_VarName}oip_shared[{Address}].zone = 0;",                                              /*НОРМА*/
                10 => $"{_VarName}oip_shared[{Address}].zone > 0;",                                             /*МАКС 1*/
                11 => $"{_VarName}oip_shared[{Address}].zone > 1;",                                             /*МАКС 2*/
                12 => $"{_VarName}oip_shared[{Address}].zone > 2;",                                             /*МАКС 3*/
                13 => $"{_VarName}oip_shared[{Address}].zone > 3;",                                             /*МАКС 4*/
                14 => $"{_VarName}oip_shared[{Address}].zone > 4;",                                             /*МАКС 5*/
                15 => $"{_VarName}oip_shared[{Address}].zone > 5;",                                             /*МАКС 6*/
                16 => $"{_VarName}oip_shared[{Address}].zone > 6;",                                             /*МАКС 7*/
                17 => $"{_VarName}oip_shared[{Address}].zone > 7;",                                             /*МАКС 8*/
                18 => $"{_VarName}oip_shared[{Address}].zone > 8;",                                             /*МАКС 9*/
                19 => $"{_VarName}oip_shared[{Address}].OutSignal.LTMax;",                                      /*ВПД*/
                21 => $"{_VarName}oip_shared[{Address}].OutSignal.Ndv;",                                        /*НДВ*/
                31 => $"{_VarName}oip_shared[{Address}].zone > 0 OR {_VarName}oip_shared[{Address}].zone < 0;", /*>= МАКС 1 или <= МИН 1*/
                32 => $"{_VarName}oip_shared[{Address}].zone > 1 OR {_VarName}oip_shared[{Address}].zone < -1;",/*>= МАКС 2 или <= МИН 2*/
                33 => $"{_VarName}oip_shared[{Address}].zone > 2 OR {_VarName}oip_shared[{Address}].zone < -2;",/*>= МАКС 3 или <= МИН 3*/
                34 => $"{_VarName}oip_shared[{Address}].zone > 3 OR {_VarName}oip_shared[{Address}].zone < -3;",/*>= МАКС 4 или <= МИН 4*/
                35 => $"{_VarName}oip_shared[{Address}].zone > 4 OR {_VarName}oip_shared[{Address}].zone < -4;",/*>= МАКС 5 или <= МИН 5*/
                36 => $"{_VarName}oip_shared[{Address}].zone > 5 OR {_VarName}oip_shared[{Address}].zone < -5;",/*>= МАКС 6 или <= МИН 6*/
                _ => ""
            };
        }
        #endregion

        #region Генерация выходных сигналов для параметров
        private static string GenOutSignal(int Address, string VarName)
        {
            return $"do_shared[{Address}] := {VarName}";
        }
        #endregion

        #region Генерация управления мехнизмами
        private static string Controls(string VarName, BaseControlUZD ControlUZD = null, BaseControlUVS ControlUVS = null, BaseControlUTS ControlUTS = null)
        {
            var fNum = "";

            #region Управление ЗД
            if (ControlUZD is not null)
            {
                fNum += "(* ============================= Управление ЗД ============================= *)\n";
                if (ControlUZD.Close is not null && !string.IsNullOrWhiteSpace(ControlUZD.Close))
                {
                    var Qty = ControlUZD.Close.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uzdgrp[{item.Trim()}].close := TRUE;\n";
                    }
                }

                if (ControlUZD.Open is not null && !string.IsNullOrWhiteSpace(ControlUZD.Open))
                {
                    var Qty = ControlUZD.Open.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uzdgrp[{item.Trim()}].open := TRUE;\n";
                    }
                }

                if (ControlUZD.NoOpen is not null && !string.IsNullOrWhiteSpace(ControlUZD.NoOpen))
                {
                    var Qty = ControlUZD.NoOpen.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uzdgrp[{item.Trim()}].block_open := TRUE;\n";
                    }
                }

                if (ControlUZD.NoClose is not null && !string.IsNullOrWhiteSpace(ControlUZD.NoClose))
                {
                    var Qty = ControlUZD.NoClose.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uzdgrp[{item.Trim()}].block_close := TRUE;\n";
                    }
                }

                if (ControlUZD.CloseAfterProt is not null && !string.IsNullOrWhiteSpace(ControlUZD.CloseAfterProt))
                {
                    var Qty = ControlUZD.CloseAfterProt.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uzdgrp[{item.Trim()}].close_after_prot := TRUE;\n";
                    }
                }

                if (ControlUZD.OpenAfterProt is not null && !string.IsNullOrWhiteSpace(ControlUZD.OpenAfterProt))
                {
                    var Qty = ControlUZD.OpenAfterProt.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uzdgrp[{item.Trim()}].open_after_prot := TRUE;\n";
                    }
                }

                if (ControlUZD.CloseAfterProtTimer is not null && !string.IsNullOrWhiteSpace(ControlUZD.CloseAfterProtTimer))
                {
                    var Qty = ControlUZD.CloseAfterProtTimer.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uzdgrp[{item.Trim()}].close_after_prot_timer := TRUE;\n";
                    }
                }

                if (ControlUZD.OpenAfterProtTimer is not null && !string.IsNullOrWhiteSpace(ControlUZD.OpenAfterProtTimer))
                {
                    var Qty = ControlUZD.OpenAfterProtTimer.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uzdgrp[{item.Trim()}].open_after_prot_timer  := TRUE;\n";
                    }
                }
            }
            #endregion

            #region Управление ВС
            if (ControlUVS is not null)
            {
                fNum += "(* ============================= Управление ВС ============================= *)\n";
                if (ControlUVS.OnPrimary is not null && !string.IsNullOrWhiteSpace(ControlUVS.OnPrimary))
                {
                    var Qty = ControlUVS.OnPrimary.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].start_osn := TRUE;\n";
                    }
                }

                if (ControlUVS.OnSecondary is not null && !string.IsNullOrWhiteSpace(ControlUVS.OnSecondary))
                {
                    var Qty = ControlUVS.OnSecondary.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].start_res := TRUE;\n";
                    }
                }

                if (ControlUVS.OffPrimary is not null && !string.IsNullOrWhiteSpace(ControlUVS.OffPrimary))
                {
                    var Qty = ControlUVS.OffPrimary.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].stop_osn := TRUE;\n";
                    }
                }

                if (ControlUVS.OffSecondary is not null && !string.IsNullOrWhiteSpace(ControlUVS.OffSecondary))
                {
                    var Qty = ControlUVS.OffSecondary.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].stop_res := TRUE;\n";
                    }
                }

                if (ControlUVS.NoOffPrimary is not null && !string.IsNullOrWhiteSpace(ControlUVS.NoOffPrimary))
                {
                    var Qty = ControlUVS.NoOffPrimary.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].no_stop_osn := TRUE;\n";
                    }
                }

                if (ControlUVS.NoOffSecondary is not null && !string.IsNullOrWhiteSpace(ControlUVS.NoOffSecondary))
                {
                    var Qty = ControlUVS.NoOffSecondary.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].no_stop_res := TRUE;\n";
                    }
                }

                if (ControlUVS.NoOnPrimary is not null && !string.IsNullOrWhiteSpace(ControlUVS.NoOnPrimary))
                {
                    var Qty = ControlUVS.NoOnPrimary.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].no_start_osn := TRUE;\n";
                    }
                }

                if (ControlUVS.NoOnSecondary is not null && !string.IsNullOrWhiteSpace(ControlUVS.NoOnSecondary))
                {
                    var Qty = ControlUVS.NoOnSecondary.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].no_start_res := TRUE;\n";
                    }
                }

                if (ControlUVS.OnPrimaryAfterProt is not null && !string.IsNullOrWhiteSpace(ControlUVS.OnPrimaryAfterProt))
                {
                    var Qty = ControlUVS.OnPrimaryAfterProt.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].start_osn_after_prot := TRUE;\n";
                    }
                }

                if (ControlUVS.OnSecondaryAfterProt is not null && !string.IsNullOrWhiteSpace(ControlUVS.OnSecondaryAfterProt))
                {
                    var Qty = ControlUVS.OnSecondaryAfterProt.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].start_res_after_prot := TRUE;\n";
                    }
                }

                if (ControlUVS.OffPrimaryAfterProt is not null && !string.IsNullOrWhiteSpace(ControlUVS.OffPrimaryAfterProt))
                {
                    var Qty = ControlUVS.OffPrimaryAfterProt.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].stop_osn_after_prot := TRUE;\n";
                    }
                }

                if (ControlUVS.OffSecondaryAfterProt is not null && !string.IsNullOrWhiteSpace(ControlUVS.OffSecondaryAfterProt))
                {
                    var Qty = ControlUVS.OffSecondaryAfterProt.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].stop_res_after_prot := TRUE;\n";
                    }
                }

                if (ControlUVS.OnPrimaryAfterProtTimer is not null && !string.IsNullOrWhiteSpace(ControlUVS.OnPrimaryAfterProtTimer))
                {
                    var Qty = ControlUVS.OnPrimaryAfterProtTimer.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].start_osn_after_prot_timer := TRUE;\n";
                    }
                }

                if (ControlUVS.OnSecondaryAfterProtTimer is not null && !string.IsNullOrWhiteSpace(ControlUVS.OnSecondaryAfterProtTimer))
                {
                    var Qty = ControlUVS.OnSecondaryAfterProtTimer.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].start_res_after_prot_timer := TRUE;\n";
                    }
                }

                if (ControlUVS.OffPrimaryAfterProtTimer is not null && !string.IsNullOrWhiteSpace(ControlUVS.OffPrimaryAfterProtTimer))
                {
                    var Qty = ControlUVS.OffPrimaryAfterProtTimer.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].stop_osn_after_prot_timer := TRUE;\n";
                    }
                }

                if (ControlUVS.OffSecondaryAfterProtTimer is not null && !string.IsNullOrWhiteSpace(ControlUVS.OffSecondaryAfterProtTimer))
                {
                    var Qty = ControlUVS.OffSecondaryAfterProtTimer.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_uvsgrp[{item.Trim()}].stop_res_after_prot_timer := TRUE;\n";
                    }
                }

            }
            #endregion

            #region Управление TC
            if (ControlUTS is not null)
            {
                fNum += "(* ============================= Управление TC ============================= *)\n";
                if (ControlUTS.Off is not null && !string.IsNullOrWhiteSpace(ControlUTS.Off))
                {
                    var Qty = ControlUTS.Off.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_utsgrp[{item.Trim()}].cmd_off := TRUE;\n";
                    }
                }

                if (ControlUTS.On is not null && !string.IsNullOrWhiteSpace(ControlUTS.On))
                {
                    var Qty = ControlUTS.On.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_utsgrp[{item.Trim()}].cmd_on := TRUE;\n";
                    }
                }

                if (ControlUTS.NoOn is not null && !string.IsNullOrWhiteSpace(ControlUTS.NoOn))
                {
                    var Qty = ControlUTS.NoOn.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_utsgrp[{item.Trim()}].cmd_block_on := TRUE;\n";
                    }
                }

                if (ControlUTS.NoOff is not null && !string.IsNullOrWhiteSpace(ControlUTS.NoOff))
                {
                    var Qty = ControlUTS.NoOff.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_utsgrp[{item.Trim()}].cmd_block_off := TRUE;\n";
                    }
                }

                if (ControlUTS.OffAfterProt is not null && !string.IsNullOrWhiteSpace(ControlUTS.OffAfterProt))
                {
                    var Qty = ControlUTS.OffAfterProt.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_utsgrp[{item.Trim()}].cmd_off_after_prot := TRUE;\n";
                    }
                }

                if (ControlUTS.OnAfterProt is not null && !string.IsNullOrWhiteSpace(ControlUTS.OnAfterProt))
                {
                    var Qty = ControlUTS.OnAfterProt.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_utsgrp[{item.Trim()}].cmd_on_after_prot := TRUE;\n";
                    }
                }

                if (ControlUTS.OffAfterProtTimer is not null && !string.IsNullOrWhiteSpace(ControlUTS.OffAfterProtTimer))
                {
                    var Qty = ControlUTS.OffAfterProtTimer.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_utsgrp[{item.Trim()}].cmd_off_after_prot_timer  := TRUE;\n";
                    }
                }

                if (ControlUTS.OnAfterProtTimer is not null && !string.IsNullOrWhiteSpace(ControlUTS.OnAfterProtTimer))
                {
                    var Qty = ControlUTS.OnAfterProtTimer.Split(',');
                    foreach (var item in Qty)
                    {
                        fNum += $"{VarName}.cmd_utsgrp[{item.Trim()}].cmd_on_after_prot_timer  := TRUE;\n";
                    }
                }
            }
            #endregion

            return fNum;
        }
        #endregion

        #region Экспорт всего проекта
        /// <summary>
        /// Экспорт всего проекта
        /// </summary>
        /// <returns></returns>
        private static bool ExportAll(object item)
        {
            ExportReadInputs(item);
            ExportReadOutputs(item);
            ExportSignalsAI(item);
            ExportSignalsDI(item);
            ExportSignalsDO(item);
            ExportSignalsAO(item);
            ExportDiagnostics(item);
            ExportEC(item);
            ExportGroupSignal(item);
            ExportFrameUSO(item);
            ExportFrame(item);
            ExportKGMPNA(item);
            ExportKTPR(item);
            ExportKTPRA(item);
            ExportKTPRAS(item);
            ExportKTPRS(item);
            ExportLIST5(item);
            ExportUMPNA_DI(item);
            ExportUZD_DI(item);
            ExportUVS_DI(item);
            ExportDO_Param(item);
            ExportUMPNA_DO(item);
            ExportUZD_DO(item);
            ExportUVS_DO(item);
            ExportDO_Others(item);

            return true;
        }
        #endregion

        #region Экспорт чтения данных с модулей
        /// <summary>
        /// Экспорт чтения данных с модулей
        /// </summary>
        /// <returns></returns>
        private static bool ExportReadInputs(object item)
        {
            //var ViewModel = item as MainWindowViewModel;
            //var Params = ViewModel.SignalsAIViewModel.SignalsAI;
            //var flAllowedPrint = false;
            //var fNum = "(* =========================================== Чтение данных с модулей =========================================== *)\n\r";

            //#region Формируем данные

            //#endregion

            //if (!flAllowedPrint)
            //    return false;

            //var FileViewer = new WindowTextEditor()
            //{
            //    Title = "read_inputs",
            //    TextView = fNum,
            //    WindowStartupLocation = WindowStartupLocation.CenterOwner,
            //    Owner = Application.Current.MainWindow
            //};
            //FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт записи данных с модулей
        /// <summary>
        /// Экспорт записи данных с модулей
        /// </summary>
        /// <returns></returns>
        private static bool ExportReadOutputs(object item)
        {
            //var ViewModel = item as MainWindowViewModel;
            //var Params = ViewModel.SignalsAIViewModel.SignalsAI;
            //var flAllowedPrint = false;
            //var fNum = "(* =========================================== Запись данных в модули =========================================== *)\n\r";

            //#region Формируем данные

            //#endregion

            //if (!flAllowedPrint)
            //    return false;

            //var FileViewer = new WindowTextEditor()
            //{
            //    Title = "write_outputs",
            //    TextView = fNum,
            //    WindowStartupLocation = WindowStartupLocation.CenterOwner,
            //    Owner = Application.Current.MainWindow
            //};
            //FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Сигналы AI"
        /// <summary>
        /// Экспорт "Сигналы AI"
        /// </summary>
        /// <returns></returns>
        private static bool ExportSignalsAI(object item)
        {
            ObservableCollection<SignalAI> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as SignalsAIUserControlViewModel
                                     where _TabItem is SignalsAIUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка входных аналоговых параметров =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Signal.Description))
                {
                    flAllowedPrint = true;
                    fNum += $"(* =========================================== {_Param.Signal.Description} =========================================== *)\n\r";

                    var VarName = _Param.Signal.VarName;

                    fNum += VarName + $".enable := TRUE;\n";
                    fNum += VarName + $".num_na := {TextToSint(_Param.IndexNA)};\n";
                    fNum += VarName + $".its_engine := {TextToSint(_Param.TypeVibration)};\n";
                    fNum += VarName + $".num_pz := {TextToSint(_Param.IndexPZ)};\n";
                    fNum += VarName + $".type_pi := {TextToSint(_Param.TypePI)};\n";
                    fNum += VarName + $".num_bd := {TextToSint(_Param.IndexBD)};\n";
                    fNum += VarName + $".its_tanks := {TextToSbool(_Param.LevelRPP)};\n";
                    fNum += VarName + $".flNeedConvertUnits := {TextToSbool(_Param.ConverterKgs)};\n";
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_ai",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Сигналы DI"
        /// <summary>
        /// Экспорт "Сигналы DI"
        /// </summary>
        /// <returns></returns>
        private static bool ExportSignalsDI(object item)
        {
            ObservableCollection<SignalDI> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as SignalsDIUserControlViewModel
                                     where _TabItem is SignalsDIUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка входных дискретных параметров =========================================== *)\n\r";
            var fNumReal = "";
            var fNumImin = "";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Signal.Description))
                {
                    flAllowedPrint = true;
                    var VarName = _Param.Signal.VarName;
                    var Link = TextToInt(_Param.Signal.Address);

                    var w = Link / 32;
                    var b = Link % 32;

                    var exp = ((int)Math.Pow(2, b)).ToString("X");

                    fNumReal += $"\t {VarName} := (HW_DI[{w}] & 16#{exp}) > 0;\n";
                    fNumImin += $"\t {VarName} := (HW_DI_IMIT[{w}] & 16#{exp}) > 0;\n";
                }
            }
            fNum += $"IF NOT nps_state.fl_simulation THEN\n{fNumReal}ELSE\n{fNumImin}END_IF;";
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_di",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Сигналы DO"
        /// <summary>
        /// Экспорт "Сигналы DO"
        /// </summary>
        /// <returns></returns>
        private static bool ExportSignalsDO(object item)
        {
            ObservableCollection<SignalDO> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as SignalsDOUserControlViewModel
                                     where _TabItem is SignalsDOUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка выходных дискретных параметров =========================================== *)\n\r";
            var fNumReal = "";
            var fNumImin = "";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Signal.Description))
                {
                    flAllowedPrint = true;
                    var VarName = _Param.Signal.VarName;
                    var Link = TextToInt(_Param.Signal.Address);

                    var w = Link / 32;
                    var b = Link % 32;

                    var exp = ((int)Math.Pow(2, b)).ToString("X");

                    fNumReal += $"\t IF {VarName} THEN " +
                        $"HW_DO[{w}] := HW_DO[{w}] OR 16#{exp}; ELSE " +
                        $"HW_DO[{w}] := HW_DO[{w}] AND NOT 16#{exp}; END_IF;\n";

                    fNumImin += $"\t IF {VarName} THEN " +
                        $"HW_DO_IMIT[{w}] := HW_DO_IMIT[{w}] OR 16#{exp}; ELSE " +
                        $"HW_DO_IMIT[{w}] := HW_DO_IMIT[{w}] AND NOT 16#{exp}; END_IF;\n";
                }
            }
            fNum += $"IF NOT nps_state.fl_simulation THEN\n{fNumReal}ELSE\n{fNumImin}END_IF;";
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "set_out_do",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Сигналы AO"
        /// <summary>
        /// Экспорт "Сигналы AO"
        /// </summary>
        /// <returns></returns>
        private static bool ExportSignalsAO(object item)
        {
            ObservableCollection<SignalAO> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as SignalsAOUserControlViewModel
                                     where _TabItem is SignalsAOUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка выходных аналоговых параметров =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Signal.Description))
                {
                    flAllowedPrint = true;
                    fNum += $"(* =========================================== {_Param.Signal.Description} =========================================== *)\n\r";

                    var VarName = _Param.Signal.VarName;

                    fNum += VarName + $".enable := TRUE;\n";
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_ao",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Диагностика"
        /// <summary>
        /// Экспорт "Диагностика"
        /// </summary>
        /// <returns></returns>
        private static bool ExportDiagnostics(object item)
        {
            ObservableCollection<SignalAO> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as SignalsAOUserControlViewModel
                                     where _TabItem is SignalsAOUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Получение диагностических данных =========================================== *)\n\r";

            #region Формируем данные

            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_diagnostics",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Секции шин"
        /// <summary>
        /// Экспорт "Секции шин"
        /// </summary>
        /// <returns></returns>
        private static bool ExportEC(object item)
        {
            ObservableCollection<BaseParam> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as ECUserControlViewModel
                                     where _TabItem is ECUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка сигналов секций шин =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Description))
                {
                    flAllowedPrint = true;
                    var TypeSignal = TextToInt(_Param.TypeSignal);
                    var Address = TextToInt(_Param.Address);
                    var Inv = TextToBool(_Param.Inv);

                    fNum += GenInSignal(TypeSignal, Address, _Param.VarName, Inv);
                    fNum += $"\t (* {_Param.Description} *)\n";
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_ec",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Группы сигналов"
        /// <summary>
        /// Экспорт "Группы сигналов"
        /// </summary>
        /// <returns></returns>
        private static bool ExportGroupSignal(object item)
        {
            ObservableCollection<GroupSignal> Params = new();
            ObservableCollection<BaseParam> ParParams = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as GroupsSignalUserControlViewModel
                                     where _TabItem is GroupsSignalUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as SignalsGroupUserControlViewModel
                                     where _TabItem is SignalsGroupUserControlViewModel
                                     select _TabItem)
                ParParams = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка сигналов групп =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Param.Description))
                {
                    fNum += $"(* ======== {_Param.Param.Description} ======== *)\n";

                    flAllowedPrint = true;
                    var qty = TextToInt(_Param.QtyInGroup);
                    var fst = TextToInt(_Param.AddressStart) - 1;
                    var lst = TextToInt(_Param.AddressEnd);
                    fNum += "i := 0;\n";

                    for (int i = fst; i < lst; i++)
                    {
                        var TypeSignal = TextToInt(ParParams[i].TypeSignal);
                        var Address = TextToInt(ParParams[i].Address);
                        var Inv = TextToBool(ParParams[i].Inv);
                        fNum += $"{GenInSignal(TypeSignal, Address, "flTmp[1]", Inv)}\n";
                        fNum += $"i := i + bool_to_int(flTmp[1]);\n";
                    }

                    fNum += $"{_Param.Param.VarName} := (i >= {qty});\n";
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_siggrp",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Рамки УСО"
        /// <summary>
        /// Экспорт "Рамки УСО"
        /// </summary>
        /// <returns></returns>
        private static bool ExportFrameUSO(object item)
        {
            ObservableCollection<GroupSignal> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as GroupsSignalUserControlViewModel
                                     where _TabItem is GroupsSignalUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка рамок для УСО =========================================== *)\n\r";

            #region Формируем данные

            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_frame_uso",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Рамки"
        /// <summary>
        /// Экспорт "Рамки"
        /// </summary>
        /// <returns></returns>
        private static bool ExportFrame(object item)
        {
            ObservableCollection<GroupSignal> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as GroupsSignalUserControlViewModel
                                     where _TabItem is GroupsSignalUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка рамок для объектов =========================================== *)\n\r";

            #region Формируем данные

            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_frame_object",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Карта готовностей агрегатов (Лист 1)"
        /// <summary>
        /// Экспорт "Карта готовностей агрегатов (Лист 1)"
        /// </summary>
        /// <returns></returns>
        private static bool ExportKGMPNA(object item)
        {
            ObservableCollection<BaseUMPNA> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UMPNAUserControlViewModel
                                     where _TabItem is UMPNAUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* ============================= Обработка параметров карты готовности насосных агрегатов ============================= *)\n\r";

            #region Формируем данные
            foreach (var _UMPNA in Params)
            {
                foreach (var _Param in _UMPNA.KGMPNA)
                {
                    if (!string.IsNullOrWhiteSpace(_Param.Param.Description))
                    {
                        flAllowedPrint = true;
                        fNum += $"(* ============================= {_Param.Param.Description} ============================= *)\n";

                        var TypeSignal = TextToInt(_Param.Param.TypeSignal);
                        var Address = TextToInt(_Param.Param.Address);
                        var Inv = TextToBool(_Param.Param.Inv);

                        fNum += $"{_Param.Param.VarName}.enable := TRUE;\n";
                        fNum += $"{_Param.Param.VarName}.NotMasked := {TextToSbool(_Param.NoMasked)};\n";

                        fNum += $"{GenInSignal(TypeSignal, Address, $"{_Param.Param.VarName}.Input", Inv)}\n\r";
                    }
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_kgmpna",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Общестанционные защиты (Лист 2)"
        /// <summary>
        /// Экспорт "Общестанционные защиты (Лист 2)"
        /// </summary>
        /// <returns></returns>
        private static bool ExportKTPR(object item)
        {
            ObservableCollection<BaseKTPR> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as KTPRUserControlViewModel
                                     where _TabItem is KTPRUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* ============================= Обработка параметров общестанционных защит ============================= *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Param.Description))
                {
                    flAllowedPrint = true;
                    fNum += $"(* ============================= {_Param.Param.Description} ============================= *)\n";

                    var TypeSignal = TextToInt(_Param.Param.TypeSignal);
                    var Address = TextToInt(_Param.Param.Address);
                    var Inv = TextToBool(_Param.Param.Inv);

                    fNum += $"{_Param.Param.VarName}.enable := TRUE;\n";
                    fNum += $"{_Param.Param.VarName}.NotMasked := {TextToSbool(_Param.NoMasked)};\n";
                    fNum += $"{_Param.Param.VarName}.AutoDeblock := {TextToSbool(_Param.Autodeblok)};\n";
                    fNum += $"{_Param.Param.VarName}.station_state := {TextToSint(_Param.StateStation)};\n";
                    fNum += $"{_Param.Param.VarName}.Group := {TextToSint(_Param.Shoulder)};\n";
                    fNum += $"{_Param.Param.VarName}.SubGroup := {TextToSint(_Param.SubShoulder)};\n";
                    fNum += $"{_Param.Param.VarName}.type_prot := {TextToSint(_Param.Type)};\n";

                    _Param.StopTypeNS ??= "0, 0";
                    var Qty = _Param.StopTypeNS.Split(',');
                    for (int i = 0; i < Qty.Length; i++)
                    {
                        fNum += i switch
                        {
                            0 => $"{_Param.Param.VarName}.NS_StopType := {TextToSint(Qty[i])};\n",
                            1 => $"{_Param.Param.VarName}.NS_SubStopType := {TextToSint(Qty[i])};\n",
                            _ => $"\n"
                        };
                    }

                    _Param.StopTypeUMPNA ??= "0, 0";
                    Qty = _Param.StopTypeUMPNA.Split(',');
                    for (int i = 0; i < Qty.Length; i++)
                    {
                        fNum += i switch
                        {
                            0 => $"{_Param.Param.VarName}.NA_StopType := {TextToSint(Qty[i])};\n",
                            1 => $"{_Param.Param.VarName}.NA_SubStopType := {TextToSint(Qty[i])};\n",
                            _ => $"\n"
                        };
                    }
                    fNum += $"{GenInSignal(TypeSignal, Address, $"{_Param.Param.VarName}.Input", Inv)}\n\r";
                    fNum += $"{Controls(_Param.Param.VarName, _Param.ControlUZD, _Param.ControlUVS, _Param.ControlUTS)}\n\r";
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_ktpr",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Агрегатные защиты (Лист 3)"
        /// <summary>
        /// Экспорт "Агрегатные защиты (Лист 3)"
        /// </summary>
        /// <returns></returns>
        private static bool ExportKTPRA(object item)
        {
            ObservableCollection<BaseUMPNA> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UMPNAUserControlViewModel
                                     where _TabItem is UMPNAUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* ============================= Обработка параметров агрегатных защит ============================= *)\n\r";

            #region Формируем данные
            foreach (var _UMPNA in Params)
            {
                foreach (var _Param in _UMPNA.KTPRA)
                {
                    if (!string.IsNullOrWhiteSpace(_Param.Param.Description))
                    {
                        flAllowedPrint = true;
                        fNum += $"(* ============================= {_Param.Param.Description} ============================= *)\n";

                        var TypeSignal = TextToInt(_Param.Param.TypeSignal);
                        var Address = TextToInt(_Param.Param.Address);
                        var Inv = TextToBool(_Param.Param.Inv);

                        fNum += $"{_Param.Param.VarName}.enable := TRUE;\n";
                        fNum += $"{_Param.Param.VarName}.NotMasked := {TextToSbool(_Param.NoMasked)};\n";
                        fNum += $"{_Param.Param.VarName}.NoAVR := {(TextToBool(_Param.AVR) ? "FALSE" : "TRUE")};\n";
                        fNum += $"{_Param.Param.VarName}.VVOffExt := {TextToSbool(_Param.Type)};\n";
                        fNum += $"{_Param.Param.VarName}.na_state := {TextToSint(_Param.StateUMPNA)};\n";
                        fNum += $"{_Param.Param.VarName}.NA_StopType := {TextToSint(_Param.StopType)};\n";

                        fNum += $"{GenInSignal(TypeSignal, Address, $"{_Param.Param.VarName}.Input", Inv)}\n\r";
                        fNum += $"{Controls(_Param.Param.VarName, _Param.ControlUZD, _Param.ControlUVS)}\n\r";
                    }
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_ktpra",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Агрегатные предупреждения (Лист 3,5)"
        /// <summary>
        /// Экспорт "Агрегатные предупреждения (Лист 3,5)"
        /// </summary>
        /// <returns></returns>
        private static bool ExportKTPRAS(object item)
        {
            ObservableCollection<BaseUMPNA> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UMPNAUserControlViewModel
                                     where _TabItem is UMPNAUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* ============================= Обработка предельных параметров агрегатных защит ============================= *)\n\r";

            #region Формируем данные
            foreach (var _UMPNA in Params)
            {
                foreach (var _Param in _UMPNA.KTPRAS)
                {
                    if (!string.IsNullOrWhiteSpace(_Param.Param.Description))
                    {
                        flAllowedPrint = true;
                        fNum += $"(* ============================= {_Param.Param.Description} ============================= *)\n";

                        var TypeSignal = TextToInt(_Param.Param.TypeSignal);
                        var Address = TextToInt(_Param.Param.Address);
                        var Inv = TextToBool(_Param.Param.Inv);

                        fNum += $"{_Param.Param.VarName}.enable := TRUE;\n";
                        fNum += $"{_Param.Param.VarName}.VVOffExt := {TextToSbool(_Param.Type)};\n";
                        fNum += $"{_Param.Param.VarName}.na_state := {TextToSint(_Param.StateUMPNA)};\n";
                        fNum += $"{_Param.Param.VarName}.type_warning := {TextToSint(_Param.TypeWarning)};\n";

                        fNum += $"{GenInSignal(TypeSignal, Address, $"{_Param.Param.VarName}.Input", Inv)}\n\r";
                    }
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_ktpras",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Предельные параметры (Лист 4)"
        /// <summary>
        /// Экспорт "Предельные параметры (Лист 4)"
        /// </summary>
        /// <returns></returns>
        private static bool ExportKTPRS(object item)
        {
            ObservableCollection<BaseKTPRS> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as KTPRSUserControlViewModel
                                     where _TabItem is KTPRSUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* ============================= Обработка предельных параметров общестанционныз защит ============================= *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Param.Description))
                {
                    flAllowedPrint = true;
                    fNum += $"(* ============================= {_Param.Param.Description} ============================= *)\n";

                    var TypeSignal = TextToInt(_Param.Param.TypeSignal);
                    var Address = TextToInt(_Param.Param.Address);
                    var Inv = TextToBool(_Param.Param.Inv);

                    fNum += $"{_Param.Param.VarName}.enable := TRUE;\n";
                    fNum += $"{_Param.Param.VarName}.type_warning := {TextToSint(_Param.TypeWarning)};\n";
                    fNum += $"{_Param.Param.VarName}.type_prot := {TextToSint(_Param.Type)};\n";

                    fNum += $"{GenInSignal(TypeSignal, Address, $"{_Param.Param.VarName}.Input", Inv)}\n\r";
                    fNum += $"{Controls(_Param.Param.VarName, _Param.ControlUZD, _Param.ControlUVS, _Param.ControlUTS)}\n\r";
                }
            }
            #endregion


            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_ktprs",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Лист 5"
        /// <summary>
        /// Экспорт "Лист 5"
        /// </summary>
        /// <returns></returns>
        private static bool ExportLIST5(object item)
        {
            ObservableCollection<BaseSignaling> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as SignalingUserControlViewModel
                                     where _TabItem is SignalingUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* ============================= Обработка общих сигналов системы диагностики ============================= *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Param.Description))
                {
                    fNum += $"(* ============================= {_Param.Param.Description} ============================= *)\n";

                    var TypeSignal = TextToInt(_Param.Param.TypeSignal);
                    var Address = TextToInt(_Param.Param.Address);
                    var Inv = TextToBool(_Param.Param.Inv);

                    fNum += $"{_Param.Param.VarName}.enable := TRUE;\n";
                    fNum += $"{_Param.Param.VarName}.num_uso := {TextToSint(_Param.IndexUSO)};\n";
                    fNum += $"{_Param.Param.VarName}.type_warning := {TextToSint(_Param.TypeWarning)};\n";

                    fNum += _Param.Color switch
                    {
                        "Краный" => $"{_Param.Param.VarName}.warning_color := 3;\n",
                        "Желтый" => $"{_Param.Param.VarName}.warning_color := 2;\n",
                        "Зеленый" => $"{_Param.Param.VarName}.warning_color := 1;\n",
                        _ => $"{_Param.Param.VarName}.warning_color := 0;\n",
                    };

                    fNum += $"{GenInSignal(TypeSignal, Address, $"{_Param.Param.VarName}.Input", Inv)}\n\r";
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_list5",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "DI агрегатов"
        /// <summary>
        /// Экспорт "DI агрегатов"
        /// </summary>
        /// <returns></returns>
        private static bool ExportUMPNA_DI(object item)
        {
            ObservableCollection<BaseUMPNA> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UMPNAUserControlViewModel
                                     where _TabItem is UMPNAUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка входных сигналов НА =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Index))
                {
                    flAllowedPrint = true;
                    fNum += $"(* =========================================== {_Param.Description} =========================================== *)\n\r";

                    var VarName = _Param.VarName;

                    fNum += VarName + $".using_mcp := {TextToSbool(_Param.UsedMCP)};\n";
                    fNum += VarName + $".using_kpd := {TextToSbool(_Param.UsedKPD)};\n";
                    fNum += VarName + $".using_local_oil := {TextToSbool(_Param.IndexGroupMS)};\n";
                    fNum += VarName + $".IsNM := {TextToSbool(_Param.TypeUMPNA)};\n";
                    fNum += VarName + $".num_zd_in := {TextToSint(_Param.IndexPZ)};\n";
                    fNum += VarName + $".num_zd_out := {TextToSint(_Param.IndexVZ)};\n";
                    fNum += VarName + $".num_grp_oil := {TextToSint(_Param.IndexGroupMS)};\n";
                    fNum += VarName + $".KKCCount := {TextToSint(_Param.CountButtonStop)};\n\r";

                    foreach (var _InputParam in _Param.InputParam)
                    {
                        if (!string.IsNullOrWhiteSpace(_InputParam.Address))
                        {
                            var TypeSignal = TextToInt(_InputParam.TypeSignal);
                            var Address = TextToInt(_InputParam.Address);
                            var Inv = TextToBool(_InputParam.Inv);
                            var ParamName = _InputParam.VarName;


                            fNum += $"{GenInSignal(TypeSignal, Address, ParamName, Inv)}\t(* {_InputParam.Description} *)\n";
                        }
                    }
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_umpna",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "DI задвижек"
        /// <summary>
        /// Экспорт "DI задвижек"
        /// </summary>
        /// <returns></returns>
        private static bool ExportUZD_DI(object item)
        {
            ObservableCollection<BaseUZD> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UZDUserControlViewModel
                                     where _TabItem is UZDUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка входных сигналов задвижек =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Index))
                {
                    flAllowedPrint = true;
                    fNum += $"(* =========================================== {_Param.Description} =========================================== *)\n\r";

                    var VarName = _Param.VarName;

                    fNum += VarName + $".enable := TRUE;\n";
                    fNum += VarName + $".num_ec := {TextToSint(_Param.IndexEC)};\n";
                    fNum += VarName + $".num_grp := {TextToSint(_Param.IndexGroup)};\n";
                    fNum += VarName + $".zd_type := {TextToSint(_Param.TypeZD)};\n";
                    fNum += VarName + $".num_pz := {TextToSint(_Param.IndexPZ)};\n";
                    fNum += VarName + $".num_bd := {TextToSint(_Param.IndexBD)};\n";
                    fNum += VarName + $"CFG.slDist := {TextToSbool(_Param.Dist)};\n";
                    fNum += VarName + $"CFG.sl2Stop := {TextToSbool(_Param.DoubleStop)};\n";
                    fNum += VarName + $"CFG.slBUR := {TextToSbool(_Param.Bur)};\n";
                    fNum += VarName + $"CFG.slCO := {TextToSbool(_Param.COz)};\n";
                    fNum += VarName + $"CFG.slCZ := {TextToSbool(_Param.CZz)};\n";
                    fNum += VarName + $"CFG.slYesEC := {TextToSbool(_Param.EC)};\n";
                    fNum += VarName + $"CFG.slCheckState := {TextToSbool(_Param.CheckState)};\n";
                    fNum += VarName + $"CFG.RS_OFF := {TextToSbool(_Param.RsOff)};\n\r";

                    foreach (var _InputParam in _Param.InputParam)
                    {
                        if (!string.IsNullOrWhiteSpace(_InputParam.Address))
                        {
                            var TypeSignal = TextToInt(_InputParam.TypeSignal);
                            var Address = TextToInt(_InputParam.Address);
                            var Inv = TextToBool(_InputParam.Inv);
                            var ParamName = _InputParam.VarName;


                            fNum += $"{GenInSignal(TypeSignal, Address, ParamName, Inv)}\t(* {_InputParam.Description} *)\n";
                        }
                    }
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_uzd",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "DI вспомсистем"
        /// <summary>
        /// Экспорт "DI вспомсистем"
        /// </summary>
        /// <returns></returns>
        private static bool ExportUVS_DI(object item)
        {
            ObservableCollection<BaseUVS> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UVSUserControlViewModel
                                     where _TabItem is UVSUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка входных сигналов вспомсистем =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Index))
                {
                    flAllowedPrint = true;
                    fNum += $"(* =========================================== {_Param.Description} =========================================== *)\n\r";

                    var VarName = _Param.VarName;
                    var VarNameGrp = $"uvsgrp_param[{_Param.DescriptionGroup}]";

                    fNum += VarName + $".enable := TRUE;\n";
                    fNum += VarName + $".pc_use := {TextToSbool(_Param.TypePressure)};\n";
                    fNum += VarName + $".opc_use := {TextToSbool(_Param.COz)};\n";
                    fNum += VarName + $".num_ec := {TextToSint(_Param.IndexEC)};\n";
                    fNum += VarName + $".num_grp := {TextToSint(_Param.IndexGroup)};\n\r";

                    if (!string.IsNullOrWhiteSpace(_Param.DescriptionGroup))
                    {
                        fNum += VarNameGrp + $".enable := TRUE;\n";
                        fNum += VarNameGrp + $".one_pc_use := {TextToSbool(_Param.OnePressureSensorGroup)};\n";
                        fNum += VarNameGrp + $".vsgrp_type := {TextToSint(_Param.TypeGroup)};\n";
                        fNum += VarNameGrp + $".pz_word := {TextToSint(_Param.Index)};\n";
                        fNum += VarNameGrp + $".AvailReserve := {TextToSbool(_Param.Reservable)};\n\r";
                    }

                    foreach (var _InputParam in _Param.InputParam)
                    {
                        if (!string.IsNullOrWhiteSpace(_InputParam.Address))
                        {
                            var TypeSignal = TextToInt(_InputParam.TypeSignal);
                            var Address = TextToInt(_InputParam.Address);
                            var Inv = TextToBool(_InputParam.Inv);
                            var ParamName = _InputParam.VarName;


                            fNum += $"{GenInSignal(TypeSignal, Address, ParamName, Inv)}\t(* {_InputParam.Description} *)\n";
                        }
                    }
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_uvs",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "Параметры DO остальных"
        /// <summary>
        /// Экспорт "Параметры DO остальных"
        /// </summary>
        /// <returns></returns>
        private static bool ExportDO_Param(object item)
        {
            ObservableCollection<BaseUTS> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UTSUserControlViewModel
                                     where _TabItem is UTSUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка общесистемных выходных параметров =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Param.Index) && !string.IsNullOrWhiteSpace(_Param.Param.Address))
                {
                    flAllowedPrint = true;
                    fNum += $"(* =========================================== {_Param.Param.Description} =========================================== *)\n\r";

                    var VarName = _Param.Param.VarName;
                    var Inv = TextToBool(_Param.Param.Inv);

                    var KCOAddress = TextToInt(_Param.KCO.Address);

                    var SODAddress = TextToInt(_Param.SignalSOD.Address);
                    var SODTypeSignal = TextToInt(_Param.Param.TypeSignal);

                    var SODErrAddress = TextToInt(_Param.SignalErrSOD.Address);
                    var SODErrTypeSignal = TextToInt(_Param.Param.TypeSignal);

                    fNum += VarName + $".enable := TRUE;\n";
                    fNum += VarName + $".EnableCVCheck := {TextToSbool(_Param.TypeCOz)};\n";
                    fNum += VarName + $".APT_OFF := {TextToSbool(_Param.AptOff)};\n";
                    fNum += VarName + $".uts_type := {TextToSint(_Param.Type)};\n";

                    fNum += VarName + $".num_grp := {TextToSint(_Param.IndexGroup)};\n";
                    fNum += VarName + $".num_pz := {TextToSint(_Param.IndexPZ)};\n";
                    fNum += VarName + $".TypeCV := {TextToSint(_Param.TypeCOz)};\n";
                    fNum += VarName + $".LockEnable := {TextToSint(_Param.IndexGroup)};\n\r";

                    fNum += $"{GenInSignal(21, KCOAddress, $"{VarName}.InCorrCV", Inv)}\n";
                    fNum += $"{GenInSignal(SODTypeSignal, SODAddress, $"{VarName}.InTriggerSOD", Inv)}\n";
                    fNum += $"{GenInSignal(SODErrTypeSignal, SODErrAddress, $"{VarName}.Err_SOD", Inv)}\n";
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "get_in_uts",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "DO агрегатов"
        /// <summary>
        /// Экспорт "DO агрегатов"
        /// </summary>
        /// <returns></returns>
        private static bool ExportUMPNA_DO(object item)
        {
            ObservableCollection<BaseUMPNA> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UMPNAUserControlViewModel
                                     where _TabItem is UMPNAUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка выходных сигналов насосных агрегатов =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Index))
                {
                    fNum += $"(* =========================================== {_Param.Description} =========================================== *)\n\r";

                    foreach (var _OutputParam in _Param.OutputParam)
                    {
                        if (!string.IsNullOrWhiteSpace(_OutputParam.Address))
                        {
                            flAllowedPrint = true;
                            var Address = TextToInt(_OutputParam.Address);
                            var ParamName = _OutputParam.VarName;
                            fNum += $"{GenOutSignal(Address, ParamName)}\t(* {_OutputParam.Description} *)\n";
                        }
                    }
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "set_out_umpna",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "DO задвижек"
        /// <summary>
        /// Экспорт "DO задвижек"
        /// </summary>
        /// <returns></returns>
        private static bool ExportUZD_DO(object item)
        {
            ObservableCollection<BaseUZD> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UZDUserControlViewModel
                                     where _TabItem is UZDUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка выходных сигналов задвижек =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Index))
                {
                    fNum += $"(* =========================================== {_Param.Description} =========================================== *)\n\r";

                    foreach (var _OutputParam in _Param.OutputParam)
                    {
                        if (!string.IsNullOrWhiteSpace(_OutputParam.Address))
                        {
                            flAllowedPrint = true;
                            var Address = TextToInt(_OutputParam.Address);
                            var ParamName = _OutputParam.VarName;
                            fNum += $"{GenOutSignal(Address, ParamName)}\t(* {_OutputParam.Description} *)\n";
                        }
                    }
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "set_out_uzd",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "DO вспомсистем"
        /// <summary>
        /// Экспорт "DO вспомсистем"
        /// </summary>
        /// <returns></returns>
        private static bool ExportUVS_DO(object item)
        {
            ObservableCollection<BaseUVS> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UVSUserControlViewModel
                                     where _TabItem is UVSUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка выходных сигналов вспомсистем =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Index))
                {
                    fNum += $"(* =========================================== {_Param.Description} =========================================== *)\n\r";

                    foreach (var _OutputParam in _Param.OutputParam)
                    {
                        if (!string.IsNullOrWhiteSpace(_OutputParam.Address))
                        {
                            flAllowedPrint = true;
                            var Address = TextToInt(_OutputParam.Address);
                            var ParamName = _OutputParam.VarName;
                            fNum += $"{GenOutSignal(Address, ParamName)}\t(* {_OutputParam.Description} *)\n";
                        }
                    }
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "set_out_uvs",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion

        #region Экспорт "DO остальные"
        /// <summary>
        /// Экспорт "DO остальные"
        /// </summary>
        /// <returns></returns>
        private static bool ExportDO_Others(object item)
        {
            ObservableCollection<BaseUTS> Params = new();
            IEnumerable<IViewModelUserControls> _ViewModelsUserControl = App.Services.GetRequiredService<IEnumerable<IViewModelUserControls>>();
            foreach (var _TabItem in from object _Item in _ViewModelsUserControl
                                     let _TabItem = _Item as UTSUserControlViewModel
                                     where _TabItem is UTSUserControlViewModel
                                     select _TabItem)
                Params = _TabItem.Params;

            var flAllowedPrint = false;
            var fNum = "(* =========================================== Обработка общесистемных выходных сигналов =========================================== *)\n\r";

            #region Формируем данные
            foreach (var _Param in Params)
            {
                if (!string.IsNullOrWhiteSpace(_Param.Param.Index) && !string.IsNullOrWhiteSpace(_Param.Param.Address))
                {
                    flAllowedPrint = true;
                    var Address = TextToInt(_Param.Param.Address);
                    var Index = TextToInt(_Param.Param.Index);
                    var VarName = _Param.Param.VarName;

                    fNum += Index switch
                    {
                        1 => $"NPS_STATE.RING_BRU := {GenOutSignal(Address, $"{VarName}.Uts.NU")}\t(* {_Param.Param.Description} *)\n",
                        2 => $"NPS_STATE.RING := {VarName}\t(* {_Param.Param.Description} *)\n",
                        _ => $"{GenOutSignal(Address, VarName)}\t(* {_Param.Param.Description} *)\n",
                    };
                }
            }
            #endregion

            if (!flAllowedPrint)
                return false;

            var FileViewer = new WindowTextEditor()
            {
                Title = "set_out_uvs",
                TextView = fNum,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            FileViewer.Show();
            return true;
        }
        #endregion
    }
}
