using System.Collections;
using System.Collections.Generic;
using Agents.LiveableAgents;
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
                if (Random.Range(0f, 10f) < 2f) {
                    // TODO: Set meat to amount from settings
                    CGrid.I.SetMeat(new Vector2Int(i, j), Random.Range(0f, 8f));
                }
                if (Random.Range(0f, 10f) < 2f) {
                    // TODO: Set meat to amount from settings
                    CGrid.I.SetMeat(new Vector2Int(i, j), Random.Range(0f, 8f));
                }
                
                if (Random.Range(0f, 10f) < 2f) {
                    // TODO: Set meat to amount from settings
                    CGrid.I.SetMeat(new Vector2Int(i, j), Random.Range(0f, 8f));
                }
                
                // if (Random.Range(0f, 10f) < 4.5f) {
                //     // TODO: Set meat to amount from settings
                //     CGrid.I.SpawnLiveable(new Predator(), new Vector2Int(i, j));
                // }
                // if (Random.Range(0f, 10f) < 4.5f) {
                //     // TODO: Set meat to amount from settings
                //     CGrid.I.SpawnLiveable(new Prey(), new Vector2Int(i, j));
                // }
            }
        }
    }
}
