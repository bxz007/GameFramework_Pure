using UnityEngine;
using System;

namespace GameMain
{
    public abstract class AbsTimer:GameFramework.IReference
    {
        private float curTime;
        private int curCount;
        public uint id { get; set; }
        public bool isPlaying { get; set; }
        protected float curDelay { get; set; }
        protected float dur { get; set; }
        protected int durCount { get; set; }
        public abstract Delegate Action { get; set; }

        public abstract void DoAction();

        public AbsTimer()
        {
        }

        /// <summary>
        /// 刷新Timer 返回false表示结束
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            if (!isPlaying)
                return true;
            curTime += Time.deltaTime;
            if (curDelay == 0)
            {
                if (dur <= 0)
                    return false;
                if (durCount > 0)
                {
                    if (curTime >= dur)
                    {
                        DoAction();
                        curTime -= dur;
                        curCount--;
                    }
                    if (curCount <= 0)
                        return false;
                }
                else
                {
                    if (curTime >= dur)
                    {
                        DoAction();
                        curTime -= dur;
                    }
                }
            }
            else
            {
                if (curTime >= curDelay)
                {
                    DoAction();
                    curCount--;
                    curTime -= curDelay;
                    curDelay = 0f;
                    if (dur <= 0)
                        return false;
                }
            }
            return true;
        }

        public void ReStart()
        {
            isPlaying = true;
            curTime = 0f;
            curCount = durCount;
            //如果延迟为0 马上执行一次
            if (curDelay == 0)
            {
                DoAction();
                curCount--;
            }
        }

        public void Play()
        {
            isPlaying = true;
        }

        public void Pause()
        {
            isPlaying = false;
        }

        public virtual void Clear()
        {
            
        }
    }

    public class TimerInfo : AbsTimer
    {
        private Action action;

        public override Delegate Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value as Action;
            }
        }

        public override void DoAction()
        {
            action.Invoke();
        }
  

        public TimerInfo()
        { }

        public TimerInfo Fill(uint _id, float _delay, float _dur, int _count)
        {
            id = _id;
            curDelay = _delay;
            dur = _dur;
            durCount = _count;
            return this;
        }

        public override void Clear()
        {
            base.Clear();
        }
    }

    public class TimerInfo<T1> : AbsTimer
    {
        private Action<T1> action;
        public T1 arg1 { get; set; }

        public override Delegate Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value as Action<T1>;
            }
        }

        public override void DoAction()
        {
            action.Invoke(arg1);
        }
        public TimerInfo()
        { }
        public TimerInfo<T1> Fill(uint _id, float _delay, float _dur, int _count)
        {
            id = _id;
            curDelay = _delay;
            dur = _dur;
            durCount = _count;
            return this;
        }

        public override void Clear()
        {
            base.Clear();
        }
    }

    public class TimerInfo<T1, T2> : AbsTimer
    {
        private Action<T1, T2> action;
        public T1 arg1 { get; set; }
        public T2 arg2 { get; set; }

        public override Delegate Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value as Action<T1, T2>;
            }
        }

        public override void DoAction()
        {
            action.Invoke(arg1, arg2);
        }
  

        public TimerInfo()
        { }

        public TimerInfo<T1, T2> Fill(uint _id, float _delay, float _dur, int _count)
        {
            id = _id;
            curDelay = _delay;
            dur = _dur;
            durCount = _count;
            return this;
        }
        public override void Clear()
        {
            base.Clear();
        }
    }

    public class TimerInfo<T1, T2, T3> : AbsTimer
    {
        private Action<T1, T2, T3> action;
        public T1 arg1 { get; set; }
        public T2 arg2 { get; set; }
        public T3 arg3 { get; set; }

        public override Delegate Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value as Action<T1, T2, T3>;
            }
        }

        public override void DoAction()
        {
            action.Invoke(arg1, arg2, arg3);
        }

        public TimerInfo()
        { }

        public TimerInfo<T1, T2, T3> Fill(uint _id, float _delay, float _dur, int _count)
        {
            id = _id;
            curDelay = _delay;
            dur = _dur;
            durCount = _count;
            return this;
        }
        public override void Clear()
        {
            base.Clear();
        }
    }

    public class TimerInfo<T1, T2, T3, T4> : AbsTimer
    {
        private Action<T1, T2, T3, T4> action;
        public T1 arg1 { get; set; }
        public T2 arg2 { get; set; }
        public T3 arg3 { get; set; }
        public T4 arg4 { get; set; }

        public override Delegate Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value as Action<T1, T2, T3, T4>;
            }
        }

        public override void DoAction()
        {
            action.Invoke(arg1, arg2, arg3, arg4);
        }

        public TimerInfo()
        { }

        public TimerInfo<T1, T2, T3, T4> Fill(uint _id, float _delay, float _dur, int _count)
        {
            id = _id;
            curDelay = _delay;
            dur = _dur;
            durCount = _count;
            return this;
        }


        public override void Clear()
        {
            base.Clear();
        }
    }
}