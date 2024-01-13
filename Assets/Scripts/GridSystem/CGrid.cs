using System;
using System.Collections;
using System.Collections.Generic;
using Agents.LiveableAgents;
using Agents.ResourceAgents;
using CoinPackage.Debugging;
using Settings;
using UnityEngine;
using Utils.Singleton;

namespace GridSystem {
    public class CGrid : Singleton<CGrid> {
        public GameObject preyPrefab;
        public GameObject predatorPrefab;
        
        [SerializeField] private GameObject cellPrefab;

        private SpriteRenderer _renderer;
        private readonly SimulationSettings _settings = DevSet.I.simulation;
        private Dictionary<(int, int), CellUI> _cells = new();

        protected override void Awake() {
            base.Awake();
            _renderer = GetComponent<SpriteRenderer>();
            PreparePrefabs();
            InitializeCells();
        }

        private void PreparePrefabs() {
            preyPrefab = DevSet.I.simulation.preyPrefab == null ? DevSet.I.developer.preyPrefab : DevSet.I.simulation.preyPrefab;
            predatorPrefab = DevSet.I.simulation.predatorPrefab == null ? DevSet.I.developer.predatorPrefab : DevSet.I.simulation.predatorPrefab;
        }

        private void InitializeCells() {
            transform.localScale = new Vector3(_settings.gridSize.x, _settings.gridSize.y, 1);

            var cellWidth = 1f / _settings.gridSize.x;
            var cellHeight = 1f / _settings.gridSize.y;
            var cellShift = new Vector3((float)-_settings.gridSize.x/2 * cellWidth, 
                                        (float)-_settings.gridSize.y/2 * cellHeight);

            if (_settings.gridSize.x % 2 == 0) {
                cellShift += new Vector3(cellWidth/2f, 0f);
            }
            if (_settings.gridSize.y % 2 == 0) {
                cellShift += new Vector3(0f, cellHeight/2f);
            }

            for (var i = 0; i < _settings.gridSize.x; i++) {
                for (var j = 0; j < _settings.gridSize.y; j++) {
                    var cellPosition = new Vector3(cellWidth * i, cellHeight * j) + cellShift;
                    var cell = Instantiate(cellPrefab, transform);
                    cell.transform.localScale = new Vector3(cellWidth, cellHeight, 1f);
                    cell.transform.localPosition = cellPosition;
                    _cells.Add((i, j), cell.GetComponent<CellUI>());
                }
            }
        }

        public void SpawnLiveable(Liveable liveable, Vector2Int cell) {
            _cells[(cell.x, cell.y)].AddLiveable(liveable);
        }

        public void DespawnLiveable(Liveable liveable, Vector2Int cell) {
            _cells[(cell.x, cell.y)].RemoveLiveable(liveable);
        }

        public void MoveLiveable(Liveable liveable, Vector2Int from, Vector2Int to) {
            _cells[(from.x, from.y)].RemoveLiveable(liveable);
            _cells[(to.x, to.y)].AddLiveable(liveable);
        }

        public void SetGrass(Vector2Int cell, float amount) {
            _cells[(cell.x, cell.y)].SetGrass(amount);
        }

        public void SetMeat(Vector2Int cell, float amount) {
            _cells[(cell.x, cell.y)].SetMeat(amount);
        }
    }
}
