using AgentBehaviour.FuzzyCognitiveMapUtilities;
using Agents.LiveableAgents;
using Agents.ResourceAgents;
using LogicGrid;
using Settings;

namespace Agents.Actions.LiveableActions
{
    public class Eat : LiveableAction
    {
        public Eat()
        {
            ActionType = LiveableActionType.Eat;
        }
        public override bool CheckConditions(Liveable agent)
        {
            ResourceAgent currentGrass = Liveable.IsPrey(agent) ? Finder.FindGrassOnAgentPosition(agent) : Finder.FindMeatOnAgentPosition(agent);
            if(currentGrass == null)
            {
                return false;
            }
            
            // TODO: PDF BAD HELP
            return currentGrass is { Quantity: >= 1 };
        }

        public override void Invoke(Liveable agent)
        {
            ResourceAgent currentFood = Liveable.IsPrey(agent) ? Finder.FindGrassOnAgentPosition(agent) : Finder.FindMeatOnAgentPosition(agent);
            if (currentFood == null)
            {
                return;
            }
            currentFood.Quantity -= 1;
            agent.Attributes[LiveableAttribute.Energy] += ResourceAgent.IsGrass(currentFood) ? DevSet.I.simulation.energyGrass : DevSet.I.simulation.energyMeat;
            agent.CognitiveMap.MultiplyNamedInternalConcept(NamedInternalConcept.Hunger, 0.25f);
            //TODO: DONE: Divide cognitive map for hunger by 4 using MultiplyNamedInternalConcept
        }
    }
}