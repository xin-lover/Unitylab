using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luwu.Net
{
    public class Billboard : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}

