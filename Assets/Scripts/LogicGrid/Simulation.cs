namespace LogicGrid {
    public static class Simulation {
        private static void UpdatePerceptionsForPreys() {
            
        }

        private static void UpdateFuzzyCognitiveMapsForPreys() {
            
        }

        private static void UpdateActionsAndEnergyForPreys() {
            
        }
        
        private static void UpdatePerceptionsForPredators() {
            
        }
        
        private static void UpdateFuzzyCognitiveMapsForPredators() {
            
        }
        
        private static void UpdateActionsAndEnergyForPredators() {
            
        }
        
        private static void UpdatePreySpecies() {
            
        }
        
        private static void UpdatePredatorSpecies() {
            
        }
        
        private static void UpdateGrass() {
            
        }
        
        private static void UpdateMeat() {
            
        }
        
        private static void UpdateAgentsAge() {
            
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