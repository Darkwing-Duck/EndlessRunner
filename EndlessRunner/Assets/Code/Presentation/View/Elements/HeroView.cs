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
		
		[SerializeField]
		private Animator _animator;

		public Animator Animator => _animator;

		private float _speed;
		private float _targetSpeed;
		private bool _jumpAction;

		private const float JumpForce = 100f;

		public void SetSpeed(float speed)
		{
			_targetSpeed = speed;
			_speed = speed;
			_rigidbody.velocity = Vector3.right * _speed;
		}
		
		public void Jump()
		{
			_jumpAction = true;
		}
		
		public void SetGravityActive(bool value)
		{
			_rigidbody.useGravity = value;
		}

		private void Update()
		{
			_speed = Mathf.Lerp(_speed, _targetSpeed, Time.deltaTime * 5f);
		}

		private void FixedUpdate()
		{
			if (_jumpAction) {
				_jumpAction = false;
				_rigidbody.AddForce(Vector3.up * JumpForce);
			}
			
			var position = _rigidbody.position;
			position.x += _speed * Time.fixedDeltaTime;
			_rigidbody.position = position;
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