using System;
using System.Collections;
using System.Collections.Generic;
using Agents.LiveableAgents;
using CoinPackage.Debugging;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace GridSystem {
    public class CellUI : MonoBehaviour {
        [SerializeField] private GameObject cell;
        [SerializeField] private Image grassPanel;
        [SerializeField] private Image meatImage;

        private Dictionary<Liveable, GameObject> _liveables = new();
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
            
            // var green = (amount) *
            //     (_settings.grassGradient.higherGreenBound - _settings.grassGradient.lowerGreenBound) /
            //     (_settings.maxGrass) + _settings.grassGradient.lowerGreenBound;

            var green = (amount) / _settings.maxGrass;

            grassPanel.color = DevSet.I.simulation.ggrassGradient.Evaluate(green);
            
            // grassPanel.color = new Color(0f, green, 0f);
        }

        public void SetMeat(float amount) {
            var meat = amount / _settings.maxMeat;
            meatImage.color = new Color(1f, 1f, 1f, meat);
            if (meat <= 0f) {
                meatImage.gameObject.SetActive(false);
            }
            else {
                meatImage.gameObject.SetActive(true);
            }
        }

        public void AddLiveable(Liveable liveable) {
            if (liveable.GetType() == typeof(Predator)) {
                var predator = Instantiate(CGrid.I.predatorPrefab, cell.transform);
                _liveables.Add(liveable, predator);
            }else if (liveable.GetType() == typeof(Prey)) {
                var prey = Instantiate(CGrid.I.preyPrefab, cell.transform);
                _liveables.Add(liveable, prey);
            }
            else {
                throw new Exception("Coś dziwnego się odjebało.");
            }
            SetLayoutSize(_liveables.Keys.Count);
        }

        public void RemoveLiveable(Liveable liveable) {
            if (_liveables.TryGetValue(liveable, out var obj)) {
                Destroy(obj);
                _liveables.Remove(liveable);
            }
            else {
                throw new Exception("W komórce nie ma takiego obiektu!");
            }
        }

        public void SetLayoutSize(int liveables) {
            var size = (float)(1 / Math.Ceiling(Math.Sqrt(liveables + (meatImage.gameObject.activeSelf ? 1 : 0))));
            _gridGroup.cellSize = new Vector2(size, size);
        }
    } 
}
