using System.Windows.Forms;

namespace GripCore.GameSystem.GameWindow
{
    public partial class LoadingWindow : Form
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }

        private void LoadingWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(timer1.Enabled)
            {
                timer1.Stop();
            }
        }

        private void LoadingWindow_Load(object sender, System.EventArgs e)
        {
            timer1.Start(); 
        }
        int  proval =0;

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            proval += 10;
            if (proval >= 500)
                proval = 0;
            uchScrollbar1.Value= proval;
        }

        private void LoadingWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
            }
        }
    }
}