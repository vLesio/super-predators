using UnityEngine;

namespace Settings {
    [CreateAssetMenu(menuName = "Settings/SimulationSettings", fileName = "SimulationSettings")]
    public class SimulationSettings : ScriptableObject {
        [Header("General app settings")]
        public Vector2Int gridSize = new Vector2Int(100, 100);

        public double activationS1 = 0.2;
        public double activationS2 = 1.0;
    }   
}
