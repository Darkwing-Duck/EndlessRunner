namespace Game.Engine
{

	/// <summary>
	/// Other app layers like Presentation have access to engine only by this interface
	/// And can call only one method to push the command.
	/// </summary>
	public interface IEngineInput
	{
		void Push(ICommand command);
	}

}