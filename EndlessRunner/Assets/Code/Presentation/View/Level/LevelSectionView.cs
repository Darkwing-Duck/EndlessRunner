using UnityEngine;

namespace Game.Presentation.View
{

	/// <summary>
	/// Level section view.
	/// Endless level is constructing from these sections.
	/// </summary>
	public class LevelSectionView : MonoBehaviour
	{
		[SerializeField]
		private Transform _startPoint;
		[SerializeField]
		private Transform _endPoint;

		public Transform StartPoint => _startPoint;
		public Transform EndPoint => _endPoint;
	}

}