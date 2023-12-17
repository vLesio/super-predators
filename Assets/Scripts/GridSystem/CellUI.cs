using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem {
    public class CellUI : MonoBehaviour {
        [SerializeField] private GameObject cell;
        
        private SpriteRenderer _cellRenderer;
        
        void Awake() {
            _cellRenderer = cell.GetComponent<SpriteRenderer>();
        }
    } 
}
