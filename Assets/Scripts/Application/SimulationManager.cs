using System;
using CoinPackage.Debugging;
using LogicGrid;
using UnityEngine;
using Utils.Singleton;

namespace Application {
    public class SimulationManager : Singleton<SimulationManager> {
        [SerializeField] private bool isPaused = false;

        private int _steps = 0;

        protected override void Awake() {
            base.Awake();
            InitializeSimulation();
        }

        private void Update() {
            if (!isPaused) {
                Step();
            }
        }

        private void InitializeSimulation() {
            var initializer = new SimulationInitializer();
            initializer.SpawnInitialPreyGroups(initializer.GetPreyClusterPoints(), 10f);
            initializer.SpawnInitialPredatorGroups(initializer.GetPredatorClusterPoints(), 10f);
        }

        private void Step() {
            var time = Time.time;
            Simulation.Update();
            CDebug.Log($"Simulation step {_steps++} took: {Time.time - time}");
        }
    }
}