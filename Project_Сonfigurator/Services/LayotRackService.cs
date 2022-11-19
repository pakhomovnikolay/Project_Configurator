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
                    _Module.ColorRack = ColorUSO.ToString();
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
                    _Module.Type = TypeModule.Unknown;
                    _Module.StartAddress = "";
                    _Module.EndAddress = "";
                    _Module.ChannelCount = 0;
                    _Module.Height = 0;
                    _Module.Channels = new List<int>();
                    _Module.Signals = new List<Signal>();
                    _Module.VisibilityAnalogs = Visibility.Collapsed;
                    _Module.VisibilityDiscrets = Visibility.Collapsed;
                    if (string.IsNullOrWhiteSpace(_Module.Name)) continue;


                    if (_Module.Name.Contains("ai", StringComparison.CurrentCultureIgnoreCase) || _Module.Name.Contains("ami", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _Module.ChannelCount = 8;
                        _Module.Height = (_Module.ChannelCount * 25);
                        _Module.Type = TypeModule.AI;
                        _Module.StartAddress = (AddressAI + (IndexAI - 1) * 8).ToString();
                        _Module.EndAddress = (AddressAI + (IndexAI - 1) * 8 + 8 - 1).ToString();
                        _Module.VisibilityAnalogs = Visibility.Visible;

                        for (int i = 0; i < _Module.ChannelCount; i++)
                        {
                            _Module.Channels.Add(i + 1);
                            _Module.Signals.Add(new Signal()
                            {
                                Id = "",
                                Description = "",
                                Address = $"HW_AI[{_Module.StartAddress + i}]",
                                Bit = "-",
                                Index = int.Parse(_Module.StartAddress) + i,
                                AddressVU = $"HW_AI_VU[{VUIndexAI++ + i}]"
                            });
                        }
                        IndexAI++;
                    }

                    if (_Module.Name.Contains("ao", StringComparison.CurrentCultureIgnoreCase) || _Module.Name.Contains("amo", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _Module.ChannelCount = 8;
                        _Module.Height = (_Module.ChannelCount * 25);
                        _Module.Type = TypeModule.AO;
                        _Module.StartAddress = (AddressAO + (IndexAO - 1) * 8).ToString();
                        _Module.EndAddress = (AddressAO + (IndexAO - 1) * 8 + 8 - 1).ToString();
                        _Module.VisibilityAnalogs = Visibility.Visible;

                        for (int i = 0; i < _Module.ChannelCount; i++)
                        {
                            _Module.Channels.Add(i + 1);
                            _Module.Signals.Add(new Signal()
                            {
                                Id = "",
                                Description = "",
                                Address = $"HW_AO[{_Module.StartAddress + i}]",
                                Bit = "-",
                                Index = 300000 + int.Parse(_Module.StartAddress) + i,
                                AddressVU = $"HW_AO_VU[{VUIndexAO++ + i}]"
                            });
                        }
                        IndexAO++;
                    }

                    if (_Module.Name.Contains("di", StringComparison.CurrentCultureIgnoreCase) || _Module.Name.Contains("ddi", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _Module.ChannelCount = 32;
                        _Module.Height = (_Module.ChannelCount * 25);
                        _Module.Type = TypeModule.DI;
                        _Module.StartAddress = (AddressDI + (IndexDI - 1)).ToString();
                        _Module.EndAddress = (AddressDI + (IndexDI - 1)).ToString();
                        _Module.VisibilityDiscrets = Visibility.Visible;

                        for (int i = 0; i < _Module.ChannelCount; i++)
                        {
                            _Module.Channels.Add(i + 1);
                            _Module.Signals.Add(new Signal()
                            {
                                Id = "",
                                Description = "",
                                Address = $"HW_DI[{_Module.StartAddress}]",
                                Bit = i.ToString(),
                                Index = 100000 + int.Parse(_Module.StartAddress) * 32 + i,
                                AddressVU = $"HW_DI_VU[{VUIndexDI}]"
                            });
                        }
                        VUIndexDI++;
                        IndexDI++;
                    }

                    if (_Module.Name.Contains("do", StringComparison.CurrentCultureIgnoreCase) || _Module.Name.Contains("ddo", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _Module.ChannelCount = 32;
                        _Module.Height = (_Module.ChannelCount * 25);
                        _Module.Type = TypeModule.DO;
                        _Module.StartAddress = (AddressDO + (IndexDO - 1)).ToString();
                        _Module.EndAddress = (AddressDO + (IndexDO - 1)).ToString();
                        _Module.VisibilityDiscrets = Visibility.Visible;

                        for (int i = 0; i < _Module.ChannelCount; i++)
                        {
                            _Module.Channels.Add(i + 1);
                            _Module.Signals.Add(new Signal()
                            {
                                Id = "",
                                Description = "",
                                Address = $"HW_DO[{_Module.StartAddress}]",
                                Bit = i.ToString(),
                                Index = 200000 + int.Parse(_Module.StartAddress) * 32 + i,
                                AddressVU = $"HW_DO_VU[{VUIndexDO}]"
                            });
                        }
                        VUIndexDO++;
                        IndexDO++;
                    }
                }

            }
        }
        #endregion

        #region Наполнение списка модулей
        /// <summary>
        /// Наполнение списка модулей
        /// </summary>
        /// <param name="Racks"></param>
        public List<string> GetListModules(TypePLC typePLC)
        {
            List<string> _List = new();
            switch (typePLC)
            {
                #region PS
                case TypePLC.PS:
                    _List.Add("");
                    _List.Add("- - - - Оконечные модули - - - -");
                    _List.Add("R500 ST 02 011");
                    _List.Add("R500 ST 02 021");
                    _List.Add("R500 ST 02 111");
                    _List.Add("R500 ST 02 121");
                    _List.Add("R500 ST 00 001");
                    _List.Add("");
                    _List.Add("- - - - - - - - DI - - - - - - - -");
                    _List.Add("R500 DI 32 011");
                    _List.Add("");
                    _List.Add("- - - - - - - - DO - - - - - - - -");
                    _List.Add("R500 DO 32 011");
                    _List.Add("R500 DO 32 012");
                    _List.Add("");
                    _List.Add("- - - - - - - - AI - - - - - - - -");
                    _List.Add("R500 AI 08 041");
                    _List.Add("R500 AI 08 051");
                    _List.Add("");
                    _List.Add("- - - - - - - - AO - - - - - - - -");
                    _List.Add("R500 AO 08 011");
                    _List.Add("");
                    _List.Add("- - - - - - - - БП - - - - - - - -");
                    _List.Add("R500 PP 00 011");
                    _List.Add("");
                    _List.Add("- - - - - - CPU и прочее - - - - - -");
                    _List.Add("R500 CU 00 061");
                    _List.Add("R500 CU 00 051");
                    _List.Add("");
                    _List.Add("- - - - - - - - - CP - - - - - - - -");
                    _List.Add("R500 CP 04 011");
                    _List.Add("R500 CP 02 021");
                    _List.Add("- - - - - - - - - DA - - - - - - - -");
                    _List.Add("R500 DA 03 011");
                    break;
                #endregion

                #region SE
                case TypePLC.SE:
                    _List.Add("- - - - - - - БП - - - - - - -");
                    _List.Add("BMX CPS 2000");
                    _List.Add("BMX CPS 2010");
                    _List.Add("BMX CPS 3020");
                    _List.Add("BMX CPS 3500");
                    _List.Add("BMX CPS 3522");
                    _List.Add("BMX CPS 3540");
                    _List.Add("BMX CPS 4002");
                    _List.Add("BMX CPS 4022");
                    _List.Add("");
                    _List.Add("- - - CPU M340 и прочее - - -");
                    _List.Add("BMX P34 1000");
                    _List.Add("BMX P34 2000");
                    _List.Add("BMX P34 2010");
                    _List.Add("BMX P34 20102");
                    _List.Add("BMX P34 2020");
                    _List.Add("BMX P34 2030");
                    _List.Add("BMX P34 20302");
                    _List.Add("BMX PRA 0100");
                    _List.Add("");
                    _List.Add("- - - - - CPU M580 - - - - - ");
                    _List.Add("BME H58 2040 HSBY");
                    _List.Add("BME H58 4040 HSBY");
                    _List.Add("BME H58 6040 HSBY");
                    _List.Add("BME H58 1020");
                    _List.Add("BME H58 2020");
                    _List.Add("BME H58 2040");
                    _List.Add("BME H58 3020");
                    _List.Add("BME H58 3040");
                    _List.Add("BME H58 4020");
                    _List.Add("BME H58 4040");
                    _List.Add("BME H58 5040");
                    _List.Add("BME H58 6040");
                    _List.Add("BME P58 4020");
                    _List.Add("");
                    _List.Add("- - - CPU M580 Safety - - - ");
                    _List.Add("BME H58 2040S HSBY");
                    _List.Add("BME H58 4040S HSBY");
                    _List.Add("BME H58 6040S HSBY");
                    _List.Add("BME H58 2040S");
                    _List.Add("BME H58 4040S");
                    _List.Add("");
                    _List.Add("- - - - = Аналоги = - - - - ");
                    _List.Add("BME AHI 0812");
                    _List.Add("BME AHO 0412");
                    _List.Add("BMX AMI 0410");
                    _List.Add("BMX AMI 0800");
                    _List.Add("BMX AMI 0810");
                    _List.Add("BMX AMM 0600");
                    _List.Add("BMX AMO 0210");
                    _List.Add("BMX AMO 0410");
                    _List.Add("BMX AMO 0802");
                    _List.Add("BMX ART 0414.2");
                    _List.Add("BMX ART 0814.2");
                    _List.Add("");
                    _List.Add("- - - - - Дискреты - - - - -");
                    _List.Add("BMX DAI 0805");
                    _List.Add("BMX DAI 0814");
                    _List.Add("BMX DAI 1602");
                    _List.Add("BMX DAI 1603");
                    _List.Add("BMX DAI 1604");
                    _List.Add("BMX DAI 1614");
                    _List.Add("BMX DAI 1615");
                    _List.Add("BMX DAO 1605");
                    _List.Add("BMX DAO 1615");
                    _List.Add("BMX DDI 1602");
                    _List.Add("BMX DDI 1603");
                    _List.Add("BMX DDI 1604");
                    _List.Add("BMX DDI 3202K");
                    _List.Add("BMX DDI 6402K");
                    _List.Add("BMX DDM 16022");
                    _List.Add("BMX DDM 16025");
                    _List.Add("BMX DDM 3202K");
                    _List.Add("BMX DDO 1602");
                    _List.Add("BMX DDO 1612");
                    _List.Add("BMX DDO 3202K");
                    _List.Add("BMX DDO 6402K");
                    _List.Add("BMX DRA 0804");
                    _List.Add("BMX DRA 0805");
                    _List.Add("BMX DRA 0815");
                    _List.Add("BMX DRA 1605");
                    _List.Add("BMX DRC 0805");
                    _List.Add("BMX ERT 1604");
                    _List.Add("");
                    _List.Add("- - - - - - Сеть - - - - - -");
                    _List.Add("BME CXM 0100");
                    _List.Add("BME NOC 0301");
                    _List.Add("BME NOC 0301.2");
                    _List.Add("BME NOC 0301.3");
                    _List.Add("BME NOC 0301.4");
                    _List.Add("BME NOC 0311");
                    _List.Add("BME NOC 0311.2");
                    _List.Add("BME NOC 0311.3");
                    _List.Add("BME NOC 0311.4");
                    _List.Add("BME NOC 0321");
                    _List.Add("BME NOP 0300");
                    _List.Add("BME NOR 2200");
                    _List.Add("BME NOS 0300");
                    _List.Add("BMX EIA 0100");
                    _List.Add("BMX NGD 0100");
                    _List.Add("BMX NOC 0402");
                    _List.Add("BMX NOM 0200");
                    _List.Add("BMX NOM 0200.2");
                    _List.Add("BMX NOM 0200.3");
                    _List.Add("BMX NOM 0200.4");
                    _List.Add("BMX NOR 0200");
                    _List.Add("BMX NOR 0200H");
                    _List.Add("BMX NRP 0200");
                    _List.Add("BMX NRP 0201");
                    _List.Add("BME CRA 312 10");
                    _List.Add("BMX CRA 312 10");
                    _List.Add("BMX CRA 312 00");
                    _List.Add("");
                    _List.Add("- - - Защитная заглушка - - -");
                    _List.Add("BMX XEM 010");
                    break;
                    #endregion
            }

            return _List;
        }
        #endregion
    }
}
