namespace Game.Engine
{

	public interface IEngineInput
	{
		void Push(ICommand command);
	}

}