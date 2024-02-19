using System;
using System.Collections;
using UnityEngine;

namespace Ozing.Assets._Scripts.Weapons
{
	public class AnimationEventHandler : MonoBehaviour
	{
		public event Action OnFinish;

		private void AnimationFinishedTrigger() => OnFinish?.Invoke();
		
		
	}
}