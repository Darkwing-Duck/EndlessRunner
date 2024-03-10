using Game.Configs;

namespace Game.Engine
{

	/// <summary>
	/// Describes hero gameplay element.
	/// </summary>
	public class Hero : Element
	{
		/// <summary>
		/// Current hero state.
		/// </summary>
		public HeroStateType State { get; internal set; } = HeroStateType.Run;
		
		/// <summary>
		/// Id of a player to which the hero corresponds
		/// </summary>
		public uint PlayerId { get; internal set; }

		public Hero(uint uid, uint configId, float speed, uint playerId) : base(uid, configId)
		{
			PlayerId = playerId;
			Stats.Register<GameStat.Speed>(speed, 5f, 12f);
		}
	}

	

}