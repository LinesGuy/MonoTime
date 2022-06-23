using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MonoTime
{
    public class Timeline
    {
        private TimeSpan _time;
        Color _backgroundColor = new Color(25, 0, 0);
        private int _preDeterminedEventsCurrentIndex = 0;
        public List<BaseEvent> FutureEvents = new List<BaseEvent>();
        public List<BaseEvent> PresentEvents = new List<BaseEvent>();
        public Stack<BaseEvent> PastEvents = new Stack<BaseEvent>();
        public Timeline()
        {

        }
        public Timeline(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            string section = "FileFormatVersion";
            foreach (string line in lines)
            {
                if (line == "" || line.StartsWith("//"))
                    continue;
                if (line.StartsWith('['))
                {
                    section = line.Substring(1, line.Length - 2);
                    continue;
                }
                // TODO implement check that predetermiend events are loaded in correct startTime order
                switch (section)
                {
                    case "FileFormatVersion":
                        if (line != "v1")
                            throw new Exception("Unsupported file format version");
                        break;
                    case "Timeline":
                        string[] parts = line.Split(';');
                        switch (parts[0])
                        {
                            case "PARTICLE":
                                FutureEvents.Add(new Particle(this, Utils.TimeFromStringMs(parts[1]), Utils.TimeFromStringMs(parts[5]), Utils.VectorFromString(parts[2]), Utils.VectorFromString(parts[3]), Utils.VectorFromString(parts[4])));
                                break;
                            case "PLAYER":
                                FutureEvents.Add(new Player(this, TimeSpan.Zero, TimeSpan.MaxValue, Utils.VectorFromString(parts[1]), Vector2.Zero));
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }

            }
            // Add any events that start at time zero to active events
            while (FutureEvents[_preDeterminedEventsCurrentIndex].StartTime == TimeSpan.Zero)
            {
                PresentEvents.Add(FutureEvents[_preDeterminedEventsCurrentIndex]);
                _preDeterminedEventsCurrentIndex++;
            }

        }

        public void Update(TimeSpan time)
        {
            //_time = time;
            TimeSpan _lastTime = _time;
            double speed = 1.0;
            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                speed /= 5.0;
            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                speed *= 5.0;
            _backgroundColor = new Color(25, 0, 0);
            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                _time -= TimeSpan.FromMilliseconds(30) * speed;
                if (_time < TimeSpan.Zero)
                {
                    _time = TimeSpan.Zero;
                }
                _backgroundColor = new Color(0, 0, 25);
            }
                
            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                _time += TimeSpan.FromMilliseconds(30) * speed;
                _backgroundColor = new Color(0, 25, 0);
            }

            if (_lastTime == _time)
                return;
            // Add events if we moved forwards past the start time
            while (_preDeterminedEventsCurrentIndex < FutureEvents.Count && FutureEvents[_preDeterminedEventsCurrentIndex].StartTime < _time)
            {
                PresentEvents.Add(FutureEvents[_preDeterminedEventsCurrentIndex]);
                _preDeterminedEventsCurrentIndex++;
            }
            // Remove events if we moved backwards past the start time
            
            while (_preDeterminedEventsCurrentIndex > 0 && _preDeterminedEventsCurrentIndex <= FutureEvents.Count  && _time < FutureEvents[_preDeterminedEventsCurrentIndex - 1].StartTime)
            {
                //_presentEvents.Remove(_futureEvents[_preDeterminedEventsCurrentIndex - 1]);
                _preDeterminedEventsCurrentIndex--;
            }
            // Remove events if we moved forwards past the end time
            //while (_activeEvents.Count > 0 && _activeEvents[_activeEvents.Count - 1].EndTime < _time)

            // Add events if we moved backwards past the end time
            while (PastEvents.Count > 0 && PastEvents.Peek().EndTime > _time)
            {
                if (PastEvents.Peek() is Player player)
                    PastEvents.Peek().EndTime = TimeSpan.MaxValue;
                PresentEvents.Add(PastEvents.Pop());
            }

            for (int i = PresentEvents.Count - 1; i >= 0; i--)
            {
                if (_time < PresentEvents[i].StartTime)
                {
                    PresentEvents.RemoveAt(i);
                    continue;
                }
                PresentEvents[i].Update(_time);
                if (PresentEvents[i].EndTime <= _time)
                {
                    PastEvents.Push(PresentEvents[i]);
                    PresentEvents.RemoveAt(i);
                }
                    
            }


        }

        public void Draw(SpriteBatch batch)
        {
            GameRoot.Instance.GraphicsDevice.Clear(_backgroundColor);
            // DEBUG
            batch.DrawString(Assets.NovaSquare24, $"Seconds passed: {_time.TotalSeconds}", new Vector2(0, 0), Color.White);
            batch.DrawString(Assets.NovaSquare24, $"index: {_preDeterminedEventsCurrentIndex}", new Vector2(0, 30), Color.White);
            batch.DrawString(Assets.NovaSquare24, $"Past: {PastEvents.Count}, Present: {PresentEvents.Count}, Future: {FutureEvents.Count}", new Vector2(0, 60), Color.White);

            foreach (BaseEvent baseEvent in PresentEvents)
                baseEvent.Draw(batch, Vector2.Zero);
        }
    }
}
