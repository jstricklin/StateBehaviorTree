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
            if (enterNode == null)
            {
                return;
            }
            if (enterNode.stateRef.currentState == null)
            {
                BehaviorEditor.settings.currentGraph.DeleteNode(b.id);
                return;
            }
            Transition transition = enterNode.stateRef.currentState.GetTransition(b.transRef.transitionId);
            if (transition == null)
            {
                return;
            }
            transition.condition 
                = (Condition)EditorGUILayout.ObjectField(transition.condition,
                typeof(Condition), false);

            if (transition.condition == null)
            {
                EditorGUILayout.LabelField("No Condition");
                b.isAssigned = false;
            }
            else {
                b.isAssigned = true;
                if (b.isDuplicate)
                {
                    EditorGUILayout.LabelField("Duplicate Condition");
                } else {
                    GUILayout.Label(transition.condition.description);
                    BaseNode targetNode = BehaviorEditor.settings.currentGraph.GetNodeWithIndex(b.targetNode);
                    if (targetNode != null)
                    {
                        if (!targetNode.isDuplicate)
                        {
                            transition.targetState = targetNode.stateRef.currentState;
                        } else {
                            transition.targetState = null;
                        }
                    } else {
                        transition.targetState = null;
                    }
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

            BaseNode enter = BehaviorEditor.settings.currentGraph.GetNodeWithIndex(b.enterNode);
            // if enterNode is null, enterNode has likely been deleted. remove this node
            if (enter == null)
            {
                BehaviorEditor.settings.currentGraph.DeleteNode(enter.id);
            } else {
                
                // color transition lines
                Color targetColor = Color.green;
                if (!b.isAssigned || b.isDuplicate)
                {
                    targetColor = Color.red;
                }
                Rect r = enter.windowRect;
                BehaviorEditor.DrawNodeCurve(r, rect, true, targetColor);
            }
            if (b.isDuplicate)
            {
                return;
            }
            if (b.targetNode > 0)
            {
                BaseNode target = BehaviorEditor.settings.currentGraph.GetNodeWithIndex(b.targetNode);
                // if target is null, target has likely been deleted. remove this node
                if (target == null)
                {
                    b.targetNode = -1;
                } else {
                    rect = b.windowRect;
                    rect.x += rect.width;
                    Rect endRect = target.windowRect;
                    endRect.x -= endRect.width * 0.5f;
                    Color targetColor = Color.green;
                    if (target.drawNode is StateNode)
                    {
                        if (!target.isAssigned || target.isDuplicate)
                        {
                            targetColor = Color.red;
                        } 
                    }
                    else 
                    {
                        if (!target.isAssigned)
                        {
                            targetColor = Color.red;
                        } else {
                            targetColor = Color.yellow;
                        }
                    }
                    Rect r = enter.windowRect;
                    BehaviorEditor.DrawNodeCurve(rect, endRect, false, targetColor);
                }
            }
        }
    }
}
