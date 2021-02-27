using System.Collections.Generic;
using UnityEngine;

namespace Extension.Utility
{
    public class GameObjectUtility
    {
        public static GameObject[] FindGameObjectsInLayer(LayerMask layer)
        {
            var gameObjArray = (GameObject[])Object.FindObjectsOfType(typeof(GameObject));
            var gameObjList = new List<GameObject>();

            foreach(var go in gameObjArray)
            {
                if(go.layer == layer)
                    gameObjList.Add(go);
            }

            if(gameObjList.Count > 0)
                return gameObjList.ToArray();
            else
                return null;
        }

        public static GameObject[] FindGameObjectsInLayer(string layer)
        {
            var gameObjArray = (GameObject[])Object.FindObjectsOfType(typeof(GameObject));
            var gameObjList = new List<GameObject>();

            foreach(var go in gameObjArray)
            {
                if(go.layer == LayerMask.NameToLayer(layer))
                    gameObjList.Add(go);
            }

            if(gameObjList.Count > 0)
                return gameObjList.ToArray();
            else
                return null;
        }

        public static GameObject[] FindGameObjectsInLayer(int layer)
        {
            var gameObjArray = (GameObject[])Object.FindObjectsOfType(typeof(GameObject));
            var gameObjList = new List<GameObject>();

            foreach(var _go in gameObjArray)
            {
                if(_go.layer == LayerMask.NameToLayer(LayerMask.LayerToName(layer)))
                    gameObjList.Add(_go);
            }

            if(gameObjList.Count > 0)
                return gameObjList.ToArray();
            else
                return null;
        }

        public static void SetLayer(GameObject go, LayerMask layer, bool willDoChildren)
        {
            if(go == null)
                return;

            go.layer = layer;

            if(willDoChildren)
            {
                foreach(Transform child in go.transform)
                {
                    if(child == null)
                        continue;

                    SetLayer(child.gameObject, layer, false);
                }
            }
        }

        public static void SetLayer(GameObject go, string layer, bool willDoChildren)
        {
            if(go == null)
                return;

            go.layer = LayerMask.NameToLayer(layer);

            if(willDoChildren)
            {
                foreach(Transform child in go.transform)
                {
                    if(child == null)
                        continue;

                    SetLayer(child.gameObject, layer, false);
                }
            }
        }

        public static void SetLayer(GameObject go, int layer, bool willDoChildren)
        {
            if(go == null)
                return;

            go.layer = layer;

            if(willDoChildren)
            {
                foreach(Transform child in go.transform)
                {
                    if(child == null)
                        continue;

                    SetLayer(child.gameObject, layer, false);
                }
            }
        }
    }
}
