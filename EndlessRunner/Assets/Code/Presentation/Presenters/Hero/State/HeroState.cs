using System;
using Game.Common;

namespace Game.Presentation
{

	public abstract class HeroState : IUpdatable
	{
		protected HeroPresenter Hero;

		protected HeroState(HeroPresenter hero) => Hero = hero;

		public abstract void OnActivate();
		public abstract void OnDeactivate(Action callback);

		public virtual void Update()
		{ }
	}

}