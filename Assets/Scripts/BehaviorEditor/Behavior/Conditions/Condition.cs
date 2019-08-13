using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool CheckCondition(StateManager state);
    }
}
