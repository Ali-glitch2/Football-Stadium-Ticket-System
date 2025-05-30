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
using System.Globalization; // at the top of your file


namespace Form1
{
    public partial class Form3: Form
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


        public Form3()
        {
            InitializeComponent();


            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 2;
            button1.FlatAppearance.BorderColor = normalForeColor;
            button1.BackColor = normalBackColor;
            button1.ForeColor = normalForeColor;
            button1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button1.Text = "Calculate";

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
            button2.Text = "Confirm";

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


        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Liverpool vs Everton");
            comboBox1.Items.Add("Liverpool vs Arsenal");
            comboBox1.Items.Add("Liverpool vs Man City");

            comboBox1.SelectedIndex = 0;

            comboBox2.Items.Add("Basic");
            comboBox2.Items.Add("Vip");
            comboBox2.Items.Add("LegendPass");

            comboBox2.SelectedIndex = 0;
        }

        private List<int> GenerateRandomSeats(int count)
        {
            Random rnd = new Random();
            List<int> seats = new List<int>();

            for (int i = 0; i < count; i++)
            {
                int seatNumber = rnd.Next(1, 101);  // random seat between 1 and 100
                seats.Add(seatNumber);
            }

            return seats;
        }



        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMatch = comboBox1.SelectedItem.ToString();
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMatch = comboBox2.SelectedItem.ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedType = comboBox2.SelectedItem.ToString();
            int seats = (int)numericUpDown1.Value;

            
            Ticket ticket;
            if (selectedType == "Basic")
            {
                ticket = new Basic(1, "User", seats, 55.5);  
            }
            else if (selectedType == "Vip")
            {
                ticket = new Vip(2, "User", seats, 75.7);
            }
            else 
            {
                ticket = new LegendPass(3, "User", seats, 104.6);
            }

            
            double totalPriceBeforeTax = ticket.Price * seats;
            double totalPriceAfterTax = ticket.Tax() * seats;

            textBox1.Text = totalPriceBeforeTax.ToString("C"); 
            textBox2.Text = totalPriceAfterTax.ToString("C");

            List<int> generatedSeats = GenerateRandomSeats(seats);
            textBox3.Text = string.Join(", ", generatedSeats);

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

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Are you sure you want to proceed with this action?",
        "Confirm Booking",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

    if (result == DialogResult.Yes)
    {
        try
        {
            string Match = comboBox1.SelectedItem?.ToString();
            string ticketType = comboBox2.SelectedItem?.ToString();
            int ticketCount = (int)numericUpDown1.Value;
            string seatLocation = textBox3.Text;

                    // Get price before tax from textBox1
                    

                    decimal totalPrice = decimal.Parse(textBox2.Text, NumberStyles.Currency, CultureInfo.CurrentCulture);



                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 👇 This is your INSERT query
                string query = "INSERT INTO Tickets ([TicketType], SeatLoaction, TicketCount, TotalPrice, [Match]) " +
                               "VALUES (@TicketType, @SeatLoaction, @TicketCount, @TotalPrice, @Match)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TicketType", ticketType);
                    cmd.Parameters.AddWithValue("@SeatLoaction", seatLocation);
                    cmd.Parameters.AddWithValue("@TicketCount", ticketCount);
                    cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                    cmd.Parameters.AddWithValue("@Match", Match);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Ticket booked and saved to database!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }


    class Ticket
    {
        private double price = 55.5;
        private int id;
        private string name;
        private int nOfSeats;



        public int ID
        {
            get { return id; }
            set
            {
                if (value >= 0)
                {
                    id = value;
                }


            }
        }




        public virtual double Price
        {
            get
            {
                return price;
            }
            protected set
            {
                if (value >= 0)
                {
                    price = value;
                }
            }

        }


        public string Name
        {
            get { return name; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    name = value;
                }


            }
        }

        public int NOfSeats
        {
            get { return nOfSeats; }
            set
            {
                if (value >= 0)
                {
                    nOfSeats = value;
                }

            }


        }


        public Ticket(int id, string name, int nOfSeats, double price)
        {
            ID = id;
            Name = name;
            NOfSeats = nOfSeats;
            Price = price;


        }




        public override string ToString()
        {
            return string.Format("Name : {0} , Price without Tax : {1} , Id : {2} , NumberOfSeats : {3}   ", Name, Price.ToString("C"), ID, NOfSeats);
        }


        public virtual double Tax()
        {
            return Price * 1.2;
        }


    }




    class Vip : Ticket
    {

        public Vip(int id, string name, int nOfSeats, double price)
       : base(id, name, nOfSeats, price)
        {
            Price = 75.7;
        }


        public override double Tax()
        {
            return Price + 0.2 * Price;
        }


        public override string ToString()
        {
            Console.WriteLine("This is a Vip ticket");
            return base.ToString() + string.Format("Price After Tax : {0}", Tax().ToString("C"));
        }

    }


    class Basic : Ticket
    {
        public Basic(int id, string name, int nOfSeats, double price)
       : base(id, name, nOfSeats, price)
        {

        }

        public override double Tax()
        {
            return Price + 0.2 * Price;
        }



        public override string ToString()
        {
            Console.WriteLine("This is a Basic ticket");
            return base.ToString() + string.Format("Price After Tax : {0}", Tax().ToString("C"));
        }

    }


    class LegendPass : Ticket
    {
        public LegendPass(int id, string name, int nOfSeats, double price)
      : base(id, name, nOfSeats, price)
        {
            Price = 104.6;
        }

        public override double Tax()
        {
            return Price + 0.2 * Price;
        }




        public override string ToString()
        {

            Console.WriteLine("This is a Legend  ticket");
            return base.ToString() + string.Format("Price After Tax : {0}", Tax().ToString("C"));
        }

    }

}
