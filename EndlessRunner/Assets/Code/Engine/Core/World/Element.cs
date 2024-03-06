namespace Game.Engine
{

	public class Element
	{
		public readonly StatsRegistry Stats = new ();

		public readonly uint Uid;
		public readonly uint ConfigId;

		public Element(uint id, uint configId)
		{
			Uid = id;
			ConfigId = configId;
		}
	}

}