using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.BehaviorEditor {
    public class CommentNode : BaseNode
    {

string comment = "This is a comment";

        public override void DrawWindow()
        {
            comment = GUILayout.TextArea(comment, 200);
        }
    }
}
