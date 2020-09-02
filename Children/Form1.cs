using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace Children
{
    public partial class Form1 : Form
    {       
        Children[,] cell;
        int number = 3;
        Brush[] m_br = { Brushes.Blue, Brushes.Red, Brushes.Green,
                         Brushes.Brown, Brushes.Orange, Brushes.Yellow,
                         Brushes.DeepPink, Brushes.Orchid, Brushes.Coral };

        bool prev;
        public static Timer timer;
        Random rand;
        Children prev_control;
        Form2 form2;

        public Form1()
        {
            int index = 0;
            cell = new Children[number, number];
            form2 = new Form2();
            form2.Owner = this;

            for (int i = 0; i < number; i++)
                for (int j = 0; j < number; j++)
                {
                    cell[i, j] = new Children();
                    cell[i, j].Parent = this;
                    cell[i, j].m_br = m_br[index];
                    cell[i, j].Name = index + "";
                    index++;
                }

            InitializeComponent();
            Text = "Brains";
            OnResize(EventArgs.Empty);

            prev = false;
            //active = false;
            rand = new Random();
            timer = new Timer();
            timer.Tick += new EventHandler(TimerOntic);
        }

        public static int col = 1;
        public static int tec = 0;

        public void TimerOntic(object sender, EventArgs e)
        {
            // child.MouseDown += Child_MouseDown;        
            if (tec == col)
            {
                col++;
                timer.Stop();
                Children.active = true;
                Children.num_click = tec;
                tec = 0;
            }
            
            if (prev)
            {            
                prev_control.BackColor = Color.FromArgb(255, 240, 240, 240);
                prev = false;
            }
            else
            {
                int num = rand.Next(9);
                Children child = (this.Controls.Find(num + "", true).FirstOrDefault() as Children);
                child.BackColor = Color.DarkGray;
                child.poryadok.Add(tec + 1);
                prev_control = child;
                // MessageBox.Show(num + ""); 
                tec++;
                prev = true;
            }

        }

        protected override void OnResize(EventArgs e)
        {
            //base.OnResize(e);
            int cx = ClientSize.Width / number; 
            int cy = (ClientSize.Height - menuStrip1.Size.Height) / number;
            for (int i = 0; i < number; i++)
                for (int j = 0; j < number; j++)
                {
                    cell[i, j].Location = new Point(i * cx, j * cy + menuStrip1.Size.Height);
                    cell[i, j].Size = new Size(cx, cy);
                }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void начатьИгруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form2.ShowDialog();
            
            if(DialogResult.OK == form2.DialogResult)
            {

                switch(form2.level)
                {
                   // case 0: return;
                    case 1: timer.Interval = 1000; break;
                    case 2: timer.Interval = 500; break;
                    case 3: timer.Interval = 200; break;

                }

                for(int i = 0; i < 9; i++)
                {
                    Children child = (this.Controls.Find(i + "", true).FirstOrDefault() as Children);
                    child.poryadok.Clear();
                }
                
                Children.active = false;
                Children.num_click = 0;
                Children.click = 0;
                col = 1;
                tec = 0;

                timer.Start();
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void играToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }

    class Children : UserControl 
    {
        bool mouse_enter, mouse_leave;
        public Brush m_br;
        public List<int> poryadok;
        public static int click = 0;

        public static int num_click = 0;
        public static bool active = false;

        public Children()
        {
            ResizeRedraw = true;
            DoubleBuffered = true;
            poryadok = new List<int>();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if(active)
            {
                BackColor = Color.DarkGray;
                Invalidate();
            }      
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (active)
            {
                BackColor = Color.FromArgb(255, 240, 240, 240);
                mouse_enter = true;

                click++;

                if(poryadok.Count > 0 && poryadok.First() == click)
                {
                    poryadok.Remove(click);
                    if (click == num_click)
                    {
                        active = false;
                        num_click = 0;
                        click = 0;
                        Form1.timer.Start();
                    }
                }
                else
                {
                    poryadok.Clear();
                    active = false;
                    num_click = 0;
                    click = 0;
                    Form1.col = 1;
                    Form1.tec = 0;
                    MessageBox.Show("Game Over");
                }

                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouse_leave = true;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouse_enter = true;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            float cx = ClientRectangle.Width / 1.3f;
            float cy = ClientRectangle.Height / 1.3f;
           
            Graphics gr = e.Graphics;
            gr.SmoothingMode = SmoothingMode.AntiAlias;

            if (mouse_enter)
            {
                Pen pen = new Pen(Color.DarkGray);
                gr.DrawRectangle(pen, ClientRectangle.X, ClientRectangle.Y,
                    ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                mouse_enter = false;
            }

            if (mouse_leave)
            {             
                Pen pen = new Pen(BackColor);
                gr.DrawRectangle(pen, ClientRectangle.X, ClientRectangle.Y, 
                    ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                mouse_leave = false;
            }

            gr.FillEllipse(m_br, ClientRectangle.Width / 2 - cx / 2, ClientRectangle.Height / 2 - cy / 2, cx, cy);
        }
    }
}
