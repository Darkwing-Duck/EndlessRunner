using System.Collections.Generic;
using Game.Configs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Infrastructure
{

	/// <summary>
	/// Game entry point
	/// </summary>
	public class ConstructionRoot : MonoBehaviour
	{
		private readonly List<PlayerInput> _players = new ();
		private GameInstance _game;

		private async void Awake()
		{
			Application.targetFrameRate = 60;
			
			// initialize Addressables
			var handle =  Addressables.InitializeAsync();
			await handle.Task;
			
			// initialize configs registry
			var configsProvider = new AddressableConfigsProvider();
			var configsRegistry = new ConfigsRegistry(configsProvider);
			await configsRegistry.InitializeAsync();
			
			// define players
			var localPlayerId = 1u;
			var aiPlayerId = 2u;

			// define game configs to create the game
			var levelConfigId = 2u;
			var grannyHeroId = 1u;
			var ortizHeroId = 2u;

			// create the game
			_game = GameInstance.Create(configsRegistry)
			                    .WithLevel(levelConfigId)
			                    .WithHero(grannyHeroId, localPlayerId)
			                    .WithHero(ortizHeroId, aiPlayerId);
			
			// create inputs for 2 players: local and AI 
			var heroActionsMapper = new HeroActionsMapper(_game.Input);
			var localPlayer = new LocalPlayerInput(localPlayerId, heroActionsMapper);
			var aiPlayer = new AIPlayerInput(aiPlayerId, heroActionsMapper);
			
			_players.Add(localPlayer);
			_players.Add(aiPlayer);
		}
		
		private void Update()
		{
			// update player inputs
			_players.ForEach(p => p.Update());
			
			// update game
			_game?.Update();
		}
	}

}