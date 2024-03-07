using UnityEngine;

namespace Game.Presentation
{

	public abstract class PresenterWithModel<TModel, TView> : Presenter<TView> 
		where TView : MonoBehaviour
	{
		public TModel Model { get; protected set; }

		public PresenterWithModel(object key, TModel model) : base(key) 
		{
			Model = model;
		}
	}

}