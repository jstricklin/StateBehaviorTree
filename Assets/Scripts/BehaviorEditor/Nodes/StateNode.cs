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
                b.isAssigned = false;
                EditorGUILayout.LabelField("Add State To Modify:");
            } else {
                if(!b.collapse)
                {

                } else {
                    b.windowRect.height = 100;
                }
                b.collapse = EditorGUILayout.Toggle(" ", b.collapse);
            }
            b.stateRef.currentState = (State)EditorGUILayout.ObjectField(b.stateRef.currentState, typeof(State), false);

            if (b.previousCollapse != b.collapse)
            {
                b.previousCollapse = b.collapse;
            }
            if (b.stateRef.previousState != b.stateRef.currentState)
            {
                b.isDuplicate = BehaviorEditor.settings.currentGraph.IsStateDuplicate(b);
                if (b.isDuplicate)
                {
                    EditorGUILayout.LabelField("State is a duplicate");
                    b.windowRect.height = 100;
                    return;
                }
                if (!b.isDuplicate)
                {
                    b.stateRef.previousState = b.stateRef.currentState;
                    for (int i = 0; i < b.stateRef.currentState.transitions.Count; i++)
                    {

                    }
                }
            }
            if (b.stateRef.currentState != null)
            {
                b.isAssigned = true;
                SerializedObject serializedState = new SerializedObject(b.stateRef.currentState);
                
                ReorderableList onStateList;
                ReorderableList onEnterList;
                ReorderableList onExitList;

                // b.stateRef.serializedState = new SerializedObject(b.stateRef.currentState);
                onStateList = new ReorderableList(serializedState, serializedState.FindProperty("onState"), true, true, true, true);
                onEnterList = new ReorderableList(serializedState, serializedState.FindProperty("onEnter"), true, true, true, true);
                onExitList = new ReorderableList(serializedState, serializedState.FindProperty("onExit"), true, true, true, true);

                // if (b.stateRef.serializedState == null)
                // {
                // }
                if (!b.collapse)
                {
                    serializedState.Update();
                    HandleReordableList(onStateList, "On state");
                    HandleReordableList(onEnterList, "On enter");
                    HandleReordableList(onExitList, "On exit");

                    EditorGUILayout.LabelField("");
                    onStateList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    onEnterList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    onExitList.DoLayoutList();
                    
                    serializedState.ApplyModifiedProperties();
                    float standard = 300;
                    standard += (onExitList.count + onStateList.count + onEnterList.count) * 20;
                    b.windowRect.height = standard;

                }
            }
            else 
            {
                b.isAssigned = false;
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

        public Transition AddTransition(BaseNode b)
        {
            return b.stateRef.currentState.AddTransition();
        }

        public void ClearReferences()
        {
            // BehaviorEditor.ClearWindowsFromList(dependencies);
            // dependencies.Clear();
        }
    }
    
}

