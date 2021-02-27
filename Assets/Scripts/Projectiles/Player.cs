using UnityEngine;

namespace Projectiles
{
    public class Player : MonoBehaviour
    {
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            GameManager.Singleton.OnTCollision();
        }
    }
}
