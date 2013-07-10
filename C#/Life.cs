using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ConwaysLife
{
    public partial class Form1 : Form
    {
        Graphics front;
        bool open, running, clickDown;
        bool[,] board1, board2;
        int[,] score;
        int width, height;
        Brush white, black;
        public Form1()
        {
            InitializeComponent();

            SetupBuffers();

            Disposed += new EventHandler(Form1_Disposed);
            Resize += new EventHandler(Form1_Resize);
            Activated += new EventHandler(Form1_Activated);
            Closing += new CancelEventHandler(Form1_Closing);
            MouseDown += new MouseEventHandler(Form1_MouseDown);
            MouseMove += new MouseEventHandler(Form1_MouseMove);
            MouseUp += new MouseEventHandler(Form1_MouseUp);

            white = new SolidBrush(Color.White);
            black = new SolidBrush(Color.Black);

            running = false;
            open = true;
            clickDown = false;
        }

        void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            clickDown = false;
        }
        static int SIZE = 5;
        int clickX, clickY;
        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (clickDown)
            {
                clickX = (e.X - SIZE / 2) / SIZE;
                clickY = (e.Y - SIZE / 2) / SIZE;
                if (clickX >= 0 && clickX < width
                    && clickY >= 0 && clickY < height
                    && board1[clickX, clickY] == board2[clickX, clickY])
                    board1[clickX, clickY] = !board1[clickX, clickY];
            }
        }

        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!running)
                clickDown = true;
        }

        void Form1_Closing(object sender, CancelEventArgs e)
        {
            if (open)
            {
                open = false;
                e.Cancel = true;
            }
        }
        void Form1_Activated(object sender, EventArgs e)
        {
            while (open)
            {
                if (running)
                {
                    UpdateIt();
                }
                Draw();
                Application.DoEvents();
            }
            Application.Exit();
        }
        int loopX, loopY;
        void UpdateIt()
        {
            for (loopY = 0; loopY < height; ++loopY)
                for (loopX = 0; loopX < width; ++loopX)
                    if (board1[loopX, loopY])
                    {
                        score[loopX, loopY]++;
                        if (loopX < width - 1)
                            score[loopX + 1, loopY]++;
                        else
                            score[0, loopY]++;
                        if (loopX > 0)
                            score[loopX - 1, loopY]++;
                        else
                            score[width - 1, loopY]++;
                        if (loopY < height - 1)
                        {
                            score[loopX, loopY + 1]++;
                            if (loopX < width - 1)
                                score[loopX + 1, loopY + 1]++;
                            else
                                score[0, loopY + 1]++;
                            if (loopX > 0)
                                score[loopX - 1, loopY + 1]++;
                            else
                                score[width - 1, loopY + 1]++;
                        }
                        else
                        {
                            score[loopX, 0]++;
                            if (loopX < width - 1)
                                score[loopX + 1, 0]++;
                            else
                                score[0, 0]++;
                            if (loopX > 0)
                                score[loopX - 1, 0]++;
                            else
                                score[width - 1, 0]++;
                        }
                        if (loopY > 0)
                        {
                            score[loopX, loopY - 1]++;
                            if (loopX < width - 1)
                                score[loopX + 1, loopY - 1]++;
                            else
                                score[0, loopY - 1]++;
                            if (loopX > 0)
                                score[loopX - 1, loopY - 1]++;
                            else
                                score[width - 1, loopY - 1]++;
                        }
                        else
                        {
                            score[loopX, height - 1]++;
                            if (loopX < width - 1)
                                score[loopX + 1, height - 1]++;
                            else
                                score[0, height - 1]++;
                            if (loopX > 0)
                                score[loopX - 1, height - 1]++;
                            else
                                score[width - 1, height - 1]++;
                        }
                    }
            for (loopY = 0; loopY < height; ++loopY)
                for (loopX = 0; loopX < width; ++loopX)
                {
                    if (board1[loopX, loopY])
                    {
                        if (score[loopX, loopY] < 3 || score[loopX, loopY] > 4)
                            board1[loopX, loopY] = false;
                    }
                    else if (score[loopX, loopY] == 3)
                        board1[loopX, loopY] = true;
                    score[loopX, loopY] = 0;
                }
        }
        void Draw()
        {
            for (loopY = 0; loopY < height; ++loopY)
            {
                for (loopX = 0; loopX < width; ++loopX)
                {
                    if (board1[loopX, loopY] != board2[loopX, loopY])
                    {
                        front.FillRectangle(board1[loopX, loopY] ? black : white, loopX * SIZE + SIZE / 2 + 1, loopY * SIZE + SIZE / 2 + 1, SIZE - 2, SIZE - 2);
                        if (!clickDown)
                            board2[loopX, loopY] = board1[loopX, loopY];
                    }
                }
            }
        }
        void SetupBuffers()
        {
            front = this.CreateGraphics();
            width = this.ClientSize.Width / SIZE - 1;
            height = this.ClientSize.Height / SIZE - 1;
            board1 = new bool[width, height];
            board2 = new bool[width, height];
            score = new int[width, height];
        }

        void Form1_Resize(object sender, EventArgs e)
        {
            front.Dispose();
            SetupBuffers();
        }

        void Form1_Disposed(object sender, EventArgs e)
        {
            front.Dispose();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (miStartStop.Text == "Start")
            {
                running = true;
                miStartStop.Text = "Stop";
            }
            else
            {
                running = false;
                miStartStop.Text = "Start";
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            for (loopY = 0; loopY < height; ++loopY)
                for (loopX = 0; loopX < width; ++loopX)
                    board1[loopX, loopY] = false;
            this.running = false;
            miStartStop.Text = "Start";
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            this.open = false;
        }
    }
}