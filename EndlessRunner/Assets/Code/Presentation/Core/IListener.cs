using Game.Engine;

namespace Game.Presentation
{

	/// <summary>
	/// Make presenters able to listen Engine's command results. 
	/// </summary>
	public interface IListener<TCmdResult> where TCmdResult : CmdResult
	{
		void On(TCmdResult cmdResult);
	}

}