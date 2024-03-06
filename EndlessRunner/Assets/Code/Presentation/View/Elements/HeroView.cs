using System;
using UnityEngine;

namespace Game.Presentation.View
{
	
	public class HeroView : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		public void SetSpeed(float speed)
		{
			_rigidbody.velocity = Vector3.right * speed;
		}

		private void OnValidate()
		{
			if (_rigidbody != null) 
				return;

			_rigidbody = gameObject.GetComponentInChildren<Rigidbody>();
		}
	}

}