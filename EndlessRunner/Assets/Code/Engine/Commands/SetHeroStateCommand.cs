using Game.Configs;

namespace Game.Engine
{

	/// <summary>
	/// Set hero state command.
	/// Changes state of hero 'TargetUid' to 'State'
	/// </summary>
	public class SetHeroStateCommand : ICommand
	{
		public readonly uint TargetUid;
		public readonly HeroStateType State;

		public SetHeroStateCommand(uint targetUid, HeroStateType state)
		{
			TargetUid = targetUid;
			State = state;
		}
		
		/// <summary>
		/// Set hero state command result
		/// </summary>
		public class Result : CmdResult
		{
			public uint TargetUid { get; internal set; }
			public HeroStateType State { get; internal set; }
		}
		
		/// <summary>
		/// Set hero state command executor
		/// Changes state of specified hero
		/// </summary>
		public class Executor : CommandExecutor<SetHeroStateCommand>
		{
			public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

			public override CmdResult Execute(SetHeroStateCommand command)
			{
				if (!World.Elements.TryFind<Hero>(command.TargetUid, out var hero)) {
					return CmdResult.FailedWith($"Can't find hero '{command.TargetUid}' in the world.");
				}

				hero.State = command.State;

				return new Result {
					Status = CmdStatus.Ok,
					TargetUid = hero.Uid,
					State = command.State
				};
			}
		}
	}

}