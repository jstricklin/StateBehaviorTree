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
            if(b.currentState == null)
            {
                EditorGUILayout.LabelField("Add State To Modify:");
            } else {
                if(!b.collapse)
                {

                } else {
                    b.windowRect.height = 100;
                }
                b.collapse = EditorGUILayout.Toggle(" ", b.collapse);
            }
            b.currentState = (State)EditorGUILayout.ObjectField(b.currentState, typeof(State), false);

            if (b.previousCollapse != b.collapse)
            {
                b.previousCollapse = b.collapse;
                // BehaviorEditor.currentGraph.SetStateNode(this);
            }
            if (b.previousState != b.currentState)
            {
                b.serializedState = null;
                b.isDuplicate = BehaviorEditor.settings.currentGraph.IsStateNodeDuplicate(this);
                if (!b.isDuplicate)
                {
                    // BehaviorEditor.currentGraph.SetNode(this);
                    b.previousState = b.currentState;
                    for (int i = 0; i < b.currentState.transitions.Count; i++)
                    {

                    }
                }
                if (b.isDuplicate)
                {
                    EditorGUILayout.LabelField("State is a duplicate");
                    b.windowRect.height = 100;
                    return;
                }
            }
            if (b.currentState != null)
            {

                if (b.serializedState == null)
                {
                    b.serializedState = new SerializedObject(b.currentState);
                    b.onStateList = new ReorderableList(b.serializedState, b.serializedState.FindProperty("onState"), true, true, true, true);
                    b.onEnterList = new ReorderableList(b.serializedState, b.serializedState.FindProperty("onEnter"), true, true, true, true);
                    b.onExitList = new ReorderableList(b.serializedState, b.serializedState.FindProperty("onExit"), true, true, true, true);
                }
                if (!b.collapse)
                {
                    b.serializedState.Update();
                    HandleReordableList(b.onStateList, "On state");
                    HandleReordableList(b.onEnterList, "On enter");
                    HandleReordableList(b.onExitList, "On exit");

                    EditorGUILayout.LabelField("");
                    b.onStateList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    b.onEnterList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    b.onExitList.DoLayoutList();
                    
                    b.serializedState.ApplyModifiedProperties();
                    float standard = 300;
                    standard += (b.onStateList.count) * 20;
                    standard += (b.onEnterList.count) * 20;
                    standard += (b.onExitList.count) * 20;
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

