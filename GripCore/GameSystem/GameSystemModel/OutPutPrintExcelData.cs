using MiniExcelLibs.Attributes;

namespace GripCore.GameSystem.GameSystemModel
{
    public class OutPutPrintExcelData
    {
        [ExcelColumnName("序号",null)]
        public  int id { get; set; }
        [ExcelColumnName("日期",null)]
        public  string examtime { get; set; }
        [ExcelColumnName("学校",null)]
        public   string school { get; set; }
        [ExcelColumnName("姓名",null)]
        public  string name { get; set; }
        [ExcelColumnName("性别",null)]
        public  string Sex { get; set; }
        [ExcelColumnName("准考证号码",null)]
        public  string idNumber { get; set; }
        [ExcelColumnName("组别",null)]
        public  string groupName { get; set; }
        [ExcelColumnName("成绩",null)]
        public  string score { get; set; }
        
    }
}