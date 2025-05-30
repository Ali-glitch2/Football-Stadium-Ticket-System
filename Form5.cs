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
using System.Configuration;





namespace Form1
{
    public partial class Form5 : Form
    {
        private List<Player> votedPlayers = new List<Player>();


        private Color normalBackColor = Color.White;
        private Color normalForeColor = Color.FromArgb(200, 16, 46);
        private Color hoverBackColor = Color.FromArgb(200, 16, 46);
        private Color hoverForeColor = Color.White;

        private Timer animationTimer;

        // Track each button's animation state
        private Dictionary<Button, float> animationProgresses = new Dictionary<Button, float>();
        private Dictionary<Button, bool> isHoveringDict = new Dictionary<Button, bool>();

        // Store original size and location for each button to scale correctly
        private Dictionary<Button, Size> originalSizes = new Dictionary<Button, Size>();
        private Dictionary<Button, Point> originalLocations = new Dictionary<Button, Point>();

        private float animationSpeed = 0.1f;
        public Form5()
        {
            InitializeComponent();



            animationTimer = new Timer();
            animationTimer.Interval = 15;
            animationTimer.Tick += AnimationTimer_Tick;

            // Example: Add animation to these buttons
            SetupButtonAnimation(button1);
            SetupButtonAnimation(button2);
            SetupButtonAnimation(button3);


        }


        private void SetupButtonAnimation(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.BackColor = normalBackColor;
            btn.ForeColor = normalForeColor;
            btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            MakeButtonRounded(btn, 20);

            // Save original size and location
            originalSizes[btn] = btn.Size;
            originalLocations[btn] = btn.Location;

            // Initialize animation states
            animationProgresses[btn] = 0f;
            isHoveringDict[btn] = false;

            // Subscribe to events
            btn.MouseEnter += (s, e) =>
            {
                isHoveringDict[btn] = true;
                animationTimer.Start();
            };
            btn.MouseLeave += (s, e) =>
            {
                isHoveringDict[btn] = false;
                animationTimer.Start();
            };
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
            bool needStop = true;

            foreach (var btn in animationProgresses.Keys.ToList())
            {
                float progress = animationProgresses[btn];
                bool isHovering = isHoveringDict[btn];

                if (isHovering)
                {
                    if (progress < 1f)
                    {
                        progress += animationSpeed;
                        if (progress > 1f) progress = 1f;
                        animationProgresses[btn] = progress;
                        needStop = false;
                    }
                }
                else
                {
                    if (progress > 0f)
                    {
                        progress -= animationSpeed;
                        if (progress < 0f) progress = 0f;
                        animationProgresses[btn] = progress;
                        needStop = false;
                    }
                }

               
                float scale = 1.0f + 0.1f * progress;

                Size originalSize = originalSizes[btn];
                Point originalLocation = originalLocations[btn];

                int newWidth = (int)(originalSize.Width * scale);
                int newHeight = (int)(originalSize.Height * scale);

                int newX = originalLocation.X - (newWidth - originalSize.Width) / 2;
                int newY = originalLocation.Y - (newHeight - originalSize.Height) / 2;

                btn.Size = new Size(newWidth, newHeight);
                btn.Location = new Point(newX, newY);

                if (progress > 0)
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

            if (needStop)
            {
                animationTimer.Stop();
            }
        }





        private void Form5_Load(object sender, EventArgs e)
        {
            
            List<Player> players = new List<Player>()
{
    new Player(1, "Mohamed Salah", "Forward"),
    new Player(2, "Virgil van Dijk", "Defender"),
    new Player(3, "Mac Allister", "Forward"),
    new Player(4, "Alisson Becker", "Goalkeeper"),
    new Player(5, "Konate", "Midfielder"),
    new Player(6, "Trent Alexander-Arnold", "Defender"),
    new Player(7, "Szoboszlai", "Midfielder"),
    new Player(8, "Gravenberch", "Midfielder")

};

            // Bind to the ListBox:
            listBox1.DataSource = players;
            listBox1.DisplayMember = "Name";

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Player selectedPlayer = (Player)listBox1.SelectedItem;

            if (selectedPlayer == null)
            {
                MessageBox.Show("Please select a player to add.");
                return;
            }

            if (votedPlayers.Contains(selectedPlayer))
            {
                MessageBox.Show("This player is already in your vote list.");
                return;
            }

            if (votedPlayers.Count >= 3)
            {
                MessageBox.Show("You can only vote for 3 players.");
                return;
            }

            votedPlayers.Add(selectedPlayer);
            listBox2.Items.Add(selectedPlayer.Name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (votedPlayers.Count == 0)
            {
                MessageBox.Show("You haven't selected any players to vote for.");
                return;
            }

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var player in votedPlayers)
                    {
                        // Insert vote record for each player name
                        string query = "INSERT INTO Votes (PlayersVotes) VALUES (@PlayersVotes)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@PlayersVotes", player.Name);
                            cmd.ExecuteNonQuery();
                        }

                        player.AddVote();
                    }
                }

                MessageBox.Show("Your vote has been stored successfully!", "Vote Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);

                votedPlayers.Clear();
                listBox2.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving votes: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a player to remove.");
                return;
            }

            string playerNameToRemove = listBox2.SelectedItem.ToString();
            Player playerToRemove = votedPlayers.FirstOrDefault(p => p.Name == playerNameToRemove);

            if (playerToRemove != null)
            {
                votedPlayers.Remove(playerToRemove);
                listBox2.Items.Remove(playerNameToRemove);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {


        }




        public class Player
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Position { get; set; }
            public int Votes { get; private set; }

            public Player(int id, string name, string position)
            {
                ID = id;
                Name = name;
                Position = position;
                Votes = 0;
            }


            public void AddVote()
            {
                Votes++;
            }

            public override string ToString()
            {
                return $"{Name} ({Position}) - Votes: {Votes}";
            }
        }

    }
}
