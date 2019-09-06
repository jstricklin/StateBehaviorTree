using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace SA.CustomUI
{
    [CustomEditor(typeof(State))]
    public class StateGUI : Editor
    {
        SerializedObject serializedState;
        ReorderableList onFixedList;
        ReorderableList onUpdateList;
        ReorderableList onEnterList;
        ReorderableList onExitList;
        ReorderableList Transitions;

        bool showDefaultGUI = false;
        bool showActions = true;
        bool showTransitions = true;

        private void OnEnable()
        {
            serializedState = null;
        }

        public override void OnInspectorGUI()
        {
            showDefaultGUI = EditorGUILayout.Toggle("DefaultGUI", showDefaultGUI);
            if (showDefaultGUI)
            {
                base.OnInspectorGUI();
                return;
            }

            showActions = EditorGUILayout.Toggle("Show Actions", showActions);
            if (serializedState == null)
            {
                SetupReorderableLists();
            }
            serializedState.Update();
            if (showActions)
            {
                EditorGUILayout.LabelField("Actions that execute on FixedUpdate()");
                onFixedList.DoLayoutList();
                EditorGUILayout.LabelField("Actions that execute on Update()");
                onUpdateList.DoLayoutList();
                EditorGUILayout.LabelField("Actions that execute when entering state");
                onEnterList.DoLayoutList();
                EditorGUILayout.LabelField("Actions that execute when exiting state");
                onExitList.DoLayoutList();
            }
            showTransitions = EditorGUILayout.Toggle("Show Transitions", showTransitions);
            if (showTransitions)
            {
                EditorGUILayout.LabelField("Conditions to exit state");
                Transitions.DoLayoutList();
            }
            serializedState.ApplyModifiedProperties();
        }

        void SetupReorderableLists()
        {
            // target is default editor object; returns selected object
            State curState = (State)target;
            serializedState = new SerializedObject(curState);
            onFixedList = new ReorderableList(serializedState, serializedState.FindProperty("onFixed"), true, true, true, true);
            onUpdateList = new ReorderableList(serializedState, serializedState.FindProperty("onUpdate"), true, true, true, true);
            onEnterList = new ReorderableList(serializedState, serializedState.FindProperty("onEnter"), true, true, true, true);
            onExitList = new ReorderableList(serializedState, serializedState.FindProperty("onExit"), true, true, true, true);
            Transitions = new ReorderableList(serializedState, serializedState.FindProperty("transitions"), true, true, true, true);

            HandleReorderableList(onFixedList, "On Fixed");
            HandleReorderableList(onUpdateList, "On Update");
            HandleReorderableList(onEnterList, "On Enter");
            HandleReorderableList(onExitList, "On Exit");
            HandleTransitionReorderable(Transitions, "Condition --> New State");
            
        }

        private void HandleReorderableList(ReorderableList list, string targetName)
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

        private void HandleTransitionReorderable(ReorderableList list, string targetName)
        {
            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, targetName);
            };

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => 
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.ObjectField(new Rect(rect.x, rect.y, 0.3f, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("condition"), GUIContent.none);
                EditorGUI.ObjectField(new Rect(rect.x + + (rect.width * 0.35f), rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("targetState"), GUIContent.none);
                EditorGUI.LabelField(new Rect(rect.x + + (rect.width * 0.75f), rect.y, rect.width * 0.2f, EditorGUIUtility.singleLineHeight), "Disable");
                EditorGUI.PropertyField(new Rect(rect.x + + (rect.width * 0.90f), rect.y, rect.width * 0.2f, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("disable"), GUIContent.none);
            };
        }
    }
}