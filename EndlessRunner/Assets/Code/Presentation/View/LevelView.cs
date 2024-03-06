using System;
using UnityEngine;

namespace Code.Presentation.View
{

	public class LevelView : MonoBehaviour
	{
		[SerializeField]
		private Transform _elementsContainer;
		
		[SerializeField]
		private CameraView _camera;
		
		public Transform ElementsContainer => _elementsContainer;
		public CameraView Camera => _camera;

		private void OnValidate()
		{
			if (_camera is null) {
				_camera = gameObject.GetComponentInChildren<CameraView>();
			}

			if (_camera is null) {
				throw new NullReferenceException($"Can't find level camera in level '{name}'.");
			}
		}
	}

}