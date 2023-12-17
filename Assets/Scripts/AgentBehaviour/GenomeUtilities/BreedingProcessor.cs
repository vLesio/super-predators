using System;
using AgentBehaviour.QuasiCognitiveMap;
using Settings;

namespace AgentBehaviour.GenomeUtilities {
    public static class BreedingProcessor {
        private static readonly Random RandomGenerator = new Random();
        
        public static double CalculateBreedingProbability(FuzzyCognitiveMap firstGenome, FuzzyCognitiveMap secondGenome) {
            var genomeDistance = firstGenome.CalculateDistance(secondGenome);
            
            if (genomeDistance > DevSet.I.simulation.breedingThreshold) {
                return 0;
            }

            return 1 - genomeDistance / DevSet.I.simulation.breedingThreshold;
        }
        
        public static bool WasBreedingSuccessful(FuzzyCognitiveMap firstGenome, FuzzyCognitiveMap secondGenome) {
            var breedingProbability = CalculateBreedingProbability(firstGenome, secondGenome);
            var randomValue = RandomGenerator.NextDouble();

            return randomValue <= breedingProbability;
        }
    }
}