using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Luwu.AR;

namespace Luwu
{
    public class HItTest : MonoBehaviour
    {
        public GameObject obj;
        // Use this for initialization
        void Start()
        {
            ARWorld.CreateWith(ARWorld.Platform.ARKit);
            ARWorld.ar.onClickPlane += (pos, rot) =>
            {
                obj.transform.position = pos;
                obj.transform.rotation = rot;
            };
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

