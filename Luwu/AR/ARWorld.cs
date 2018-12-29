using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luwu.AR
{
    public class ARWorldMonoBehaviour : MonoBehaviour
    {
        public System.Action updateHandler = null;

        private void Update()
        {
            if (updateHandler != null)
            {
                updateHandler();
            }
        }
    }

    public abstract class ARImpl
    {
        protected GameObject worldObj;

        public float maxRayDistance = 30f;
        public int collisionLayer = 1 << 10;

        public virtual void Init()
        {
            worldObj = new GameObject("ARWorld");
            worldObj.AddComponent<ARWorldMonoBehaviour>().updateHandler = Run; 
            GameObject.DontDestroyOnLoad(worldObj);
        }

        public virtual void Release()
        {
            GameObject.Destroy(worldObj);
        }

        protected abstract void Run();
        protected virtual void OnClickPlane(Vector3 position, Quaternion rotation)
        {
            if(onClickPlane != null)
            {
                onClickPlane(position, rotation);
            }
        }

        public event System.Action<Vector3,Quaternion> onClickPlane;
    }

    public static class ARWorld
    {
        public enum Platform
        {
            ARKit,
            ARCore,
        }

        private static ARImpl sImpl;
        public static ARImpl ar
        {
            get
            {
                return sImpl;
            }
        }

        public static void CreateWith(Platform platform)
        {
            switch(platform)
            {
                case Platform.ARKit:
                    sImpl = new ARKitCreater();
                    sImpl.Init();
                    break;
                case Platform.ARCore:
                    sImpl = new ARCoreCreater();
                    sImpl.Init();
                    break;
                default:
                    Logger.Error("Error platform!!!!");
                    break;
            }
        }

        public static void Destroy()
        {

            sImpl.Release();
        }
    }

}
