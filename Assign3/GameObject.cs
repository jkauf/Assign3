using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assign3
{
    class GameObject
    {
        //TODO: what units are we actually using? and what do they mean??
        private PointF location;
        private SizeF extents;
        private PointF velocity;
        private bool alive;         // true = alive, false = dead

        public GameObject(PointF location, SizeF extents, PointF velocity)
        {
            // GameObject.GameObject() 
            this.location = location;
            this.extents = extents;
            this.velocity = velocity;
            alive = true;
        }

        public float Speed { get { return (float)Math.Sqrt(velocity.Y * velocity.Y + velocity.Y * velocity.Y); } }
        public PointF Location => location;
        public SizeF Extents => extents;

        public void Move(TimeSpan timePassed)
        {
            // dy = vy * dt;    dx = vx * dt;
            // newy = curry + vy * dt;      newx = currx + vx * dt;
            float newy = location.Y + velocity.Y * (float)timePassed.TotalSeconds;
            float newx = location.X + velocity.X * (float)timePassed.TotalSeconds;

            location = new PointF(newx, newy);
        }

        public bool Alive => alive;
        public void Kill()
        {
            alive = false;
        }

        public void ChangeVelocity(float dvx, float dvy)
        {
            velocity = new PointF(velocity.X + dvx, velocity.Y + dvy);
        }

        public void SetHorizontalVelocity(float vx)
        {
            velocity = new PointF(vx, velocity.Y);
        }
    }

    interface ICarObserver
    {
        void UpdateVelocity();
    }

    class CarModel : GameObject
    {
        private List<ICarObserver> observers;

        public CarModel(PointF location, SizeF extents, PointF velocity) : base(location, extents, velocity)
        {
            observers = new List<ICarObserver>();
        }

        public void RegisterObserver(ICarObserver observer) { observers.Add(observer); }
        public void UnRegisterObserver(ICarObserver observer) { observers.Remove(observer); }

        private void NotifyVelocityObservers()
        {
            foreach (ICarObserver observer in observers)
                observer.UpdateVelocity();
        }

        public bool Collides(GameObject obj)
        {
            // car collides with object when its extents overlap with obj's extents
            return Math.Abs(this.Location.X - obj.Location.X) < (this.Extents.Width / 2 + obj.Extents.Width / 2) &&
               Math.Abs(this.Location.Y - obj.Location.Y) < (this.Extents.Height / 2 + obj.Extents.Height / 2);
                
        }

        public bool Passes(GameObject obj)
        {
            // car passes the object when the car's y location > object's y location
            return this.Location.Y > obj.Location.Y;
        }

    }

    class ObstacleModel : GameObject
    {
        private int pointsForPassing;
        

        public ObstacleModel(PointF location, SizeF extents, PointF velocity, int pointsForPassing) : base(location, extents, velocity)
        {
            this.pointsForPassing = pointsForPassing;
            Passed = false;
        }

        public int PointsForPassing => pointsForPassing;
        public bool Passed { get; set; }
    }

    class RoadModel : GameObject
    {
        public RoadModel(PointF location, SizeF extents) : base(location, extents, new PointF(0,0))
        {

        }
    }

}
