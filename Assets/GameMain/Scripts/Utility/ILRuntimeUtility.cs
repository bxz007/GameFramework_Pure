using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Events;
using UnityEngine;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Stack;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace GameMain
{
    public delegate void TestDelegate(int a, int b, int c);
    public static class ILRuntimeUtility
    {
        public static void InitILRuntime(AppDomain appdomain)
        {
            //TODO:注册重定向方法
            InitRedirection(appdomain);

            //TODO:适配委托
            appdomain.DelegateManager.RegisterMethodDelegate<float>();
            appdomain.DelegateManager.RegisterMethodDelegate<object, GameFramework.Event.GameEventArgs>();


            appdomain.DelegateManager.RegisterMethodDelegate<List<System.Object>>();


            appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
            appdomain.DelegateManager.RegisterMethodDelegate<byte[], int, int>();

            appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
            appdomain.DelegateManager.RegisterMethodDelegate<object, EventArgs>();

            appdomain.DelegateManager.RegisterMethodDelegate<IAsyncResult>();
            appdomain.DelegateManager.RegisterMethodDelegate<object, IAsyncResult>();

            appdomain.DelegateManager.RegisterMethodDelegate<int,int,int>();

            appdomain.DelegateManager.RegisterFunctionDelegate<bool>();

            appdomain.DelegateManager.RegisterFunctionDelegate<int,bool>();


            appdomain.DelegateManager.RegisterMethodDelegate<System.Single, System.Single, System.Int32>();            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32>();            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Int32, System.Int32>();            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Int32>>((act) =>
            {
                return new System.Comparison<System.Int32>((x, y) =>
                {
                    return ((Func<System.Int32, System.Int32, System.Int32>)act)(x, y);
                });
            });
            

            //注册CLR绑定代码
            CLRBindings.Initialize(appdomain);

        }      

        #region MonoBehaviorAdapter


        unsafe static void InitializeILRuntime(AppDomain appDomain)
        {
            //这里做一些ILRuntime的注册
            appDomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
            //处理Mono的绑定相关 get add
            SetupCLRRedirection(appDomain);
            SetupCLRRedirection2(appDomain);
        }

        unsafe static void SetupCLRRedirection(this AppDomain appDomain)
        {
            //这里面的通常应该写在InitializeILRuntime，这里为了演示写这里
            var arr = typeof(GameObject).GetMethods();
            foreach (var i in arr)
            {
                if (i.Name == "AddComponent" && i.GetGenericArguments().Length == 1)
                {
                    appDomain.RegisterCLRMethodRedirection(i, AddComponent);
                }
            }
        }

        unsafe static void SetupCLRRedirection2(this AppDomain appDomain)
        {
            //这里面的通常应该写在InitializeILRuntime，这里为了演示写这里
            var arr = typeof(GameObject).GetMethods();
            foreach (var i in arr)
            {
                if (i.Name == "GetComponent" && i.GetGenericArguments().Length == 1)
                {
                    appDomain.RegisterCLRMethodRedirection(i, GetComponent);
                }
            }
        }

        unsafe static MonoBehaviourAdapter.Adaptor GetComponent(this GameObject gameObject, ILType type)
        {
            var arr = gameObject.GetComponents<MonoBehaviourAdapter.Adaptor>();
            for (int i = 0; i < arr.Length; i++)
            {
                var instance = arr[i];
                if (instance.ILInstance != null && instance.ILInstance.Type == type)
                {
                    return instance;
                }
            }
            return null;
        }

        unsafe static StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new System.NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponent应该有且只有1个泛型参数
            if (genericArgument != null && genericArgument.Length == 1)
            {
                var type = genericArgument[0];
                object res;
                if (type is CLRType)
                {
                    //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                    res = instance.AddComponent(type.TypeForCLR);
                }
                else
                {
                    //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
                    var ilInstance = new ILTypeInstance(type as ILType, false);//手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
                                                                               //接下来创建Adapter实例
                    var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adaptor>();
                    //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
                    clrInstance.ILInstance = ilInstance;
                    clrInstance.AppDomain = __domain;
                    //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
                    ilInstance.CLRInstance = clrInstance;

                    res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance

                    clrInstance.Awake();//因为Unity调用这个方法时还没准备好所以这里补调一次
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }       

        unsafe static StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new System.NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponent应该有且只有1个泛型参数
            if (genericArgument != null && genericArgument.Length == 1)
            {
                var type = genericArgument[0];
                object res = null;
                if (type is CLRType)
                {
                    //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                    res = instance.GetComponent(type.TypeForCLR);
                }
                else
                {
                    //因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
                    var clrInstances = instance.GetComponents<MonoBehaviourAdapter.Adaptor>();
                    for (int i = 0; i < clrInstances.Length; i++)
                    {
                        var clrInstance = clrInstances[i];
                        if (clrInstance.ILInstance != null)//ILInstance为null, 表示是无效的MonoBehaviour，要略过
                        {
                            if (clrInstance.ILInstance.Type == type)
                            {
                                res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance
                                break;
                            }
                        }
                    }
                }
                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }
        #endregion


        #region 重定向
       
        unsafe static void InitRedirection(AppDomain appDomain)
        {
            Type type = typeof(UnityEngine.Debug);
            var Log = type.GetMethod("Log", new[] { typeof(object) });
            //重定向 debgug
            appDomain.RegisterCLRMethodRedirection(Log, DLog);

            var Waring = type.GetMethod("LogWarning", new[] { typeof(object) });
            appDomain.RegisterCLRMethodRedirection(Waring, DWaring);

            var Error = type.GetMethod("LogError", new[] { typeof(object) });
            appDomain.RegisterCLRMethodRedirection(Error, DError);
            appDomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            appDomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
            appDomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
            //appDomain.RegisterValueTypeBinder(typeof(TestMainStruct), new TestMainStructBinder());
            //appDomain.RegisterValueTypeBinder(typeof(MechanicsTimeChangeBuff), new MechanicsTimeChangeBuffBinder());

        }

        //重定向debug
        unsafe static StackObject* DLog(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            //只有一个参数，所以返回指针就是当前栈指针ESP - 1
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            //第一个参数为ESP -1， 第二个参数为ESP - 2，以此类推
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            //获取参数message的值
            object message = StackObject.ToObject(ptr_of_this_method, __domain, __mStack);
            //需要清理堆栈
            __intp.Free(ptr_of_this_method);
            //如果参数类型是基础类型，例如int，可以直接通过int param = ptr_of_this_method->Value获取值，
            //关于具体原理和其他基础类型如何获取，请参考ILRuntime实现原理的文档。

            //通过ILRuntime的Debug接口获取调用热更DLL的堆栈
            string stackTrace = __domain.DebugService.GetStackTrace(__intp);
            Debug.Log(string.Format("Log:{0}\n{1}", message, stackTrace));

            return __ret;
        }

        //重定向Waring
        unsafe static StackObject* DWaring(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            //只有一个参数，所以返回指针就是当前栈指针ESP - 1
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            //第一个参数为ESP -1， 第二个参数为ESP - 2，以此类推
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            //获取参数message的值
            object message = StackObject.ToObject(ptr_of_this_method, __domain, __mStack);
            //需要清理堆栈
            __intp.Free(ptr_of_this_method);
            //如果参数类型是基础类型，例如int，可以直接通过int param = ptr_of_this_method->Value获取值，
            //关于具体原理和其他基础类型如何获取，请参考ILRuntime实现原理的文档。

            //通过ILRuntime的Debug接口获取调用热更DLL的堆栈
            string stackTrace = __domain.DebugService.GetStackTrace(__intp);
            Debug.LogWarning(string.Format("Waring:{0}\n{1}",message, stackTrace));

            return __ret;
        }

        //重定向Error
        unsafe static StackObject* DError(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            //只有一个参数，所以返回指针就是当前栈指针ESP - 1
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            //第一个参数为ESP -1， 第二个参数为ESP - 2，以此类推
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            //获取参数message的值
            object message = StackObject.ToObject(ptr_of_this_method, __domain, __mStack);
            //需要清理堆栈
            __intp.Free(ptr_of_this_method);
            //如果参数类型是基础类型，例如int，可以直接通过int param = ptr_of_this_method->Value获取值，
            //关于具体原理和其他基础类型如何获取，请参考ILRuntime实现原理的文档。

            //通过ILRuntime的Debug接口获取调用热更DLL的堆栈
            string stackTrace = __domain.DebugService.GetStackTrace(__intp);
            Debug.LogError(string.Format("Error:{0}\n{1}", message, stackTrace));

            return __ret;
        }

        #endregion
    }
}

