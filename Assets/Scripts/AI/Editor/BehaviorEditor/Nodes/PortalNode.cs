﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.BehaviorEditor
{
[CreateAssetMenu(menuName = "Portal Node")]
    public class PortalNode : DrawNode
    {
        public override void DrawCurve(BaseNode b)
        {
            
        }

        public override void DrawWindow(BaseNode b)
        {
            b.stateRef.currentState = (State)EditorGUILayout.ObjectField(b.stateRef.currentState, typeof(State), false);
            b.isAssigned = b.stateRef.currentState != null;
        }
    }
    
}
