using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Extension.Editor
{
    public class ApplyPrefab
    {
        [MenuItem("Shortcuts/Apply Prefab _F3")]
        private static void ApplyPrefabs()
        {
            var gos = new List<GameObject>();

            foreach(var go in Selection.gameObjects)
            {
                if(!gos.Contains(go))
                {
                    gos.Add(PrefabUtility.FindRootGameObjectWithSameParentPrefab(go));
                }
            }

            foreach(var go in gos)
            {
                PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(PrefabUtility.FindPrefabRoot(go)), ReplacePrefabOptions.ConnectToPrefab);
            }
        }
    }
}
