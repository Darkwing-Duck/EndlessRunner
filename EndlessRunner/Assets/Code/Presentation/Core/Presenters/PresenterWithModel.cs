using UnityEngine;

namespace Game.Presentation
{

	public abstract class PresenterWithModel<TModel, TView> : Presenter<TView> 
		where TView : MonoBehaviour
	{
		protected readonly TModel Model;
		
		public PresenterWithModel(TModel model) : base(model) 
		{
			Model = model;
		}
	}

}