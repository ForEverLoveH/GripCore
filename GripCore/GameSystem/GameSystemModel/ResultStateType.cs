using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripCore.GameSystem.GameSystemModel
{
    public  class ResultStateType
    {
        public static int foul = 4;
        public static int MissTest = 3;
        public static int NoTest = 0;
        public static int Test = 1;
        public static int waiver = 5;
        public static int Withdraval = 6;
        public  static string Match(int state)
        {
            string  res = string.Empty;
            switch (state)
            {
                case 0:
                    res = "未测试";
                    break;

                case 1:
                    res = "已测试";
                    break;
                case 2:
                    res = "中退";
                    break;
                case 3:
                    res = "缺考";
                    break;
                case 4:
                    res = "犯规";
                    break;
                case 5:
                    res = "弃权";
                    break;
                default:
                    res = string.Empty;
                    break;
            }
            return res;
        }
        public static int ResultStateToInt(string state)
        {
            int res = 0;
            if (state != "未测试")
            {
                if (state != "已测试")
                {
                    if (state!="中退")
                    {
                        if (state != "缺考")
                        {
                            if (state != "犯规")
                            {
                                if (state != "弃权")
                                {
                                    res = 0;
                                }
                                else
                                {
                                    res = ResultStateType.waiver;
                                }
                            }
                            else
                            {
                                res = ResultStateType.foul;
                            }
                        }
                        else
                        {
                            res = ResultStateType.MissTest;
                        }
                    }
                    else
                    {
                        res = ResultStateType.Withdraval;
                    }
                }
                else
                {
                    res = ResultStateType.Test;
                }
            }
            else
            {
                res = ResultStateType.NoTest;
            }
            return res;
        } 
    }
}
