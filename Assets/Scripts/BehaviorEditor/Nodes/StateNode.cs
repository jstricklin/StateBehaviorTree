using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using SA;

namespace SA.BehaviorEditor
{
    [CreateAssetMenu(menuName = "Editor/Nodes/State Node")]
    public class StateNode : DrawNode
    {
        public override void DrawWindow(BaseNode b)
        {
            if(b.stateRef.currentState == null)
            {
                EditorGUILayout.LabelField("Add State To Modify:");
            } else {
                if(!b.stateRef.collapse)
                {

                } else {
                    b.windowRect.height = 100;
                }
                b.stateRef.collapse = EditorGUILayout.Toggle(" ", b.stateRef.collapse);
            }
            b.stateRef.currentState = (State)EditorGUILayout.ObjectField(b.stateRef.currentState, typeof(State), false);

            if (b.stateRef.previousCollapse != b.stateRef.collapse)
            {
                b.stateRef.previousCollapse = b.stateRef.collapse;
                // BehaviorEditor.currentGraph.SetStateNode(this);
            }
            if (b.stateRef.previousState != b.stateRef.currentState)
            {
                b.stateRef.serializedState = null;
                b.stateRef.isDuplicate = BehaviorEditor.settings.currentGraph.IsStateNodeDuplicate(this);
                if (!b.stateRef.isDuplicate)
                {
                    // BehaviorEditor.currentGraph.SetNode(this);
                    b.stateRef.previousState = b.stateRef.currentState;
                    for (int i = 0; i < b.stateRef.currentState.transitions.Count; i++)
                    {

                    }
                }
                if (b.stateRef.isDuplicate)
                {
                    EditorGUILayout.LabelField("State is a duplicate");
                    b.windowRect.height = 100;
                    return;
                }
            }
            if (b.stateRef.currentState != null)
            {

                if (b.stateRef.serializedState == null)
                {
                    b.stateRef.serializedState = new SerializedObject(b.stateRef.currentState);
                    b.stateRef.onStateList = new ReorderableList(b.stateRef.serializedState, b.stateRef.serializedState.FindProperty("onState"), true, true, true, true);
                    b.stateRef.onEnterList = new ReorderableList(b.stateRef.serializedState, b.stateRef.serializedState.FindProperty("onEnter"), true, true, true, true);
                    b.stateRef.onExitList = new ReorderableList(b.stateRef.serializedState, b.stateRef.serializedState.FindProperty("onExit"), true, true, true, true);
                }
                if (!b.stateRef.collapse)
                {
                    b.stateRef.serializedState.Update();
                    HandleReordableList(b.stateRef.onStateList, "On state");
                    HandleReordableList(b.stateRef.onEnterList, "On enter");
                    HandleReordableList(b.stateRef.onExitList, "On exit");

                    EditorGUILayout.LabelField("");
                    b.stateRef.onStateList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    b.stateRef.onEnterList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    b.stateRef.onExitList.DoLayoutList();
                    
                    b.stateRef.serializedState.ApplyModifiedProperties();
                    float standard = 300;
                    standard += (b.stateRef.onStateList.count) * 20;
                    standard += (b.stateRef.onEnterList.count) * 20;
                    standard += (b.stateRef.onExitList.count) * 20;
                    b.windowRect.height = standard;

                }

            }
        }

        void HandleReordableList(ReorderableList list, string targetName)
        {
            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, targetName);
            };

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => 
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };
        }

        public override void DrawCurve(BaseNode b)
        {

        }

        public Transition AddTransition()
        {
            // return b.currentState.AddTransition();
            return null;
        }

        public void ClearReferences()
        {
            // BehaviorEditor.ClearWindowsFromList(dependencies);
            // dependencies.Clear();
        }
    }
    
}

