using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameMain.Hotfix
{
    public class TestHotFixMain
    {
        private float testFloat;
        private int testInt1;
        public float testFloat1;
        public float tesetBool;
        public int TestInt;

        public static void Initialize()
        {
            Debug.LogError("Initialize");
            float abc = 125;
            float bcd = abc + 234;
        }

        public void Test()
        {
            Debug.LogError("HotFix start  Test");
            float abc = 125;
            float bcd = abc + 234;
        }
    }
}
