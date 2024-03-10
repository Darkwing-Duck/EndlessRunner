using UnityEngine;

namespace Game.Presentation
{

	/// <summary>
	/// Base class for presenters with specific model type.
	/// </summary>
	/// <typeparam name="TModel">Model corresponds to the presenter</typeparam>
	/// <typeparam name="TView">View corresponds to the presenter</typeparam>
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