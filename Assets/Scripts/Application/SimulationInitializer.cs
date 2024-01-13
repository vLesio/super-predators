using System;
using Settings;

namespace Application {
    public class SimulationInitializer {


        public void InitializeClusters() {
            var preyClustersCount = 
                Math.Ceiling(DevSet.I.simulation.initNbPrey / DevSet.I.simulation.sizeClusterPrey);
            var predatorClustersCount =
                Math.Ceiling(DevSet.I.simulation.initNbPredator / DevSet.I.simulation.sizeClusterPredator);
        }
    }
}