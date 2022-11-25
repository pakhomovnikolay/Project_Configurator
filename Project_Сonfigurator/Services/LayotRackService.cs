using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Project_Сonfigurator.Services
{
    public class LayotRackService : ILayotRackService
    {
        #region Обновление индексов модулей
        /// <summary>
        /// Обновление индексов модулей
        /// </summary>
        /// <param name="Modules"></param>
        /// <param name="IndexRack"></param>
        public void RefreshIndexModule(List<RackModule> Modules, int IndexRack)
        {
            var i = 0;
            foreach (var _Module in Modules)
            {
                _Module.Index = $"A{IndexRack}.{++i}";
            }
        }
        #endregion

        #region Обновление адресов модулей
        /// <summary>
        /// Обновление адресов модулей
        /// </summary>
        /// <param name="Modules"></param>
        /// <param name="IndexRack"></param>
        public void RefreshAddressModule(List<USO> USOList, int IndexRack = 0)
        {
            var VUIndexAI = 0;
            var VUIndexDI = 0;
            var VUIndexAO = 0;
            var VUIndexDO = 0;
            var index = 0;
            foreach (var _USO in USOList)
            {
                var r = new Random();
                _USO.Color = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 239), (byte)r.Next(1, 239), (byte)r.Next(1, 239))).ToString();

                foreach (var _Rack in _USO.Racks)
                {
                    index++;
                    var AddressAI = (index - 1) * 16 * 8;
                    var AddressAO = (index - 1) * 16 * 8;
                    var AddressDI = (index - 1) * 16;
                    var AddressDO = (index - 1) * 16;
                    var IndexAI = 1;
                    var IndexDI = 1;
                    var IndexAO = 1;
                    var IndexDO = 1;
                    foreach (var _Module in _Rack.Modules)
                    {
                        _Module.StartAddress = "";
                        _Module.EndAddress = "";
                        _Module.Channels = new List<Channel>();

                        switch (_Module.Type)
                        {
                            case TypeModule.AI:
                            case TypeModule.AO:
                                var VarName = _Module.Type == TypeModule.AI ? $"HW_AI" : $"HW_AO";
                                var VarNameVU = _Module.Type == TypeModule.AI ? $"HW_AI_VU" : $"HW_AO_VU";
                                var VUIndex = _Module.Type == TypeModule.AI ? VUIndexAI : VUIndexAO;
                                var address = _Module.Type == TypeModule.AI ? 0 : 300000;

                                _Module.StartAddress = _Module.Type == TypeModule.AI ?
                                    (AddressAI + (IndexAI - 1) * 8).ToString() :
                                    (AddressAO + (IndexAO - 1) * 8).ToString();

                                _Module.EndAddress = _Module.Type == TypeModule.AI ?
                                    (AddressAI + (IndexAI++ - 1) * 8 + 8 - 1).ToString() :
                                    (AddressAO + (IndexAO++ - 1) * 8 + 8 - 1).ToString();

                                for (int i = 0; i < 8; i++)
                                {
                                    var ch = new Channel()
                                    {
                                        Index = $"{i + 1}",
                                        Id = "",
                                        Description = "",
                                        VarName = $"{VarName}[{int.Parse(_Module.StartAddress) + i}]",
                                        Bit = "-",
                                        Address = $"{int.Parse(_Module.StartAddress) + i + address}",
                                        VarNameVU = $"{VarNameVU}[{VUIndex + i}]"
                                    };
                                    _Module.Channels.Add(ch);
                                }
                                if (_Module.Type == TypeModule.AI) VUIndexAI += 8;
                                else VUIndexAI += 8;
                                break;

                            case TypeModule.DI:
                            case TypeModule.DO:
                                VarName = _Module.Type == TypeModule.DI ? $"HW_DI" : $"HW_DO";
                                VarNameVU = _Module.Type == TypeModule.DI ? $"HW_DI_VU" : $"HW_DO_VU";
                                VUIndex = _Module.Type == TypeModule.DI ? VUIndexDI++ : VUIndexDO++;
                                address = _Module.Type == TypeModule.DI ? 100000 : 200000;

                                _Module.StartAddress = _Module.Type == TypeModule.DI ?
                                    (AddressDI + (IndexDI - 1)).ToString() :
                                    (AddressDO + (IndexDO - 1)).ToString();

                                _Module.EndAddress = _Module.Type == TypeModule.DI ?
                                    (AddressDI + (IndexDI++ - 1)).ToString() :
                                    (AddressDO + (IndexDO++ - 1)).ToString();

                                for (int i = 0; i < 32; i++)
                                {
                                    var ch = new Channel()
                                    {
                                        Index = $"{i + 1}",
                                        Id = "",
                                        Description = "",
                                        VarName = $"{VarName}[{int.Parse(_Module.StartAddress) + i}]",
                                        Bit = $"{i}",
                                        Address = $"{int.Parse(_Module.StartAddress) * 32 + i + address}",
                                        VarNameVU = $"{VarNameVU}[{VUIndex + i}]"
                                    };
                                    _Module.Channels.Add(ch);
                                }
                                if (_Module.Type == TypeModule.AI) VUIndexAI += 8;
                                else VUIndexAI += 8;
                                break;

                            case TypeModule.DA:
                                break;
                        }
                    }
                }
            }

            //var VUIndexAI = 0;
            //var VUIndexDI = 0;
            //var VUIndexAO = 0;
            //var VUIndexDO = 0;
            //foreach (var Rack in Racks)
            //{
            //    var AddressAI = (Rack.Index - 1) * 16 * 8;
            //    var AddressAO = (Rack.Index - 1) * 16 * 8;
            //    var AddressDI = (Rack.Index - 1) * 16;
            //    var AddressDO = (Rack.Index - 1) * 16;
            //    var IndexAI = 1;
            //    var IndexDI = 1;
            //    var IndexAO = 1;
            //    var IndexDO = 1;

            //    foreach (var _Module in Rack.Modules)
            //    {
            //        if (string.IsNullOrWhiteSpace(_Module.Name))
            //        {
            //            _Module.StartAddress = "";
            //            _Module.EndAddress = "";
            //            _Module.Channels = new();
            //            continue;
            //        }

            //        switch (_Module.Type)
            //        {
            //            case TypeModule.Unknown:
            //                _Module.StartAddress = "";
            //                _Module.EndAddress = "";
            //                break;
            //            case TypeModule.AI:
            //            case TypeModule.AO:
            //                var VarName = _Module.Type == TypeModule.AI ? $"HW_AI" : $"HW_AO";
            //                var VarNameVU = _Module.Type == TypeModule.AI ? $"HW_AI_VU" : $"HW_AO_VU";
            //                var VUIndex = _Module.Type == TypeModule.AI ? VUIndexAI : VUIndexAO;
            //                var address = _Module.Type == TypeModule.AI ? 0 : 300000;

            //                _Module.StartAddress = _Module.Type == TypeModule.AI ?
            //                    (AddressAI + (IndexAI - 1) * 8).ToString() :
            //                    (AddressAO + (IndexAO - 1) * 8).ToString();

            //                _Module.EndAddress = _Module.Type == TypeModule.AI ?
            //                    (AddressAI + (IndexAI++ - 1) * 8 + 8 - 1).ToString() :
            //                    (AddressAO + (IndexAO++ - 1) * 8 + 8 - 1).ToString();

            //                _Module.Channels = new List<Channel>();

            //                for (int i = 0; i < 8; i++)
            //                {
            //                    _Module.Channels.Add(new Channel()
            //                    {
            //                        Index = i + 1,
            //                        Id = "",
            //                        Name = "",
            //                        VarName = $"{VarName}[{int.Parse(_Module.StartAddress) + i}]",
            //                        Bit = "-",
            //                        Address = $"{int.Parse(_Module.StartAddress) + i + address}",
            //                        VarNameVU = $"{VarNameVU}[{VUIndex + i}]"
            //                    });
            //                }
            //                if (_Module.Type == TypeModule.AI)
            //                    VUIndexAI += 8;
            //                else
            //                    VUIndexAO += 8;
            //                break;

            //            case TypeModule.DI:
            //            case TypeModule.DO:
            //                VarName = _Module.Type == TypeModule.DI ? $"HW_DI" : $"HW_DO";
            //                VarNameVU = _Module.Type == TypeModule.DI ? $"HW_DI_VU" : $"HW_DO_VU";
            //                VUIndex = _Module.Type == TypeModule.DI ? VUIndexDI++ : VUIndexDO++;
            //                address = _Module.Type == TypeModule.DI ? 100000 : 200000;

            //                _Module.StartAddress = _Module.Type == TypeModule.DI ?
            //                    (AddressDI + (IndexDI - 1)).ToString() :
            //                    (AddressDO + (IndexDO - 1)).ToString();

            //                _Module.EndAddress = _Module.Type == TypeModule.DI ?
            //                    (AddressDI + (IndexDI++ - 1)).ToString() :
            //                    (AddressDO + (IndexDO++ - 1)).ToString();

            //                _Module.Channels = new List<Channel>();
            //                for (int i = 0; i < 32; i++)
            //                {
            //                    _Module.Channels.Add(new Channel()
            //                    {
            //                        Index = i + 1,
            //                        Id = "",
            //                        Name = "",
            //                        VarName = $"{VarName}[{_Module.StartAddress}]",
            //                        Bit = $"{_Module.Channels.Count}",
            //                        Address = $"{int.Parse(_Module.StartAddress) * 32 + i + address}",
            //                        VarNameVU = $"{VarNameVU}[{VUIndex}]"
            //                    });
            //                }
            //                break;
            //        }
            //    }
            //}
        }
        #endregion
    }
}
