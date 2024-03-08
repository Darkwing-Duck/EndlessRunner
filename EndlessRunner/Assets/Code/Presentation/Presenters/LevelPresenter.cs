using System;
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
		private float _collectibleSpawnRate = 15f; // sec
		private float _elapsedTime;
		
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

			GenerateLevel();

			// update camera position
			View.Camera.transform.position = _followTarget.View.transform.position;

			TryToGenerateCollectible();
		}

		private void TryToGenerateCollectible()
		{
			_elapsedTime += Time.deltaTime;

			if (_elapsedTime >= _collectibleSpawnRate) {
				_elapsedTime = 0f;
				EngineInput.Push(new GenerateRandomCollectible());
			}
		}

		/// <summary>
		/// Generates new level section if distance between hero
		/// and last level section's endPoint less than constant value
		/// </summary>
		private void GenerateLevel()
		{
			var lastLevelSectionStartPosition = View.LastSection.StartPoint.transform.position;
			var lastLevelSectionEndPosition = View.LastSection.EndPoint.transform.position;
			var heroDistanceToEndOfLastSection = Math.Abs(_followTarget.View.transform.position.x - lastLevelSectionEndPosition.x);
			var triggerDistance = lastLevelSectionEndPosition.x - lastLevelSectionStartPosition.x;

			if (heroDistanceToEndOfLastSection <= triggerDistance) {
				View.GenerateSection();
			}
		}
		
		/// <summary>
		/// Sets hero to follow by the camera
		/// </summary>
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

			var cameraPosition = View.Camera.transform.position;
			var newCollectiblePosition = cameraPosition.x + 20f;

			var pos = new Vector3(newCollectiblePosition, 1.5f, 0f);
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