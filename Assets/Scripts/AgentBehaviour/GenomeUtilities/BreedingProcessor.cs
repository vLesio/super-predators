using System;
using AgentBehaviour.FuzzyCognitiveMapUtilities;
using Agents.LiveableAgents;
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

        public static bool MutationOccurred() {
            return RandomGenerator.NextDouble() <= DevSet.I.simulation.probaMut;
        }

        public static Liveable Interbreed(Liveable firstParent, Liveable secondParent) {
            var energyGiver = RandomGenerator.NextDouble() <= 0.5 ? firstParent : secondParent;
            var birthEnergy = energyGiver.BirthEnergy;
            
            if (MutationOccurred()) {
                var r = (RandomGenerator.NextDouble() * 2.0 - 1.0) * DevSet.I.simulation.highMut;
                birthEnergy += birthEnergy * r;
            }
            
            var birthEnergyMax = energyGiver.MaxBirthEnergy;
            var childBirthEnergy = Math.Min(birthEnergy, birthEnergyMax);
            var birthEnergyDifference = DevSet.I.simulation.birthEnergyPreyMax - childBirthEnergy;
            
            var bornChildEnergy = birthEnergyMax * (childBirthEnergy + RandomGenerator.NextDouble()
                * birthEnergyDifference) * 0.01;
            
            var childFuzzyCognitiveMap = FuzzyCognitiveMap.InterbreedBrain(firstParent, secondParent);
            
            var randomValue = RandomGenerator.NextDouble() * 50.0 - 25.0;
            var childMaxAge = energyGiver.MaxAge * (1.0 + randomValue * 0.01);
            
            var energyCoefficient = (0.05 + birthEnergy) * 0.005;
            var maxEnergy = energyGiver.MaxEnergy;
            
            firstParent.Attributes[LiveableAttribute.Energy] -= maxEnergy * energyCoefficient;
            secondParent.Attributes[LiveableAttribute.Energy] -= maxEnergy * energyCoefficient;
            
            var child = firstParent.IdenticalLiveable;
            
            child.Attributes[LiveableAttribute.Energy] = bornChildEnergy;
            child.Attributes[LiveableAttribute.MaxAge] = childMaxAge;
            
            child.CognitiveMap = childFuzzyCognitiveMap;
            
            child.CurrentPosition = firstParent.CurrentPosition;
            
            return child;
        }
    }
}