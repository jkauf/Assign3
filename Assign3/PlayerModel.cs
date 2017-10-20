using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign3
{
    class PlayerModel
    {
        private const float TURN_VELOCITY = 3.5f;      //meters per sec
        private const float ACCELERATION = 5.0f;       //meters per sec
        private int score;
        private CarModel playersCar;

        public PlayerModel(CarModel playersCar)
        {
            this.playersCar = playersCar;
            score = 0;
        }

        public CarModel Car => playersCar;
        public int Score { get { return score; } set { score = value; } }

        public void TurnRight()
        {
            playersCar.SetHorizontalVelocity(TURN_VELOCITY);
        }

        public void TurnLeft()
        {
            playersCar.SetHorizontalVelocity(-TURN_VELOCITY);
        }

        public void GoStraight()
        {
            playersCar.SetHorizontalVelocity(0.0f);
        }

        public void SpeedUp()
        {
            playersCar.ChangeVelocity(0.0f, ACCELERATION);
        }

        public void SlowDown()
        {
            playersCar.ChangeVelocity(0.0f, -ACCELERATION);
        }

    }
}
