namespace Game.Engine
{

	/// <summary>
	/// Game specific stat types 
	/// </summary>
	public class GameStat
	{
		/// <summary>
		/// Health stat can be used to store current element health.
		/// </summary>
		public class Health : Stat { public Health(float baseValue) : base(baseValue) { } }
		
		/// <summary>
		/// Stores actual element speed
		/// </summary>
		public class Speed : Stat { public Speed(float baseValue) : base(baseValue) { } }
	}

}