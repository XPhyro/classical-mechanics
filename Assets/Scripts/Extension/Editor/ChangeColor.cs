using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Extension.Editor
{
    public class ChangeColor : ScriptableWizard
    {
        [SerializeField]
        private List<Transform> objects;
        [SerializeField]
        private Color color;

        [MenuItem("Window/Extensions/Change Color")]
        private static void CreateWizard()
        {
            DisplayWizard<ChangeColor>("Change Color", "Color selected object(s).");
        }

        private void OnWizardCreate()
        {
            if(objects != null && objects.Count > 0)
                foreach(Transform t in objects)
                {
                    var rend = t.GetComponent<Renderer>();

                    if(rend)
                    {
                        Undo.RecordObject(rend.sharedMaterial, "Change Material Color");
                        rend.sharedMaterial.color = color;
                    }
                }
            else
                foreach(GameObject g in Selection.gameObjects)
                {
                    var rend = g.GetComponent<Renderer>();

                    if(rend)
                    {
                        Undo.RecordObject(rend.sharedMaterial, "Change Material Color");
                        rend.sharedMaterial.color = color;
                    }
                }

            CreateWizard();
        }
    }
}
