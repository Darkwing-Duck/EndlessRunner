using System.Linq;
using Game.Configs;
using UnityEngine;

namespace Game.Engine
{

	public class GenerateRandomCollectible : ICommand
	{ }

	public class Executor : CommandExecutor<GenerateRandomCollectible>
	{
		public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

		public override CmdResult Execute(GenerateRandomCollectible command)
		{
			var collectibleConfigs = Configs.Collectibles.GetAll().ToArray();
			var randomIndex = Random.Range(0, collectibleConfigs.Count());
			var randomConfig = collectibleConfigs[randomIndex];
			
			CommandCenter.Enqueue(new CreateCollectibleCommand(randomConfig.Id));

			return CmdResult.Ok;
		}
	}
}