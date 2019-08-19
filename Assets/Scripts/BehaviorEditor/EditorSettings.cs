using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.BehaviorEditor
{
    [CreateAssetMenu(menuName ="Editor/Settings")]
    public class EditorSettings : ScriptableObject
    {
        public BehaviorGraph currentGraph;
        public StateNode stateNode;
        public TransitionNode transitionNode;
        public CommentNode commentNode;
    }
}
