namespace Game.Engine
{

	public interface IEngineReactionOn<in T> where T : CmdResult
	{
		void ReactOn(T cmdResult);
	}

}