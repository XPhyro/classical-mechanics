using UnityEngine;

namespace Projectiles
{
    public class EnvironmentScaler : MonoBehaviour
    {
        private Transform player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            transform.localScale = new Vector3(player.position.z / 5 + 4, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, transform.position.y, 5 * (transform.localScale.x - 4));
        }
    }
}
