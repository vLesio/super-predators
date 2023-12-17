using Agents.LiveableAgents;
using LogicGrid;

namespace Agents.Actions.LiveableActions
{
    public class Breed : LiveableAction
    {
        public readonly LiveableActionType Type = LiveableActionType.Breed;
        public override bool CheckConditions(Liveable agent)
        {
            agent.attributes[LiveableAttribute.Energy] <= 0.125f * 
            
            var nearestMate = Finder.FindNearestMateForAgent(agent);
            
            
            
            
            
        }

        public override void Invoke(Liveable agent)
        {
            throw new System.NotImplementedException();
        }
    }
}