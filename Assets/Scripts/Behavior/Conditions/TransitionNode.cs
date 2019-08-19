using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.BehaviorEditor
{
    public class TransitionNode : ScriptableObject
    {
        /* public bool isDuplicate;
        public Condition targetCondition;
        public Condition previousCondition;

        public Transition transition;

        public StateNode enterState;
        public StateNode targetNode;

        public void Init(StateNode enterState, Transition transition)
        {
            this.enterState = enterState;
        }

        public override void DrawWindow()
        {
            EditorGUILayout.LabelField("");
            targetCondition = (Condition)EditorGUILayout.ObjectField(targetCondition, typeof(Condition), false);

            if (targetCondition == null)
            {
                EditorGUILayout.LabelField("No Condition");
            }
            else {
                if (isDuplicate)
                {
                    EditorGUILayout.LabelField("Duplicate Condition");
                } else {
                    // if (transition != null)
                    // {
                        // transition.disable = EditorGUILayout.Toggle("Disable", transition.disable);
                    // }
                } 
            }
            if (previousCondition != targetCondition)
            {
                isDuplicate = BehaviorEditor.currentGraph.IsTransitionDuplicate(this);
                if (!isDuplicate)
                {
                    BehaviorEditor.currentGraph.SetNode(this);
                }
                previousCondition = targetCondition;
            }
        }

        public override void DrawCurve()
        {
            if (enterState)
            {
                Rect rect = windowRect;
                rect.y += windowRect.height * 0.5f;
                rect.width = 1;
                rect.height = 1;

                BehaviorEditor.DrawNodeCurve(enterState.windowRect, rect, true, Color.black);
            }

        } */
    }
}
