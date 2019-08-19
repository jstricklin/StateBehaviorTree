using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class Transition 
    {
        public bool disable;
        public Condition condition;
        public State targetState;
    }
    
}
