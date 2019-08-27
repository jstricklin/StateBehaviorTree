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

            BaseNode enterNode = BehaviorEditor.settings.currentGraph.GetNodeWithIndex(b.enterNode);

            Transition transition = enterNode.stateRef.currentState.GetTransition(b.transRef.transitionId);

            transition.condition 
                = (Condition)EditorGUILayout.ObjectField(transition.condition,
                typeof(Condition), false);

            if (transition.condition == null)
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
            if (b.transRef.previousCondition != transition.condition)
            {
                b.transRef.previousCondition = transition.condition;
                b.isDuplicate = BehaviorEditor.settings.currentGraph.IsTransitionDuplicate(b);
                if (!b.isDuplicate)
                {
                    // BehaviorEditor.settings.currentGraph.SetNode(this);
                }
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
