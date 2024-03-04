namespace Game.Engine
{

	public abstract class CommandExecutor<TCommand> where TCommand : ICommand
	{
		protected readonly World World;
		protected readonly CommandCenter CommandCenter;

		protected CommandExecutor(World world, CommandCenter commandCenter)
		{
			World = world;
			CommandCenter = commandCenter;
		}

		public abstract CmdResult Execute(TCommand command);
	}
	
	public enum CmdStatus
	{
		Failed = 0, Ok = 1 
	}

	public class CmdResult
	{
		public static readonly CmdResult Ok = new (CmdStatus.Ok);
		public static readonly CmdResult Failed = new (CmdStatus.Failed, "Failed");
		
		public readonly CmdStatus Status;
		public readonly string ErrorMessage;

		public CmdResult(CmdStatus status, string error = null)
		{
			Status = status;
			ErrorMessage = error;
		}

		public static CmdResult FailedWith(string reason) => new (CmdStatus.Failed, reason);
	}

}