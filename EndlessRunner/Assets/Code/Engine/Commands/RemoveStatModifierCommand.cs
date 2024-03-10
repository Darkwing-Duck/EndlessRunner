using Game.Configs;

namespace Game.Engine
{
	
	/// <summary>
	/// Removes modifier of specified type of stat 
	/// </summary>
	public class RemoveStatModifierCommand<T> : ICommand where T : Stat
	{
		public readonly uint ElementId;
		public readonly IStatModifier Modifier;

		public RemoveStatModifierCommand(uint elementId, IStatModifier modifier)
		{
			ElementId = elementId;
			Modifier = modifier;
		}
		
		/// <summary>
		/// Remove stat modifier command result
		/// </summary>
		/// <typeparam name="TStat"></typeparam>
		public class Result<TStat> : CmdResult where TStat : Stat
		{
			public uint TargetUid { get; internal set; }
		}
		
		/// <summary>
		/// Remove stat modifier command executor
		/// Removes modifier of specified type of stat 
		/// </summary>
		public class Executor : CommandExecutor<RemoveStatModifierCommand<T>>
		{
			public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

			public override CmdResult Execute(RemoveStatModifierCommand<T> command)
			{
				if (!World.Elements.TryFind(command.ElementId, out var element)) {
					return CmdResult.FailedWith($"Can't find element '{command.ElementId}'.");
				}
				
				if (!element.Stats.TryFind<T>(out var targetStat)) {
					var type = typeof(T);
					return CmdResult.FailedWith($"Can't find stat '{type.Name}' on element '{command.ElementId}'.");
				}
				
				targetStat.RemoveModifier(command.Modifier);

				return new Result<T> {
					Status = CmdStatus.Ok,
					TargetUid = element.Uid
				};
			}
		}
	}

}