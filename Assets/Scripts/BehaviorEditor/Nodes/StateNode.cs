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
    public class StateNode : BaseNode
    {
        public bool collapse;
        bool previousCollapse;
        public bool isDuplicate;
        public State currentState;
        public State previousState;

        SerializedObject serializedState;
        ReorderableList onStateList;
        ReorderableList onEnterList;
        ReorderableList onExitList;

        public List<BaseNode> dependencies = new List<BaseNode>();

        public override void DrawWindow()
        {
            if(currentState == null)
            {
                EditorGUILayout.LabelField("Add State To Modify:");
            } else {
                if(!collapse)
                {

                } else {
                    windowRect.height = 100;
                }
                collapse = EditorGUILayout.Toggle(" ", collapse);
            }
            currentState = (State)EditorGUILayout.ObjectField(currentState, typeof(State), false);

            if (previousCollapse != collapse)
            {
                previousCollapse = collapse;
                // BehaviorEditor.currentGraph.SetStateNode(this);
            }
            if (previousState != currentState)
            {
                serializedState = null;
                isDuplicate = BehaviorEditor.currentGraph.IsStateNodeDuplicate(this);
                if (!isDuplicate)
                {
                    BehaviorEditor.currentGraph.SetNode(this);
                    previousState = currentState;
                    for (int i = 0; i < currentState.transitions.Count; i++)
                    {

                    }
                }
                if (isDuplicate)
                {
                    EditorGUILayout.LabelField("State is a duplicate");
                    windowRect.height = 100;
                    return;
                }
            }
            if (currentState != null)
            {

                if (serializedState == null)
                {
                    serializedState = new SerializedObject(currentState);
                    onStateList = new ReorderableList(serializedState, serializedState.FindProperty("onState"), true, true, true, true);
                    onEnterList = new ReorderableList(serializedState, serializedState.FindProperty("onEnter"), true, true, true, true);
                    onExitList = new ReorderableList(serializedState, serializedState.FindProperty("onExit"), true, true, true, true);
                }
                if (!collapse)
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
                    standard += (onStateList.count) * 20;
                    standard += (onEnterList.count) * 20;
                    standard += (onExitList.count) * 20;
                    windowRect.height = standard;

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

        public override void DrawCurve()
        {

        }

        public Transition AddTransition()
        {
            return currentState.AddTransition();
        }

        public void ClearReferences()
        {
            BehaviorEditor.ClearWindowsFromList(dependencies);
            dependencies.Clear();
        }
    }
    
}

