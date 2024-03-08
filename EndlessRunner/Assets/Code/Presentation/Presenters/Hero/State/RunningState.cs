using Game.Engine;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Presentation
{

	public class RunningState : HeroState
	{
		private static readonly int _run = Animator.StringToHash("Run");
		private static readonly int _speed = Animator.StringToHash("Speed");

		public RunningState(HeroPresenter hero) : base(hero) { }

		public override void OnActivate()
		{
			Hero.View.Animator.SetTrigger(_run);
		}

		public override void OnDeactivate()
		{
			
		}

		public override void Update()
		{
			var speedStat = Hero.Model.Stats.Find<GameStat.Speed>();
			Hero.View.Animator.SetFloat(_speed, speedStat.GetValue());
		}
	}

}