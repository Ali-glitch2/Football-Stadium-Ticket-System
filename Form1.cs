using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Form1
{


   

    public partial class Form1: Form
    {

        private Color normalBackColor = Color.White;
        private Color normalForeColor = Color.FromArgb(200, 16, 46); 
        private Color hoverBackColor = Color.FromArgb(200, 16, 46);
        private Color hoverForeColor = Color.White;

        private Timer animationTimer;
        private bool isHovering = false;

        
        private Size originalSize;
        private Point originalLocation;

        
        private float animationProgress = 0f; 
        private float animationSpeed = 0.1f; 
        public Form1()
        {
            InitializeComponent();




            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 2;
            button1.FlatAppearance.BorderColor = normalForeColor;
            button1.BackColor = normalBackColor;
            button1.ForeColor = normalForeColor;
            button1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button1.Text = "Welcome";

            MakeButtonRounded(button1, 20);

            
            originalSize = button1.Size;
            originalLocation = button1.Location;

            
            button1.MouseEnter += button1_MouseEnter;
            button1.MouseLeave += button1_MouseLeave;

            animationTimer = new Timer();
            animationTimer.Interval = 15;
            animationTimer.Tick += AnimationTimer_Tick;

        }








        private  void MakeButtonRounded(Button btn, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(btn.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(btn.Width - radius, btn.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, btn.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            btn.Region = new Region(path);
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (isHovering)
            {
                if (animationProgress < 1f)
                {
                    animationProgress += animationSpeed;
                    if (animationProgress > 1f) animationProgress = 1f;
                }
            }
            else
            {
                if (animationProgress > 0f)
                {
                    animationProgress -= animationSpeed;
                    if (animationProgress < 0f) animationProgress = 0f;
                }
            }

           
            float scale = 1.0f + 0.1f * animationProgress;

            
            int newWidth = (int)(originalSize.Width * scale);
            int newHeight = (int)(originalSize.Height * scale);

            
            int newX = originalLocation.X - (newWidth - originalSize.Width) / 2;
            int newY = originalLocation.Y - (newHeight - originalSize.Height) / 2;

            button1.Size = new Size(newWidth, newHeight);
            button1.Location = new Point(newX, newY);

            
            if (animationProgress > 0)
            {
                button1.BackColor = hoverBackColor;
                button1.ForeColor = hoverForeColor;
                button1.FlatAppearance.BorderColor = hoverBackColor;
            }
            else
            {
                button1.BackColor = normalBackColor;
                button1.ForeColor = normalForeColor;
                button1.FlatAppearance.BorderColor = normalForeColor;
            }

            
            MakeButtonRounded(button1, 20);
        }







        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 secondForm = new Form6(); 
            secondForm.Show();
        }
        private  void button1_MouseEnter(object sender, EventArgs e)
        {
            isHovering = true;
            animationTimer.Start();
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            isHovering = false;
            animationTimer.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
