using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoTime
{
    public class Particle : BaseEvent
    {
        private Vector2 _displacement;
        private Vector2 _startPosition;
        private Vector2 _startVelocity;
        private Vector2 _acceleration;
        public Particle(Timeline timeline, TimeSpan startTime, TimeSpan endTime, Vector2 startPosition, Vector2 startVelocity, Vector2 acceleration) : base(timeline, startTime, endTime)
        {
            _startPosition = startPosition;
            _startVelocity = startVelocity;
            _acceleration = acceleration;
        }
        public override void Update(TimeSpan time)
        {
            float t = (float)(time - StartTime).TotalSeconds;
            _displacement = _startVelocity * t + 0.5f * _acceleration * t * t;
        }
        public override void Draw(SpriteBatch batch, Vector2 offset)
        {
            batch.Draw(Assets.Pixel, _startPosition + _displacement + offset, null, Color.Red, 0, new Vector2(0.5f), 10f, 0, 0);
        }
    }
}
