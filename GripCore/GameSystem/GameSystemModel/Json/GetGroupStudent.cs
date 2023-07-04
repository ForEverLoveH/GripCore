using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using GripCore.GameSystem.GameHelper;
using Newtonsoft.Json.Linq;

namespace GripCore.GameSystem.GameSystemModel 
{
    public class GetGroupStudent
    {
        public Results Results { get; set; }
        public string Error { get; set; }


        public static string[] CheckJson(string json)
        {
            string[] strs = new string[2];
            string ResultISNull = "0";
            string ResultError = "";
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var jsObject = JObject.Parse(json);
                    foreach (JToken child in jsObject.Children())
                    {
                        var property1 = child as JProperty;
                        if (property1.Name == "Error")
                        {
                            if (string.IsNullOrEmpty(property1.Value.ToString()))
                            {
                                ResultISNull = "1";
                            }
                            else
                            {
                                ResultISNull = "0";
                                ResultError = property1.Value.ToString();
                            }

                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    ResultISNull = "0";
                    ResultError = "";
                }
            }

            strs[0] = ResultISNull;
            strs[1] = ResultError;
            return strs;

        }
    }

    public class Results
    {
        public  List<GroupItems> groups { get; set; }
    }

   

     
}