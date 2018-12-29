using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace Luwu
{
    namespace UI
    {
        public class UIManager
        {

            public static UIManager instance = new UIManager();
            private GameObject mCanvas;

            private Dictionary<System.Type, UIWidget> mWindows = new Dictionary<System.Type, UIWidget>();

            private UIManager()
            {
                mCanvas = GameObject.Find("Canvas");
                Debug.Assert(mCanvas != null);
                if (mCanvas == null)
                {
                    Logger.Error("Can't find 'Canvas' in scene...");
                }
            }

            public T Get<T>() where T : UIWidget
            {
                UIWidget wnd;
                if (!mWindows.TryGetValue(typeof(T), out wnd))
                {
                    System.Type t = typeof(T);
                    Transform trans = mCanvas.transform.Find(t.Name);
                    Debug.Assert(trans != null);

                    wnd = Activator.CreateInstance(t) as UIWidget;
                    wnd.Init(trans);
                    mWindows.Add(t, wnd);
                }

                return wnd as T;
            }

        }
    }


}

