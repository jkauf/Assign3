using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assign3
{
    public partial class Form1 : Form
    {

        private GameModel theGame;
        private DateTime lastTick;
        private float PIXELS_PER_METER = 10.0f;
        public Form1()
        {
                      
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NewGame();
        }

        private void NewGame()
        {
            theGame = new GameModel();
            animationTimer.Start();
            lastTick = DateTime.Now;
        }
      

        private void DrawGame(Graphics g, Size screenSize)
        {
            // draw road, obstacles, and car
            g.Clear(Color.Green);

            // define the cameraOffset to be so many meters behind the car
            float cameraOffset = theGame.PlayerCar.Location.Y - theGame.PlayerCar.Extents.Height;

            // road           
            Rectangle roadRect = ModelToScreen(theGame.Road.Location, theGame.Road.Extents, cameraOffset, ClientRectangle.Size);
            g.FillRectangle(Brushes.Gray, roadRect);

            // obstacles
            foreach(GameObject obj in theGame.Obstacles)
            {
                ObstacleModel obstacle = obj as ObstacleModel;
                Rectangle obstacleRect = ModelToScreen(obstacle.Location, obstacle.Extents, cameraOffset, ClientRectangle.Size);
                g.FillEllipse(Brushes.Blue, obstacleRect);
            }
            

            // car
            Rectangle carRect = ModelToScreen(theGame.PlayerCar.Location, theGame.PlayerCar.Extents, cameraOffset, ClientRectangle.Size);
            if(theGame.PlayerCar.Alive)               
                g.FillRectangle(Brushes.Red, carRect);
            else
                g.FillRectangle(Brushes.Black, carRect);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawGame(e.Graphics, ClientRectangle.Size);
        }

        private Point ModelToScreen(PointF modelPt, float cameraOffset, Size screenSize)
        {
            Point screenPt = new Point();

            screenPt.X = (int)(screenSize.Width / 2 + modelPt.X * PIXELS_PER_METER);
            screenPt.Y = (int)(screenSize.Height - (modelPt.Y - cameraOffset) * PIXELS_PER_METER);

            return screenPt;
        }

        private Rectangle ModelToScreen(PointF modelLocation, SizeF modelExtents, float cameraOffset, Size screenSize)
        {
            
            // scale width and height from meters to pixels
            Size screenExtents = new Size((int)(modelExtents.Width * PIXELS_PER_METER), (int)(modelExtents.Height * PIXELS_PER_METER));

            // translate the location of the object to screen coordinates
            Point screenLocation = ModelToScreen(modelLocation, cameraOffset, screenSize);

            // move the location to the upper left corner of the rectangle
            screenLocation.Offset(-screenExtents.Width / 2, -screenExtents.Height / 2);

            return new Rectangle(screenLocation, screenExtents);
        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            // temporarily, allow the user to advance the game by clicking in the form
            theGame.UpdateGameState(DateTime.Now - lastTick);
            if (theGame.GameOver)
                animationTimer.Stop();
            lastTick = DateTime.Now;
            Refresh();
            scoreLabel.Text = theGame.Player.Score.ToString();
            timeLabel.Text = theGame.TimeRemaining.Seconds.ToString();
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                theGame.Player.TurnLeft();
            }
            else if (e.KeyCode == Keys.Right)
            {
                theGame.Player.TurnRight();
            }
            else if (e.KeyCode == Keys.Up)
            {
                theGame.Player.SpeedUp();
            }
            else if(e.KeyCode == Keys.Down)
            {
                theGame.Player.SlowDown();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                theGame.Player.GoStraight();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            NewGame();
        }
    }
}
 