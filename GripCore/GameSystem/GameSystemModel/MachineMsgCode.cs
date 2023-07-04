namespace GripCore.GameSystem.GameSystemModel
{
    public class MachineMsgCode
    {
         
            public  int type { get; set; }
            public string itemtype
            {
                get;
                set;
            }
            public  string mac { get; set; }
            public int code { get; set; }
            public  double fh1_result { get; set; }//肺活量成绩
            public  double tz_result { get; set; }//体重成绩
            public  double sg_result { get; set; }//身高成绩
            public  double bmi_result { get; set; }//bmi成绩
            public  double wl_result { get; set; } // 握力成绩
    }

    public class SerialMsg
    {
        public MachineMsgCode mms;
        public int number;

        public SerialMsg(MachineMsgCode m, int n)
        {
            mms = m;
            number = n;
        }
    }
}