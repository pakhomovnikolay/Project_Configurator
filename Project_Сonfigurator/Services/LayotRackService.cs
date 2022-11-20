using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Project_Сonfigurator.Services
{
    public class LayotRackService : ILayotRackService
    {
        #region Обновление корзин
        /// <summary>
        /// Обновление корзин
        /// </summary>
        /// <param name="Racks"></param>
        public void RefreshRack(List<Rack> Racks)
        {
            RefreshIndexModule(Racks);
            RefreshAddressModule(Racks);
        }
        #endregion

        #region Обновление индексов модулей
        /// <summary>
        /// Обновление индексов модулей
        /// </summary>
        /// <param name="Racks"></param>
        public void RefreshIndexModule(List<Rack> Racks)
        {
            var NameUSO = "";
            var IndexRack = 0;
            var IndexUSO = 0;
            Brush ColorUSO = Brushes.LightBlue;

            foreach (var _Rack in Racks)
            {
                if (_Rack.NameUSO != NameUSO)
                {
                    IndexRack = 1;
                    NameUSO = _Rack.NameUSO;
                    _Rack.Name = $"A{IndexRack}";
                    _Rack.IndexUSO = ++IndexUSO;
                }
                else if (_Rack.NameUSO == NameUSO)
                {
                    _Rack.Name = $"A{++IndexRack}";
                    _Rack.IndexUSO = IndexUSO;
                }
            }

            NameUSO = "";
            foreach (var _Rack in Racks)
            {
                if (_Rack.NameUSO != NameUSO)
                {
                    var r = new Random();
                    ColorUSO = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 239), (byte)r.Next(1, 239), (byte)r.Next(1, 239)));
                }
                NameUSO = _Rack.NameUSO;


                var i = 0;
                foreach (var _Module in _Rack.Modules)
                {
                    _Module.Index = $"{_Rack.Name}.{++i}";
                    _Module.NameUSO = _Rack.NameUSO;
                    _Module.ColorUSO = ColorUSO.ToString();
                }
            }
        }
        #endregion

        #region Обновление адресов модулей
        /// <summary>
        /// Обновление адресов модулей
        /// </summary>
        /// <param name="Racks"></param>
        public void RefreshAddressModule(List<Rack> Racks)
        {
            var VUIndexAI = 0;
            var VUIndexDI = 0;
            var VUIndexAO = 0;
            var VUIndexDO = 0;
            foreach (var Rack in Racks)
            {
                var AddressAI = (Rack.Index - 1) * 16 * 8;
                var AddressAO = (Rack.Index - 1) * 16 * 8;
                var AddressDI = (Rack.Index - 1) * 16;
                var AddressDO = (Rack.Index - 1) * 16;
                var IndexAI = 1;
                var IndexDI = 1;
                var IndexAO = 1;
                var IndexDO = 1;

                foreach (var _Module in Rack.Modules)
                {
                    
                    if (string.IsNullOrWhiteSpace(_Module.Name)) continue;


                    switch (_Module.Type)
                    {
                        case TypeModule.Unknown:
                            _Module.StartAddress = "";
                            _Module.EndAddress = "";
                            break;
                        case TypeModule.AI:
                        case TypeModule.AO:
                            var VarName = _Module.Type == TypeModule.AI ? $"HW_AI" : $"HW_AO";
                            var VarNameVU = _Module.Type == TypeModule.AI ? $"HW_AI_VU" : $"HW_AO_VU";
                            var VUIndex = _Module.Type == TypeModule.AI ? VUIndexAI++ : VUIndexAO++;

                            _Module.StartAddress = _Module.Type == TypeModule.AI ?
                                (AddressAI + (IndexAI - 1) * 8).ToString():
                                (AddressAO + (IndexAO - 1) * 8).ToString();

                            _Module.EndAddress = _Module.Type == TypeModule.AI ?
                                (AddressAI + (IndexAI - 1) * 8 + 8 - 1).ToString():
                                (AddressAO + (IndexAO - 1) * 8 + 8 - 1).ToString();

                            for (int i = 0; i < 8; i++)
                            {
                                _Module.Channels.Add(new Channel()
                                {
                                    Index = i + 1,
                                    Id = "",
                                    Name= "",
                                    VarName = $"{VarName}[{i + 1}]",
                                    Bit = "-",
                                    Address = $"{_Module.StartAddress + i}",
                                    VarNameVU = $"{VarNameVU}[{VUIndex + 1}]"
                                });
                            
                            }
                            break;
                        case TypeModule.DI:
                        case TypeModule.DO:
                            VarName = _Module.Type == TypeModule.DI ? $"HW_DI" : $"HW_DO";
                            VarNameVU = _Module.Type == TypeModule.DI ? $"HW_DI_VU" : $"HW_DO_VU";
                            VUIndex = _Module.Type == TypeModule.DI ? VUIndexDI++ : VUIndexDO++;

                            _Module.StartAddress = _Module.Type == TypeModule.DI ?
                                (AddressDI + (IndexDI - 1)).ToString():
                                (AddressDO + (IndexDO - 1)).ToString();

                            _Module.EndAddress = _Module.Type == TypeModule.DI ?
                                (AddressDI + (IndexDI - 1)).ToString():
                                (AddressDO + (IndexDO - 1)).ToString();

                            for (int i = 0; i < 32; i++)
                            {
                                _Module.Channels.Add(new Channel()
                                {
                                    Index = i + 1,
                                    Id = "",
                                    Name = "",
                                    VarName = $"{VarName}[{i + 1}]",
                                    Bit = "-",
                                    Address = $"{_Module.StartAddress + i}",
                                    VarNameVU = $"{VarNameVU}[{VUIndex + 1}]"
                                });

                            }
                            break;

                    }
                }

            }
        }
        #endregion
    }
}
