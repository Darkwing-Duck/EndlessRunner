namespace Game.Engine
{

	/// <summary>
	/// Basic type of world entity containing config id
	/// </summary>
	public class WorldEntityWithConfig : WorldEntity
	{
		public readonly uint ConfigId;
		public WorldEntityWithConfig(uint uid, uint configId) : base(uid) => ConfigId = configId;
	}

}