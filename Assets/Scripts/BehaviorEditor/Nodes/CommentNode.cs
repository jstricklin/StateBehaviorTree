using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.BehaviorEditor {
    [CreateAssetMenu(menuName = "Editor/Nodes/Comment Node")]
    public class CommentNode : DrawNode
    {
        public override void DrawCurve(BaseNode n)
        {
            // throw new System.NotImplementedException();
        }

        public override void DrawWindow(BaseNode b)
        {
            // string comment = b.commentRef.comment != null ? b.commentRef.comment : "Enter a comment!";
            GUILayout.TextArea(b.comment, 200);
        }
    }
}
