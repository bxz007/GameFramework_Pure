
using GameFramework;
using GameFramework.Task;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 定时器组件。
    /// </summary>
    [DisallowMultipleComponent]  
    public sealed class TimerComponent : GameFrameworkComponent
    {
        private static uint timerID = 0;
        public List<AbsTimer> timerList = new List<AbsTimer>();

        public List<AbsTimer> nextFrameList = new List<AbsTimer>();
        private uint tempRemoveID;

        private void Update()
        {
            if (timerList.Count > 0)
            {
                for (int i = 0; i < timerList.Count; i++)
                {
                    tempRemoveID = timerList[i].id;
                    if (!timerList[i].Update())
                    {
                        RemoveTimer(tempRemoveID);
                    }
                }
            }
            while (nextFrameList.Count > 0)
            {
                nextFrameList[0].DoAction();
                nextFrameList.RemoveAt(0);
            }
        }

        public void CallNextFrame(System.Action action)
        {
            TimerInfo info = ReferencePool.Acquire<TimerInfo>();
            info.Action = action;
            nextFrameList.Add(info);
        }

        public void CallNextFrame<T1>(System.Action<T1> action, T1 t1)
        {           
            TimerInfo <T1> info = ReferencePool.Acquire<TimerInfo<T1>>();
            info.Action = action;
            info.arg1 = t1;
            nextFrameList.Add(info);
        }

        public void CallNextFrame<T1, T2>(System.Action<T1, T2> action, T1 t1, T2 t2)
        {
            TimerInfo<T1, T2> info = ReferencePool.Acquire<TimerInfo<T1, T2>>();
            info.Action = action;
            info.arg1 = t1;
            info.arg2 = t2;
            nextFrameList.Add(info);
        }

        public void CallNextFrame<T1, T2, T3>(System.Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            TimerInfo<T1, T2, T3> info = ReferencePool.Acquire<TimerInfo<T1, T2, T3>>();
            info.Action = action;
            info.arg1 = t1;
            info.arg2 = t2;
            info.arg3 = t3;
            nextFrameList.Add(info);
        }

        public void CallNextFrame<T1, T2, T3, T4>(System.Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            TimerInfo<T1, T2, T3, T4> info = ReferencePool.Acquire<TimerInfo<T1, T2, T3, T4>>();
            info.Action = action;
            info.arg1 = t1;
            info.arg2 = t2;
            info.arg3 = t3;
            info.arg4 = t4;
            nextFrameList.Add(info);
        }

        /// <summary>
        /// 表示延迟Delay秒后按dur的频率执行多少次数
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        /// <param name="dur"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public TimerInfo AddTimer(System.Action action, float delay, float dur = 0, int count = -1)
        {
            if (timerID > 1.8e+19)
                timerID = 0;
            TimerInfo info = ReferencePool.Acquire<TimerInfo>().Fill(timerID++,delay, dur,count);        
            timerList.Add(info);
            info.Action = action;
            info.ReStart();
            return info;
        }

        public TimerInfo<T1> AddTimer<T1>(System.Action<T1> action, T1 t1, float delay, float dur = 0, int count = -1)
        {
            if (timerID > 1.8e+19)
                timerID = 0;
            TimerInfo<T1> info = ReferencePool.Acquire<TimerInfo<T1>>().Fill(timerID++,delay, dur, count);
            info.Action = action;
            info.arg1 = t1;
            timerList.Add(info);
            info.ReStart();
            return info;
        }

        public TimerInfo<T1, T2> AddTimer<T1, T2>(System.Action<T1, T2> action, T1 t1, T2 t2, float delay, float dur = 0, int count = -1)
        {
            if (timerID > 1.8e+19)
                timerID = 0;
            TimerInfo<T1, T2> info = ReferencePool.Acquire<TimerInfo<T1,T2>>().Fill(timerID++, delay, dur, count);
            info.Action = action;
            info.arg1 = t1;
            info.arg2 = t2;
            timerList.Add(info);
            info.ReStart();
            return info;
        }

        public TimerInfo<T1, T2, T3> AddTimer<T1, T2, T3>(System.Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3, float delay, float dur = 0, int count = -1)
        {
            if (timerID > 1.8e+19)
                timerID = 0;
            TimerInfo<T1, T2, T3> info = ReferencePool.Acquire<TimerInfo<T1, T2,T3>>().Fill(timerID++, delay, dur, count);
            info.Action = action;
            info.arg1 = t1;
            info.arg2 = t2;
            info.arg3 = t3;
            timerList.Add(info);
            info.ReStart();
            return info;
        }

        public TimerInfo<T1, T2, T3, T4> AddTimer<T1, T2, T3, T4>(System.Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4, float delay, float dur = 0, int count = -1)
        {
            if (timerID > 1.8e+19)
                timerID = 0;
            TimerInfo<T1, T2, T3, T4> info = ReferencePool.Acquire<TimerInfo<T1, T2,T3,T4>>().Fill(timerID++, delay, dur, count);
            info.Action = action;
            info.arg1 = t1;
            info.arg2 = t2;
            info.arg3 = t3;
            info.arg4 = t4;
            timerList.Add(info);
            info.ReStart();
            return info;
        }

        public void RemoveTimer(uint id)
        {
            AbsTimer info = timerList.Find(item => item.id == id) as AbsTimer;
            RemoveTimer(info);
        }

        public void RemoveTimer(AbsTimer t)
        {
            if (t != null)
            {
                //Debuger.LogError("Delete" + t.id);
                timerList.Remove(t);
                ReferencePool.Release(t);
                t = null;
            }
        }

        public bool IsContain(AbsTimer t)
        {
            return timerList.Contains(t);
        }
        public bool IsContain(uint id)
        {
            return timerList.Find(t => t.id == id) != null;
        }

        public void Clear()
        {
            while (timerList.Count > 0)
            {
                RemoveTimer(timerList[0]);
            }
            timerList.Clear();
            while (nextFrameList.Count > 0)
            {
                RemoveTimer(nextFrameList[0]);
            }
            nextFrameList.Clear();
        }
    }
}
