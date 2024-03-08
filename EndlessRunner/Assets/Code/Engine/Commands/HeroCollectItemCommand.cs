using Game.Configs;

namespace Game.Engine
{

	public class HeroCollectItemCommand : ICommand
	{
		public readonly uint HeroUid;
		public readonly uint CollectibleUid;

		public HeroCollectItemCommand(uint heroUid, uint collectibleUid)
		{
			HeroUid = heroUid;
			CollectibleUid = collectibleUid;
		}
		
		public class Executor : CommandExecutor<HeroCollectItemCommand>
		{
			public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

			public override CmdResult Execute(HeroCollectItemCommand command)
			{
				// destroy collectible
				CommandCenter.Enqueue(new DestroyCollectibleCommand(command.CollectibleUid));
				
				if (!World.Elements.TryFind<Hero>(command.HeroUid, out _)) {
					return CmdResult.FailedWith($"Hero '{command.HeroUid}' doesn't exist in the world.");
				}
				
				// apply collectible effects
				if (World.Elements.TryFind<Collectible>(command.CollectibleUid, out var collectible)) {
					var collectibleConfig = Configs.Collectibles.Get(collectible.ConfigId);

					foreach (var effect in collectibleConfig.Effects) {
						var effectCommand = new ApplyEffectCommand(collectible.Uid,command.HeroUid, effect);
						CommandCenter.Enqueue(effectCommand);
					}
				} else {
					return CmdResult.FailedWith($"Collectible '{command.CollectibleUid}' doesn't exist in the world.");
				}
				

				return CmdResult.Ok;
			}
		}
	}

}