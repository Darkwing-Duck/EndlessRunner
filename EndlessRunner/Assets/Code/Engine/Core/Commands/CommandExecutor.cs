using System;

namespace Game.Engine
{

	public interface ICommandExecutor<out TCommand> where TCommand : ICommand
	{
		CmdResult Execute(ICommand command);
	}
	
	public abstract class CommandExecutor<TCommand> : ICommandExecutor<TCommand> where TCommand : ICommand
	{
		protected readonly World World;
		protected readonly CommandCenter CommandCenter;

		protected CommandExecutor(World world, CommandCenter commandCenter)
		{
			World = world;
			CommandCenter = commandCenter;
		}

		CmdResult ICommandExecutor<TCommand>.Execute(ICommand command)
		{
			if (command is not TCommand castedCommand) {
				throw new ArgumentException("Wrong command type has been passed.");
			}

			return Execute(castedCommand);
		}

		public abstract CmdResult Execute(TCommand command);
	}
	
	public enum CmdStatus
	{
		Failed = 0, Ok = 1 
	}

	public interface ICmdResult
	{
		
	}

	public class CmdResult : ICmdResult
	{
		public static readonly CmdResult Ok = new CmdResult { Status = CmdStatus.Ok };
		public static readonly CmdResult Failed = new CmdResult { Status = CmdStatus.Failed, ErrorMessage = "Failed"};

		public CmdStatus Status { get; internal set; }
		public string ErrorMessage { get; internal set; }

		public static CmdResult FailedWith(string reason) => new CmdResult {
			Status = CmdStatus.Failed, 
			ErrorMessage = reason
		};
	}

}