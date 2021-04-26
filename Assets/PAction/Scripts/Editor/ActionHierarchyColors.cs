using UnityEngine;

namespace Pashmak.Action
{
#if UNITY_EDITOR
    partial class ActionHierarchy
    {
        // error
        public static Color COLOR_ERROR = new Color(1f, 0f, 0f, 0.6f);
        public static Color COLOR_NULL_NOT_ENABLED = new Color(1f, 0f, 0f, 0.4f);

        // warning
        public static Color COLOR_WARNING = new Color(1f, 0.9f, 0.4f, 0.6f);
        public static Color COLOR_WARNING_NOT_ENABLED = new Color(1f, 0.8f, 0.2f, 0.2f);

        // comment
        public static Color COLOR_COMMENTS = new Color(0f, 0.5f, 0f, 0.4f);
        public static Color COLOR_COMMENTS_NOT_ENABLED = Color.gray;

        // default node
        public static Color COLOR_NODE = new Color(.3f, .6f, .8f, 0.5f);
        public static Color COLOR_FunctionNode = new Color(.6f, .3f, .6f, 1f);
        public static Color COLOR_SpacialNode = new Color(.3f, .8f, 1f, 0.5f);

        public static Color COLOR_NODE_NOT_ENABLED = COLOR_COMMENTS;

        // runtime
        public static Color COLOR_Running = Color.yellow;

        public static Color ChangeAlphaColor(Color color, float alpha = 0.5f)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}
#endif