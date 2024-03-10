using Game.Configs;

namespace Game.Engine
{
	
	public class HeroLandCommand : ICommand
	{
		public readonly uint HeroUid;

		public HeroLandCommand(uint heroUid) => HeroUid = heroUid;
		
		/// <summary>
		/// Hero land command result
		/// </summary>
		public class Result : CmdResult
		{
			public uint HeroUid { get; internal set; }
		}
		
		/// <summary>
		/// Hero land command executor
		/// Sets the state of hero to Run
		/// </summary>
		public class Executor : CommandExecutor<HeroLandCommand>
		{
			public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

			public override CmdResult Execute(HeroLandCommand command)
			{
				if (!World.Elements.TryFind<Hero>(command.HeroUid, out var hero)) {
					return CmdResult.FailedWith($"Can't find hero for player '{command.HeroUid}'.");
				}

				hero.State = HeroStateType.Run;
				
				return new Result {
					Status = CmdStatus.Ok,
					HeroUid = hero.Uid
				};
			}
		}
	}

}