using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luwu.Net
{
    public class Bullet : MonoBehaviour
    {



        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            var hit = collision.gameObject;
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(10);
            }
            Destroy(gameObject);
        }
    }
}

