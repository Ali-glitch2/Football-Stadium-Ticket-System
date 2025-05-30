using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;




namespace Form1
{
    public partial class Form4: Form
    {

        private Color normalBackColor = Color.White;
        private Color normalForeColor = Color.FromArgb(200, 16, 46);
        private Color hoverBackColor = Color.FromArgb(200, 16, 46);
        private Color hoverForeColor = Color.White;

        private Timer animationTimer;
       


        private Size originalSize;
        private Point originalLocation;


        private float animationProgress = 0f;
        private float animationSpeed = 0.1f;

        private Size button2OriginalSize;
        private Point button2OriginalLocation;

        private bool isHoveringButton1 = false;
        private bool isHoveringButton2 = false;

        public Form4()
        {
            InitializeComponent();
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 2;
            button1.FlatAppearance.BorderColor = normalForeColor;
            button1.BackColor = normalBackColor;
            button1.ForeColor = normalForeColor;
            button1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button1.Text = "Sumbit";

            MakeButtonRounded(button1, 20);


            originalSize = button1.Size;
            originalLocation = button1.Location;


            button1.MouseEnter += button1_MouseEnter;
            button1.MouseLeave += button1_MouseLeave;

            animationTimer = new Timer();
            animationTimer.Interval = 15;
            animationTimer.Tick += AnimationTimer_Tick;


            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 2;
            button2.FlatAppearance.BorderColor = normalForeColor;
            button2.BackColor = normalBackColor;
            button2.ForeColor = normalForeColor;
            button2.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button2.Text = "Clear";

            MakeButtonRounded(button2, 20);

            button2OriginalSize = button2.Size;
            button2OriginalLocation = button2.Location;

            button2.MouseEnter += button2_MouseEnter;
            button2.MouseLeave += button2_MouseLeave;




        }


        private void MakeButtonRounded(Button btn, int radius)
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
            // Animate button1
            AnimateButton(button1, ref isHoveringButton1, ref originalSize, ref originalLocation);

            // Animate button2
            AnimateButton(button2, ref isHoveringButton2, ref button2OriginalSize, ref button2OriginalLocation);

            // Stop timer if no hover animation is needed
            if (!isHoveringButton1 && !isHoveringButton2)
            {
                animationTimer.Stop();
            }
        }

        private void AnimateButton(Button btn, ref bool isHoveringBtn, ref Size origSize, ref Point origLocation)
        {
            // Track animation progress per button
            // You might want separate animationProgress for each button if you want smooth independent animation
            // For simplicity, reusing the same animationProgress here applies the same animation progress to both buttons

            if (isHoveringBtn)
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

            int newWidth = (int)(origSize.Width * scale);
            int newHeight = (int)(origSize.Height * scale);

            int newX = origLocation.X - (newWidth - origSize.Width) / 2;
            int newY = origLocation.Y - (newHeight - origSize.Height) / 2;

            btn.Size = new Size(newWidth, newHeight);
            btn.Location = new Point(newX, newY);

            if (animationProgress > 0)
            {
                btn.BackColor = hoverBackColor;
                btn.ForeColor = hoverForeColor;
                btn.FlatAppearance.BorderColor = hoverBackColor;
            }
            else
            {
                btn.BackColor = normalBackColor;
                btn.ForeColor = normalForeColor;
                btn.FlatAppearance.BorderColor = normalForeColor;
            }

            MakeButtonRounded(btn, 20);
        }




        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string matchSelected = comboBox1.SelectedItem?.ToString();
            string firstScorer = textBox1.Text.Trim();
            int homeGoals = (int)numericUpDown1.Value;
            int awayGoals = (int)numericUpDown2.Value;

            // === Validation ===
            if (string.IsNullOrWhiteSpace(matchSelected))
            {
                MessageBox.Show("Please select a match.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(firstScorer))
            {
                MessageBox.Show("Please enter the first scorer.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Predictions (MatchSelected, HomeGoals, AwayGoals, FirstScorer) VALUES (@MatchSelected, @HomeGoals, @AwayGoals, @FirstScorer)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MatchSelected", matchSelected);
                        cmd.Parameters.AddWithValue("@HomeGoals", homeGoals);
                        cmd.Parameters.AddWithValue("@AwayGoals", awayGoals);
                        cmd.Parameters.AddWithValue("@FirstScorer", firstScorer);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Prediction submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMatch = comboBox1.SelectedItem.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMatch = comboBox2.SelectedItem.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, @"^[a-zA-Z\s]*$"))
            {
                MessageBox.Show("Please enter letters only.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Text = ""; // clear invalid input
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only letters, control keys (like backspace), and spaces
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true; // Reject invalid characters
            }
        }





        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            textBox1.Text = "";
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Liverpool vs Everton");
            comboBox1.Items.Add("Liverpool vs Arsenal");
            comboBox1.Items.Add("Liverpool vs Man City");

            comboBox1.SelectedIndex = 0;

            comboBox2.Items.Add("Home Win");
            comboBox2.Items.Add("Draw");
            comboBox2.Items.Add("Away Win");

            comboBox2.SelectedIndex = 0;


            textBox1.KeyPress += textBox1_KeyPress;

        }



        private void button1_MouseEnter(object sender, EventArgs e)
        {
            isHoveringButton1 = true;  // FIXED
            animationTimer.Start();
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            isHoveringButton1 = false;  // FIXED
            animationTimer.Start();
        }



        private void button2_MouseEnter(object sender, EventArgs e)
        {
            isHoveringButton2 = true;
            animationTimer.Start();
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            isHoveringButton2 = false;
            animationTimer.Start();
        }
    }
}
