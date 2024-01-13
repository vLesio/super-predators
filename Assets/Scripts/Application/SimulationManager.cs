using System;
using CoinPackage.Debugging;
using LogicGrid;
using UnityEngine;
using Utils.Singleton;

namespace Application {
    public class SimulationManager : Singleton<SimulationManager> {
        [SerializeField] private bool isPaused = false;

        private int _steps = 0;
        private void Update() {
            if (!isPaused) {
                Step();
            }
        }

        private void Step() {
            var time = Time.time;
            Simulation.Update();
            CDebug.Log($"Simulation step {_steps++} took: {Time.time - time}");
        }
    }
}