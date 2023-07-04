using System.Windows.Forms;

namespace GripCore.GameSystem.GameHelper 
{
    public class ListViewHelper
    {
        public static void InitListViewHeader(ListView listView1,int rountCount)
        {
            listView1.View = View.Details;
            ColumnHeader[] headers = new ColumnHeader[100];
            int sp = 0;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "序号";
            headers[sp].Width = 50;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "时间";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "学校";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "组别名称";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "姓名" ;
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "准考证号码";
            headers[sp].Width = 200;
            sp++;  
            for(int i = 0; i < rountCount; i++)
            {
                headers[sp] = new ColumnHeader();
                headers[sp].Text = $"第{i+1}轮";
                headers[sp].Width = 150;
                sp++;
                headers[sp]= new ColumnHeader();
                headers[sp].Text = "上传状态";
                headers[sp].Width = 150;
                sp++;
            }
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "最好成绩";
            headers[sp].Width = 150;
            sp++;
            ColumnHeader[] columnHeaders = new ColumnHeader[sp];
            for(int j = 0;j<columnHeaders.Length;j++)
            {
                columnHeaders[j] = headers[j];
            }
            listView1.Columns.AddRange(columnHeaders);
        }

        public static void InitListViewHeader(ListView listView1)
        {
            listView1.View = View.Details;
            ColumnHeader[] headers = new ColumnHeader[100];
            int sp = 0;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "序号";
            headers[sp].Width = 50;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "时间";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "学校";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "组别名称";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "姓名" ;
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "准考证号码";
            headers[sp].Width = 200;
            sp++;  
            ColumnHeader[] columnHeaders = new ColumnHeader[sp];
            for(int j = 0;j<columnHeaders.Length;j++)
            {
                columnHeaders[j] = headers[j];
            }
            listView1.Columns.AddRange(columnHeaders);
        }
    }
}