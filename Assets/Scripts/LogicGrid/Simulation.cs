using System;
using System.Linq;

namespace LogicGrid {
    public static class Simulation {
        private static void UpdatePerceptionsForPreys() {
            foreach (var preyList in SimulationGrid.PreyAgents.Values) {
                foreach (var prey in preyList) {
                    prey.UpdateAttributesDependentOnGrid();
                    prey.UpdateAttributesDependentOnLocalCell();
                }
            }
        }

        private static void UpdateFuzzyCognitiveMapsForPreys() {
            foreach (var preyList in SimulationGrid.PreyAgents.Values) {
                foreach (var prey in preyList) {
                    prey.CognitiveMap.UpdateState();
                }
            }
        }

        private static void UpdateActionsAndEnergyForPreys() {
            throw new NotImplementedException();
        }
        
        private static void UpdatePerceptionsForPredators() {
            foreach (var predatorList in SimulationGrid.PredatorAgents.Values) {
                foreach (var predator in predatorList) {
                    predator.UpdateAttributesDependentOnGrid();
                    predator.UpdateAttributesDependentOnLocalCell();
                }
            }
        }
        
        private static void UpdateFuzzyCognitiveMapsForPredators() {
            foreach (var predatorList in SimulationGrid.PredatorAgents.Values) {
                foreach (var predator in predatorList) {
                    predator.CognitiveMap.UpdateState();
                }
            }
        }
        
        private static void UpdateActionsAndEnergyForPredators() {
            throw new NotImplementedException();
        }
        
        private static void UpdatePreySpecies() {
            throw new NotImplementedException();
        }
        
        private static void UpdatePredatorSpecies() {
            throw new NotImplementedException();
        }
        
        private static void UpdateGrass() {
            throw new NotImplementedException();
        }
        
        private static void UpdateMeat() {
            throw new NotImplementedException();
        }
        
        private static void UpdateAgentsAge() {
            throw new NotImplementedException();
        }


        private static void UpdatePreysStep() {
            UpdatePerceptionsForPreys();
            UpdateFuzzyCognitiveMapsForPreys();
            UpdateActionsAndEnergyForPreys();
        }
        
        private static void UpdatePredatorsStep() {
            UpdatePerceptionsForPredators();
            UpdateFuzzyCognitiveMapsForPredators();
            UpdateActionsAndEnergyForPredators();
        }
        
        private static void UpdateSpeciesStep() {
            UpdatePreySpecies();
            UpdatePredatorSpecies();
        }
        
        private static void UpdateEnvironmentStep() {
            UpdateGrass();
            UpdateMeat();
        }
        
        public static void Update() {
            UpdatePreysStep();
            UpdatePredatorsStep();
            UpdateSpeciesStep();
            UpdateEnvironmentStep();
            UpdateAgentsAge();
        }
    }
}