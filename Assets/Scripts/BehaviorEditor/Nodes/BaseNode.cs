using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SA.BehaviorEditor
{
    public class BaseNode : ScriptableObject
    {
        public Rect windowRect;
        public string windowTitle;

        public virtual void DrawWindow()
        {

        }
        public virtual void DrawCurve()
        {

        }

    }
}