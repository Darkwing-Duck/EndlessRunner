using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Presentation
{

	public class SuperFlyState : HeroState
	{
		private static readonly int _superFly = Animator.StringToHash("SuperFly");
		private static readonly int _land = Animator.StringToHash("Landing");
		
		public SuperFlyState(HeroPresenter hero) : base(hero) { }

		public override void OnActivate()
		{
			Hero.View.SetGravityActive(false);
			Hero.View.Animator.SetTrigger(_superFly);
			Hero.View.transform.DOMoveY(2f, 0.3f);
		}

		public override void OnDeactivate(Action callback)
		{
			Hero.View.Animator.SetTrigger(_land);
			Hero.View.SetGravityActive(true);
			Hero.View.transform.DOMoveY(0f, 0.5f)
			    .OnComplete(() => {
				    callback?.Invoke();
			    });
			
		}
	}

}