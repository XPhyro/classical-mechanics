using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Extension.Editor
{
    public class DealWithChildren : ScriptableWizard
    {
        [SerializeField]
        private List<Transform> parents = new List<Transform>();
        [SerializeField]
        private Vector3 rotation = Vector3.zero;
        [SerializeField]
        private bool willCreateHolderForRotation = true;

        [MenuItem("Window/Extensions/Deal With Children")]
        private static void CreateWizard()
        {
            DisplayWizard<DealWithChildren>("Deal With Children", "Rotate Children", "Zero Out Parent(s)");
        }

        private void OnWizardCreate()
        {
            if(!willCreateHolderForRotation)
            {
                if(parents.Count != 0)
                {
                    foreach(Transform t in parents)
                    {
                        for(int i = 0; i < t.childCount; i++)
                        {
                            t.GetChild(i).rotation *= Quaternion.Euler(rotation);
                        }
                    }
                }
                else
                {
                    foreach(var g in Selection.gameObjects)
                    {
                        for(int i = 0; i < Selection.gameObjects.Length; i++)
                        {
                            g.transform.GetChild(i).rotation *= Quaternion.Euler(rotation);
                        }
                    }
                }
            }
            else
            {
                if(parents.Count != 0)
                {
                    GameObject go = new GameObject();
                    go.name = "RotationAdjuster";

                    foreach(var t in parents)
                    {
                        var transforms = t.GetComponentsInChildren<Transform>();

                        var gameObj = Instantiate(go, Vector3.zero, Quaternion.identity, t);

                        foreach(var tr in transforms)
                        {
                            tr.SetParent(gameObj.transform);
                        }
                    }
                }
                else
                {
                    GameObject go = new GameObject();
                    go.name = "RotationAdjuster";

                    foreach(var t in Selection.gameObjects)
                    {
                        var transforms = t.GetComponentsInChildren<Transform>();

                        var gameObj = Instantiate(go, Vector3.zero, Quaternion.identity, t.transform);

                        foreach(var tr in transforms)
                        {
                            tr.SetParent(gameObj.transform);
                        }
                    }
                }
            }

            DealWithChildren.CreateWizard();
        }

        private void OnWizardOtherButton()
        {
            if(parents.Count != 0)
                foreach(Transform t in parents)
                {
                    for(int i = 0; i < t.childCount; i++)
                    {
                        t.GetChild(i).position += t.position;
                    }

                    t.position = Vector3.zero;
                }
            else
                foreach(var g in Selection.gameObjects)
                {
                    for(int i = 0; i < Selection.gameObjects.Length; i++)
                    {
                        g.transform.GetChild(i).position += g.transform.position;
                    }

                    g.transform.position = Vector3.zero;
                }
        }
    }
}
