using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Presentation.View
{

	/// <summary>
	/// Level view.
	/// Encapsulates endless level generating logic.
	/// But in real project the logic have to be moved to presenter.
	/// </summary>
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

		private readonly uint _initializePoolItems = 5;
		private Stack<LevelSectionView> _pool = new();
		
		public Transform ElementsContainer => _elementsContainer;
		public CameraView Camera => _camera;

		private List<LevelSectionView> _sections = new ();

		public LevelSectionView LastSection => _sections.Last();

		private Transform _poolContainer;

		private void Awake()
		{
			// Generate initial level sections
			for (var i = 0; i < _initialSectionsCount; i++) {
				GenerateSection();
			}
		}

		private void Start()
		{
			_poolContainer = new GameObject("Pool").transform;
			_poolContainer.SetParent(transform);
			
			StartCoroutine(InitializePool());
		}

		/// <summary>
		/// Initialize pool with initial sections count
		/// </summary>
		private IEnumerator InitializePool()
		{
			for (var i = 0; i < _initializePoolItems; i++) {
				var newSection = Instantiate(_sectionPrefab, _poolContainer);
				newSection.gameObject.SetActive(false);
				_pool.Push(newSection);
				
				yield return null;
			}
		}

		/// <summary>
		/// Generates new level section.
		/// Takes it from pool or instantiates new
		/// </summary>
		public void GenerateSection()
		{
			LevelSectionView newSection;

			if (_pool.Count > 0) {
				newSection = _pool.Pop();
				newSection.transform.SetParent(_sectionsContainer);
				newSection.gameObject.SetActive(true);
			} else {
				newSection = Instantiate(_sectionPrefab, _sectionsContainer);
			}

			if (_sections.Count > 0) {
				var lastSection = LastSection;
				var pos = lastSection.EndPoint.position - newSection.StartPoint.localPosition;
				newSection.transform.position = pos;
			} else {
				newSection.transform.position = Vector3.zero;
			}
			
			_sections.Add(newSection);
			TryKeepMaxSectionCount();
		}

		/// <summary>
		/// Determines if we need to remove some sections or not
		/// </summary>
		private void TryKeepMaxSectionCount()
		{
			if (_sections.Count <= _maxSections)
				return;

			var removeCount = _sections.Count - _maxSections;

			for (var i = 0; i < removeCount; i++) {
				RemoveSection();
			}
		}

		/// <summary>
		/// Removes sections from the beginning of level and put them to pool.
		/// </summary>
		private void RemoveSection()
		{
			var sectionToRemove = _sections[0];
			_sections.RemoveAt(0);
			
			sectionToRemove.gameObject.SetActive(false);
			sectionToRemove.transform.SetParent(_poolContainer);
			_pool.Push(sectionToRemove);
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