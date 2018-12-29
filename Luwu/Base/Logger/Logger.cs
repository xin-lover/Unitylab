﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luwu
{
    public class Logger
    {

        public static void Error(string message)
        {
            Debug.LogError(message);
        }

        public static void Warning(string message)
        {
            Debug.LogWarning(message);
        }

        public static void Info(string message)
        {
            Debug.Log(message);
        }
    }
}

