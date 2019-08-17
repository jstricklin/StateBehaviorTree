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
        static List<BaseNode> windows = new List<BaseNode>();
        Vector3 mousePosition;
        bool makeTransition;
        bool clickedOnWindow;
        BaseNode selectedNode;

        public static BehaviorGraph currentGraph;
        static GraphNode graphNode;

        public enum UserActions 
        {
            addState,
            addTransitionNode,
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
            if (graphNode == null)
            {
                graphNode = CreateInstance<GraphNode>();
                graphNode.windowRect = new Rect(10, position.height * 0.7f, 200, 100);
                graphNode.windowTitle = "Graph";
            }
            windows.Clear();
            windows.Add(graphNode);
            LoadGraph();
        }
        #endregion

        #region GUI Methods
        void OnGUI()
        {
            Event e = Event.current;
            mousePosition = e.mousePosition;
            UserInput(e);
            DrawWindows();
        }
        void DrawWindows()
        {
            BeginWindows();
            foreach (BaseNode n in windows)
            {
                n.DrawCurve();
            }
            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
            }
            EndWindows();
        }

        void DrawNodeWindow(int id)
        {
            windows[id].DrawWindow();
            GUI.DragWindow();
        }

        void UserInput(Event e)
        {
            if (e.button == 1 && !makeTransition)
            {
                if (e.type == EventType.MouseDown)
                {
                    RightClick(e);
                }
            }
            if (e.button == 0 && !makeTransition)
            {
                if (e.type == EventType.MouseDown)
                {
                    // LeftClick(e);
                }
                if (e.type == EventType.MouseDrag)
                {
                    for (int i = 0; i < windows.Count; i++)
                    {
                        if (windows[i].windowRect.Contains(e.mousePosition))
                        {
                            if (currentGraph != null)
                            {
                                currentGraph.SetNode(windows[i]);
                            }
                            break;
                        }
                    }
                }
            }

        }

        private void RightClick(Event e)
        {
            selectedNode = null;
            clickedOnWindow = false;
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(e.mousePosition))
                {
                    clickedOnWindow = true;
                    selectedNode = windows[i];
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

        void AddNewNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddSeparator("");
            if (currentGraph != null)
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
            if (selectedNode is StateNode)
            {
                StateNode stateNode = (StateNode)selectedNode;
                if (stateNode.currentState != null)
                    {
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Add Transition"), false, ContextCallback, UserActions.addTransitionNode);
                    } else {

                    menu.AddSeparator("");
                    menu.AddDisabledItem(new GUIContent("Add Transition"));
                    }
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deleteNode);
            }
            if (selectedNode is CommentNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deleteNode);
            }
            if (selectedNode is TransitionNode)
            {
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
                    AddStateNode(mousePosition);
                    break;
                case UserActions.addTransitionNode : 
                    if (selectedNode is StateNode)
                    {
                        StateNode from = (StateNode)selectedNode;
                        // Transition transition = from.AddTransition();
                        AddTransitionNode(from.currentState.transitions.Count, null, from);
                    }
                    break;
                case UserActions.commentNode :
                    AddCommentNode(mousePosition);
                    break;
                case UserActions.deleteNode :
                    if (selectedNode is StateNode)
                    {
                        StateNode target = (StateNode)selectedNode;
                        target.ClearReferences();
                        windows.Remove(target);
                    }
                    if (selectedNode is TransitionNode)
                    {
                        TransitionNode target = (TransitionNode)selectedNode;
                        windows.Remove(target);
                        // if (target.enterState.currentState.transitions.Contains(target.targetCondition))
                        // {
                        //     target.enterState.currentState.transitions.Remove(target.targetCondition);
                        // }

                    }
                    if (selectedNode is CommentNode)
                    {
                        windows.Remove(selectedNode);
                    }
                    break;
            }
        }
        #endregion

        #region HelperMethods
        public static StateNode AddStateNode(Vector2 pos)
        {
            StateNode stateNode = CreateInstance<StateNode>();
            stateNode.windowRect = new Rect(pos.x, pos.y, 200, 300);
            stateNode.windowTitle = "State";
            windows.Add(stateNode);
            // currentGraph.SetStateNode(stateNode);
            return stateNode;
        }
        public static CommentNode AddCommentNode(Vector2 pos)
        {
            CommentNode commentNode = CreateInstance<CommentNode>();
            commentNode.windowRect = new Rect(pos.x, pos.y, 200, 100);
            commentNode.windowTitle = "Comment";
            windows.Add(commentNode);
            return commentNode;
        }

        public static TransitionNode AddTransitionNode(int index, Transition transition, StateNode from)
        {
            Rect fromRect = from.windowRect;
            fromRect.x += 50;
            float targetY = fromRect.y - fromRect.height;
            if (from.currentState != null)
            {
                targetY += (index * 100);
            }

            fromRect.y = targetY;
            fromRect.x += 200 + 100;
            fromRect.y += (fromRect.height * 0.7f);
            Vector2 pos = new Vector2(fromRect.x, fromRect.y);
            return AddTransitionNode(pos, transition, from);
        }

        public static TransitionNode AddTransitionNode(Vector2 pos, Transition transition, StateNode from)
        {
            TransitionNode transitionNode = CreateInstance<TransitionNode>();
            transitionNode.Init(from, transition);
            transitionNode.windowRect = new Rect(pos.x, pos.y, 200, 80);
            transitionNode.windowTitle = "Condition Check";
            windows.Add(transitionNode);
            from.dependencies.Add(transitionNode);
            return transitionNode;
        }

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
                if (windows.Contains(l[i]))
                {
                    windows.Remove(l[i]);
                }
            }
        }

        public static void LoadGraph()
        {
            windows.Clear();
            windows.Add(graphNode);
            if (currentGraph == null)
            {
                return;
            }
            currentGraph.Init();
            List<Saved_StateNode> l = new List<Saved_StateNode>();
            l.AddRange(currentGraph.savedStateNodes);
            currentGraph.savedStateNodes.Clear();

            for (int i = l.Count - 1; i >= 0; i--)
            {
                StateNode s = AddStateNode(l[i].position);
                s.currentState = l[i].state;
                s.collapse = l[i].isCollapsed;
                for (int t = l[i].savedConditions.Count - 1; t >= 0; t--)
                {
                    TransitionNode transNode = AddTransitionNode(l[i].savedConditions[t].position, l[i].savedConditions[t].transition, s);
                    transNode.targetCondition = l[i].savedConditions[t].condition;
                }
            }
        }
        #endregion
    }
}
