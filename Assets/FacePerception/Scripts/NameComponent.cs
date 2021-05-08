using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameComponent : MonoBehaviour {
	private string _text;
	public string text {
		get { return _text; }
		set {
			if (_text == value) return;
			_text = value;
			Text.text = _text;
		}
	}

	private bool _active = true;
	public bool active {
		get { return _active; }
		set {
			if (_active == value) return;
			_active = value;
			ChangeColor();
		}
	}

	private bool _hover = false;
	public bool hover {
		get { return _hover; }
		private set {
			if (_hover == value) return;
			_hover = value;
			ChangeColor();
		}
	}

	public TextMesh Text;
	public KMSelectable Selectable;

	private void Start() {
		Selectable.OnHighlight += () => hover = true;
		Selectable.OnHighlightEnded += () => hover = false;
	}

	private void ChangeColor() {
		Text.color = _active ? (_hover ? Color.green : Color.yellow) : Color.gray * Color.yellow;
	}
}
