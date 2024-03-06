using System.Threading.Tasks;

namespace Game.Configs
{

	public interface IConfigsProvider
	{
		Task<TConfig[]> GetAllInPathAsync<TConfig>(string path) where TConfig : IGameConfig;
	}

}