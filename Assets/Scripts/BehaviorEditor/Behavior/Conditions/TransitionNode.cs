using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.BehaviorEditor
{
    public class TransitionNode : BaseNode
    {
        public Transition targetTransition;
        public StateNode enterState;
        public StateNode targetNode;

        public void Init(StateNode enterState, Transition transition)
        {
            this.enterState = enterState;
            targetTransition = transition;
        }

        public override void DrawWindow()
        {
            if (targetTransition == null)
            {
                return;
            }
            EditorGUILayout.LabelField("");
            targetTransition.condition = (Condition)EditorGUILayout.ObjectField(targetTransition.condition, typeof(Condition), false);

            if (targetTransition.condition == null)
            {
                EditorGUILayout.LabelField("No Condition");
            }
            else {
                targetTransition.disable = EditorGUILayout.Toggle("Disable", targetTransition.disable);
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

        }
    }
}
