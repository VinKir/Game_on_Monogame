﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectX.Components;
using System;
using System.Collections.Generic;

namespace ProjectX.Entities
{
    public class GameObject : ICloneable
    {
        List<IComponent> components = new List<IComponent>();

        public Transform transform = new Transform();
        public Sprite sprite;
        public bool isStatic;

        public float LifeSpan = 0f;
        public bool isRemoved = false;

        public Rectangle rectangle
        {
            get
            {
                return new Rectangle((int)transform.Position.X - (int)transform.Origin.X,
                (int)transform.Position.Y - (int)transform.Origin.Y,
                (int)((sprite != null ? sprite.texture.Width : 0) * transform.scale),
                (int)((sprite != null ? sprite.texture.Height : 0) * transform.scale));
            }
        }

        public GameObject()
        {
            AddComponent(transform);
        }

        public GameObject(Texture2D texture)
        {
            AddComponent(transform);
            sprite = new Sprite(texture);
            AddComponent(sprite);
            transform.Origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void AddComponent(IComponent component)
        {
            components.Add(component);
            component.boundEntity = this;
        }

        public void RemoveComponent(IComponent component)
        {
            component.boundEntity = null;
            components.Remove(component);
        }

        public T GetComponent<T>() where T : class
        {
            foreach (var item in components)
                if (item as T != null)
                    return item as T;
            return null;
        }

        public virtual void Update(GameTime gameTime)
        {
            transform.Direction = new Vector2((float)Math.Cos(transform.Rotation), (float)Math.Sin(transform.Rotation));
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (sprite != null)
                spriteBatch.Draw(sprite.texture, transform.Position, null, sprite.color,
                    transform.Rotation, transform.Origin, transform.scale, SpriteEffects.None, sprite.layer);
        }

        public object Clone()
        {
            var deepClone = new GameObject();
            deepClone.transform = transform.Clone() as Transform;
            deepClone.sprite = sprite.Clone() as Sprite;
            deepClone.isStatic = isStatic;
            deepClone.isRemoved = isRemoved;
            return deepClone;
        }

        #region Collision

        public bool IsTouchingLeft(GameObject gameObject)
        {
            return rectangle.Right + transform.Velocity.X > gameObject.rectangle.Left &&
                rectangle.Left < gameObject.rectangle.Left &&
                rectangle.Bottom > gameObject.rectangle.Top &&
                rectangle.Top < gameObject.rectangle.Bottom;
        }

        public bool IsTouchingRight(GameObject gameObject)
        {
            return rectangle.Left + transform.Velocity.X < gameObject.rectangle.Right &&
                rectangle.Right > gameObject.rectangle.Right &&
                rectangle.Bottom > gameObject.rectangle.Top &&
                rectangle.Top < gameObject.rectangle.Bottom;
        }

        public bool IsTouchingTop(GameObject gameObject)
        {
            return rectangle.Bottom + transform.Velocity.Y > gameObject.rectangle.Top &&
                rectangle.Top < gameObject.rectangle.Top &&
                rectangle.Right > gameObject.rectangle.Left &&
                rectangle.Left < gameObject.rectangle.Right;
        }

        public bool IsTouchingBottom(GameObject gameObject)
        {
            return rectangle.Top + transform.Velocity.Y < gameObject.rectangle.Bottom &&
                rectangle.Bottom > gameObject.rectangle.Bottom &&
                rectangle.Right > gameObject.rectangle.Left &&
                rectangle.Left < gameObject.rectangle.Right;
        }

        public bool IsTouching(GameObject gameObject)
        {
            return IsTouchingLeft(gameObject) || IsTouchingRight(gameObject) ||
                    IsTouchingTop(gameObject) || IsTouchingBottom(gameObject);
        }

        #endregion
    }
}
