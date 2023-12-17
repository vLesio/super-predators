using GridSystem.Structs;
using UnityEngine;

namespace Settings {
    [CreateAssetMenu(menuName = "Settings/SimulationSettings", fileName = "SimulationSettings")]
    public class SimulationSettings : ScriptableObject {
        [Header("General app settings")]
        public Vector2Int gridSize = new Vector2Int(100, 100);

        [Header("Parameters")]
        public float maxGrass = 10f;

        [Header("Visualization")] 
        public Color groundColor = new Color(132f, 59f, 0f);
        public GrassGradient grassGradient = new GrassGradient();
    }   
}
