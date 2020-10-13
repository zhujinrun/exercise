using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    public class DirectorBase
    {
        protected IList<DecisionBase> decisions = new List<DecisionBase>();

        protected virtual void Insert(DecisionBase decision)
        {
            if (decision == null || decision.Factory == null || decision.Quantity < 0) throw new ArgumentException("decision");
        }
        public virtual IEnumerable<DecisionBase> Decisions { get { return decisions; } }
    }
}
