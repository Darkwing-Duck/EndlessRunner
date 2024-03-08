using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Configs
{

	public class ConfigRegistryOf<T> where T : IGameConfig
	{
		private readonly Dictionary<uint, T> _configs;
		private readonly IConfigsProvider _configsProvider;

		public ConfigRegistryOf(IConfigsProvider provider)
		{
			_configs = new Dictionary<uint, T>();
			_configsProvider = provider;
		}

		private void Add(T config) { _configs.Add(config.Id, config); }

		public T Get(uint id) => _configs[id];
		public T GetFirst(Func<T, bool> filter) => _configs.Values.FirstOrDefault(filter);

		public bool TryGet(uint id, out T value) => _configs.TryGetValue(id, out value);

		public async Task InitializeAsync()
		{
			var configType = typeof(T);
			
			// compute final specific configs root path like 'Configs/Hero'
			var path = Path.Combine("Configs", configType.Name.Replace("Config", ""));
			var result = await _configsProvider.GetAllInPathAsync<T>(path);
			
			foreach (var config in result) {
				Add(config);
			}
		}

		public IEnumerable<T> GetAll()
		{
			foreach (var value in _configs.Values) {
				yield return value;
			}
		}
	}

}