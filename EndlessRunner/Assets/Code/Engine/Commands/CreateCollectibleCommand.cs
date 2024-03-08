using Game.Configs;

namespace Game.Engine
{

	public class CreateCollectibleCommand : ICommand
	{
		public readonly uint ConfigId;

		public CreateCollectibleCommand(uint configId)
		{
			ConfigId = configId;
		}
		
		public class Result : CmdResult
		{
			public uint CollectibleUid { get; internal set; }
		}
		
		public class Executor : CommandExecutor<CreateCollectibleCommand>
		{
			public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

			public override CmdResult Execute(CreateCollectibleCommand command)
			{
				var elementId = World.Elements.GenerateNextUid();
				var collectible = new Collectible(elementId, command.ConfigId);
				
				World.Elements.Add(collectible);

				return new Result {
					Status = CmdStatus.Ok,
					CollectibleUid = collectible.Uid
				};
			}
		}
	}

}