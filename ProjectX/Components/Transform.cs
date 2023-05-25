using Microsoft.Xna.Framework;
using ProjectX.Entities;
using System;

namespace ProjectX.Components
{
    public class Transform : ICloneable
    {
        public float scale;
        public Vector2 Position;
        public Vector2 Origin;
        public Vector2 Direction;
        public Transform Parent;

        public Vector2 Velocity;
        public float RotationVelocity = 3f;
        public float LinearVelocity = 3f;

        protected float rotation;
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
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
