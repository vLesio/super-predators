using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Utils.Singleton;

namespace Settings {
    [RequireComponent(typeof(DoNotDestroy))]
    public class DevSet : Singleton<DevSet> {
        public SimulationSettings simulation;
        public DeveloperSettings developer;
    }   
}
