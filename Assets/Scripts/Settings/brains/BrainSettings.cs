using GridSystem.Structs;
using UnityEngine;

namespace Settings.brains {
    [CreateAssetMenu(menuName = "Settings/BrainSettings", fileName = "BrainSettings")]
    public class BrainSettings : ScriptableObject {
        [Header("Brain matrix")]
        [SerializeField]
        public MatrixRow[] BrainMatrix = new MatrixRow[10];
    }   
}
