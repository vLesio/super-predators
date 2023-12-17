using System.Collections.Generic;
using AgentBehaviour.QuasiCognitiveMap;
using Agents.Actions.LiveableActions;
using Unity.VisualScripting;
using UnityEngine;

namespace Agents.LiveableAgents
{
    public enum LiveableAttribute
    {
        Age,
        Energy,
        Speed,
        FoodClose,
        FoodFar,
        MateClose,
        MateFar,
        EnemyClose,
        EnemyFar,
        QuantityOfLocalFood,
        QuantityOfLocalMates,
        SexualNeeds
    }
    public abstract class Liveable : SimulationAgent
    {
        public abstract List<LiveableAction> PossibleActions {
            get;
        }
        
        public Dictionary<LiveableAttribute, float> attributes = new Dictionary<LiveableAttribute, float>();
        public LiveableAction currentAction;
        public FuzzyCognitiveMap cogMap;
        
        public abstract void CreateMap();
        
        public void InitLiveable()
        {
            attributes.Add(LiveableAttribute.Age, 0);
            attributes.Add(LiveableAttribute.Energy, 0);
            attributes.Add(LiveableAttribute.Speed, 0);
            attributes.Add(LiveableAttribute.FoodClose, 0);
            attributes.Add(LiveableAttribute.FoodClose, 0);
            attributes.Add(LiveableAttribute.FoodFar, 0);
            attributes.Add(LiveableAttribute.MateClose, 0);
            attributes.Add(LiveableAttribute.MateFar, 0);
            attributes.Add(LiveableAttribute.EnemyClose, 0);
            attributes.Add(LiveableAttribute.EnemyFar, 0);
            attributes.Add(LiveableAttribute.QuantityOfLocalFood, 0);
            attributes.Add(LiveableAttribute.QuantityOfLocalMates, 0);
            attributes.Add(LiveableAttribute.SexualNeeds, 0);
        }

        public static bool IsPrey(Liveable agent)
        {
            return agent.GetType() == typeof(Prey);
        } 
    }
}