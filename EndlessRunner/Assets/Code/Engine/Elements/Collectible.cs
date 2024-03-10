namespace Game.Engine
{

	/// <summary>
	/// Describes collectible gameplay element.
	/// Means that this element can be collected by other element.
	/// </summary>
	public class Collectible : Element
	{
		public Collectible(uint id, uint configId) : base(id, configId) { }
	}

}