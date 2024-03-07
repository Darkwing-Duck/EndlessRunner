using System;
using UnityEngine;

namespace Game.Presentation.View
{
	
	[RequireComponent(typeof(Rigidbody))]
	public class HeroView : ElementView
	{
		public event Action<ElementUidRef> OnCollideWith; 
		
		[SerializeField]
		private Rigidbody _rigidbody;

		private float _speed;

		public void SetSpeed(float speed)
		{
			_speed = speed;
			_rigidbody.velocity = Vector3.right * speed;
		}

		private void FixedUpdate()
		{
			var moveVector = Vector3.right * _speed * Time.fixedDeltaTime; 
			_rigidbody.MovePosition(_rigidbody.position + moveVector);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<ElementUidRef>(out var component)) {
				OnCollideWith?.Invoke(component);
			}
		}

		private void OnDestroy()
		{
			OnCollideWith = null;
		}

		private void OnValidate()
		{
			if (_rigidbody != null)
				return;

			_rigidbody = gameObject.GetComponent<Rigidbody>();
		}
	}

}