using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;


namespace Helpers
{
    public class GameTimerManager : Singleton<GameTimerManager>
    {
        private Dictionary<IncrementType, List<GameTimer2>> _timers = new()
        {
            { IncrementType.UPDATE, new()},
            { IncrementType.FIXED_UPDATE, new()},
            { IncrementType.MANUAL, new()},
        };
        
        public GameTimer2 StartTimer(float duration, Action o, IncrementType i, string name = "Timer")
        {
            GameTimer2 t = GameTimer2.StartNewTimer(duration, o, i, name);
            t.OnComplete = () =>
            {
                o?.Invoke();
                RemoveTimer(t);
            };
            _timers[i].Add(t);
            return t;
        }

        public void FixedUpdate()
        {
            _timers[IncrementType.FIXED_UPDATE].ToList().ForEach(t => GameTimer2.FixedUpdate(t));
        }
        
        public void Update()
        {
            _timers[IncrementType.UPDATE].ToList().ForEach(t => GameTimer2.Update(t));
        }

        public void RemoveTimer(GameTimer2 t)
        {
            _timers[t.TIncrementType].Remove(t);
            GameTimer2.Clear(t);
        }
    }
}