using Code.Presentation.View;
using Game.Common;
using Game.Configs;
using Game.Engine;
using UnityEngine;

namespace Game.Presentation
{

	public class LevelPresenter : PresenterWithModel<LevelConfig, LevelView>
		, IListener<CreateHeroCommand.Result>
		, IListener<CreateCollectibleCommand.Result>
		, IUpdatable
	{
		public LevelPresenter(LevelConfig model) : base(model) { }
		protected override string InitializeViewKey() => Model.ViewKey;
		protected override string InitializeViewGroup() => "Level";
		
		private HeroPresenter _followTarget;

		protected override void OnActivate()
		{
			View.name = "Level";
		}

		public void Update()
		{
			if (_followTarget is null)
				return;

			View.Camera.transform.position = _followTarget.View.transform.position;
		}
		
		public void SetActiveHero(HeroPresenter hero)
		{
			_followTarget = hero;
		}

		public void On(CreateHeroCommand.Result cmdResult)
		{
			var heroModel = World.Elements.Find<Hero>(cmdResult.HeroUid);
			var heroPresenter = new HeroPresenter(heroModel);
			
			// add presenter to presentation root to be displayed on screen
			Root.Add(heroPresenter);
			
			SetActiveHero(heroPresenter);
		}

		public void On(CreateCollectibleCommand.Result cmdResult)
		{
			var collectibleModel = World.Elements.Find<Collectible>(cmdResult.CollectibleUid);
			var collectiblePresenter = new CollectiblePresenter(collectibleModel);
			
			// add presenter to presentation root to be displayed on screen
			Root.Add(collectiblePresenter);

			var pos = new Vector3(5f * cmdResult.CollectibleUid, 1.5f, 0f);
			collectiblePresenter.View.transform.localPosition = pos;
		}
	}

}