using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripCoreModel.GameModel
{
    public  class SportProjectInfos
    {
        [Column(IsIdentity = true,IsPrimary =true)]
        public int Id { get; set; }
        public string CreateTime { get; set; }
        public int SortID { get; set; }
        public int IsRemove { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public  int RountCount { get; set; }
        public int BestScore { get; set; }
        public int TestMethod { get; set; } 
        public int FloatType { get; set; }
        public int TurnsNumber1 { get; set; }
        public int TurnsNumber2 { get;set; }
    }
}
