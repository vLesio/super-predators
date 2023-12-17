using System.Collections;
using System.Collections.Generic;
using GridSystem;
using Settings;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < DevSet.I.simulation.gridSize.x; i++) {
            for (var j = 0; j < DevSet.I.simulation.gridSize.y; j++) {
                CGrid.I.SetGrass(new Vector2Int(i, j), Random.Range(-DevSet.I.simulation.maxGrass, DevSet.I.simulation.maxGrass));
            }
        }
    }
}
