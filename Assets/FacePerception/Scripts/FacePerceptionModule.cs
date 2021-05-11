using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using Random = UnityEngine.Random;

public class FacePerceptionModule : MonoBehaviour {
	private static int moduleIdCounter = 1;

	public readonly string TwitchHelpMessage = new string[] {
		"\"!{0} face\" - press large display",
		"\"!{0} name1 name2\" - press names",
		"\"!{0} reset\" - press reset button",
		"use carefully: \"!{0} skip\" - skip all faces",
	}.Join(" | ");

	public KMSelectable Selectable;
	public KMAudio Audio;
	public KMBombInfo BombInfo;
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
	public Texture SportGlassesSprite;

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
	private string[] replacements = Enumerable.Range(0, 10).Select(_ => "").ToArray();
	private List<NameComponent> names = new List<NameComponent>();
	private Dictionary<string, int> scores;

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
		FacePerceptionData.Generate(out stages, out persons, out answer, out scores);
		foreach (FacePerceptionData.Person person in persons) Debug.LogFormat("[Face Perception #{0}] {1}", moduleId, person.ToString());
		for (int i = 0; i < stages.Length; i++) Debug.LogFormat("[Face Perception #{0}] Stage #{1}: {2} = {3}", moduleId, i + 1, stages[i].person.name, stages[i].score);
		foreach (string name in answer) Debug.LogFormat("[Face Perception #{0}] {1}'s score is {2}", moduleId, name, scores[name]);
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
		replacements[BombInfo.GetIndicators().Count() % 10] += "A";
		replacements[BombInfo.GetOnIndicators().Count() % 10] += "B";
		replacements[BombInfo.GetOffIndicators().Count() % 10] += "C";
		replacements[new HashSet<string>(BombInfo.GetPorts()).Count % 10] += "D";
		replacements[BombInfo.GetPorts().Count() % 10] += "E";
		replacements[(BombInfo.GetSerialNumberLetters().Last() - 'A' + 1) % 10] += "F";
		replacements[(BombInfo.GetSerialNumberLetters().First() - 'A' + 1) % 10] += "G";
		replacements[BombInfo.GetSerialNumberNumbers().Sum() % 10] += "H";
		replacements[BombInfo.GetPortPlateCount() % 10] += "I";
		Face.OnInteract += OnFacePressed;
		Reset.OnInteract += OnResetPressed;
		RenderStage();
	}

	private bool OnFacePressed() {
		if (!activated) return false;
		Audio.PlaySoundAtTransform("FacePerceptionStageChanged", Face.transform);
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
			List<string> submittedNames = new List<string>(answer.Take(submittedNamesCount));
			submittedNames.Add(nameComponent.text);
			Debug.LogFormat("[Face Perception #{0}] Submitted wrong answer: ", moduleId, submittedNames.Join(", "));
			if (!readyToReset) {
				Score.text = "RESET";
				List<KMSelectable> children = new List<KMSelectable>(Selectable.Children);
				children.Add(Reset);
				Selectable.Children = children.ToArray();
				Selectable.UpdateChildren();
				readyToReset = true;
			}
		} else {
			submittedNamesCount++;
			nameComponent.active = false;
			if (submittedNamesCount == answer.Length) {
				Debug.LogFormat("[Face Perception #{0}] Module solved", moduleId);
				Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
				readyToReset = false;
				Score.text = "SOLVED";
				NamesContainer.SetActive(false);
				FaceContainer.SetActive(true);
				fakeFaces = StartCoroutine(FakeFaces(1f, 1.1f));
				Module.HandlePass();
			} else Audio.PlaySoundAtTransform("FacePerceptionNamePressed", nameComponent.transform);
		}
		return false;
	}

	private bool OnResetPressed() {
		if (!readyToReset) return false;
		Debug.LogFormat("[Face Perception #{0}] Reset pressed", moduleId);
		Audio.PlaySoundAtTransform("FacePerceptionResetPressed", Reset.transform);
		readyToReset = false;
		FaceContainer.SetActive(true);
		NamesContainer.SetActive(false);
		Selectable.Children = new KMSelectable[] { Face };
		Selectable.UpdateChildren();
		stage = 0;
		RenderStage();
		return false;
	}

	private IEnumerator ProcessTwitchCommand(string command) {
		command = command.Trim().ToLower();
		if (command == "next") {
			yield return null;
			if (stage >= stages.Length) {
				yield return "sendtochat {0}, !{1}: face not present on module";
				yield break;
			}
			yield return new KMSelectable[] { Face };
			yield break;
		}
		if (command == "reset") {
			yield return null;
			if (!readyToReset) {
				yield return "sendtochat {0}, !{1}: reset button not active";
				yield break;
			}
			yield return new KMSelectable[] { Reset };
			yield break;
		}
		if (command == "skip") {
			yield return null;
			if (stage >= stages.Length) {
				yield return "sendtochat {0}, !{1}: faces not present on module";
				yield break;
			}
			int pressesCount = stages.Length - stage;
			yield return Enumerable.Range(0, pressesCount).Select(_ => Face).ToArray();
			yield break;
		}
		if (!Regex.IsMatch(command, "^[a-z ]+$")) yield break;
		string[] names = command.Split(' ').Where(s => s != "").ToArray();
		if (names.Length != new HashSet<string>(names).Count) {
			yield return "sendtochat {0}, !{1}: duplicate names provided";
			yield break;
		}
		if (names.Length > 6) {
			yield return "sendtochat {0}, !{1}: too much names provided";
			yield break;
		}
		yield return null;
		if (stage < stages.Length) {
			yield return "sendtochat {0}, !{1}: names not present on module";
			yield break;
		}
		Dictionary<string, NameComponent> components = new Dictionary<string, NameComponent>();
		HashSet<string> notFoundNames = new HashSet<string>(names);
		HashSet<string> notActiveNames = new HashSet<string>(names);
		foreach (NameComponent nameComponent in this.names) {
			notFoundNames.Remove(nameComponent.text.ToLower());
			if (nameComponent.active) notActiveNames.Remove(nameComponent.text.ToLower());
			components[nameComponent.text.ToLower()] = nameComponent;
		}
		if (notFoundNames.Count > 0) {
			yield return "sendtochat {0}, !{1}: name(s) " + notFoundNames.Join(", ") + " not found";
			yield break;
		}
		if (notActiveNames.Count > 0) {
			yield return "sendtochat {0}, !{1}: name(s) " + notActiveNames.Join(", ") + " already pressed";
			yield break;
		}
		yield return names.Select(n => components[n].Selectable).ToArray();
	}

	public void TwitchHandleForcedSolve() {
		Debug.LogFormat("[Face Perception #{0}] Module force-solved", moduleId);
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
		readyToReset = false;
		Score.text = "SOLVED";
		NamesContainer.SetActive(false);
		FaceContainer.SetActive(true);
		fakeFaces = StartCoroutine(FakeFaces(1f, 1.1f));
		Module.HandlePass();
	}

	private void RenderPerson(FacePerceptionData.Person person) {
		SetTexture(HairRenderer, GetHairTexture(person.color, person.style));
		SetTexture(BeardRenderer, GetBeardTexture(person.color, person.beard));
		SetTexture(AccessoriesRenderer, GetAccessoryTexture(person.accessory));
	}

	private void RenderStage() {
		FacePerceptionData.Person person = stages[stage].person;
		RenderPerson(person);
		Score.text = stages[stage].score.ToString().Select(c => replacements[c - '0'].Length > 0 && Random.Range(0, 3) == 0 ? replacements[c - '0'].PickRandom() : c).Join("");
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
		return new Texture[] { null, RoundGlassesSprite, SquareGlassesSprite, SportGlassesSprite }[type];
	}
}
