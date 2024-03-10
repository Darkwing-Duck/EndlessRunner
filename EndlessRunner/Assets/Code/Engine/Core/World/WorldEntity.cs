namespace Game.Engine
{

	/// <summary>
	/// Basic type of world entity.
	/// </summary>
	public class WorldEntity
	{
		public readonly uint Uid;
		public WorldEntity(uint uid) => Uid = uid;

		internal virtual void Update() { }
	}

}