using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoinPackage.Debugging;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Application {
    public class RunParametersLoader : MonoBehaviour {
        [SerializeField] private Button startSimulationButton;
        [SerializeField] private Button[] selectParametersButtons;
        [SerializeField] private TextMeshProUGUI selectedParameterTitle;

        private List<SimulationSettings> _runParameters;
        private SimulationSettings _selectedParameter;
        
        private readonly CLogger _logger = Loggers.LoggersList[Loggers.LoggerType.PARAM_SELECTOR];

        private void Awake() {
            LoadRunParametersFromResources();
            InitializeButtons();
        }

        private void InitializeButtons() {
            startSimulationButton.onClick.AddListener(OnStartSimulationClicked);
            var i = 0;
            foreach (var runParameter in _runParameters) {
                selectParametersButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(runParameter.name);
                selectParametersButtons[i].onClick.AddListener((() => {
                    SetSelectedParameter(runParameter);
                }));
                i++;
                if (i >= selectParametersButtons.Length) break;
            }
        }

        private void LoadRunParametersFromResources() {
            _runParameters = Resources.LoadAll<SimulationSettings>(DevSet.I.developer.runParametersResPath).ToList();
        }

        private void OnStartSimulationClicked() {
            if (_selectedParameter is null) {
                _logger.LogWarning("No settings selected.");
                return;
            }
            _logger.Log($"Starting simulation with {_selectedParameter.name % Colorize.Magenta} settings.");
            SceneManager.LoadScene(DevSet.I.developer.simulationSceneName, LoadSceneMode.Single);
        }

        private void SetSelectedParameter(SimulationSettings simulationSettings) {
            _selectedParameter = simulationSettings;
            DevSet.I.simulation = _selectedParameter;
            selectedParameterTitle.SetText(_selectedParameter.name);
        }
    }
}
