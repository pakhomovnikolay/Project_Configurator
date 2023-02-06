using Project_Сonfigurator.Infrastructures.Commands.Base;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class CmdRefreshModbusAddress : Command
    {
        protected override bool CanExecute(object p) =>
            App.Settings.Config.ModbusTCP_HR is not null &&
            App.Settings.Config.ModbusTCP_HR.Count > 0 &&
            App.Settings.Config.ModbusTCP_IR is not null &&
            App.Settings.Config.ModbusTCP_IR.Count > 0;

        protected override void Execute(object p)
        {
            var ModbusTCP_HR = App.Settings.Config.ModbusTCP_HR;
            var ModbusTCP_IR = App.Settings.Config.ModbusTCP_IR;
            for (int i = 0; i < ModbusTCP_HR.Count; i++)
            {
                if (i == 0 || string.IsNullOrWhiteSpace(ModbusTCP_HR[i].LengthWord))
                {
                    ModbusTCP_HR[i].AddressInPLC = ModbusTCP_HR[i].AddressStart;
                    if (i == 0) continue;
                    ModbusTCP_HR[i].Description = "";
                    ModbusTCP_HR[i].PathTag = "";
                    ModbusTCP_HR[i].LengthWord = "";
                    ModbusTCP_HR[i].LengthByte = "";
                    ModbusTCP_HR[i].AddressStart = "";
                    ModbusTCP_HR[i].AddressEnd = "";
                    ModbusTCP_HR[i].AddressInPLC = "";
                    continue;
                }
                _ = int.TryParse(ModbusTCP_HR[i - 1].AddressStart, out int AddressStart);
                _ = int.TryParse(ModbusTCP_HR[i - 1].LengthWord, out int LengthWord);
                ModbusTCP_HR[i].AddressStart = $"{AddressStart + LengthWord}";

                _ = int.TryParse(ModbusTCP_HR[i].AddressStart, out AddressStart);
                _ = int.TryParse(ModbusTCP_HR[i].LengthWord, out LengthWord);
                ModbusTCP_HR[i].AddressEnd = $"{AddressStart + LengthWord - 1}";
                ModbusTCP_HR[i].AddressInPLC = ModbusTCP_HR[i].AddressStart;
            }

            for (int i = 0; i < ModbusTCP_IR.Count; i++)
            {
                if (i == 0 || string.IsNullOrWhiteSpace(ModbusTCP_IR[i].LengthWord))
                {
                    ModbusTCP_IR[i].AddressInPLC = ModbusTCP_IR[i].AddressStart;
                    if (i == 0) continue;
                    ModbusTCP_IR[i].Description = "";
                    ModbusTCP_IR[i].PathTag = "";
                    ModbusTCP_IR[i].LengthWord = "";
                    ModbusTCP_IR[i].LengthByte = "";
                    ModbusTCP_IR[i].AddressStart = "";
                    ModbusTCP_IR[i].AddressEnd = "";
                    ModbusTCP_IR[i].AddressInPLC = "";
                    continue;
                }
                _ = int.TryParse(ModbusTCP_IR[i - 1].AddressStart, out int AddressStart);
                _ = int.TryParse(ModbusTCP_IR[i - 1].LengthWord, out int LengthWord);
                ModbusTCP_IR[i].AddressStart = $"{AddressStart + LengthWord}";

                _ = int.TryParse(ModbusTCP_IR[i].AddressStart, out AddressStart);
                _ = int.TryParse(ModbusTCP_IR[i].LengthWord, out LengthWord);
                ModbusTCP_IR[i].AddressEnd = $"{AddressStart + LengthWord - 1}";
                ModbusTCP_IR[i].AddressInPLC = ModbusTCP_IR[i].AddressStart;
            }


            //var Index = 0;




            //foreach (var ModbusTCP_HR in App.Settings.Config.ModbusTCP_HR)
            //{
            //    if (Index > 0)
            //    {
            //        ModbusTCP_HR
            //    }

            //    Index++;
            //}
        }
    }
}
