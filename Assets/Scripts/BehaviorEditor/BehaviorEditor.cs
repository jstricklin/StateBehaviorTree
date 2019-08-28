using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace SA.BehaviorEditor
{
    public class BehaviorEditor : EditorWindow
    {
        #region Variables
        Vector3 mousePosition;
        bool clickedOnWindow;
        // selected index logic was fix from vid that did not work - does nothing currently
        int selectedIndex;
        BaseNode selectedNode;

        public static EditorSettings settings;
        int transitFromId;
        Rect mouseRect = new Rect(0,0,1,1);

        public enum UserActions 
        {
            addState,
            addTransitionNode,
            makeTransition,
            deleteNode,
            commentNode
        }

        #endregion

        #region Init
        [MenuItem("Behavior Editor/Editor")]
        static void ShowEditor()
        {
            BehaviorEditor editor = EditorWindow.GetWindow<BehaviorEditor>();
            editor.minSize = new Vector2(800, 600);
            
        }
        void OnEnable()
        {
            settings = Resources.Load("EditorSettings") as EditorSettings;
        }
        #endregion

        #region GUI Methods
        void OnGUI()
        {
            Event e = Event.current;
            mousePosition = e.mousePosition;
            UserInput(e);
            DrawWindows();
            if (e.type == EventType.MouseDrag)
            {
                settings.currentGraph.DeleteWindowsThatNeedTo();
                Repaint();
            }
            if (settings.makeTransition)
            {
                mouseRect.x = mousePosition.x;
                mouseRect.y = mousePosition.y;
                Rect from = settings.currentGraph.GetNodeWithIndex(transitFromId).windowRect;
                DrawNodeCurve(from, mouseRect,true,Color.blue);
                Repaint();
            }
        }
        void DrawWindows()
        {
            BeginWindows();
            EditorGUILayout.LabelField(" ", GUILayout.Width(100));
            EditorGUILayout.LabelField("Assign Graph", GUILayout.Width(100));
            settings.currentGraph = (BehaviorGraph)EditorGUILayout.ObjectField(settings.currentGraph, typeof(BehaviorGraph), false, GUILayout.Width(200));
            if (settings.currentGraph != null)
            {
                foreach (BaseNode n in settings.currentGraph.windows)
                {
                    n.DrawCurve();
                }
                for (int i = 0; i < settings.currentGraph.windows.Count; i++)
                {
                    settings.currentGraph.windows[i].windowRect = GUI.Window(i, settings.currentGraph.windows[i].windowRect, DrawNodeWindow, settings.currentGraph.windows[i].windowTitle);
                }
            }
            EndWindows();
        }

        void DrawNodeWindow(int id)
        {
            settings.currentGraph.windows[id].DrawWindow();
            GUI.DragWindow();
        }

        void UserInput(Event e)
        {
            if (settings.currentGraph == null)
            {
                return;
            }
            if (e.button == 1 && !settings.makeTransition)
            {
                if (e.type == EventType.MouseDown)
                {
                    RightClick(e);
                }
            }
            if (e.button == 0 && !settings.makeTransition)
            {
                if (e.type == EventType.MouseDown)
                {
                    // LeftClick(e);
                }
            }
            if (e.button == 0 && settings.makeTransition)
            {
                if (e.type == EventType.MouseDown)
                {
                    MakeTransition();
                }
            }
        }

        private void RightClick(Event e)
        {
            selectedIndex = -1;
            clickedOnWindow = false;
            for (int i = 0; i < settings.currentGraph.windows.Count; i++)
            {
                if (settings.currentGraph.windows[i].windowRect.Contains(e.mousePosition))
                {
                    clickedOnWindow = true;
                    selectedNode = settings.currentGraph.windows[i];
                    selectedIndex = i;
                    break;
                }
            }
            if (!clickedOnWindow)
            {
                AddNewNode(e);
            } else {
                ModifyNode(e);
            }
        }

        void MakeTransition()
        {
            clickedOnWindow = false;
            settings.makeTransition = false;
            for (int i = 0; i < settings.currentGraph.windows.Count; i++)
            {
                if (settings.currentGraph.windows[i].windowRect.Contains(mousePosition))
                {
                    clickedOnWindow = true;
                    selectedNode = settings.currentGraph.windows[i];
                    selectedIndex = i;
                    break;
                }
            }
            if (clickedOnWindow)
            {
                if (selectedNode.drawNode is StateNode)
                {
                    if (selectedNode.id != transitFromId)
                    {
                        BaseNode transNode = settings.currentGraph.GetNodeWithIndex(transitFromId);
                        transNode.targetNode = selectedNode.id;
                        
                        BaseNode enterNode = BehaviorEditor.settings.currentGraph.GetNodeWithIndex(transNode.enterNode);
                        Transition transition = enterNode.stateRef.currentState.GetTransition(enterNode.transRef.transitionId);
                        transition.targetState = selectedNode.stateRef.currentState;
                    }
                }
            }
        }
        #endregion
        #region Context Menus

        void AddNewNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddSeparator("");
            if (settings.currentGraph != null)
            {
                menu.AddItem(new GUIContent("Add State"), false, ContextCallback, UserActions.addState);
                menu.AddItem(new GUIContent("Add Comment"), false, ContextCallback, UserActions.commentNode);

            } else 
            {
                menu.AddDisabledItem(new GUIContent("Add State"));
                menu.AddDisabledItem(new GUIContent("Add Comment"));
            }
            menu.ShowAsContext();
            e.Use();
        }

        void ModifyNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            if (selectedNode.drawNode == null) 
                return;
            if (selectedNode.drawNode is StateNode)
            {
                StateNode stateNode = (StateNode)selectedNode.drawNode;
                if (stateNode != null)
                    {
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Add Condition"), false, ContextCallback, UserActions.addTransitionNode);
                    } else {
                    menu.AddSeparator("");
                    menu.AddDisabledItem(new GUIContent("Add Condition"));
                    }
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deleteNode);
            }
            if (selectedNode.drawNode is CommentNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deleteNode);
            }
            if (selectedNode.drawNode is TransitionNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Make Transition"), false, ContextCallback, UserActions.makeTransition);

                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deleteNode);
            }
            menu.ShowAsContext();
            e.Use(); 
        }
        // dropdown menu
        void ContextCallback(object o)
        {
            UserActions a = (UserActions)o;
            switch(a)
            {
                case UserActions.addState :
                    BaseNode stateNode = settings.AddNodeOnGraph(settings.stateNode, 200, 100, "State Node", mousePosition);
                    break;
                case UserActions.addTransitionNode : 
                    BaseNode transNode = settings.AddNodeOnGraph(settings.transitionNode, 200, 100, "Transition Node", mousePosition);
                    transNode.enterNode = selectedNode.id;
                    Transition transition = settings.stateNode.AddTransition(selectedNode);
                    transNode.transRef.transitionId = transition.id;
                    break;
                case UserActions.commentNode :
                    BaseNode commentNode = settings.AddNodeOnGraph(settings.commentNode, 200, 100, "Comment", mousePosition);
                    commentNode.comment = "This is a comment";
                    
                    break;
                case UserActions.deleteNode :
                    settings.currentGraph.DeleteNode(selectedNode.id);
                    break;
                case UserActions.makeTransition :
                    transitFromId = selectedNode.id;
                    settings.makeTransition = true;
                    break;
            } 
            EditorUtility.SetDirty(settings);
        }

        #endregion
        #region HelperMethods

        public static void DrawNodeCurve(Rect start, Rect end, bool left, Color curveColor)
        {
            Vector3 startPos = new Vector3(
                (left) ? start.x + start.width : start.x,
                start.y + (start.height * 0.5f),
                0);
            Vector3 endPos = new Vector3(end.x + (end.width * 0.5f), end.y + (end.height * 0.5f), 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;

            Color shadow = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++)
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, curveColor, null, 1);
            }
        }

        public static void ClearWindowsFromList(List<BaseNode> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                // if (windows.Contains(l[i]))
                // {
                //     windows.Remove(l[i]);
                // }
            }
        }

        #endregion
    }
}
