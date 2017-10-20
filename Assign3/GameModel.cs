using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assign3
{
    class GameModel
    {
        //game constants in meters
        private const float ROAD_LENGTH = 2000.0f;
        private const float ROAD_WIDTH = 10.0f;
        private const float CAR_LENGTH = 4.0f;
        private const float CAR_WIDTH = 2.5f;
        private const float MAX_CAR_SPEED = 28.0f;  // meters per second
        private const float OBSTACLE_LENGTH = 2.0f;
        private const float OBSTACLE_WIDTH = 2.0f;
        private const int POINTS_FOR_PASSING = 2;


        private bool gameOver;
        private TimeSpan timeRemaining;
        private PlayerModel player;
        private List<GameObject> objects;
        private RoadModel road;

        public GameModel()
        {
            // create a new game
            gameOver = false;
            timeRemaining = TimeSpan.FromSeconds(60.0);         
            objects = new List<GameObject>();

            // road
            road = new RoadModel(new PointF(0.0f, ROAD_LENGTH/2), new SizeF(ROAD_WIDTH, ROAD_LENGTH));
            objects.Add(road);

            // obstacles
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                ObstacleModel obstacle = new ObstacleModel(
                    new PointF((float)(rand.NextDouble() * ROAD_WIDTH - ROAD_WIDTH / 2), (i + 1) * 20.0f), 
                    new SizeF(OBSTACLE_WIDTH, OBSTACLE_LENGTH), 
                    new PointF(0.0f,0.0f), 
                    POINTS_FOR_PASSING);
                objects.Add(obstacle);

            }

            // car
            CarModel car = new CarModel(new PointF(0.0f, CAR_LENGTH), new SizeF(CAR_WIDTH, CAR_LENGTH), new PointF(0.0f, 5.0f));
            objects.Add(car);

            // player
            player = new PlayerModel(car);

        }

        public bool GameOver => gameOver;
        public TimeSpan TimeRemaining => timeRemaining;
        public RoadModel Road => road;
        public PlayerModel Player => player;
        public CarModel PlayerCar => player.Car;
        public IEnumerable<GameObject> Obstacles => objects.FindAll((x) => { return x is ObstacleModel; });


        public void UpdateGameState(TimeSpan timePassed)
        {

            // update time remaining
            timeRemaining -= timePassed;

            //end game when time runs out
            if (timeRemaining.TotalSeconds <= 0.0)
            {
                // end game when time runs out
                gameOver = true;
                return;
            }

            // move all the objects (based on their current location, current speed and time passed)
            
            foreach(GameObject obj in objects)
            {
                obj.Move(timePassed);

                
                if (obj != player.Car && obj != road )
                {
                    ObstacleModel obstacle = obj as ObstacleModel;

                    // check for collisions
                    if (player.Car.Collides(obstacle))
                    {
                        // kill the car/obstacle and end the game
                        player.Car.Kill();
                        gameOver = true;
                        return;
                    }

                    // update score based on passing obstacles
                    if (!obstacle.Passed && player.Car.Passes(obstacle))
                    {
                        obstacle.Passed = true;
                        player.Score += (int)(obstacle.PointsForPassing * player.Car.Speed);

                    }
                }

            }

        }
        
    }
}
