namespace Game.Engine
{

	/// <summary>
	/// Describes an math operation above applied to stat base value
	/// </summary>
	public interface IStatModifier
	{
		/// <summary>
		/// Group of modifier.
		/// To simplify remove all modifiers by group;
		/// </summary>
		object Group { get; }

		/// <summary>
		/// Method describing the math operation. 
		/// </summary>
		/// <param name="statValue">Stat's base value</param>
		float Modify(float statValue);
	}

}