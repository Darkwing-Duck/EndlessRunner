using System.Collections.Generic;
using Game.Common;
using Game.Configs;
using Game.Engine;
using Game.Presentation;

namespace Game.Infrastructure
{

	public class GameInstance : IUpdatable
	{
		private readonly GameEngine _engine;
		private readonly PresentationRoot _presentation;
		private readonly List<PlayerInput> _players;
		private readonly ConfigsRegistry _configsRegistry;

		public IEngineInput Input => _engine;
		public PresentationRoot Presentation => _presentation;

		private GameInstance(ConfigsRegistry configsRegistry)
		{
			_configsRegistry = configsRegistry;
			_engine = new GameEngine(configsRegistry);
			_presentation = new PresentationRoot(_engine.World, configsRegistry, _engine);

			RegisterEngineReactions(_presentation);
		}

		public void Update()
		{
			_engine.Update();
			_presentation.Update();
		}

		public static GameInstance Create(ConfigsRegistry configsRegistry) => new (configsRegistry);
		
		public GameInstance WithLevel(uint levelConfigId)
		{
			var levelConfig = _configsRegistry.Levels.Get(levelConfigId);
			var levelPresenter = new LevelPresenter(levelConfig);
			_presentation.Add(levelPresenter);
			return this;
		}
		
		public GameInstance WithHero(uint heroConfigId, uint forPlayerId)
		{
			var heroConfig = _configsRegistry.Heroes.Get(heroConfigId);
			_engine.Push(new CreateHeroCommand(heroConfigId, heroConfig.Speed, forPlayerId));
			return this;
		}
		
		/// <summary>
		/// Registers reactions for each command result type that should be listened in presenters
		/// This is the only way to get notification about any events from the engine.
		/// </summary>
		private void RegisterEngineReactions(IListenersProvider listenersProvider)
		{
			_engine.RegisterReaction(new EngineReactionOn<CreateHeroCommand.Result>(listenersProvider));
			_engine.RegisterReaction(new EngineReactionOn<CreateCollectibleCommand.Result>(listenersProvider));
			_engine.RegisterReaction(new EngineReactionOn<DestroyCollectibleCommand.Result>(listenersProvider));
			_engine.RegisterReaction(new EngineReactionOn<AddStatusCommand.Result>(listenersProvider));
			_engine.RegisterReaction(new EngineReactionOn<RemoveStatusCommand.Result>(listenersProvider));
			_engine.RegisterReaction(new EngineReactionOn<SetHeroStateCommand.Result>(listenersProvider));
			_engine.RegisterReaction(new EngineReactionOn<HeroJumpCommand.Result>(listenersProvider));
			_engine.RegisterReaction(new EngineReactionOn<HeroLandCommand.Result>(listenersProvider));
			_engine.RegisterReaction(new EngineReactionOn<AddStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>(listenersProvider));
			_engine.RegisterReaction(new EngineReactionOn<RemoveStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>(listenersProvider));
		}
	}

}