using System;
using ASK.Core;

namespace Helpers
{
    public class GameTimer2
    {
        private GameTimer _timer;
        public Action OnComplete;
        public IncrementType TIncrementType;

        protected GameTimer2(float duration, string name, Action o, IncrementType i)
        {
            _timer = GameTimer.StartNewTimer(duration, name);
            TIncrementType = i;
            OnComplete = o;
        }
        
        public static GameTimer2 StartNewTimer(float duration, Action o, IncrementType i, string name = "Timer")
        {
            GameTimer2 timer = new GameTimer2(duration, name, o, i);
            return timer;
        }

        public static void FixedUpdate(GameTimer2 t)
        {
            GameTimer.FixedUpdate(t._timer);
            if (TimerFinished(t))
            {
                t.OnComplete?.Invoke();
            }
        }
        
        public static void Update(GameTimer2 t)
        {
            GameTimer.Update(t._timer);
            if (TimerFinished(t))
            {
                t.OnComplete?.Invoke();
            }
        }
        
        public static bool TimerRunning(GameTimer2 t)
        {
            return t != null && GameTimer.GetTimerState(t._timer) == TimerState.Running;
        }
        
        public static bool TimerFinished(GameTimer2 t)
        {
            return t != null && GameTimer.TimerFinished(t._timer);
        }

        public static void Clear(GameTimer2 t)
        {
            if (t != null) GameTimer.Clear(t._timer);
        }
    }

    public enum IncrementType {
        UPDATE,
        FIXED_UPDATE,
        MANUAL
    }
}