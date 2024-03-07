using Game.Configs;
using Game.Engine;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Presentation
{

	public abstract class ElementPresenter<TConfig, TModel, TView> : PresenterWithModel<TModel, TView>
		where TConfig : GameplayElementConfig
		where TModel : Element
		where TView : MonoBehaviour
	{
		protected TConfig Config { get; set; }

		public ElementPresenter(TModel model) : base(model) { }

		internal override void Configure()
		{
			Config = InitializeConfig();
		}

		internal override void Build()
		{
			base.Build();

			var elementUid = View.gameObject.AddComponent<ElementUidRef>();
			elementUid.Value = Model.Uid;
		}

		protected abstract TConfig InitializeConfig();

		protected override string InitializeViewKey() => $"{Config.ViewKey}";
	}

}