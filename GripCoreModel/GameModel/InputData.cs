using MiniExcelLibs;
using MiniExcelLibs.Attributes;

namespace GripCoreModel.GameModel
{
    public class InputData
    {
        [ExcelColumnName("序号",null)] public  int  ID { get; set; }
        [ExcelColumnName("日期",null)] public  string ExamDate { get; set; }
        [ExcelColumnName("学校",null)] public  string SchoolName { get; set; }
        [ExcelColumnName("年级",null)] public  string  Grade { get; set; } 
        [ExcelColumnName("班级",null)] public  string ClassName { get; set; }
        [ExcelColumnName("姓名", null)] public string Name { get; set; }
        [ExcelColumnName("性别",null)] public  string     Sex { get; set; }
        [ExcelColumnName("准考证号码",null)] public  string  IDNumber { get; set; }
        [ExcelColumnName("组别名称",null)] public  string GroupName { get; set; }
        

    }

    public class ExcelUtils
    {
        public static void OutPutExcel(string PATH, object value)
        {
            MiniExcel.SaveAs(PATH,value,true,"sheet1",ExcelType.UNKNOWN,null,false);
        }
    }
}