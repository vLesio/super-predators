using System;
using Settings;

namespace Agents.ResourceAgents
{
    public class Meat : ResourceAgent
    {
        public override void ChooseAction()
        {
            throw new System.NotImplementedException();
        }

        public override void Act()
        {
            throw new System.NotImplementedException();
        }
        
        public override bool UpdateQuantity() {
            if (Quantity <= 0) {
                Quantity = 0;
                return false;
            }

            Quantity -= DevSet.I.simulation.decreaseMeat;
            Quantity = Math.Max(0, Quantity);
            
            return true;
        }
    }
}