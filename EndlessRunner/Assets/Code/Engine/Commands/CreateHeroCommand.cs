using Game.Configs;

namespace Game.Engine
{

	public class CreateHeroCommand : ICommand
	{
		public readonly uint ConfigId;
		public readonly uint PlayerId;
		public readonly float Speed;

		public CreateHeroCommand(uint configId, float speed, uint playerId)
		{
			ConfigId = configId;
			Speed = speed;
			PlayerId = playerId;
		}

		public class Result : CmdResult
		{
			public uint HeroUid { get; internal set; }
			public uint PlayerId { get; internal set; }
		}
		
		public class Executor : CommandExecutor<CreateHeroCommand>
		{
			public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

			public override CmdResult Execute(CreateHeroCommand command)
			{
				var elementId = World.Elements.GenerateNextUid();
				var hero = new Hero(elementId, command.ConfigId, command.Speed, command.PlayerId);
				
				World.Elements.Add(hero);

				return new Result {
					Status = CmdStatus.Ok,
					HeroUid = hero.Uid,
					PlayerId = command.PlayerId
				};
			}
		}
	}

}