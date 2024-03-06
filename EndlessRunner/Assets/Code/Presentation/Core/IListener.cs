using Game.Engine;

namespace Game.Presentation
{

	public interface IListener<TCmdResult> where TCmdResult : ICmdResult
	{
		void On(TCmdResult cmdResult);
	}

}