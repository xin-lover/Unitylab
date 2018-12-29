using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luwu
{
    namespace UI
    {
        public class UIWidget
        {

            protected Transform mRootTrans;

            public virtual void Init(Transform trans)
            {
                mRootTrans = trans;
            }

            public virtual bool isShow
            {
                get
                {
                    return mRootTrans.gameObject.activeSelf;
                }
            }

            public virtual void Show()
            {
                mRootTrans.gameObject.SetActive(true);
            }

            public virtual void Hide()
            {
                mRootTrans.gameObject.SetActive(false);
            }
        }
    }

}

