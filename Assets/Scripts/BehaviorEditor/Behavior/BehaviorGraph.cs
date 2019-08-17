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
                SetTransitionNode((TransitionNode)node);
            }
            if (node is CommentNode)
            {
                
            }
        }
        public bool IsStateNodeDuplicate(StateNode node)
        {
            bool retVal = false;
            StateNode prevNode = null;
            stateDict.TryGetValue(node.currentState, out prevNode);
            if (prevNode != null)
            {
                retVal = true;
            }
            return retVal;
        }
        #region StateNodes
        void SetStateNode(StateNode node)
        {
            if (node.isDuplicate)
            {
                return;
            }
            if (node.previousState != null)
            {
                stateDict.Remove(node.previousState);
            }
            if (node.currentState == null)
            {
                return;
            }
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
            stateDict.Add(s.state, node);
        }

        void ClearStateNode(StateNode node)
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

        StateNode GetStateNode(State state)
        {
            StateNode r = null;
            stateDict.TryGetValue(state, out r);
            return r;
        }
        #endregion
        #region TransitionNodes

        public bool IsTransitionDuplicate(TransitionNode node)
        {
            bool retVal = false;
            Saved_StateNode savedState = GetSavedState(node.enterState);
            retVal = savedState.IsTransitionDuplicate(node);
            return retVal;
        }

        public void SetTransitionNode(TransitionNode node)
        {
            Saved_StateNode savedState = GetSavedState(node.enterState);
            savedState.SetTransitionNode(node);
        }
        public void ClearTransitionNode()
        {

        }
        #endregion
    }
    [System.Serializable]
    public class Saved_StateNode
    {
        public State state;
        public Vector2 position;
        public bool isCollapsed;

        public List<Saved_Conditions> savedConditions = new List<Saved_Conditions>();
        Dictionary<TransitionNode, Saved_Conditions> savedTransDict = new Dictionary<TransitionNode, Saved_Conditions>();
        Dictionary<Condition, TransitionNode> condDict = new Dictionary<Condition, TransitionNode>();

        public void Init()
        {
            savedTransDict.Clear();
            condDict.Clear();
        }
        public bool IsTransitionDuplicate(TransitionNode node)
        {
            bool retVal = false;
            TransitionNode prevNode = null;
            condDict.TryGetValue(node.targetCondition, out prevNode);
            if (prevNode != null)
            {
                retVal = true;
            }
            return retVal;
        }
        public void SetTransitionNode(TransitionNode node)
        {
            if (node.isDuplicate)
            {
                return;
            }
            if (node.previousCondition != null)
            {
                condDict.Remove(node.targetCondition);
            }
            if (node.targetCondition == null)
            {
                return;
            }
            Saved_Conditions c = GetSavedCondition(node);
            if (c == null)
            {
                c = new Saved_Conditions();
                savedConditions.Add(c);
                savedTransDict.Add(node, c);
                node.transition = node.enterState.currentState.AddTransition();
            }
            c.transition = node.transition;
            c.condition = node.targetCondition;
            c.transition.condition = c.condition;
            c.position = new Vector2(node.windowRect.x, node.windowRect.y);
            condDict.Add(c.condition,node);
        }
        Saved_Conditions GetSavedCondition(TransitionNode node)
        {
            Saved_Conditions r = null;
            savedTransDict.TryGetValue(node, out r);
            return r;
        }

        TransitionNode GetTransitionNode(Transition transition)
        {
            TransitionNode r = null;
            condDict.TryGetValue(transition.condition, out r);
            return r;
        }
    }
    [System.Serializable]
    public class Saved_Conditions
    {
        public Transition transition;
        public Condition condition;
        public Vector2 position;
    }
}
