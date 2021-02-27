using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Extension.Editor
{
    public class HandleColliders : EditorWindow
    {
        private enum ColliderType
        {
            Box, Sphere, Capsule, Mesh, All
        }

        private ColliderType colliderType = ColliderType.Mesh;

        private List<GameObject> parents = new List<GameObject>();

        private bool willOnlyUseGraphics = true;

        private bool doForChildren = true;

        private bool willRemoveAllCollidersOfType = true;

        [MenuItem("Window/Extensions/Handle Colliders")]
        private static void ShowWindow()
        {
            GetWindow<HandleColliders>(true, "Handle Colliders", true);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Collider type");
            colliderType = (ColliderType)EditorGUILayout.EnumPopup(colliderType);
            EditorGUILayout.EndHorizontal();

            int newCount = Mathf.Max(0, EditorGUILayout.IntField("Size", parents.Count));

            while(newCount < parents.Count)
            {
                parents.RemoveAt(parents.Count - 1);
            }
            while(newCount > parents.Count)
            {
                parents.Add(null);
            }

            for(int i = 0; i < parents.Count; i++)
            {
                parents[i] = (GameObject)EditorGUILayout.ObjectField(parents[i], typeof(GameObject), true);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Object must have a renderer");
            willOnlyUseGraphics = EditorGUILayout.Toggle(willOnlyUseGraphics);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Do for children");
            doForChildren = EditorGUILayout.Toggle(doForChildren);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Remove all colliders of type");
            willRemoveAllCollidersOfType = EditorGUILayout.Toggle(willRemoveAllCollidersOfType);
            EditorGUILayout.EndHorizontal();

            if(GUILayout.Button("Add Collider"))
            {
                OnAddButtonDown();
            }
            if(GUILayout.Button("Remove Collider"))
            {
                OnRemoveButtonDown();
            }
            if(GUILayout.Button("Change Convex Attribute"))
            {
                OnChangeConvexAttributeButton();
            }
        }

        private void OnAddButtonDown()
        {
            if(parents.Count == 0 && Selection.objects.Length > 0)
            {
                AddCollider(Selection.gameObjects.ToList(), colliderType, willOnlyUseGraphics, doForChildren);
            }
            else if(parents.Count > 0)
            {
                AddCollider(parents, colliderType, willOnlyUseGraphics, doForChildren);
            }
        }

        private void OnRemoveButtonDown()
        {
            if(parents.Count == 0 && Selection.objects.Length > 0)
            {
                RemoveCollider(Selection.gameObjects.ToList(), colliderType, willOnlyUseGraphics, doForChildren, willRemoveAllCollidersOfType);
            }
            else if(parents.Count > 0)
            {
                RemoveCollider(parents, colliderType, willOnlyUseGraphics, doForChildren, willRemoveAllCollidersOfType);
            }
        }

        private void OnChangeConvexAttributeButton()
        {
            if(parents.Count == 0 && Selection.objects.Length > 0)
            {
                ChangeConvexAttribute(Selection.gameObjects.ToList(), willOnlyUseGraphics, doForChildren);
            }
            else if(parents.Count > 0)
            {
                ChangeConvexAttribute(parents, willOnlyUseGraphics, doForChildren);
            }
        }

        private void AddCollider(List<GameObject> gos, ColliderType col, bool onlyAddToRenderers, bool doSameForChildren)
        {
            if(col == ColliderType.All)
            {
                Debug.LogWarning("Cannot add all colliders.");

                return;
            }

            if(col != ColliderType.Mesh)
            {
                foreach(var go in gos)
                {
                    if((onlyAddToRenderers && go.GetComponent<Renderer>()) || !onlyAddToRenderers)
                    {
                        go.AddComponent(ReturnColliderType(col));
                    }

                    if(doSameForChildren)
                    {
                        for(int i = 0; i < go.transform.childCount; i++)
                        {
                            AddCollider(go.transform.GetChild(i).gameObject, col, onlyAddToRenderers, doSameForChildren);
                        }
                    }
                }
            }
            else
            {
                foreach(var go in gos)
                {
                    if((go.GetComponent<Renderer>()))
                    {
                        go.AddComponent(ReturnColliderType(col));

                        var mCol = go.GetComponent<MeshCollider>();

                        var meshFilter = go.GetComponent<MeshFilter>();
                        var skinnedMesh = go.GetComponent<SkinnedMeshRenderer>();

                        if(meshFilter)
                        {
                            mCol.sharedMesh = meshFilter.sharedMesh;
                        }
                        else if(skinnedMesh)
                        {
                            mCol.sharedMesh = skinnedMesh.sharedMesh;
                        }
                    }

                    if(doSameForChildren)
                    {
                        for(int i = 0; i < go.transform.childCount; i++)
                        {
                            AddCollider(go.transform.GetChild(i).gameObject, col, onlyAddToRenderers, doSameForChildren);
                        }
                    }
                }
            }
        }

        private void AddCollider(GameObject go, ColliderType col, bool onlyAddToRenderers, bool doSameForChildren)
        {
            if(col != ColliderType.Mesh)
            {
                if((onlyAddToRenderers && go.GetComponent<Renderer>()) || !onlyAddToRenderers)
                {
                    go.AddComponent(ReturnColliderType(col));
                }

                if(doSameForChildren)
                {
                    for(int i = 0; i < go.transform.childCount; i++)
                    {
                        AddCollider(go.transform.GetChild(i).gameObject, col, onlyAddToRenderers, doSameForChildren);
                    }
                }
            }
            else
            {
                if((go.GetComponent<Renderer>()))
                {
                    go.AddComponent(ReturnColliderType(col));

                    var mCol = go.GetComponent<MeshCollider>();

                    var meshFilter = go.GetComponent<MeshFilter>();
                    var skinnedMesh = go.GetComponent<SkinnedMeshRenderer>();

                    if(meshFilter)
                    {
                        mCol.sharedMesh = meshFilter.sharedMesh;
                    }
                    else if(skinnedMesh)
                    {
                        mCol.sharedMesh = skinnedMesh.sharedMesh;
                    }
                }

                if(doSameForChildren)
                {
                    for(int i = 0; i < go.transform.childCount; i++)
                    {
                        AddCollider(go.transform.GetChild(i).gameObject, col, onlyAddToRenderers, doSameForChildren);
                    }
                }
            }
        }

        private void RemoveCollider(List<GameObject> gos, ColliderType col, bool onlyRemoveFromRenderers, bool doSameForChildren, bool removeAllCollidersOfType)
        {
            foreach(var go in gos)
            {
                if(!removeAllCollidersOfType)
                {
                    if((onlyRemoveFromRenderers && go.GetComponent<Renderer>()) || !onlyRemoveFromRenderers)
                    {
                        foreach(var component in go.GetComponents(ReturnColliderType(col)))
                        {
                            DestroyImmediate(go.GetComponent(ReturnColliderType(col)));
                        }
                    }
                }
                else
                {
                    if((onlyRemoveFromRenderers && go.GetComponent<Renderer>()) || !onlyRemoveFromRenderers)
                    {
                        foreach(var component in go.GetComponents(ReturnColliderType(col)))
                        {
                            DestroyImmediate(component);
                        }
                    }
                }

                if(doSameForChildren)
                {
                    for(int i = 0; i < go.transform.childCount; i++)
                    {
                        RemoveCollider(go.transform.GetChild(i).gameObject, col, onlyRemoveFromRenderers, doSameForChildren, removeAllCollidersOfType);
                    }
                }
            }
        }

        private void RemoveCollider(GameObject go, ColliderType col, bool onlyRemoveFromRenderers, bool doSameForChildren, bool removeAllCollidersOfType)
        {
            if(!removeAllCollidersOfType)
            {
                if((onlyRemoveFromRenderers && go.GetComponent<Renderer>()) || !onlyRemoveFromRenderers)
                {
                    foreach(var component in go.GetComponents(ReturnColliderType(col)))
                    {
                        DestroyImmediate(go.GetComponent(ReturnColliderType(col)));
                    }
                }
            }
            else
            {
                if((onlyRemoveFromRenderers && go.GetComponent<Renderer>()) || !onlyRemoveFromRenderers)
                {
                    foreach(var component in go.GetComponents(ReturnColliderType(col)))
                    {
                        DestroyImmediate(component);
                    }
                }
            }

            if(doSameForChildren)
            {
                for(int i = 0; i < go.transform.childCount; i++)
                {
                    RemoveCollider(go.transform.GetChild(i).gameObject, col, onlyRemoveFromRenderers, doSameForChildren, removeAllCollidersOfType);
                }
            }
        }

        private void ChangeConvexAttribute(List<GameObject> gos, bool onlyChangeForRenderers, bool doSameForChildren)
        {
            foreach(var go in gos)
            {
                if((onlyChangeForRenderers && go.GetComponent<Renderer>()) || !onlyChangeForRenderers)
                {
                    var col = go.GetComponent<MeshCollider>();
                    go.GetComponent<MeshCollider>().convex = !col.convex;

                }

                if(doSameForChildren)
                {
                    for(int i = 0; i < go.transform.childCount; i++)
                    {
                        ChangeConvexAttribute(go.transform.GetChild(i).gameObject, onlyChangeForRenderers, doSameForChildren);
                    }
                }
            }

        }

        private void ChangeConvexAttribute(GameObject go, bool onlyChangeForRenderers, bool doSameForChildren)
        {
            if((onlyChangeForRenderers && go.GetComponent<Renderer>()) || !onlyChangeForRenderers)
            {
                var col = go.GetComponent<MeshCollider>();
                go.GetComponent<MeshCollider>().convex = !col.convex;

            }

            if(doSameForChildren)
            {
                for(int i = 0; i < go.transform.childCount; i++)
                {
                    ChangeConvexAttribute(go.transform.GetChild(i).gameObject, onlyChangeForRenderers, doSameForChildren);
                }
            }
        }

        private Type ReturnColliderType(ColliderType col)
        {
            switch(col)
            {
                case ColliderType.Box:
                    return typeof(BoxCollider);
                case ColliderType.Sphere:
                    return typeof(SphereCollider);
                case ColliderType.Capsule:
                    return typeof(CapsuleCollider);
                case ColliderType.Mesh:
                    return typeof(MeshCollider);
                default:
                    return typeof(Collider);
            }
        }
    }
}
