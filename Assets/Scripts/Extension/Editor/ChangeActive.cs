using UnityEditor;
using UnityEngine;

namespace Extension.Editor
{
    public static class ChangeActive
    {
        [MenuItem("Shortcuts/Toggle Active")]
        public static void ToggleActive()
        {
            if(Application.isPlaying)
            {
                return;
            }

            Undo.RecordObjects(Selection.gameObjects, "Enabled Selected GameObjects");

            foreach(var go in Selection.gameObjects)
            {
                go.SetActive(!go.activeInHierarchy);
            }
        }

        [MenuItem("Shortcuts/Activate All _F4")]
        public static void Activateall()
        {
            if(Application.isPlaying)
            {
                return;
            }

            Undo.RecordObjects(Selection.gameObjects, "Enabled All Children");

            foreach(var go in Selection.gameObjects)
            {
                ChangeActiveRecursively(go, true);
            }
        }

        [MenuItem("Shortcuts/Deactivate All _F5")]
        public static void Deactivateall()
        {
            if(Application.isPlaying)
            {
                return;
            }

            Undo.RecordObjects(Selection.gameObjects, "Disabled All Children");

            foreach(var go in Selection.gameObjects)
            {
                ChangeActiveRecursively(go, false);
            }
        }

        private static void ChangeActiveRecursively(GameObject go, bool active)
        {
            var child = go.transform.GetComponentInChildren<Transform>();

            foreach(Transform t in child)
            {
                ChangeActiveRecursively(t.gameObject, active);
            }

            Undo.RecordObject(go, active ? "Enabled" : "Disabled" + " All Children");
            go.SetActive(active);
        }
    }
}
