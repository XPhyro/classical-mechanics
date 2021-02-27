using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extension.Utility
{
    public class RendererUtility
    {
        public enum ObjectToDestroy
        {
            GameObject, Renderer
        }

        private static Dictionary<Renderer, Color> fadingRends = new Dictionary<Renderer, Color>();

        public static void Fade(Renderer renderer, Color from, Color to, float fadeInTime)
        {
            UtilityManager.Singleton.StartCoroutine(_Fade(renderer, from, to, fadeInTime, 0, false));

        }

        public static void Fade(Renderer renderer, Color from, Color to, float fadeInTime, float fadeOutTime, bool willLoop)
        {
            UtilityManager.Singleton.StartCoroutine(_Fade(renderer, from, to, fadeInTime, fadeOutTime, willLoop));
        }

        public static void Fade(Renderer renderer, Color from, Color to, float fadeInTime, bool willDestroyRenderer, float destroyDelay, ObjectToDestroy objectToDestroy)
        {
            UtilityManager.Singleton.StartCoroutine(_Fade(renderer, from, to, fadeInTime, 0, false));

            if(willDestroyRenderer)
            {
                UtilityManager.Singleton.StartCoroutine(DestroyRendererAfterFade(renderer, to, destroyDelay, objectToDestroy));
            }
        }

        public static void Fade(Renderer renderer, Color from, Color to, float fadeInTime, float fadeOutTime, bool willLoop, bool willDestroyRenderer, float destroyDelay, ObjectToDestroy objectToDestroy)
        {
            UtilityManager.Singleton.StartCoroutine(_Fade(renderer, from, to, fadeInTime, fadeOutTime, willLoop));

            if(willDestroyRenderer)
            {
                UtilityManager.Singleton.StartCoroutine(DestroyRendererAfterFade(renderer, to, destroyDelay, objectToDestroy));
            }
        }

        private static IEnumerator _Fade(Renderer renderer, Color from, Color to, float fadeInTime, float fadeOutTime, bool willLoop)
        {
            if(!fadingRends.ContainsKey(renderer) && renderer)
            {
                fadingRends.Add(renderer, to);

                var ftime = Time.time;

                while(true)
                {
                    if(!renderer)
                    {
                        break;
                    }

                    if(renderer.material.color != to)
                    {
                        renderer.material.color = Color.Lerp(from, to, Mathf.Clamp01((Time.time - ftime) / fadeInTime));

                        yield return new WaitForSeconds(Time.deltaTime);
                    }
                    else
                    {
                        break;
                    }
                }

                fadingRends.Remove(renderer);

                if(willLoop)
                    UtilityManager.Singleton.StartCoroutine(_Fade(renderer, to, from, fadeOutTime, fadeInTime, false));
            }
        }

        private static IEnumerator DestroyRendererAfterFade(Renderer renderer, Color targetColor, float destroyDelay, ObjectToDestroy objectToDestroy)
        {
            while(true)
            {
                if(!renderer)
                {
                    break;
                }

                if(renderer.material.color == targetColor)
                {
                    switch(objectToDestroy)
                    {
                        case ObjectToDestroy.GameObject:
                            Object.Destroy(renderer.gameObject, destroyDelay);
                            break;
                        case ObjectToDestroy.Renderer:
                            Object.Destroy(renderer, destroyDelay);
                            break;
                        default:
                            Object.Destroy(renderer, destroyDelay);
                            break;
                    }

                    break;
                }

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}
