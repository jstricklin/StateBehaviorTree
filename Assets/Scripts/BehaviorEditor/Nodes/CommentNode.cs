using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.BehaviorEditor {
    public class CommentNode : DrawNode
    {
        public override void DrawCurve(BaseNode n)
        {
            throw new System.NotImplementedException();
        }

        // string comment = "This is a comment";

        public override void DrawWindow(BaseNode b)
        {
            // comment = GUILayout.TextArea(comment, 200);
        }
    }
}
