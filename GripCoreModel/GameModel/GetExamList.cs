using System.Collections.Generic;

namespace GripCoreModel.GameModel
{
    public class GetExamList
    {
        public List<GetExamListResult> results { get; set; }
        public string Error { get; set; }
    }

    public class GetExamListResult
    {
        public  string exam_id{ get; set; }
        public  string title { get; set; }
    }
}