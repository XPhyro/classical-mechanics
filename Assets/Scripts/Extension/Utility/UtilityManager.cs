using UnityEngine;

namespace Extension.Utility
{
    public class UtilityManager : MonoBehaviour
    {
        public static UtilityManager Singleton;

        private void Awake()
        {
            if(!Singleton)
                Singleton = this;
        }
    }
}
