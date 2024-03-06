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
			// initialize Addressables
			var handle =  Addressables.InitializeAsync();
			await handle.Task;
			
			// initialize configs registry
			var configsProvider = new AddressableConfigsProvider();
			var configsRegistry = new ConfigsRegistry(configsProvider);
			await configsRegistry.InitializeAsync();
			
			_engine = new GameEngine();
			_presentation = new PresentationRoot(_engine.World, configsRegistry);

			RegisterEngineReactions(_engine, _presentation);
			
			var levelConfig = configsRegistry.Levels.Get(2);
			var levelPresenter = new LevelPresenter(levelConfig);
			_presentation.Add(levelPresenter);

			var heroConfig = configsRegistry.Heroes.Get(1);
			var collectibleConfig = configsRegistry.Collectibles.Get(1);
			
			_engine.Apply(new CreateHeroCommand(heroConfig.Id, heroConfig.Speed));
			_engine.Apply(new CreateCollectibleCommand(collectibleConfig.Id));
		}

		private void RegisterEngineReactions(GameEngine engine, IListenersProvider listenersProvider)
		{
			engine.RegisterReaction(new ReactionOnCreateHero(listenersProvider));
			engine.RegisterReaction(new ReactionOnCreateCollectible(listenersProvider));
		}
		
		private void Update()
		{
			_presentation?.Update();
		}
	}

}