using System;
using CoinPackage.Debugging;
using LogicGrid;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Singleton;

namespace Application {
    public class SimulationManager : Singleton<SimulationManager> {
        [SerializeField] private bool isPaused = false;
        [SerializeField] private Button stepOverButton;

        private int _steps = 0;

        private readonly CLogger _simlogger = Loggers.LoggersList[Loggers.LoggerType.SIMULATION];

        protected override void Awake() {
            base.Awake();
            stepOverButton.onClick.AddListener(Step);
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
            initializer.InitializeGrass();
        }

        private void Step() {
            Simulation.predatorActionsTaken.Clear();
            Simulation.preyActionsTaken.Clear();
            Simulation.Update();
            Simulation.LogTakenActions();
            _simlogger.Log($"Step {_steps++ % Colorize.Cyan}, took: {Time.deltaTime % Colorize.Cyan}. Predators: {SimulationGrid.PredatorAgents.Count}, preys: {SimulationGrid.PreyAgents.Count}");
        }
    }
}