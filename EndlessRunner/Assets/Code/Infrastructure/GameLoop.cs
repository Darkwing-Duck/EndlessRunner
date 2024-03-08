using System.Collections.Generic;
using Game.Configs;
using Game.Engine;
using Game.Presentation;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Infrastructure
{

	public class GameLoop : MonoBehaviour
	{
		private PresentationRoot _presentation;
		private GameEngine _engine;
		private List<PlayerInput> _players = new ();
		
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
			
			_engine = new GameEngine(configsRegistry);
			_presentation = new PresentationRoot(_engine.World, configsRegistry, _engine);

			RegisterEngineReactions(_engine, _presentation);
			
			var levelConfig = configsRegistry.Levels.Get(2);
			var levelPresenter = new LevelPresenter(levelConfig);
			_presentation.Add(levelPresenter);

			var heroGranny = configsRegistry.Heroes.Get(1);
			var heroOrtiz = configsRegistry.Heroes.Get(2);

			var heroActionsMapper = new HeroActionsMapper(_engine);
			var localPlayer = new LocalPlayerInput(1, heroActionsMapper);
			var aiPlayer = new AIPlayerInput(2, heroActionsMapper);
			
			_players.Add(localPlayer);
			_players.Add(aiPlayer);

			// _engine.Push(new CreateHeroCommand(heroOrtiz.Id, heroOrtiz.Speed, aiPlayer.PlayerId));
			_engine.Push(new CreateHeroCommand(heroGranny.Id, heroGranny.Speed, localPlayer.PlayerId));

			// var heroActionsMapper = new HeroActionsMapper()
			
			// GameInstance.Create()
			//             .WithLevel(2)
			//             .WithPlayer(new (1,), 1);
		}

		/// <summary>
		/// Registers reactions for each command result type that should be listened in presenters 
		/// </summary>
		private void RegisterEngineReactions(GameEngine engine, IListenersProvider listenersProvider)
		{
			engine.RegisterReaction(new EngineReactionOn<CreateHeroCommand.Result>(listenersProvider));
			engine.RegisterReaction(new EngineReactionOn<CreateCollectibleCommand.Result>(listenersProvider));
			engine.RegisterReaction(new EngineReactionOn<DestroyCollectibleCommand.Result>(listenersProvider));
			engine.RegisterReaction(new EngineReactionOn<AddStatusCommand.Result>(listenersProvider));
			engine.RegisterReaction(new EngineReactionOn<RemoveStatusCommand.Result>(listenersProvider));
			engine.RegisterReaction(new EngineReactionOn<SetHeroStateCommand.Result>(listenersProvider));
			engine.RegisterReaction(new EngineReactionOn<HeroJumpCommand.Result>(listenersProvider));
			engine.RegisterReaction(new EngineReactionOn<AddStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>(listenersProvider));
			engine.RegisterReaction(new EngineReactionOn<RemoveStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>(listenersProvider));
		}
		
		private void Update()
		{
			_engine?.Update();
			_presentation?.Update();
			
			_players.ForEach(p => p.Update());
		}
	}

}