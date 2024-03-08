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
		
		public GameInstance()
		{
			// _engine = new GameEngine(configsRegistry);
			// _presentation = new PresentationRoot(_engine.World, configsRegistry, _engine);
		}

		public void Update()
		{
			
		}

		public static GameBuilder Create() => new ();
	}

	public class GameBuilder
	{
		private uint _levelConfigId;
		private ConfigsRegistry _configsRegistry;
		private List<PlayerInput> _players = new();
		private Dictionary<uint, uint> _playersToHeroConfigIdMap = new();
		
		public GameBuilder WithConfigs(ConfigsRegistry configs)
		{
			_configsRegistry = configs;
			return this;
		}
		
		public GameBuilder WithLevel(uint levelConfigId)
		{
			_levelConfigId = levelConfigId;
			return this;
		}
		
		public GameBuilder WithPlayer(PlayerInput playerInput, uint heroConfigId)
		{
			_players.Add(playerInput);
			_playersToHeroConfigIdMap.Add(playerInput.PlayerId, heroConfigId);
			return this;
		}

		public GameInstance Build()
		{
			var engine = new GameEngine(_configsRegistry);
			var presentationRoot = new PresentationRoot(engine.World, _configsRegistry, engine);

			// create level
			var levelConfig = _configsRegistry.Levels.Get(_levelConfigId);
			var levelPresenter = new LevelPresenter(levelConfig);
			presentationRoot.Add(levelPresenter);
			
			// create players
			foreach (var playerInfo in _players) {
				var heroConfigId = _playersToHeroConfigIdMap[playerInfo.PlayerId];
				var heroConfig = _configsRegistry.Heroes.Get(heroConfigId);
				engine.Push(new CreateHeroCommand(heroConfigId, heroConfig.Speed, playerInfo.PlayerId));
			}

			return new GameInstance();
		}
	}

}