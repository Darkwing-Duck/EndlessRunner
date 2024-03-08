

using System.Threading.Tasks;

namespace Game.Configs
{

	public class ConfigsRegistry
	{
		public readonly ConfigRegistryOf<HeroConfig> Heroes;
		public readonly ConfigRegistryOf<CollectibleConfig> Collectibles;
		public readonly ConfigRegistryOf<LevelConfig> Levels;
		public readonly ConfigRegistryOf<StatusConfig> Statuses;

		public ConfigsRegistry(IConfigsProvider provider)
		{
			Heroes = new ConfigRegistryOf<HeroConfig>(provider);
			Collectibles = new ConfigRegistryOf<CollectibleConfig>(provider);
			Levels = new ConfigRegistryOf<LevelConfig>(provider);
			Statuses = new ConfigRegistryOf<StatusConfig>(provider);
		}

		public async Task InitializeAsync()
		{
			await Heroes.InitializeAsync();
			await Collectibles.InitializeAsync();
			await Levels.InitializeAsync();
			await Statuses.InitializeAsync();
		}
	}

}