namespace Game.Engine
{

	public class RemoveStatModifierCommand<T> : ICommand where T : Stat
	{
		public readonly uint ElementId;
		public readonly StatModifier Modifier;

		public RemoveStatModifierCommand(uint elementId, StatModifier modifier)
		{
			ElementId = elementId;
			Modifier = modifier;
		}
		
		public class Executor : CommandExecutor<RemoveStatModifierCommand<T>>
		{
			public Executor(World world, CommandCenter commandCenter) : base(world, commandCenter) { }

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
				
				return CmdResult.Ok;
			}
		}
	}

}