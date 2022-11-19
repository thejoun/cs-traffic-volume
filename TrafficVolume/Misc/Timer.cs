using System;

namespace TrafficVolume.Misc
{
    public static class LoopingTimerExtensions
    {
        public static LoopingTimer WithLoopCount(this LoopingTimer timer, int loopCount)
        {
            timer.SetLoopLimit(loopCount);
            return timer;
        }
    }

    public static class TimerExtensions
    {
        public static Timer WithGoalTime(this Timer timer, float newGoalTime)
        {
            timer.SetGoalTime(newGoalTime);
            return timer;
        }

        public static Timer WithCallback(this Timer timer, Action callback)
        {
            timer.SetCallback(callback);
            return timer;
        }

        public static Timer Run(this Timer timer)
        {
            timer.SetRunning(true);
            return timer;
        }

        public static LoopingTimer Looping(this Timer timer)
        {
            return new LoopingTimer(timer);
        }
    }
    
    public class LoopingTimer : Timer
    {
        private bool m_hasLoopLimit;
        private int m_loopLimit;
        
        protected float m_newGoalTime;
        protected bool m_hasNewGoalTime;

        protected Action m_newCallback;
        protected bool m_hasNewCallback;
        
        private int m_loopsFinished;
        
        public int LoopsFinished => m_loopsFinished;
            
        public LoopingTimer(Timer wrappedTimer) : base(wrappedTimer)
        {
            m_hasLoopLimit = false;
            m_loopLimit = 0;
        }
            
        public void SetLoopLimit(int loopLimit)
        {
            m_hasLoopLimit = true;
            m_loopsFinished = 0;
            m_loopLimit = loopLimit;
        }

        public override void SetGoalTime(float newGoalTime)
        {
            m_newGoalTime = newGoalTime;
            m_hasNewGoalTime = true;
        }

        public override void SetCallback(Action action)
        {
            m_newCallback = action;
            m_hasNewCallback = true;
        }

        protected override void OnGoalReached()
        {
            m_lapTime -= m_goalTime;
            
            // Callback?.Invoke();
            RaiseCallback();
            
            m_loopsFinished++;
                    
            if (m_hasNewGoalTime)
            {
                m_goalTime = m_newGoalTime;

                m_newGoalTime = default;
                m_hasNewGoalTime = false;
            }

            if (m_hasNewCallback)
            {
                // Callback = m_newCallback;
                base.SetCallback(m_newCallback);

                m_newCallback = null;
                m_hasNewCallback = false;
            }

            if (m_hasLoopLimit && m_loopsFinished >= m_loopLimit)
            {
                m_isRunning = false;
            }
        }
    }
    
    public class Timer
    {
        private float m_totalTime;
        protected float m_lapTime;
        protected float m_goalTime = float.MaxValue;
        
        protected bool m_isRunning;

        public float TotalTime => m_totalTime;
        public float LapTime => m_lapTime;
        public bool IsRunning => m_isRunning;
        public float Progress => m_lapTime / m_goalTime;
        public bool GoalTimeReached => m_lapTime >= m_goalTime;
        public bool IsDitchable => GoalTimeReached && !m_isRunning;

        public event Action Callback;

        public Timer()
        {
            
        }
        
        public Timer(Timer original)
        {
            m_totalTime = original.m_totalTime;
            m_lapTime = original.m_lapTime;
            m_goalTime = original.m_goalTime;
            
            // m_isLooping = original.m_isLooping;
            m_isRunning = original.m_isRunning;
        }

        protected void RaiseCallback()
        {
            Callback?.Invoke();
        }
        
        public virtual void SetGoalTime(float newGoalTime)
        {
            m_goalTime = newGoalTime;
        }
        
        public virtual void SetCallback(Action action)
        {
            Callback = action;
        }

        public void SetRunning(bool running)
        {
            m_isRunning = running;
        }

        public void Stop()
        {
            m_isRunning = false;
        }
        
        public void Reset()
        {
            m_totalTime = 0f;
            m_lapTime = 0f;
            
            SetRunning(true);
        }

        public void ClearCallback()
        {
            Callback = null;
        }

        public void SetTime(float time)
        {
            m_totalTime = time;
            m_lapTime = time;
        }

        public void Advance(float deltaTime)
        {
            if (!m_isRunning)
            {
                return;
            }
            
            m_totalTime += deltaTime;
            m_lapTime += deltaTime;

            while (GoalTimeReached && m_isRunning)
            {
                OnGoalReached();
            }
        }

        protected virtual void OnGoalReached()
        {
            Callback?.Invoke();
            m_isRunning = false;
        }
    }
}