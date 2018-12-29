using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luwu
{
    namespace UnityUtil
    {
        public class CoroutineBehaviour:MonoBehaviour
        {
            //for call startcoroutine
        }

        public class Coroutine
        {
            private static CoroutineBehaviour instance_ = new GameObject("CoroutineRunTime").AddComponent<CoroutineBehaviour>();

            private static Dictionary<int, UnityEngine.Coroutine> sCoroutines = new Dictionary<int, UnityEngine.Coroutine>();
            private static int sCount = 0;

            public static int Create(System.Action func, float delay, float interval = 0)
            {
                sCount++;
                sCoroutines[sCount] = instance_.StartCoroutine(TimerFunc(sCount, func, delay, interval));
                return sCount;
            }

            public static IEnumerator TimerFunc(int id,System.Action func, float delay, float interval )
            {
                yield return new WaitForSeconds(delay);

                while(true)
                {
                    func();

                    if(interval <= 0)
                    {
                        break;
                    }
                    else
                    {
                        yield return new WaitForSeconds(interval);
                    }
                }

                sCoroutines.Remove(id);
            }

            public static void Stop(int id)
            {
                UnityEngine.Coroutine cu;
                if(sCoroutines.TryGetValue(id, out cu))
                {
                    instance_.StopCoroutine(cu);
                    sCoroutines.Remove(id);
                }

            }

            public static void StopAll()
            {
                instance_.StopAllCoroutines();
                sCoroutines.Clear();
            }
        }
    }
}