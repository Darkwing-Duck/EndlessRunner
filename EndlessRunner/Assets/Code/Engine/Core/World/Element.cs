using Game.Engine.Core;

namespace Game.Engine
{

	/// <summary>
	/// Describes dynamic world entity taking a part in gameplay
	/// </summary>
	public class Element : WorldEntityWithConfig
	{
		/// <summary>
		/// Supported stats
		/// </summary>
		public readonly StatsRegistry Stats = new ();
		
		/// <summary>
		/// Applied statuses
		/// </summary>
		public readonly StatusRegistry Statuses = new ();

		public Element(uint uid, uint configId) : base(uid, configId) { }

		internal override void Update()
		{
			base.Update();

			foreach (var status in Statuses.GetAll()) {
				status.Update();
			}
		}
	}

}