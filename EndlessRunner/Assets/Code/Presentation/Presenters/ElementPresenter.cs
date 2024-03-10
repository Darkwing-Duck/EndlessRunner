using Game.Configs;
using Game.Engine;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Presentation
{

	/// <summary>
	/// Base element presenter.
	/// Uses model's uid field as a presenter key.
	/// Also have a reference to game config related to the element.
	/// </summary>
	public abstract class ElementPresenter<TConfig, TModel, TView> : PresenterWithModel<TModel, TView>
		where TConfig : GameplayElementConfig
		where TModel : Element
		where TView : MonoBehaviour
	{
		protected TConfig Config { get; set; }

		public ElementPresenter(TModel model) : base(model.Uid, model) { }

		internal override void Configure()
		{
			Config = InitializeConfig();
		}

		internal override void Build()
		{
			base.Build();

			// adds ElementUidRef component to the gameObject to simplify 
			// determine the element by the gameObject
			var elementUid = View.gameObject.AddComponent<ElementUidRef>();
			elementUid.Value = Model.Uid;
		}

		protected abstract TConfig InitializeConfig();

		/// <summary>
		/// Initializes view key to load from addressables.
		/// </summary>
		protected override string InitializeViewKey() => $"{Config.ViewKey}";
	}

}