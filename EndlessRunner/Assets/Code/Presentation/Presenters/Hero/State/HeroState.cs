using Game.Common;
using Game.Presentation.View;

namespace Game.Presentation
{

	public abstract class HeroState : IUpdatable
	{
		protected HeroPresenter Hero;

		protected HeroState(HeroPresenter hero) => Hero = hero;

		public abstract void OnActivate();
		public abstract void OnDeactivate();

		public virtual void Update()
		{ }
	}

}