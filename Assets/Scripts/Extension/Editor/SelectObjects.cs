using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Extension.Editor
{
    public class SelectObjects : EditorWindow
    {
        private string layerName;
        private string tagName;

        private bool isActive;
        private bool willCheckChildren;

        private List<GameObject> resultGos;

        private List<GameObject> gos = new List<GameObject>();

        [MenuItem("Window/Extensions/Select Objects")]
        private static void CreateWindow()
        {
            GetWindow<SelectObjects>(true, "Select Objects", true);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Layer name");
            layerName = EditorGUILayout.TextField(layerName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Tag name");
            tagName = EditorGUILayout.TextField(tagName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Is active");
            isActive = EditorGUILayout.Toggle(isActive);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Will check children");
            willCheckChildren = EditorGUILayout.Toggle(willCheckChildren);
            EditorGUILayout.EndHorizontal();

            int newCount = Mathf.Max(0, EditorGUILayout.IntField("Size", gos.Count));

            while(newCount < gos.Count)
            {
                gos.RemoveAt(gos.Count - 1);
            }
            while(newCount > gos.Count)
            {
                gos.Add(null);
            }
            for(int i = 0; i < gos.Count; i++)
            {
                gos[i] = (GameObject)EditorGUILayout.ObjectField(gos[i], typeof(GameObject), true);
            }

            if(GUILayout.Button("Select with tag."))
            {
                SelectWithTag();
            }

            if(GUILayout.Button("Select in layer."))
            {
                SelectInLayer();
            }

            if(GUILayout.Button("Select active/deactive."))
            {
                SelectOnActivity();
            }
        }

        private void SelectWithTag()
        {
            if(UnityEditorInternal.InternalEditorUtility.tags.Contains(tagName))
            {
                Selection.objects = GameObject.FindGameObjectsWithTag(tagName);
            }
        }

        private void SelectInLayer()
        {
            var gameObjects = Utility.GameObjectUtility.FindGameObjectsInLayer(layerName);

            if(gameObjects != null)
            {
                Selection.objects = gameObjects;
            }
        }

        private void SelectOnActivity()
        {
            if(gos.Count == 0)
            {
                List<GameObject> gos = Selection.gameObjects.ToList();

                if(!willCheckChildren)
                {
                    foreach(var go in gos.ToList())
                    {
                        if(go.activeInHierarchy != isActive)
                        {
                            gos.Remove(go);
                        }
                    }
                }
                else
                {
                    resultGos = new List<GameObject>();
                    SelectChildrenOnActivity(gos);

                    gos = resultGos;
                }

                Selection.objects = gos.ToArray();
            }
            else
            {
                List<GameObject> gos = this.gos;

                if(!willCheckChildren)
                {
                    foreach(var go in gos.ToList())
                    {
                        if(go.activeInHierarchy != isActive)
                        {
                            gos.Remove(go);
                        }
                    }
                }
                else
                {
                    resultGos = new List<GameObject>();
                    SelectChildrenOnActivity(gos);

                    gos = resultGos;
                }

                Selection.objects = gos.ToArray();
            }
        }

        private void SelectChildrenOnActivity(GameObject go)
        {
            for(int i = 0; i < go.transform.childCount; i++)
            {
                SelectChildrenOnActivity(go.transform.GetChild(i).gameObject);
            }

            if(go.activeInHierarchy != isActive)
            {
                resultGos.Remove(go);
            }
            else if(!gos.Contains(go))
            {
                resultGos.Add(go);
            }
        }

        private void SelectChildrenOnActivity(List<GameObject> gos)
        {
            foreach(var go in gos.ToList())
            {
                for(int i = 0; i < go.transform.childCount; i++)
                {
                    SelectChildrenOnActivity(go.transform.GetChild(i).gameObject);
                }

                if(go.activeInHierarchy != isActive)
                {
                    gos.Remove(go);
                }
                else if(!gos.Contains(go))
                {
                    gos.Add(go);
                }
            }

            foreach(var go in gos)
            {
                resultGos.Add(go);
            }
        }
    }
}
