using System;
using GridSystem.Structs;
using UnityEngine;

namespace Settings {
    [CreateAssetMenu(menuName = "Settings/SimulationSettings", fileName = "SimulationSettings")]
    public class SimulationSettings : ScriptableObject {
        [Header("General app settings")]
        public Vector2Int gridSize = new Vector2Int(100, 100);

        [Header("Cognitive parameters")]
        public double activationS1 = 0.2;
        public double activationS2 = 1.0;
        
        [Header("Breeding parameters")]
        public double breedingThreshold = 10.0;
        
        [Header("Parameters")]
        public float maxGrass = 10f;
        public float maxMeat = 8f;
        public float probaGrass = 0.07f;
        public float energyGrass = 250f;
        public float energyMeat = 500f;
        public float sizeClusterPrey = 10f;
        public float sizeClusterPredator = 3f;
        public float sizeCluster = 5f;
        public float initNbPrey = 12000f;
        public float initNbPredator = 1200f;
        public float maxEnergyPrey = 650f;
        public float maxEnergyPredator = 1000f;
        public float probaMut = 0.005f;
        public float Mut = 0.15f;
        public float SmallProbaMut = 0.001f;
        public float probaGrowGrass = 0.0085f;
        public float TPrey = 0.75f;
        public float maxAgePrey = 39f;
        public float maxAgePredator = 34f;
        public float ageInterbreedPrey = 6f;
        public float ageInterbreedPredator = 8f;
        public float maxSpeedPrey = 7f;
        public float maxSpeedPredator = 12f;
        public int distanceVisionPrey = 40;
        public int distanceVisionPredator = 50;
        public float birthEnergyPreyMax = 60f;
        public float birthEnergyPredatorMax = 75f;
        public float birthEnergyPrey = 30f;
        public float birthEnergyPredator = 50f;
        public float minEdge = 0.075f;
        public float highMut = 0.2f;
        public float growGrass = 0.8f;
        public float decreaseMeat = 1f;
        public float TPredator = 0.7f;
        
        //TODO: WYPIERDOLIC TO
        public int cogMapComplexity = 30;

        [Header("Visualization")]
        [Tooltip("Can be null, default value will be used")] public GameObject predatorPrefab;
        [Tooltip("Can be null, default value will be used")] public GameObject preyPrefab;
        public Color groundColor = new Color(132f, 59f, 0f);
        public Gradient ggrassGradient;
        public GrassGradient grassGradient = new GrassGradient();
    }   
}
