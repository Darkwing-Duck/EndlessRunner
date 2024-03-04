namespace Game.Engine
{
	
	public class AddStatModifierCommand<T> : ICommand where T : Stat
	{
		public readonly uint ElementId;
		public readonly StatModifier Modifier;

		public AddStatModifierCommand(uint elementId, StatModifier modifier)
		{
			ElementId = elementId;
			Modifier = modifier;
		}
		
		public class Executor : CommandExecutor<AddStatModifierCommand<T>>
		{
			public Executor(World world, CommandCenter commandCenter) : base(world, commandCenter) { }

			public override CmdResult Execute(AddStatModifierCommand<T> command)
			{
				if (!World.Elements.TryFind(command.ElementId, out var element)) {
					return CmdResult.FailedWith($"Can't find element '{command.ElementId}'.");
				}
				
				if (!element.Stats.TryFind<T>(out var targetStat)) {
					var type = typeof(T);
					return CmdResult.FailedWith($"Can't find stat '{type.Name}' on element '{command.ElementId}'.");
				}
				
				targetStat.AddModifier(command.Modifier);
				
				return CmdResult.Ok;
			}
		}
	}
}