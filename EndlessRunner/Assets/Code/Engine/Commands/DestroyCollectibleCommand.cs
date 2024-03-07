namespace Game.Engine
{

	public class DestroyCollectibleCommand : ICommand
	{
		public readonly uint CollectibleUid;

		public DestroyCollectibleCommand(uint collectibleUid)
		{
			CollectibleUid = collectibleUid;
		}

		public class Result : CmdResult
		{
			public uint CollectibleUid { get; internal set; }
		}
		
		public class Executor : CommandExecutor<DestroyCollectibleCommand>
		{
			public Executor(World world, CommandCenter commandCenter) : base(world, commandCenter) { }

			public override CmdResult Execute(DestroyCollectibleCommand command)
			{
				if (!World.Elements.TryFind(command.CollectibleUid, out var element)) {
					return CmdResult.FailedWith($"Can't find collectible '{command.CollectibleUid}'.");
				}
				
				World.Elements.Remove(command.CollectibleUid);

				return new Result {
					Status = CmdStatus.Ok,
					CollectibleUid = command.CollectibleUid
				};
			}
		}
	}

}