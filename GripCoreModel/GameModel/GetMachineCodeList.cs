using System.Collections.Generic;

namespace GripCoreModel.GameModel
{
    public class GetMachineCodeList
    {
        public List<GetMachineCodeListResult> Results { get; set; }
        public string Error { get; set; }
    }

    public class GetMachineCodeListResult
    {
        public  string title { get; set; }
        public  string  machineCode { get; set; }
    }
}