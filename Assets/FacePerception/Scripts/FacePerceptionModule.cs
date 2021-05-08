using System.Linq;
using UnityEngine;

public class FacePerceptionModule : MonoBehaviour {
	private static int moduleIdCounter = 1;

	public KMSelectable Selectable;
	public KMBombModule Module;
	public KMSelectable Face;
	public TextMesh Score;

	public Renderer HairRenderer;
	public Renderer BeardRenderer;
	public Renderer AccessoriesRenderer;

	public Texture BlackWildHairSprite;
	public Texture BlackRaisedHairSprite;
	public Texture BlackCombedHairSprite;
	public Texture BlackLongHairSprite;

	public Texture BlondeWildHairSprite;
	public Texture BlondeRaisedHairSprite;
	public Texture BlondeCombedHairSprite;
	public Texture BlondeLongHairSprite;

	public Texture RedWildHairSprite;
	public Texture RedRaisedHairSprite;
	public Texture RedCombedHairSprite;
	public Texture RedLongHairSprite;

	public Texture WhiteWildHairSprite;
	public Texture WhiteRaisedHairSprite;
	public Texture WhiteCombedHairSprite;
	public Texture WhiteLongHairSprite;

	public Texture BlackMustacheSprite;
	public Texture BlondeMustacheSprite;
	public Texture RedMustacheSprite;
	public Texture WhiteMustacheSprite;

	public Texture BlackGoateeSprite;
	public Texture BlondeGoateeSprite;
	public Texture RedGoateeSprite;
	public Texture WhiteGoateeSprite;

	public Texture BlackFullBeardSprite;
	public Texture BlondeFullBeardSprite;
	public Texture RedFullBeardSprite;
	public Texture WhiteFullBeardSprite;

	public Texture RoundGlassesSprite;
	public Texture SquareGlassesSprite;
	public Texture HeadphonesSprite;

	public Texture NoneTexture;

	private int stage;
	private int moduleId;
	private string[] answer;
	private FacePerceptionData.Stage[] stages;
	private FacePerceptionData.Person[] persons;

	private void Start () {
		moduleId = moduleIdCounter++;
		Module.OnActivate += Activate;
	}

	private void Activate() {
		FacePerceptionData.Generate(out stages, out persons, out answer);
		Debug.LogFormat("[Face Perception #{0}] Persons:\n\t{1}", moduleId, persons.Select(p => p.ToString()).Join("\n\t"));
		Debug.LogFormat("[Face Perception #{0}] Stages:\n\t{1}", moduleId, stages.Select(s => string.Format("{0}: {1}", s.person.name, s.score)).Join("\n\t"));
		Debug.LogFormat("[Face Perception #{0}] Answer: {1}", moduleId, answer.Join(", "));
		RenderStage();
		Face.OnInteract += OnFacePressed;
	}

	private bool OnFacePressed() {
		if (stage == stages.Length - 1) {
			// show names
			return false;
		}
		stage++;
		RenderStage();
		return false;
	}

	private void RenderStage() {
		FacePerceptionData.Person person = stages[stage].person;
        SetTexture(HairRenderer, GetHairTexture(person.color, person.style));
        SetTexture(BeardRenderer, GetBeardTexture(person.color, person.beard));
		SetTexture(AccessoriesRenderer, GetAccessoryTexture(person.accessory));
		Score.text = stages[stage].score.ToString();
	}

	private void SetTexture(Renderer renderer, Texture texture) {
		if (texture == null) texture = NoneTexture;
        renderer.material.SetTexture("_MainTex", texture);
	}

	private Texture GetHairTexture(int color, int style) {
		return new Texture[][] {
			new Texture[] { BlackWildHairSprite, BlackRaisedHairSprite, BlackCombedHairSprite, BlackLongHairSprite },
			new Texture[] { BlondeWildHairSprite, BlondeRaisedHairSprite, BlondeCombedHairSprite, BlondeLongHairSprite },
			new Texture[] { RedWildHairSprite, RedRaisedHairSprite, RedCombedHairSprite, RedLongHairSprite },
			new Texture[] { WhiteWildHairSprite, WhiteRaisedHairSprite, WhiteCombedHairSprite, WhiteLongHairSprite },
		}[color][style];
	}

	private Texture GetBeardTexture(int color, int style) {
		if (style == 0) return null;
		return new Texture[][] {
			new Texture[] { BlackMustacheSprite, BlondeMustacheSprite, RedMustacheSprite, WhiteMustacheSprite },
			new Texture[] { BlackGoateeSprite, BlondeGoateeSprite, RedGoateeSprite, WhiteGoateeSprite },
			new Texture[] { BlackFullBeardSprite, BlondeFullBeardSprite, RedFullBeardSprite, WhiteFullBeardSprite },
		}[style - 1][color];
	}

	private Texture GetAccessoryTexture(int type) {
		return new Texture[] { null, RoundGlassesSprite, SquareGlassesSprite, HeadphonesSprite }[type];
	}
}
