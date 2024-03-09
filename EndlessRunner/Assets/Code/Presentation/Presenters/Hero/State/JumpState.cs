using System;
using UnityEngine;

namespace Game.Presentation
{

	public class JumpState : HeroState
	{
		private static readonly int _jump = Animator.StringToHash("Jump");
		
		public JumpState(HeroPresenter hero) : base(hero) { }

		public override void OnActivate()
		{
			Hero.View.Jump();
			Hero.View.Animator.SetTrigger(_jump);
		}

		public override void OnDeactivate(Action callback)
		{
			callback();
		}
	}

}