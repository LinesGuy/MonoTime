using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoTime
{
    public class Player : BaseEvent
    {
        private Vector2 _displacement;
        private Vector2 _startPosition;
        private Vector2 _startVelocity = Vector2.Zero;
        public Player(Timeline timeline, TimeSpan startTime, TimeSpan endTime, Vector2 startPosition, Vector2 startVelocity) : base(timeline, startTime, endTime)
        {
            _startPosition = startPosition;
            _startVelocity = startVelocity;
        }
        public override void Update(TimeSpan time)
        {
            float t = (float)(time - StartTime).TotalSeconds;
            Vector2 newVelocity = Vector2.Zero;
            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                newVelocity += new Vector2(-100, 0);
            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                newVelocity += new Vector2(100, 0);
            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                newVelocity += new Vector2(0, -100);
            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                newVelocity += new Vector2(0, 100);
            if (_startVelocity != newVelocity)
            {
                EndTime = time;
                _timeline.PresentEvents.Add(new Player(_timeline, time, TimeSpan.MaxValue, _startPosition + _displacement, newVelocity));
            }
            _displacement = _startVelocity * t; // + 0.5f * _startAcceleration * t * t;
            if (_startVelocity != Vector2.Zero && Utils.Random.Next(3) == 0)
                _timeline.PresentEvents.Add(new Particle(_timeline, time, time + TimeSpan.FromSeconds(1), _startPosition + _displacement, new Vector2(100 * Utils.RandomF - 50, 100 * Utils.RandomF - 50) - _startVelocity, Vector2.Zero));
            if (Input.WasKeyJustDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                for (int i = 0; i < 100; i++)
                {
                    _timeline.PresentEvents.Add(new Particle(_timeline, time, time + TimeSpan.FromSeconds(Utils.Random.NextDouble() * 5.0), _startPosition + _displacement, new Vector2(100 * Utils.RandomF - 50, 100 * Utils.RandomF - 50), Vector2.Zero));
                }
                
            }
        }
        public override void Draw(SpriteBatch batch, Vector2 offset)
        {
            batch.Draw(Assets.Pixel, _startPosition + _displacement + offset, null, Color.Green, 0, new Vector2(0.5f), 10f, 0, 0);
        }
    }
}
