using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoTime
{
    public abstract class BaseEvent
    {
        protected Timeline _timeline;
        public TimeSpan StartTime;
        public TimeSpan EndTime;
        public BaseEvent(Timeline timeline, TimeSpan startTime, TimeSpan endTime)
        {
            _timeline = timeline;
            StartTime = startTime;
            EndTime = endTime;
        }
        public abstract void Update(TimeSpan time);
        public abstract void Draw(SpriteBatch batch, Vector2 offset);
    }
}
