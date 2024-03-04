namespace Game.Engine
{

	public class Element
	{
		public readonly StatsRegistry Stats = new ();

		public readonly uint Id;

		public Element(uint id) => Id = id;
	}

}