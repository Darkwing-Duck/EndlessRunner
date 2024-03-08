using Game.Engine.Core;

namespace Game.Engine
{

	public class Element : WorldEntityWithConfig
	{
		public readonly StatsRegistry Stats = new ();
		public readonly StatusRegistry Statuses = new ();

		public Element(uint uid, uint configId) : base(uid, configId) { }
	}

}