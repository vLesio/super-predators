using System;
using System.Collections;
using System.Collections.Generic;
using CoinPackage.Debugging;
using Settings;
using UnityEngine;

namespace GridSystem {
    public class CGrid : MonoBehaviour {
        [SerializeField] private GameObject cellPrefab;
        
        private SpriteRenderer _renderer;

        private readonly SimulationSettings _settings = DevSet.I.simulation;
        // Start is called before the first frame update
        void Awake() {
            _renderer = GetComponent<SpriteRenderer>();
            InitializeCells();
        }

        private void InitializeCells() {
            transform.localScale = new Vector3(_settings.gridSize.x, _settings.gridSize.y, 1);

            var cellWidth = 1f / _settings.gridSize.x;
            var cellHeight = 1f / _settings.gridSize.y;
            var cellShift = new Vector3((float)-Math.Floor(_settings.gridSize.x/2) * cellWidth, 
                                        (float)-Math.Floor(_settings.gridSize.y/2) * cellHeight);

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
                }
            }
        }
    }
}
