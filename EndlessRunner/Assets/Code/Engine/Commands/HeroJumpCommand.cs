using Game.Configs;

namespace Game.Engine
{

	public class HeroJumpCommand : ICommand
	{
		public readonly uint PlayerId;

		public HeroJumpCommand(uint playerId) => PlayerId = playerId;
		
		public class Result : CmdResult
		{
			public uint HeroUid { get; internal set; }
		}
		
		public class Executor : CommandExecutor<HeroJumpCommand>
		{
			public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

			public override CmdResult Execute(HeroJumpCommand command)
			{
				if (!World.Elements.TryFind<Hero>(e => e.PlayerId == command.PlayerId, out var hero)) {
					return CmdResult.FailedWith($"Can't find hero for player '{command.PlayerId}'.");
				}
				
				if (hero.State != HeroStateType.Run) {
					return CmdResult.FailedWith("Hero can jump only while running.");
				}
				
				if (hero.State == HeroStateType.Jump) {
					return CmdResult.FailedWith("Hero is already in jump.");
				}
				
				hero.State = HeroStateType.Jump;
				
				return new Result {
					Status = CmdStatus.Ok,
					HeroUid = hero.Uid
				};
			}
		}
	}

}