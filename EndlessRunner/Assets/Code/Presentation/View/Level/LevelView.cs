using System;
using System.Collections.Generic;
using System.Linq;
using Game.Presentation.View;
using UnityEngine;

namespace Code.Presentation.View
{

	public class LevelView : MonoBehaviour
	{
		[SerializeField]
		private LevelSectionView _sectionPrefab;
		
		[SerializeField]
		private Transform _sectionsContainer;
		
		[SerializeField]
		private uint _initialSectionsCount = 2;
		
		[SerializeField]
		private uint _maxSections = 5;
		
		[SerializeField]
		private Transform _elementsContainer;
		
		[SerializeField]
		private CameraView _camera;
		
		public Transform ElementsContainer => _elementsContainer;
		public CameraView Camera => _camera;

		private List<LevelSectionView> _sections = new ();

		private void Awake()
		{
			for (var i = 0; i < _initialSectionsCount; i++) {
				GenerateSection();
			}
		}

		public void GenerateSection()
		{
			var newSection = Instantiate(_sectionPrefab, _sectionsContainer);

			if (_sections.Count > 0) {
				var lastSection = _sections.Last();
				var pos = lastSection.EndPoint.position - newSection.StartPoint.localPosition;
				newSection.transform.position = pos;
			}
			
			_sections.Add(newSection);

			TryKeepMaxSectionCount();
		}

		private void TryKeepMaxSectionCount()
		{
			if (_sections.Count <= _maxSections)
				return;

			var removeCount = _sections.Count - _maxSections;

			for (var i = 0; i < removeCount; i++) {
				RemoveSection();
			}
		}

		private void RemoveSection()
		{
			var sectionToRemove = _sections[0];
			_sections.RemoveAt(0);
			
			Destroy(sectionToRemove);
		}

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