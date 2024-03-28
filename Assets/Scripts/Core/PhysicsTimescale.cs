using UnityEngine;
using System;
using ASK.Core;

namespace Core
{
    public class PhysicsTimescale : MonoBehaviour
    {
        private float _timer = 0;
		private float _physTimePassed = 0;
		
		private void Awake() {
			Physics2D.simulationMode = SimulationMode2D.Script;
		}
		
		private void OnEnable() {
			Physics2D.simulationMode = SimulationMode2D.Script;
		}
		
		private void OnDisable() {
			Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
		}
		
		private void FixedUpdate() {
			_timer += Game.TimeManager.DeltaTime;
			// Unity recommends simulating physics in intervals of Time.fixedDeltaTime for stability.
			while (_timer >= Time.fixedDeltaTime)
			{
				_timer -= Time.fixedDeltaTime;
				_physTimePassed += Time.fixedDeltaTime;
				Debug.Log(_physTimePassed);
                Physics2D.Simulate(Time.fixedDeltaTime);
			}
		}
    }
}