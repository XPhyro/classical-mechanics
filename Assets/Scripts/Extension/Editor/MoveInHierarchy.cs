using UnityEditor;
using UnityEngine;

namespace Extension.Editor
{
    public static class MoveInHierarchy
    {
        [MenuItem("Shortcuts/Move Up In Hierarchy &UP")]
        public static void MoveUpwards()
        {
            if(Application.isPlaying)
            {
                return;
            }

            foreach(var go in Selection.gameObjects)
            {
                var t = go.transform;

                if(t.parent)
                {
                    if(t.GetSiblingIndex() == 0)
                    {
                        var parent = t.parent;

                        t.parent = null;
                        t.SetSiblingIndex(parent.GetSiblingIndex() - 1);
                    }
                    else
                    {
                        t.SetSiblingIndex(t.GetSiblingIndex() - 1);
                    }
                }
                else
                {
                    var x = t.GetSiblingIndex() - 1;
                    t.SetSiblingIndex(Mathf.Clamp(x, 0, x));
                }
            }
        }

        [MenuItem("Shortcuts/Move Down In Hierarchy &DOWN")]
        public static void MoveDownwards()
        {
            if(Application.isPlaying)
            {
                return;
            }

            for(int i = Selection.gameObjects.Length - 1; i >= 0; i--)
            {
                var t = Selection.gameObjects[i].transform;

                if(t.parent)
                {
                    if(t.GetSiblingIndex() == t.parent.childCount - 1)
                    {
                        var parent = t.parent;

                        t.parent = null;
                        t.SetSiblingIndex(parent.GetSiblingIndex() + 1);
                    }
                    else
                    {
                        t.SetSiblingIndex(t.GetSiblingIndex() + 1);
                    }
                }
                else
                {
                    t.SetSiblingIndex(t.GetSiblingIndex() + 1);
                }
            }
        }

        #region Deprecated
        //private void OnSceneGUI()
        //{
        //    if(Selection.gameObjects.Length == 0)
        //        return;

        //    Event e = Event.current;

        //    switch(e.type)
        //    {
        //        case EventType.keyDown:
        //            if(Event.current.keyCode == KeyCode.DownArrow && Event.current.alt)
        //            {
        //                foreach(var go in Selection.gameObjects)
        //                {
        //                    Transform t = go.transform;

        //                    t.SetSiblingIndex(t.GetSiblingIndex() + 1);
        //                }
        //            }
        //            else if(Event.current.keyCode == KeyCode.UpArrow && Event.current.alt)
        //            {
        //                foreach(var go in Selection.gameObjects)
        //                {
        //                    Transform t = go.transform;

        //                    t.SetSiblingIndex(t.GetSiblingIndex() - 1);
        //                }
        //            }
        //            break;
        //    }
        //}
        #endregion
    }
}
