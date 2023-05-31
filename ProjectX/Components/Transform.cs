using Microsoft.Xna.Framework;
using ProjectX.Entities;
using System;

namespace ProjectX.Components
{
    public class Transform : IComponent, ICloneable
    {
        public float scale;
        public Vector2 Position;
        public Vector2 Origin;
        public Transform Parent;

        public Vector2 Velocity;
        public float RotationVelocity = 3f;
        public float LinearVelocity = 3f;

        GameObject gameObject;
        public GameObject boundEntity { get => gameObject; set => gameObject = value; }

        protected float rotation;
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        Vector2 direction;
        public Vector2 Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                Rotation = (float)Math.Atan2(direction.Y, direction.X);
            }
        }

        public Transform()
        {
            Position = new Vector2(0, 0);
            Origin = new Vector2(0, 0);
            Direction = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            scale = 1;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

    }
}
