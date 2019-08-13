using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA.BehaviorEditor;

namespace SA
{
    [CreateAssetMenu]
    public class BehaviorGraph : ScriptableObject
    {
        public List<Saved_StateNode> savedStateNodes = new List<Saved_StateNode>();
        Dictionary<StateNode, Saved_StateNode> stateNodesDict = new Dictionary<StateNode, Saved_StateNode>();
        Dictionary<State, StateNode> stateDict = new Dictionary<State, StateNode>();

        public void Init()
        {
            stateNodesDict.Clear();
            stateDict.Clear();
        }
        public void SetNode(BaseNode node)
        {
            if (node is StateNode)
            {
                SetStateNode((StateNode)node);
            }
            if (node is TransitionNode)
            {

            }
            if (node is TransitionNode)
            {
                
            }
        }
        #region StateNodes
        public void SetStateNode(StateNode node)
        {
            Saved_StateNode s = GetSavedState(node);
            if (s == null)
            {
                s = new Saved_StateNode();
                savedStateNodes.Add(s);
                stateNodesDict.Add(node, s);
            }
            s.state = node.currentState;
            s.position = new Vector2(node.windowRect.x, node.windowRect.y);
            s.isCollapsed = node.collapse;
        }

        public void ClearStateNode(StateNode node)
        {
            Saved_StateNode s = GetSavedState(node);
            if (s != null)
            {
                savedStateNodes.Remove(s);
                stateNodesDict.Remove(node);
            }
        }

        Saved_StateNode GetSavedState(StateNode node)
        {
            Saved_StateNode r = null;
            stateNodesDict.TryGetValue(node, out r);
            return r;
        }

        public StateNode GetStateNode(State state)
        {
            StateNode r = null;
            stateDict.TryGetValue(state, out r);
            return r;
        }
        #endregion
    }
    [System.Serializable]
    public class Saved_StateNode
    {
        public State state;
        public Vector2 position;
        public bool isCollapsed;

    }
    [System.Serializable]
    public class Saved_Transition
    {

    }
}
