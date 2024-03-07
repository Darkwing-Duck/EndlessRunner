namespace Game.Engine
{

	public interface IExecutorRegistry
	{
		void RegisterReaction<T>(IEngineReactionOn<T> reaction) where T : CmdResult;
		void UnregisterReaction<T>(IEngineReactionOn<T> reaction) where T : CmdResult;
	}

}