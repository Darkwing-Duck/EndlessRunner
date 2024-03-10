namespace Game.Engine
{

	/// <summary>
	/// Reaction interface used to forward the command result to
	/// </summary>
	public interface IEngineReactionOn<in T> where T : CmdResult
	{
		void ReactOn(T cmdResult);
	}

}