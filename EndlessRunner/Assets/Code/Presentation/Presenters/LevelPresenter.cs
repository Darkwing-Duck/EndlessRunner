using Code.Presentation.View;
using Game.Engine;

namespace Game.Presentation
{

	public class LevelPresenter : PresenterWithModel<World, LevelView>
		, IListener<CreateHeroCommand.Result>
		, IListener<CreateCollectibleCommand.Result>
	{
		public LevelPresenter(World model) : base(model) { }
		protected override string InitializeViewKey() => "Default";
		protected override string InitializeViewGroup() => "Level";

		protected override void OnActivate()
		{
			View.name = "Level";
		}

		public void On(CreateHeroCommand.Result cmdResult)
		{
			var heroModel = Model.Elements.Find<Hero>(cmdResult.HeroUid);
			var heroPresenter = new HeroPresenter(heroModel);
			
			// add presenter to presentation root to be displayed on screen
			Root.Add(heroPresenter);
		}

		public void On(CreateCollectibleCommand.Result cmdResult)
		{
			var collectibleModel = Model.Elements.Find<Collectible>(cmdResult.CollectibleUid);
			var collectiblePresenter = new CollectiblePresenter(collectibleModel);
			
			// add presenter to presentation root to be displayed on screen
			Root.Add(collectiblePresenter);
		}
	}

}