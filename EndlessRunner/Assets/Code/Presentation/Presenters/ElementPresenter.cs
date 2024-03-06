using Game.Configs;
using Game.Engine;
using UnityEngine;

namespace Game.Presentation
{

	public abstract class ElementPresenter<TConfig, TModel, TView> : PresenterWithModel<TModel, TView>
		where TConfig : ElementConfig
		where TModel : Element
		where TView : MonoBehaviour
	{
		protected TConfig Config { get; set; }

		public ElementPresenter(TModel model) : base(model) { }

		internal override void Configure()
		{
			Config = InitializeConfig();
		}

		protected abstract TConfig InitializeConfig();

		protected override string InitializeViewKey() => $"{Config.ViewKey}";
	}

}