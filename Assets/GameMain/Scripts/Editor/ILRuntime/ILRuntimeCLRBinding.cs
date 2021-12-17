﻿using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using GameMain;

namespace GameMain.Editor
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public class ILRuntimeCLRBinding
    {
        //废弃不用了
        //[MenuItem("ILRuntime/Generate CLR Binding Code")]
        //static void GenerateCLRBinding()
        //{
        //    List<Type> types = new List<Type>();
        //    types.Add(typeof(int));
        //    types.Add(typeof(float));
        //    types.Add(typeof(long));
        //    types.Add(typeof(object));
        //    types.Add(typeof(string));
        //    types.Add(typeof(Array));
        //    types.Add(typeof(Vector2));
        //    types.Add(typeof(Vector3));
        //    types.Add(typeof(Quaternion));
        //    types.Add(typeof(GameObject));
        //    types.Add(typeof(UnityEngine.Object));
        //    types.Add(typeof(Transform));
        //    types.Add(typeof(RectTransform));
        //    types.Add(typeof(Time));
        //    types.Add(typeof(Debug));
        //    //所有DLL内的类型的真实C#类型都是ILTypeInstance
        //    types.Add(typeof(List<ILRuntime.Runtime.Intepreter.ILTypeInstance>));

        //    ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(types, "Assets/GameMain/Scripts/ILRuntime/Generated");
        //    AssetDatabase.Refresh();
        //}

        [MenuItem("ILRuntime/Generate CLR Binding Code by Analysis")]
        static void GenerateCLRBindingByAnalysis()
        {
            //用新的分析热更dll调用引用来生成绑定代码
            ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();

            System.IO.FileStream fs = new System.IO.FileStream("Assets/GameMain/HotfixDLL/Hotfix.dll.bytes", System.IO.FileMode.Open, System.IO.FileAccess.Read); ;
            domain.LoadAssembly(fs);
            //Crossbind Adapter is needed to generate the correct binding code
            ILRuntimeUtility.InitILRuntime(domain);
            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, "Assets/GameMain/Scripts/ILRuntime/Generated");
            AssetDatabase.Refresh();
            fs.Close();
        }

    }

}
