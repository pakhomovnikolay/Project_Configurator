using Project_Сonfigurator.Infrastructures.Enum;
using Project_Сonfigurator.Models.LayotRack;
using Project_Сonfigurator.Services.Base;
using Project_Сonfigurator.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Project_Сonfigurator.Services
{
    public class LayotRackService : BaseService, ILayotRackService
    {
        #region Обновление индексов модулей
        /// <summary>
        /// Обновление индексов модулей
        /// </summary>
        /// <param name="SelectedUSO"></param>
        public void RefreshIndexModule(USO SelectedUSO)
        {
            var i = 0;
            foreach (var _Rack in SelectedUSO.Racks)
            {
                if (_Rack.IsEnable)
                {
                    _Rack.Name = $"A{++i}";
                    _Rack.Index = $"{i}";
                }
                var j = 0;
                foreach (var _Module in _Rack.Modules)
                    _Module.Index = $"{_Rack.Name}.{++j}";
            }
        }
        #endregion

        #region Обновление адресов модулей
        /// <summary>
        /// Обновление адресов модулей
        /// </summary>
        /// <param name="USOList"></param>
        public void RefreshAddressModule(ObservableCollection<USO> USOList)
        {
            var VUIndexAI = 0;
            var VUIndexDI = 0;
            var VUIndexAO = 0;
            var VUIndexDO = 0;
            var index = 0;
            foreach (var _USO in USOList)
            {
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
                        _Module.Channels = new ObservableCollection<Channel>();

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
                                        VarName = $"{VarName}[{int.Parse(_Module.StartAddress)}]",
                                        Bit = $"{i}",
                                        Address = $"{int.Parse(_Module.StartAddress) * 32 + i + address}",
                                        VarNameVU = $"{VarNameVU}[{VUIndex}]"
                                    };
                                    _Module.Channels.Add(ch);
                                }
                                break;

                            case TypeModule.DA:
                                break;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
