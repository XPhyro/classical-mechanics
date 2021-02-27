using UnityEditor;
using UnityEngine;

namespace Extension.Editor
{
    public class ResetTransform
    {
        [MenuItem("Shortcuts/Reset Transform Position _F1")]
        private static void ResetPosition()
        {
            if(Application.isPlaying)
            {
                return;
            }

            foreach(var go in Selection.gameObjects)
            {
                var t = go.GetComponent<Transform>();

                Undo.RecordObject(t, "Zero Out Position");
                if(t.localPosition == Vector3.zero)
                {
                    t.position = Vector3.zero;
                }
                else
                {
                    t.localPosition = Vector3.zero;
                }
            }
        }

        [MenuItem("Shortcuts/Reset Transform Rotation &r")]
        private static void ResetRotation()
        {
            if(Application.isPlaying)
            {
                return;
            }

            foreach(var go in Selection.gameObjects)
            {
                var t = go.GetComponent<Transform>();

                Undo.RecordObject(t, "Zero Out Rotation");
                t.rotation = Quaternion.identity;
            }
        }
    }
}
