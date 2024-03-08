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

			var heroConfig = configsRegistry.Heroes.Get(1);
			
			_engine.Push(new CreateHeroCommand(heroConfig.Id, heroConfig.Speed));
			
			_engine.Push(new CreateCollectibleCommand(1));
			// _engine.Push(new CreateCollectibleCommand(2));
			// _engine.Push(new CreateCollectibleCommand(3));
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
			engine.RegisterReaction(new EngineReactionOn<AddStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>(listenersProvider));
			engine.RegisterReaction(new EngineReactionOn<RemoveStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>(listenersProvider));
		}
		
		private void Update()
		{
			_engine?.Update();
			_presentation?.Update();
		}
	}

}