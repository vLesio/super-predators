using UnityEngine;

namespace Settings {
    [CreateAssetMenu(menuName = "Settings/SimulationSettings", fileName = "SimulationSettings")]
    public class SimulationSettings : ScriptableObject {
        [Header("General app settings")]
        public Vector2 gridSize = new Vector2(100, 100);
    }   
}
