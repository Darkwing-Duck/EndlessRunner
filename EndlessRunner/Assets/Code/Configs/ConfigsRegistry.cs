

using System.Threading.Tasks;

namespace Game.Configs
{

	public class ConfigsRegistry
	{
		public readonly RegistryOf<HeroConfig> Heroes;
		public readonly RegistryOf<CollectibleConfig> Collectibles;
		public readonly RegistryOf<LevelConfig> Levels;

		public ConfigsRegistry(IConfigsProvider provider)
		{
			Heroes = new RegistryOf<HeroConfig>(provider);
			Collectibles = new RegistryOf<CollectibleConfig>(provider);
			Levels = new RegistryOf<LevelConfig>(provider);
		}

		public async Task InitializeAsync()
		{
			await Heroes.InitializeAsync();
			await Collectibles.InitializeAsync();
			await Levels.InitializeAsync();
		}
	}

}