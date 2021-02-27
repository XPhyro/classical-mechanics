using UnityEngine;

namespace Force
{
    public class EnvironmentFollow : MonoBehaviour
    {
        private Transform player;

        private const float distance = 60;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            var z = transform.position.z - player.position.z;
            if(z > distance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z - distance);
            }
            else if(z < -distance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + distance);
            }
        }
    }
}
