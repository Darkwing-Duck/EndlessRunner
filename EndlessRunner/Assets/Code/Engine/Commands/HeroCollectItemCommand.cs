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
			public Executor(World world, CommandCenter commandCenter) : base(world, commandCenter) { }

			public override CmdResult Execute(HeroCollectItemCommand command)
			{
				// destroy collectible
				CommandCenter.Enqueue(new DestroyCollectibleCommand(command.CollectibleUid));
				
				// apply collectible effects
				// TODO:

				return CmdResult.Ok;
			}
		}
	}

}