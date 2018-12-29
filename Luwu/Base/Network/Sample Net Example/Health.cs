using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Luwu.Net
{
    public class Health : NetworkBehaviour
    {

        public const int maxHealth = 100;

        [SyncVar(hook = "OnChangeHealth")]
        public int currentHealth = maxHealth;

        public RectTransform healthBar;

        public bool isDestroyOnDeath;

        private NetworkStartPosition[] spawnPoints;

        // Use this for initialization
        void Start()
        {
            if (isLocalPlayer)
            {
                spawnPoints = FindObjectsOfType<NetworkStartPosition>();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TakeDamage(int amount)
        {
            if (!isServer)
            {
                return;
            }
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                currentHealth = maxHealth;

                if (isDestroyOnDeath)
                {
                    Destroy(gameObject);
                }
                else
                {
                    RpcRespawn();
                }
            }

            //healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
        }

        void OnChangeHealth(int health)
        {
            healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
        }

        [ClientRpc]
        void RpcRespawn()
        {
            if (isLocalPlayer)
            {
                Vector3 spawnPoint = Vector3.zero;

                if (spawnPoints != null && spawnPoints.Length > 0)
                {
                    spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                }
                transform.position = spawnPoint;
            }
        }
    }
}

