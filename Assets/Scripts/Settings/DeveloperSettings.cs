using UnityEngine;

namespace Settings {
    [CreateAssetMenu(menuName = "Settings/DeveloperSettings", fileName = "DeveloperSettings")]
    public class DeveloperSettings : ScriptableObject {
        [Header("Scenes")]
        public string simulationSceneName = "Simulation";
        public string paramLoaderSceneName = "ParamLoader";

        [Header("Resources paths")]
        public string runParametersResPath = "RunParameters";

        [Header("Default prefabs")] 
        public GameObject predatorPrefab;
        public GameObject preyPrefab;

        [Header("Navigation")]
        [Range(0f, 2f)] public float cameraMovementSpeed = 5f;
        public Vector2 cameraZoomRange = new Vector2(0.1f, 30f);
        public bool slowWhenZoomed = true;
    }   
}
