using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class FacePerceptionData {
	private static Dictionary<string, int[]> _data = new Dictionary<string, int[]>() {
		{ "Aaron", new int[4] {-1, -1, 1, 0}},
		{ "Adam", new int[4] {1, -1, 2, -1}},
		{ "Alan", new int[4] {-1, 0, -1, 0}},
		{ "Albert", new int[4] {2, -1, -1, 1}},
		{ "Alexander", new int[4] {2, -1, -1, 0}},
		{ "Andrew", new int[4] {0, -1, -1, 3}},
		{ "Anthony", new int[4] {-1, 1, -1, 1}},
		{ "Arthur", new int[4] {-1, -1, 1, 1}},
		{ "Austin", new int[4] {-1, 0, 2, -1}},
		{ "Benjamin", new int[4] {-1, 3, -1, 2}},
		{ "Billy", new int[4] {-1, 0, 0, -1}},
		{ "Brandon", new int[4] {-1, -1, 3, 2}},
		{ "Brian", new int[4] {0, 1, -1, -1}},
		{ "Bruce", new int[4] {-1, 1, 3, -1}},
		{ "Bryan", new int[4] {3, 0, -1, -1}},
		{ "Carl", new int[4] {-1, 0, -1, 1}},
		{ "Charles", new int[4] {0, -1, 3, -1}},
		{ "Christian", new int[4] {3, -1, -1, 1}},
		{ "Christopher", new int[4] {-1, 2, 2, -1}},
		{ "Daniel", new int[4] {-1, -1, 3, 1}},
		{ "David", new int[4] {-1, 1, -1, 2}},
		{ "Dennis", new int[4] {-1, 2, -1, 2}},
		{ "Donald", new int[4] {-1, 1, -1, 0}},
		{ "Douglas", new int[4] {-1, 1, 2, -1}},
		{ "Dylan", new int[4] {0, -1, -1, 1}},
		{ "Edward", new int[4] {2, 2, -1, -1}},
		{ "Eric", new int[4] {2, 3, -1, -1}},
		{ "Ethan", new int[4] {-1, 3, 3, -1}},
		{ "Eugene", new int[4] {-1, -1, 1, 2}},
		{ "Frank", new int[4] {-1, 1, 0, -1}},
		{ "Gabriel", new int[4] {-1, 3, 0, -1}},
		{ "Gary", new int[4] {3, 1, -1, -1}},
		{ "George", new int[4] {0, -1, -1, 0}},
		{ "Gerald", new int[4] {1, -1, 1, -1}},
		{ "Gregory", new int[4] {-1, 3, -1, 0}},
		{ "Harold", new int[4] {2, -1, 3, -1}},
		{ "Henry", new int[4] {-1, -1, 3, 3}},
		{ "Jack", new int[4] {0, 3, -1, -1}},
		{ "Jacob", new int[4] {-1, 3, -1, 1}},
		{ "James", new int[4] {-1, 1, -1, 3}},
		{ "Jason", new int[4] {-1, 2, 0, -1}},
		{ "Jeffrey", new int[4] {-1, -1, 0, 2}},
		{ "Jeremy", new int[4] {-1, 0, -1, 3}},
		{ "Jerry", new int[4] {3, -1, -1, 3}},
		{ "Jesse", new int[4] {2, -1, 0, -1}},
		{ "Joe", new int[4] {3, 2, -1, -1}},
		{ "John", new int[4] {3, -1, -1, 0}},
		{ "Jonathan", new int[4] {1, -1, -1, 3}},
		{ "Jordan", new int[4] {-1, 2, -1, 3}},
		{ "Jose", new int[4] {-1, 0, 3, -1}},
		{ "Joseph", new int[4] {3, -1, 2, -1}},
		{ "Joshua", new int[4] {2, 1, -1, -1}},
		{ "Juan", new int[4] {0, -1, 2, -1}},
		{ "Justin", new int[4] {0, 2, -1, -1}},
		{ "Keith", new int[4] {2, -1, -1, 2}},
		{ "Kenneth", new int[4] {-1, 3, 1, -1}},
		{ "Kevin", new int[4] {1, -1, -1, 1}},
		{ "Kyle", new int[4] {-1, 2, -1, 0}},
		{ "Larry", new int[4] {3, -1, 0, -1}},
		{ "Lawrence", new int[4] {-1, 2, -1, 1}},
		{ "Logan", new int[4] {-1, 3, -1, 3}},
		{ "Louis", new int[4] {3, 3, -1, -1}},
		{ "Mark", new int[4] {-1, -1, 1, 3}},
		{ "Matthew", new int[4] {1, 0, -1, -1}},
		{ "Michael", new int[4] {1, 1, -1, -1}},
		{ "Nathan", new int[4] {-1, 2, 3, -1}},
		{ "Nicholas", new int[4] {-1, 2, 1, -1}},
		{ "Noah", new int[4] {1, 3, -1, -1}},
		{ "Patrick", new int[4] {0, -1, 1, -1}},
		{ "Paul", new int[4] {0, 0, -1, -1}},
		{ "Peter", new int[4] {2, -1, -1, 3}},
		{ "Ralph", new int[4] {-1, -1, 0, 3}},
		{ "Randy", new int[4] {-1, 0, -1, 2}},
		{ "Raymond", new int[4] {1, -1, -1, 0}},
		{ "Richard", new int[4] {0, -1, 0, -1}},
		{ "Robert", new int[4] {1, 2, -1, -1}},
		{ "Roger", new int[4] {1, -1, -1, 2}},
		{ "Ronald", new int[4] {2, 0, -1, -1}},
		{ "Roy", new int[4] {-1, -1, 0, 1}},
		{ "Russell", new int[4] {1, -1, 0, -1}},
		{ "Ryan", new int[4] {1, -1, 3, -1}},
		{ "Samuel", new int[4] {3, -1, 3, -1}},
		{ "Scott", new int[4] {-1, -1, 2, 0}},
		{ "Sean", new int[4] {-1, 0, 1, -1}},
		{ "Stephen", new int[4] {-1, -1, 2, 3}},
		{ "Steven", new int[4] {-1, -1, 0, 0}},
		{ "Terry", new int[4] {0, -1, -1, 2}},
		{ "Thomas", new int[4] {-1, -1, 3, 0}},
		{ "Timothy", new int[4] {3, -1, -1, 2}},
		{ "Tyler", new int[4] {-1, -1, 2, 2}},
		{ "Vincent", new int[4] {2, -1, 1, -1}},
		{ "Walter", new int[4] {2, -1, 2, -1}},
		{ "Wayne", new int[4] {-1, 1, 1, -1}},
		{ "William", new int[4] {-1, -1, 2, 1}},
		{ "Willie", new int[4] {-1, 3, 2, -1}},
		{ "Zachary", new int[4] {3, -1, 1, -1}}
	};

	private static string[] hairColors = new string[4] { "black", "blonde", "red", "white" };
	private static string[] hairStyles = new string[4] { "wild", "raised", "combed", "long" };
	private static string[] beards = new string[4] { "smooth-faced", "mustache", "goatee", "full beard" };
	private static string[] accessories = new string[4] { "no glasses", "round glasses", "square glasses", "sport glasses" };

	public struct Person {
		public int style;
		public int color;
		public int beard;
		public int accessory;
		public string name;
		public Person(int style, int color, int beard, int accessory, string name) {
			this.style = style;
			this.color = color;
			this.beard = beard;
			this.accessory = accessory;
			this.name = name;
		}
		public override string ToString() {
			return string.Format("{0}: {1} {2} hair, {3}, {4}", name, hairColors[color], hairStyles[style], beards[beard], accessories[accessory]);
		}
		public static Person GenerateRandom() {
			return new Person(Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4), "RANDOM");
		}
	}

	public struct Stage {
		public Person person;
		public int score;
		public Stage(Person person, int score) {
			this.person = person;
			this.score = score;
		}
	}

	private static int PowInt(int a, int b) {
		if (b < 0) throw new System.Exception("Not allowed unsigned power");
		if (b == 0) return 1;
		return a * PowInt(a, b - 1);
	}

	private static HashSet<int> GetPossibleAttributes(string name, int from = 0) {
		if (from == 4) return new HashSet<int>() { 0 };
		HashSet<int> subargs = GetPossibleAttributes(name, from + 1);
		int power = PowInt(4, 3 - from);
		if (_data[name][from] >= 0) return new HashSet<int>(subargs.Select(s => s + _data[name][from] * power));
		return new HashSet<int>(Enumerable.Range(0, 4).SelectMany(i => subargs.Select(s => s + i * power)));
	}

	private static Person GeneratePerson(string name, int attrs) {
		int accessory = attrs % 4;
		attrs /= 4;
		int beard = attrs % 4;
		attrs /= 4;
		int style = attrs % 4;
		attrs /= 4;
		return new Person(style, attrs % 4, beard, accessory, name);
	}

	public static void Generate(out Stage[] stages, out Person[] persons, out string[] answer, out Dictionary<string, int> scores) {
		HashSet<string> allNames = new HashSet<string>(_data.Keys);
		Dictionary<string, HashSet<int>> preresult = new Dictionary<string, HashSet<int>>();
		HashSet<int> usedAttrs = new HashSet<int>();
		for (int i = 0; i < 6; i++) {
			string name = allNames.PickRandom();
			allNames.Remove(name);
			HashSet<int> posAttrs = GetPossibleAttributes(name);
			if (posAttrs.All(s => usedAttrs.Contains(s))) break;
			if (preresult.Values.Any(s => s.Where(k => !posAttrs.Contains(k)).Count() == 0)) break;
			foreach (string preresultName in preresult.Keys.ToArray()) {
				preresult[preresultName] = new HashSet<int>(preresult[preresultName].Where(s => !posAttrs.Contains(s)));
			}
			posAttrs = new HashSet<int>(posAttrs.Where(s => !usedAttrs.Contains(s)));
			foreach (int attr in posAttrs) usedAttrs.Add(attr);
			preresult[name] = posAttrs;
		}
		persons = preresult.Keys.Select(name => GeneratePerson(name, preresult[name].PickRandom())).ToArray();
		Stage?[] _stages = new Stage?[Random.Range(4, 10) + persons.Length];
		Dictionary<string, int> _scores = new Dictionary<string, int>();
		for (int i = 0; i < persons.Length; i++) {
			int score = Random.Range(10, 100);
			_stages[i] = new Stage(persons[i], score);
			_scores[persons[i].name] = score;
		}
		_stages = _stages.Shuffle();
		for (int i = 0; i < _stages.Length; i++) {
			if (_stages[i] != null) continue;
			Person person = persons.Where(p => (
				(i == 0 || _stages[i - 1] == null || _stages[i - 1].Value.person.name != p.name) &&
				(i == _stages.Length - 1 || _stages[i + 1] == null || _stages[i + 1].Value.person.name != p.name)
			)).PickRandom();
			if (Random.Range(0, 3) == 0) _stages[i] = new Stage(person, Random.Range(10, 100));
			else {
				int curPersonScore = _scores[person.name];
				IEnumerable<int> posScores = _scores.Values.Select(v => v - curPersonScore).Where(v => v >= 10 && v < 100);
				_stages[i] = new Stage(person, posScores.Count() > 0 ? posScores.PickRandom() : Random.Range(10, 100));
			}
		}
		stages = _stages.Select(v => v.Value).ToArray();
		Dictionary<string, KeyValuePair<int, int>> answerCalculator = new Dictionary<string, KeyValuePair<int, int>>();
		for (int i = 0; i < stages.Length; i++) {
			Stage stage = stages[i];
			string name = stage.person.name;
			answerCalculator[name] = new KeyValuePair<int, int>(stage.score + (answerCalculator.ContainsKey(name) ? answerCalculator[name].Key : 0), i);
		}
		answer = persons.Select(p => p.name).ToArray();
		Array.Sort(answer, (string a, string b) => {
			int aScore = answerCalculator[a].Key;
			int bScore = answerCalculator[b].Key;
			if (aScore == bScore) return answerCalculator[a].Value - answerCalculator[b].Value;
			return bScore - aScore;
		});
		scores = new Dictionary<string, int>();
		foreach (string name in answer) scores[name] = answerCalculator[name].Key;
	}
}
