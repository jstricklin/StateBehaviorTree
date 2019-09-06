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
                b.stateRef.previousState = b.stateRef.currentState;
                if (!b.isDuplicate)
                {
                    Vector3 pos = new Vector3(b.windowRect.x, b.windowRect.y, 0);
                    pos.x += b.windowRect.width * 2;
                    SetupReorderableLists(b);
                    //  load transitions
                    for (int i = 0; i < b.stateRef.currentState.transitions.Count; i++)
                    {
                        pos.y += i * 100;
                        BehaviorEditor.AddTransitionNodeFromTransition(b.stateRef.currentState.transitions[i], b, pos);
                    }
                    BehaviorEditor.forceSetDirty = true;
                }
            }
            if (b.isDuplicate)
            {
                EditorGUILayout.LabelField("State is a duplicate");
                b.windowRect.height = 100;
                return;
            }
            if (b.stateRef.currentState != null)
            {
                b.isAssigned = true;

                if (b.stateRef.serializedState == null)
                {
                    // in case unit has closed, will organize serialized list   
                    SetupReorderableLists(b);
                }
                if (!b.collapse)
                {
                    b.stateRef.serializedState.Update();

                    EditorGUILayout.LabelField("");
                    b.stateRef.onStateList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    b.stateRef.onEnterList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    b.stateRef.onExitList.DoLayoutList();
                    
                    b.stateRef.serializedState.ApplyModifiedProperties();
                    float standard = 300;
                    standard += (b.stateRef.onExitList.count + b.stateRef.onStateList.count + b.stateRef.onEnterList.count) * 20;
                    b.windowRect.height = standard;

                }
            }
            else 
            {
                b.isAssigned = false;
            }
        }

        private void SetupReorderableLists(BaseNode b)
        {
            b.stateRef.serializedState = new SerializedObject(b.stateRef.currentState);

            // b.stateRef.serializedState = new SerializedObject(b.stateRef.currentState);
            b.stateRef.onStateList = new ReorderableList(b.stateRef.serializedState, b.stateRef.serializedState.FindProperty("onState"), true, true, true, true);
            b.stateRef.onEnterList = new ReorderableList(b.stateRef.serializedState, b.stateRef.serializedState.FindProperty("onEnter"), true, true, true, true);
            b.stateRef.onExitList = new ReorderableList(b.stateRef.serializedState, b.stateRef.serializedState.FindProperty("onExit"), true, true, true, true);

            HandleReordableList(b.stateRef.onStateList, "On state");
            HandleReordableList(b.stateRef.onEnterList, "On enter");
            HandleReordableList(b.stateRef.onExitList, "On exit");
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

