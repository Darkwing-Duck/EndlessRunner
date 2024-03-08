namespace Game.Engine
{

	public interface IStatModifier
	{
		object Group { get; }

		float Modify(float statValue);
	}

}