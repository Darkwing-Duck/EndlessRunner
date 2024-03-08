using Game.Engine;

namespace Game.Presentation
{

	public interface IListener<TCmdResult> where TCmdResult : CmdResult
	{
		void On(TCmdResult cmdResult);
	}

}