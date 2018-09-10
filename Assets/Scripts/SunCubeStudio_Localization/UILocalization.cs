// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SunCubeStudio.Localization
{
	public class UILocalization : MonoBehaviour
	{
		public string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
				this.Localize();
			}
		}

		private void Start()
		{
			this.Initialize();
		}

		private void Initialize()
		{
			LocalizationService instance = LocalizationService.Instance;
			instance.OnChangeLocalization = (Action)Delegate.Combine(instance.OnChangeLocalization, new Action(this.OnChangeLocalization));
			this.UiText = base.gameObject.GetComponent<Text>();
			this.MeshText = base.gameObject.GetComponent<TextMesh>();
			this.OnChangeLocalization();
		}

		private void OnChangeLocalization()
		{
			this.Localize();
		}

		private void Localize()
		{
			this.SetTextValue(LocalizationService.Instance.GetTextByKey(this._key));
		}

		private void SetTextValue(string text)
		{
			text = this.ParceText(text);
			if (this.UiText != null)
			{
				this.UiText.text = text;
			}
			if (this.MeshText != null)
			{
				this.MeshText.text = text;
			}
			if (text == "[EMPTY]" || text == string.Format("[ERROR KEY {0}]", this._key))
			{
				if (this.UiText != null)
				{
					this.UiText.color = Color.red;
				}
				if (this.MeshText != null)
				{
					this.MeshText.color = Color.red;
				}
			}
		}

		private string ParceText(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			return text.Replace("\\n", Environment.NewLine);
		}

		private void OnDestroy()
		{
			if (LocalizationService.Instance != null)
			{
				LocalizationService instance = LocalizationService.Instance;
				instance.OnChangeLocalization = (Action)Delegate.Remove(instance.OnChangeLocalization, new Action(this.OnChangeLocalization));
			}
		}

		public bool IsHasOutputHelper()
		{
			this.UiText = base.gameObject.GetComponent<Text>();
			this.MeshText = base.gameObject.GetComponent<TextMesh>();
			return this.UiText != null || this.MeshText != null;
		}

		public string _key;

		private Text UiText;

		private TextMesh MeshText;
	}
}
