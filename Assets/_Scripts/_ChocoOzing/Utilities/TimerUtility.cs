using System;
using System.Collections;
using UnityEngine;

namespace Ozing.Utilities
{
	public class TimerUtility
	{
		public event Action OnTimerDone;

		private float startTime;
		private float duration;
		private float targetTime;

		private bool isActive;

		public TimerUtility(float duration)
		{
			this.duration = duration;
		}

		public void StartTimer()
		{
			startTime = Time.time;
			targetTime = startTime + duration;
			isActive = true;
		}

		public void StopTimer()
		{
			isActive = false;
		}

		public void Tick()
		{
			if (!isActive) return;
            if (Time.time >= targetTime)
			{
				Debug.Log("AttackCount Reset!!");
				OnTimerDone?.Invoke();
				StopTimer();
			}
        }
	}
}