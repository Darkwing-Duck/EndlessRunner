using Game.Configs;

namespace Game.Engine
{

	public class RemoveStatusCommand : ICommand
	{
		public readonly uint TargetUid;
		public readonly uint StatusUid;

		public RemoveStatusCommand(uint targetUid, uint statusUid)
		{
			TargetUid = targetUid;
			StatusUid = statusUid;
		}
		
		public class Result : CmdResult
		{
			public uint TargetUid { get; internal set; }
			public uint RemovedStatusUid { get; internal set; }
			public uint RemovedStatusConfigId { get; internal set; }
		}
		
		public class Executor : CommandExecutor<RemoveStatusCommand>
		{
			public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

			public override CmdResult Execute(RemoveStatusCommand command)
			{
				if (!World.Elements.TryFind(command.TargetUid, out var target)) {
					return CmdResult.FailedWith($"Target element '{command.TargetUid}' is not in the world.");
				}
				
				if (!target.Statuses.TryGet(command.StatusUid, out var status)) {
					return CmdResult.FailedWith($"Can't find status '{command.StatusUid}' on element '{command.TargetUid}' is not in the world.");
				}
				
				target.Statuses.Remove(command.StatusUid);

				var statusConfig = Configs.Statuses.Get(status.ConfigId);
				
				foreach (var effect in statusConfig.Effects) {
					switch (effect.Action) {
						case EffectAction.ModifySpeed:
							var stat = target.Stats.Find<GameStat.Speed>();
							var modifiers = stat.FindAllModifiersBy(status.Uid);

							foreach (var modifier in modifiers) {
								CommandCenter.Enqueue(new RemoveStatModifierCommand<GameStat.Speed>(target.Uid, modifier));
							}
							break;
					}
				}
				
				return new Result {
					Status = CmdStatus.Ok,
					TargetUid = target.Uid,
					RemovedStatusUid = command.StatusUid,
					RemovedStatusConfigId = status.ConfigId
				};
			}
		}
	}

}