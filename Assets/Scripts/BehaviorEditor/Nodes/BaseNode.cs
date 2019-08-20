using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SA.BehaviorEditor
{
    [System.Serializable]
    public class BaseNode 
    {
        public DrawNode drawNode;
        // [HideInInspector]
        public Rect windowRect;
        // [HideInInspector]
        public string windowTitle;

        public StateNodeReferences stateRef;
        public TransitionNodeReferences transRef;

        public void DrawWindow()
        {
            if (drawNode != null)
            {
                drawNode.DrawWindow(this);
            }
        }
        public void DrawCurve()
        {
            if (drawNode != null)
            {
                drawNode.DrawCurve(this);
            }
        }
    }
    [System.Serializable]
    public class StateNodeReferences
    {
        [HideInInspector]
        public bool collapse;
        [HideInInspector]
        public bool previousCollapse;
        [HideInInspector]
        public bool isDuplicate;
        [HideInInspector]
        public State currentState;
        [HideInInspector]
        public State previousState;
        [HideInInspector]
        public SerializedObject serializedState;
        [HideInInspector]
        public ReorderableList onStateList;
        [HideInInspector]
        public ReorderableList onEnterList;
        [HideInInspector]
        public ReorderableList onExitList;

    }
    [System.Serializable]
    public class TransitionNodeReferences
    {
        [HideInInspector]
        public bool collapse;
        [HideInInspector]
        public bool previousCollapse;
        [HideInInspector]
        public bool isDuplicate;
        [HideInInspector]
        public State currentState;
        [HideInInspector]
        public State previousState;
        [HideInInspector]
        public SerializedObject serializedState;
        [HideInInspector]
        public ReorderableList onStateList;
        [HideInInspector]
        public ReorderableList onEnterList;
        [HideInInspector]
        public ReorderableList onExitList;
        [HideInInspector]
        public Condition targetCondition;
        [HideInInspector]
        public Condition previousCondition;
        [HideInInspector]
        public StateNode enterState;
        [HideInInspector]
        public StateNode targetNode;
    }
}