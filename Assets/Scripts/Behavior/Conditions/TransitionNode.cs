using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.BehaviorEditor
{
    public class TransitionNode : DrawNode
    {
        // public bool isDuplicate;
        // public Condition targetCondition;
        // public Condition previousCondition;

        // public Transition transition;

        // public StateNode enterState;
        // public StateNode targetNode;

        public void Init(StateNode enterState, Transition transition)
        {
            // this.enterState = enterState;
        }

        public override void DrawWindow(BaseNode b)
        {
            EditorGUILayout.LabelField("");

            if (b.transRef.targetCondition == null)
            {
                EditorGUILayout.LabelField("No Condition");
            }
            else {
                if (b.transRef.isDuplicate)
                {
                    EditorGUILayout.LabelField("Duplicate Condition");
                } else {
                    // if (transition != null)
                    // {
                        // transition.disable = EditorGUILayout.Toggle("Disable", transition.disable);
                    // }
                } 
            }
            if (b.transRef.previousCondition != b.transRef.targetCondition)
            {
                b.transRef.isDuplicate = BehaviorEditor.settings.currentGraph.IsTransitionDuplicate(this);
                if (!b.transRef.isDuplicate)
                {
                    // BehaviorEditor.settings.currentGraph.SetNode(this);
                }
                b.transRef.previousCondition = b.transRef.targetCondition;
            }
        }

        public override void DrawCurve(BaseNode b)
        {
            if (b.transRef.enterState)
            {
                Rect rect = b.windowRect;
                rect.y += b.windowRect.height * 0.5f;
                rect.width = 1;
                rect.height = 1;

                // BehaviorEditor.DrawNodeCurve(b.transRef.enterState.windowRect, rect, true, Color.black);
            }

        }
    }
}
