﻿using System;
using CoinPackage.Debugging;
using LogicGrid;
using UnityEngine;
using UnityEngine.UI;
using Utils.Singleton;

namespace Application {
    public class SimulationManager : Singleton<SimulationManager> {
        [SerializeField] private bool isPaused = false;
        [SerializeField] private Button stepOverButton;

        private int _steps = 0;

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
            Simulation.Update();
            CDebug.Log($"Predators: {SimulationGrid.PredatorAgents.Count}");
            CDebug.Log($"Preys: {SimulationGrid.PreyAgents.Count}");
            CDebug.Log($"Simulation step {_steps++} took: {Time.deltaTime}");
        }
    }
}