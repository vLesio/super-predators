using System;
using CoinPackage.Debugging;
using LogicGrid;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Singleton;

namespace Application {
    public class SimulationManager : Singleton<SimulationManager> {
        [SerializeField] private bool isPaused = false;
        [SerializeField] private Button stepOverButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private TextMeshProUGUI sliderValueText;

        private int _steps = 0;
        private float _lastStep = 0f;
        private TextMeshProUGUI _playButtonText;
        

        private readonly CLogger _simlogger = Loggers.LoggersList[Loggers.LoggerType.SIMULATION];

        protected override void Awake() {
            base.Awake();

            _playButtonText = playButton.GetComponentInChildren<TextMeshProUGUI>();

            stepOverButton.onClick.AddListener(Step);
            speedSlider.onValueChanged.AddListener(value => sliderValueText.SetText($"TBS: {value.ToString("F")}"));
            playButton.onClick.AddListener(() => {
                isPaused = !isPaused;
                _playButtonText.text = isPaused ? "Play" : "Pause";
                stepOverButton.interactable = isPaused;
            });
            InitializeSimulation();
        }

        private void Update() {
            if (!isPaused && Time.time - _lastStep > speedSlider.value) {
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
            _lastStep = Time.time;
            Simulation.LogTakenActions();
            _simlogger.Log($"Step {_steps++ % Colorize.Cyan}, took: {Time.deltaTime % Colorize.Cyan}." +
                           $"\tPredators -> {SimulationGrid.GetNumberOfPredators() % Colorize.Magenta} ({SimulationGrid.PredatorAgents.Count}), " +
                           $"Preys -> {SimulationGrid.GetNumberOfPreys() % Colorize.Magenta} ({SimulationGrid.PreyAgents.Count})");
        }
    }
}