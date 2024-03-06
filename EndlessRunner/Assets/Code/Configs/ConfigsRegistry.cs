

using System.Threading.Tasks;

namespace Game.Configs
{

	public class ConfigsRegistry
	{
		public readonly RegistryOf<HeroConfig> Heroes;
		public readonly RegistryOf<CollectibleConfig> Collectibles;

		public ConfigsRegistry(IConfigsProvider provider)
		{
			Heroes = new RegistryOf<HeroConfig>(provider);
			Collectibles = new RegistryOf<CollectibleConfig>(provider);
		}

		public async Task InitializeAsync()
		{
			await Heroes.InitializeAsync();
			await Collectibles.InitializeAsync();
		}
	}

}