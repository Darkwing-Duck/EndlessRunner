namespace Game.Engine
{

	public interface IEngineOutputGateway
	{
		void Flush(ICommand command);
	}

}