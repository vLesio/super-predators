using UnityEngine;

namespace Settings {
    [CreateAssetMenu(menuName = "Settings/DeveloperSettings", fileName = "DeveloperSettings")]
    public class DeveloperSettings : ScriptableObject {
        [Header("Scenes")]
        public string simulationSceneName = "Simulation";
        public string paramLoaderSceneName = "ParamLoader";

        [Header("Resources paths")]
        public string runParametersResPath = "RunParameters";
    }   
}
