using System;
using Game.Common;

namespace Game.Presentation
{

	/// <summary>
	/// Base hero visual state
	/// </summary>
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