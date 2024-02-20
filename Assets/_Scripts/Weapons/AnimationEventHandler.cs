using System;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons
{
	public class AnimationEventHandler : MonoBehaviour
	{
		public event Action OnFinish;

		private void AnimationFinishedTrigger() => OnFinish?.Invoke();
		
		
	}
}