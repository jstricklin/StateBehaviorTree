using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA.BehaviorEditor;

namespace SA
{
    [CreateAssetMenu]
    public class BehaviorGraph : ScriptableObject
    {
        public List<BaseNode> windows = new List<BaseNode>();
        #region Checkers
        public bool IsStateNodeDuplicate(StateNode node)
        {
            return false;
        }
        #endregion
    }

}