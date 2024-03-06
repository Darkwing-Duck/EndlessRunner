using Game.Configs;
using Game.Engine;
using Game.Presentation;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Infrastructure
{

	public class GameLoop : MonoBehaviour
	{
		private async void Start()
		{
			// initialize Addressables
			var handle =  Addressables.InitializeAsync();
			await handle.Task;
			
			// initialize configs registry
			var configsProvider = new AddressableConfigsProvider();
			var configsRegistry = new ConfigsRegistry(configsProvider);
			await configsRegistry.InitializeAsync();
			
			var engine = new GameEngine();
			var presentation = new PresentationRoot(engine.World, configsRegistry);

			RegisterEngineReactions(engine, presentation);
			
			var levelPresenter = new LevelPresenter(engine.World);
			presentation.Add(levelPresenter);

			var heroConfig = configsRegistry.Heroes.Get(1);
			var collectibleConfig = configsRegistry.Collectibles.Get(1);
			
			engine.Apply(new CreateHeroCommand(heroConfig.Id, heroConfig.Speed));
			engine.Apply(new CreateCollectibleCommand(collectibleConfig.Id));
		}

		private void RegisterEngineReactions(GameEngine engine, IListenersProvider listenersProvider)
		{
			engine.RegisterReaction(new ReactionOnCreateHero(listenersProvider));
			engine.RegisterReaction(new ReactionOnCreateCollectible(listenersProvider));
		}
		
		private void Update()
		{
			
		}
	}

}