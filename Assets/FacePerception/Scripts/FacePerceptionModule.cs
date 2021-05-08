using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FacePerceptionModule : MonoBehaviour {
	private static int moduleIdCounter = 1;

	public KMSelectable Selectable;
	public KMBombModule Module;
	public KMSelectable Face;
	public TextMesh Score;
	public KMSelectable Reset;
	public GameObject NamesContainer;
	public GameObject FaceContainer;

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
	public NameComponent NamePrefab;

	private bool activated = false;
	private bool readyToReset = false;
	private int stage;
	private int moduleId;
	private int submittedNamesCount = 0;
	private string[] answer;
	private FacePerceptionData.Stage[] stages;
	private FacePerceptionData.Person[] persons;
	private Coroutine fakeFaces;
	private List<NameComponent> names = new List<NameComponent>();

	private void Start() {
		moduleId = moduleIdCounter++;
		Module.OnActivate += Activate;
		fakeFaces = StartCoroutine(FakeFaces(.1f, .2f));
	}

	private IEnumerator FakeFaces(float min, float max) {
		while (true) {
			RenderPerson(FacePerceptionData.Person.GenerateRandom());
			yield return new WaitForSeconds(Random.Range(min, max));
		}
	}

	private void Activate() {
		StopCoroutine(fakeFaces);
		activated = true;
		FacePerceptionData.Generate(out stages, out persons, out answer);
		Debug.LogFormat("[Face Perception #{0}] Persons:\n\t{1}", moduleId, persons.Select(p => p.ToString()).Join("\n\t"));
		Debug.LogFormat("[Face Perception #{0}] Stages:\n\t{1}", moduleId, stages.Select(s => string.Format("{0}: {1}", s.person.name, s.score)).Join("\n\t"));
		Debug.LogFormat("[Face Perception #{0}] Answer: {1}", moduleId, answer.Join(", "));
		for (int i = 0; i < persons.Length; i++) {
			NameComponent Name = Instantiate(NamePrefab);
			Name.transform.parent = NamesContainer.transform;
			Name.transform.localPosition = new Vector3(0f, 0f, -0.045f + 0.018f * i);
			Name.transform.localRotation = Quaternion.identity;
			Name.transform.localScale = Vector3.one;
			Name.Selectable.Parent = Selectable;
			Name.text = persons[i].name;
			Name.active = true;
			Name.Selectable.OnInteract += () => OnNamePressed(Name);
			names.Add(Name);
		}
		Face.OnInteract += OnFacePressed;
		Reset.OnInteract += OnResetPressed;
		RenderStage();
	}

	private bool OnFacePressed() {
		if (!activated) return false;
		stage++;
		if (stage == stages.Length) {
			FaceContainer.SetActive(false);
			NamesContainer.SetActive(true);
			Score.text = "";
			Selectable.Children = names.Select(n => n.Selectable).ToArray();
			Selectable.UpdateChildren();
			return false;
		}
		RenderStage();
		return false;
	}

	private bool OnNamePressed(NameComponent nameComponent) {
		if (!nameComponent.active) return false;
		if (nameComponent.text != answer[submittedNamesCount]) {
			Module.HandleStrike();
			if (!readyToReset) {
				Score.text = "RESET";
				List<KMSelectable> children = new List<KMSelectable>(Selectable.Children);
				children.Add(Reset);
				Selectable.Children = children.ToArray();
				Selectable.UpdateChildren();
				readyToReset = true;
			}
		} else {
			readyToReset = false;
			submittedNamesCount++;
			nameComponent.active = false;
			if (submittedNamesCount == answer.Length) {
				Score.text = "SOLVED";
				NamesContainer.SetActive(false);
				FaceContainer.SetActive(true);
				fakeFaces = StartCoroutine(FakeFaces(1f, 1.1f));
				Module.HandlePass();
			}
		}
		return false;
	}

	private bool OnResetPressed() {
		if (!readyToReset) return false;
		readyToReset = false;
		FaceContainer.SetActive(true);
		NamesContainer.SetActive(false);
		Selectable.Children = new KMSelectable[] { Face };
		Selectable.UpdateChildren();
		stage = 0;
		RenderStage();
		return false;
	}

	private void RenderPerson(FacePerceptionData.Person person) {
		SetTexture(HairRenderer, GetHairTexture(person.color, person.style));
		SetTexture(BeardRenderer, GetBeardTexture(person.color, person.beard));
		SetTexture(AccessoriesRenderer, GetAccessoryTexture(person.accessory));
	}

	private void RenderStage() {
		FacePerceptionData.Person person = stages[stage].person;
		RenderPerson(person);
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
