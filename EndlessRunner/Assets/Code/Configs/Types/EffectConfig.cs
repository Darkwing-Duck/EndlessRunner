using System;
using UnityEngine;

namespace Game.Configs
{

	public enum EffectAction
	{
		ModifySpeed = 0,
		AddStatus,
		RemoveStatus,
		SetState
	}
	
	public enum EffectValueType
	{
		Id = 0, Int, Float, State
	}
	
	public enum HeroStateType
	{
		Run, SuperFly, Jump
	}
	
	[Serializable]
	public class EffectConfig
	{
		[SerializeField]
		private EffectValueType _valueType;
		
		[SerializeField]
		private EffectAction _action;
		
		[Space]
		[SerializeField]
		private uint _idValue;
		
		[SerializeField]
		private float _floatValue;
		
		[SerializeField]
		private int _intValue;
		
		[SerializeField]
		private HeroStateType _stateValue;

		public EffectValueType ValueType => _valueType;
		public EffectAction Action => _action;

		public float FloatValue => _floatValue;
		public int IntValue => _intValue;
		public uint IdValue => _idValue;
		public HeroStateType StateValue => _stateValue;
	}

}