using UnityEngine;
using System;
using ASK.Core;

namespace Core
{
    public class PhysicsTimescale : MonoBehaviour
    {
        private float _timer = 0;
		
		private void Awake() {
			Physics.simulationMode = SimulationMode.Script;
		}
		
		private void OnEnable() {
			Physics.simulationMode = SimulationMode.Script;
		}
		
		private void OnDisable() {
			Physics.simulationMode = SimulationMode.FixedUpdate;
		}
		
		private void Update() {
			_timer += Game.TimeManager.DeltaTime;
			// Unity recommends simulating physics in intervals of Time.fixedDeltaTime for stability.
			while (_timer >= Time.fixedDeltaTime)
			{
				_timer -= Time.fixedDeltaTime;
                Physics.Simulate(Time.fixedDeltaTime);
			}
		}
    }
}