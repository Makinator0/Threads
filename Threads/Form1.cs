using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input; // Для класса Keyboard

namespace Threads
{
    public partial class Form1 : Form
    {
        private Button button1;
        private Button button2;
        private Thread t1, t2;

        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1 = new Button { Location = new System.Drawing.Point(100, 100), Size = new System.Drawing.Size(75, 23), Text = "Button 1" };
            button2 = new Button { Location = new System.Drawing.Point(200, 100), Size = new System.Drawing.Size(75, 23), Text = "Button 2" };

            this.Controls.Add(button1);
            this.Controls.Add(button2);

            t1 = new Thread(MoveButton1) { IsBackground = true };
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();

            t2 = new Thread(MoveButton2) { IsBackground = true };
            t2.SetApartmentState(ApartmentState.STA);
            t2.Start();
        }

        private void MoveButton1()
        {
            while (true) 
            {
                this.Invoke(new Action(() =>
                {
                    if (Keyboard.IsKeyDown(Key.W))
                        button1.Top -= 10;
                    if (Keyboard.IsKeyDown(Key.A))
                        button1.Left -= 10;
                    if (Keyboard.IsKeyDown(Key.S))
                        button1.Top += 10;
                    if (Keyboard.IsKeyDown(Key.D))
                        button1.Left += 10;

                    LogKeyPress("Player1", button1.Location);
                }));
                Thread.Sleep(10);
            }
        }

        private void MoveButton2()
        {
            while (true) 
            {
                this.Invoke(new Action(() =>
                {
                    if (Keyboard.IsKeyDown(Key.Up))
                        button2.Top -= 10;
                    if (Keyboard.IsKeyDown(Key.Left))
                        button2.Left -= 10;
                    if (Keyboard.IsKeyDown(Key.Down))
                        button2.Top += 10;
                    if (Keyboard.IsKeyDown(Key.Right))
                        button2.Left += 10;

                    LogKeyPress("Player2", button2.Location);
                }));
                Thread.Sleep(10);
            }
        }

        private void LogKeyPress(string player, System.Drawing.Point location)
        {
            Console.WriteLine($"{player} button location: {location}");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (t1 != null && t1.IsAlive) t1.Abort();
            if (t2 != null && t2.IsAlive) t2.Abort();
            base.OnFormClosing(e);
        }
    }
}