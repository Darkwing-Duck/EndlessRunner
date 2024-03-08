using UnityEngine;

namespace Game.Presentation.View
{

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