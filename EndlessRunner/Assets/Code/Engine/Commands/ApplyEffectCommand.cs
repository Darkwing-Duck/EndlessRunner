using System;
using Game.Configs;

namespace Game.Engine
{

	public class ApplyEffectCommand : ICommand
	{
		public readonly uint OwnerUid;
		public readonly uint TargetUid;
		public readonly EffectConfig Effect;

		public ApplyEffectCommand(uint ownerUid, uint targetUid, EffectConfig effect)
		{
			OwnerUid = ownerUid;
			TargetUid = targetUid;
			Effect = effect;
		}
		
		public class Executor : CommandExecutor<ApplyEffectCommand>
		{
			public Executor(World world, CommandCenter commandCenter, ConfigsRegistry configs) : base(world, commandCenter, configs) { }

			public override CmdResult Execute(ApplyEffectCommand command)
			{
				var applyEffectCommand = CreateCommandByEffect(command);
				CommandCenter.Enqueue(applyEffectCommand);

				return CmdResult.Ok;
			}

			/// <summary>
			/// Creates effect apply command based on effect config
			/// </summary>
			private ICommand CreateCommandByEffect(ApplyEffectCommand command) =>
				command.Effect.Action switch {
					// add speed modifier depend on the sign of value
					EffectAction.ModifySpeed => command.Effect.FloatValue > 0
						? new AddStatModifierCommand<GameStat.Speed>(command.TargetUid, new StatModifier.Add(command.Effect.FloatValue, command.OwnerUid))
						: new AddStatModifierCommand<GameStat.Speed>(command.TargetUid, new StatModifier.Sub(Math.Abs(command.Effect.FloatValue), command.OwnerUid)),

					EffectAction.AddStatus => new AddStatusCommand(command.TargetUid, command.Effect.IdValue),
					EffectAction.RemoveStatus => new RemoveStatusCommand(command.TargetUid, command.Effect.IdValue),
					EffectAction.SetState => new SetHeroStateCommand(command.TargetUid, command.Effect.StateValue),
					_ => throw new ArgumentOutOfRangeException()
				};
		}
	}

}