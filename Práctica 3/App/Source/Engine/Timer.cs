using System.Collections.Generic;

namespace TcGame
{
    public delegate void TimerDelegate();

    public class Timer
    {
        private class Entry
        {
            public float timeLeft;

            public float time;

            public TimerDelegate callback;

            public bool loop;

            public Entry(float seconds, TimerDelegate inCallback, bool inLoop)
            {
                timeLeft = seconds;
                time = seconds;
                callback = inCallback;
                loop = inLoop;
            }
        }

        private List<Entry> entries;

        public Timer()
        {
            entries = new List<Entry>();
        }

        /// <summary>
        /// Register a new Timer
        /// </summary>
        public void SetTimer(float seconds, TimerDelegate callback, bool loop = false)
        {
            ClearTimer(callback);

            Entry e = new Entry(seconds, callback, loop);
            entries.Add(e);
        }

        /// <summary>
        /// Removes a previous registered timer
        /// </summary>
        public void ClearTimer(TimerDelegate callback)
        {
            entries.RemoveAll(x => x.callback == callback);
        }

        public void Update(float deltaSeconds)
        {
            for (int i = 0; i < entries.Count; ++i)
            {
                Entry e = entries[i];

                e.timeLeft -= deltaSeconds;

                if (e.timeLeft <= 0.0f)
                {
                    e.callback.Invoke();

                    if (e.loop)
                    {
                        e.timeLeft = e.time;
                    }
                }
            }

            entries.RemoveAll(x => x.timeLeft < 0.0f);
        }
    }
}

