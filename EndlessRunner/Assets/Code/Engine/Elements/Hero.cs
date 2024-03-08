using Game.Configs;

namespace Game.Engine
{

	public class Hero : Element
	{
		public HeroStateType State { get; internal set; } = HeroStateType.Run;
		public bool IsInJump { get; internal set; }
		public uint PlayerId { get; internal set; }

		public Hero(uint uid, uint configId, float speed, uint playerId) : base(uid, configId)
		{
			PlayerId = playerId;
			Stats.Register<GameStat.Speed>(speed, 5f, 12f);
		}
	}

	

}