using Game.Configs;

namespace Game.Engine
{
	
	/// <summary>
	/// Adds stat modifier 'Modifier' of stat 'T' to the element 'ElementUid'
	/// </summary>
	public class AddStatModifierCommand<T> : ICommand where T : Stat
	{
		public readonly uint ElementUid;
		public readonly StatModifier Modifier;

		public AddStatModifierCommand(uint elementUid, StatModifier modifier)
		{
			ElementUid = elementUid;
			Modifier = modifier;
		}
		
		public class Result<TStat> : CmdResult where TStat : Stat
		{
			public uint TargetUid { get; internal set; }
		}
		
		public class Executor : CommandExecutor<AddStatModifierCommand<T>>
		{
			public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

			public override CmdResult Execute(AddStatModifierCommand<T> command)
			{
				if (!World.Elements.TryFind(command.ElementUid, out var element)) {
					return CmdResult.FailedWith($"Can't find element '{command.ElementUid}'.");
				}
				
				if (!element.Stats.TryFind<T>(out var targetStat)) {
					var type = typeof(T);
					return CmdResult.FailedWith($"Can't find stat '{type.Name}' on element '{command.ElementUid}'.");
				}
				
				targetStat.AddModifier(command.Modifier);

				return new Result<T> {
					Status = CmdStatus.Ok,
					TargetUid = element.Uid
				};
			}
		}
	}
}