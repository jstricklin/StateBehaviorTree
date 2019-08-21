using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.BehaviorEditor
{
    [CreateAssetMenu(menuName = "Editor/Nodes/Transition Node")]
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
            b.transRef.targetCondition = (Condition)EditorGUILayout.ObjectField(b.transRef.targetCondition, typeof(Condition), false);

            if (b.transRef.targetCondition == null)
            {
                EditorGUILayout.LabelField("No Condition");
            }
            else {
                if (b.isDuplicate)
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
                b.isDuplicate = BehaviorEditor.settings.currentGraph.IsTransitionDuplicate(b);
                if (!b.isDuplicate)
                {
                    // BehaviorEditor.settings.currentGraph.SetNode(this);
                }
                b.transRef.previousCondition = b.transRef.targetCondition;
            }
        }

        public override void DrawCurve(BaseNode b)
        {
                Rect rect = b.windowRect;
                rect.y += b.windowRect.height * 0.5f;
                rect.width = 1;
                rect.height = 1;

                BaseNode node = BehaviorEditor.settings.currentGraph.GetNodeWithIndex(b.enterNode);
                // if enterNode is null, enterNode has likely been deleted. remove this node
                if (node == null)
                {
                    BehaviorEditor.settings.currentGraph.DeleteNode(node.id);
                } else {
                    Rect r = node.windowRect;
                    BehaviorEditor.DrawNodeCurve(r, rect, true, Color.black);
                }
        }
    }
}
