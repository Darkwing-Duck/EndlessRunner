using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Game.Configs;
using UnityEngine.AddressableAssets;

namespace Game.Infrastructure
{

	public class AddressableConfigsProvider : IConfigsProvider
	{
		private const string _regexPattern = @"\/(.+.asset)";
		
		public async Task<TConfig[]> GetAllInPathAsync<TConfig>(string path) where TConfig : IGameConfig
		{
			path = path.Replace("/", "\\/");
			var regexPattern = $"{path}{_regexPattern}";
			var regex = new Regex(regexPattern);
			var keys = GetConfigsKeys(regex);

			var handle = Addressables.LoadAssetsAsync<TConfig>((IEnumerable)keys, asset => {
			}, Addressables.MergeMode.Union);

			var result = await handle.Task;
			return result.ToArray();
		}
		
		private string[] GetConfigsKeys(Regex regex)
		{
			var resultKeys = new List<string>();

			foreach (var locator in Addressables.ResourceLocators) {
				foreach (var key in locator.Keys) {
					var typedKey = key.ToString();

					if (regex.IsMatch(typedKey))
						resultKeys.Add(typedKey);
				}
			}

			return resultKeys.ToArray();
		}
	}

}