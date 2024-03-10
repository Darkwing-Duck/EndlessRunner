using System;
using System.Collections.Generic;
using Game.Common;
using Game.Configs;
using Game.Engine;
using Game.Presentation.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Presentation
{

	/// <summary>
	/// Displays level.
	/// </summary>
	public class LevelPresenter : PresenterWithModel<LevelConfig, LevelView>
		, IListener<CreateHeroCommand.Result>
		, IListener<CreateCollectibleCommand.Result>
		, IListener<DestroyCollectibleCommand.Result>
		, IUpdatable
	{
		// collectible item spawns every 15 seconds
		private const float CollectibleSpawnRate = 15f; // sec
		
		// collectible item spawns at camera.position + 15 
		private const float CollectibleSpawnDistance = 15f;
		private float _elapsedTime;
		
		// hardcoded 2 collectible Y position to be spawned at
		private List<float> _collectibleYPos = new List<float> {
			1.5f, 3f
		};
		
		public LevelPresenter(LevelConfig model) : base(model, model) { }
		
		protected override string InitializeViewKey() => Model.ViewKey;
		protected override string InitializeViewGroup() => "Level";
		
		// hero to be followed by the camera 
		private HeroPresenter _followTarget;
		
		// target Z position of camera to be animated at by the time
		private float _targetZPosition;

		protected override void OnActivate()
		{
			View.name = "Level";
		}

		public void Update()
		{
			if (_followTarget is null)
				return;

			// generates endless level
			GenerateLevel();

			// calculates new camera position by Z depend on hero speed to make some effects of zoom in/out
			var speedStat = _followTarget.Model.Stats.Find<GameStat.Speed>();
			var targetHeroSpeed = speedStat.GetValue() - 3f;
			var newCameraPosition = _followTarget.View.transform.position;
			_targetZPosition = Mathf.Lerp(-4f,2f , 1f - targetHeroSpeed / 5f);
			newCameraPosition.z = Mathf.Lerp(View.Camera.transform.position.z, _targetZPosition, Time.deltaTime * 7f);

			// update camera position
			View.Camera.transform.position = newCameraPosition;

			TryToGenerateCollectible();
		}

		// Determines when and generates collectibles
		private void TryToGenerateCollectible()
		{
			_elapsedTime += Time.deltaTime;

			if (_elapsedTime >= CollectibleSpawnRate) {
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

			// If it's the local player we need camera to follow him
			if (cmdResult.PlayerId == 1) {
				SetActiveHero(heroPresenter);
			}
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
			var newCollectiblePosition = cameraPosition.x + CollectibleSpawnDistance;
			var posYIndex = Random.Range(0, _collectibleYPos.Count);
			var positionY = _collectibleYPos[posYIndex];

			var pos = new Vector3(newCollectiblePosition, positionY, 0f);
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