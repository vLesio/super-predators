using System;
using System.Collections;
using System.Collections.Generic;
using Agents.LiveableAgents;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace GridSystem {
    public class CellUI : MonoBehaviour {
        [SerializeField] private GameObject cell;
        [SerializeField] private Image grassPanel;

        private List<Liveable> _liveables = new();
        private GridLayoutGroup _gridGroup;

        private SimulationSettings _settings;

        private void Awake() {
            _settings = DevSet.I.simulation;
            _gridGroup = cell.GetComponent<GridLayoutGroup>();
            // grassPanel.color = _settings.groundColor;
        }

        public void SetGrass(float amount) {
            if (amount <= 0f) {
                grassPanel.color = _settings.groundColor;
                return;
            }
            // TODO: Add max gradient from settings to calculation
            var green = (amount) *
                (_settings.grassGradient.higherGreenBound - _settings.grassGradient.lowerGreenBound) /
                (_settings.maxGrass) + _settings.grassGradient.lowerGreenBound;
            grassPanel.color = new Color(0f, green, 0f);
            Debug.Log(grassPanel.color);
        }
        public void SetMeat(float amount){}
        public void SetLayoutSize() {}
    } 
}
