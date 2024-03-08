using Game.Configs;
using Game.Engine.Core;

namespace Game.Engine
{

	public class AddStatusCommand : ICommand
	{
		public readonly uint TargetUid;
		public readonly uint ConfigId;

		public AddStatusCommand(uint targetUid, uint configId)
		{
			TargetUid = targetUid;
			ConfigId = configId;
		}
		
		public class Result : CmdResult
		{
			public uint AddedStatusUid { get; internal set; }
		}
		
		public class Executor : CommandExecutor<AddStatusCommand>
		{
			public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

			public override CmdResult Execute(AddStatusCommand command)
			{
				if (!World.Elements.TryFind(command.TargetUid, out var target)) {
					return CmdResult.FailedWith($"Target element '{command.TargetUid}' is not in the world.");
				}

				var statusUid = World.Elements.GenerateNextUid();
				var statusConfig = Configs.Statuses.Get(command.ConfigId);
				var status = new Status(statusUid, command.ConfigId, statusConfig.Duration);
				
				target.Statuses.Add(status);
				
				foreach (var effect in statusConfig.Effects) {
					var effectCommand = new ApplyEffectCommand(status.Uid, command.TargetUid, effect);
					CommandCenter.Enqueue(effectCommand);
				}
				
				return new Result {
					Status = CmdStatus.Ok,
					AddedStatusUid = statusUid
				};
			}
		}
	}

}