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
		, IListener<DestroyCollectibleCommand.Result>
		, IUpdatable
	{
		public LevelPresenter(LevelConfig model) : base(model, model) { }
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

		/// <summary>
		/// React on result of CrateHeroCommand
		/// </summary>
		public void On(CreateHeroCommand.Result cmdResult)
		{
			var heroModel = World.Elements.Find<Hero>(cmdResult.HeroUid);
			var heroPresenter = new HeroPresenter(heroModel);
			
			// add presenter to presentation root to be displayed on screen
			Root.Add(heroPresenter);
			
			SetActiveHero(heroPresenter);
		}

		/// <summary>
		/// React on result of CreateCollectibleCommand
		/// </summary>
		public void On(CreateCollectibleCommand.Result cmdResult)
		{
			var collectibleModel = World.Elements.Find<Collectible>(cmdResult.CollectibleUid);
			var collectiblePresenter = new CollectiblePresenter(collectibleModel);
			
			// add presenter to presentation root to be displayed on screen
			Root.Add(collectiblePresenter);

			var pos = new Vector3(5f * cmdResult.CollectibleUid, 1.5f, 0f);
			collectiblePresenter.View.transform.localPosition = pos;
		}

		/// <summary>
		/// React on result of DestroyCollectibleCommand
		/// </summary>
		public void On(DestroyCollectibleCommand.Result cmdResult)
		{
			// Removes presenter by key
			// Any of ElementPresenter use element uid as a key
			Root.Remove(cmdResult.CollectibleUid);
		}
	}

}