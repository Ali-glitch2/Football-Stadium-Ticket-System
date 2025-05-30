using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;





namespace Form1
{
    public partial class Form6: Form
    {
        private string placeholderUsername = "Enter username";
        private string placeholderPassword = "Password";



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


        public Form6()
        {
            InitializeComponent();
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 2;
            button1.FlatAppearance.BorderColor = normalForeColor;
            button1.BackColor = normalBackColor;
            button1.ForeColor = normalForeColor;
            button1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button1.Text = "Login";

            MakeButtonRounded(button1, 20);


            originalSize = button1.Size;
            originalLocation = button1.Location;


            button1.MouseEnter += button1_MouseEnter;
            button1.MouseLeave += button1_MouseLeave;

            animationTimer = new Timer();
            animationTimer.Interval = 15;
            animationTimer.Tick += AnimationTimer_Tick;

            textBox1.Text = placeholderUsername;
            textBox1.ForeColor = Color.Gray;
            textBox1.Enter += TextBox1_Enter;
            textBox1.Leave += TextBox1_Leave;

            textBox2.Text = placeholderPassword;
            textBox2.ForeColor = Color.Gray;
            textBox2.Enter += TextBox2_Enter;
            textBox2.Leave += TextBox2_Leave;
            textBox2.UseSystemPasswordChar = false; // Don't hide placeholder

        }


        private void TextBox1_Enter(object sender, EventArgs e)
{
    if (textBox1.Text == placeholderUsername)
    {
        textBox1.Text = "";
        textBox1.ForeColor = Color.Black;
    }
}

private void TextBox1_Leave(object sender, EventArgs e)
{
    if (string.IsNullOrWhiteSpace(textBox1.Text))
    {
        textBox1.Text = placeholderUsername;
        textBox1.ForeColor = Color.Gray;
    }
}

private void TextBox2_Enter(object sender, EventArgs e)
{
    if (textBox2.Text == placeholderPassword)
    {
        textBox2.Text = "";
        textBox2.ForeColor = Color.Black;
        textBox2.UseSystemPasswordChar = true; // hide characters
    }
}

private void TextBox2_Leave(object sender, EventArgs e)
{
    if (string.IsNullOrWhiteSpace(textBox2.Text))
    {
        textBox2.Text = placeholderPassword;
        textBox2.ForeColor = Color.Gray;
        textBox2.UseSystemPasswordChar = false; // show placeholder
    }
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




        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string username = textBox1.Text;
            string password = textBox2.Text;

            bool validUsername = username != "" && username != placeholderUsername;
            bool validPassword = password != "" && password != placeholderPassword;

            if (validUsername && validPassword)
            {

                AddUserToDatabase(username, password);
                Form2 thirdForm = new Form2();
                thirdForm.Show();
            }
            else
            {
                MessageBox.Show("Please enter both username and password.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddUserToDatabase(string username, string password)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Check if username already exists to prevent duplicates
                    string checkQuery = "SELECT COUNT(1) FROM Login WHERE UserName = @username";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userExists > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose a different username.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert new user into database
                    string insertQuery = "INSERT INTO Login (UserName, Password) VALUES (@username, @password)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@username", username);
                        insertCmd.Parameters.AddWithValue("@password", password);
                        insertCmd.ExecuteNonQuery();

                        MessageBox.Show(" Logged in successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reset textboxes
                        textBox1.Text = placeholderUsername;
                        textBox1.ForeColor = Color.Gray;
                        textBox2.Text = placeholderPassword;
                        textBox2.ForeColor = Color.Gray;
                        textBox2.UseSystemPasswordChar = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            isHovering = true;
            animationTimer.Start();
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            isHovering = false;
            animationTimer.Start();
        }
    }
}
