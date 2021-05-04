/**
 * @param {T[]} arr
 * @returns {T[]}
 * @template T
 */
function shuffled(arr) {
  const result = [...arr];
  for (let i = 0; i < result.length; i++) {
    const j = Math.floor(Math.random() * result.length);
    [result[i], result[j]] = [result[j], result[i]];
  }
  return result;
}

/**
 * @param {IterableIterator<T>} arr
 * @returns {T[]}
 * @template T
 */
function sorted(arr) {
  const result = [...arr];
  result.sort();
  return result;
}

const names = shuffled([
  "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles", "Christopher", "Daniel", "Matthew", "Anthony", "Donald", "Mark", "Paul",
  "Steven", "Andrew", "Kenneth", "Joshua", "Kevin", "Brian", "George", "Edward", "Ronald", "Timothy", "Jason", "Jeffrey", "Ryan", "Jacob", "Gary", "Nicholas", "Eric",
  "Jonathan", "Stephen", "Larry", "Justin", "Scott", "Brandon", "Benjamin", "Samuel", "Frank", "Gregory", "Raymond", "Alexander", "Patrick", "Jack", "Dennis", "Jerry",
  "Tyler", "Aaron", "Jose", "Henry", "Adam", "Douglas", "Nathan", "Peter", "Zachary", "Kyle", "Walter", "Harold", "Jeremy", "Ethan", "Carl", "Keith", "Roger", "Gerald",
  "Christian", "Terry", "Sean", "Arthur", "Austin", "Noah", "Lawrence", "Jesse", "Joe", "Bryan", "Billy", "Jordan", "Albert", "Dylan", "Bruce", "Willie", "Gabriel", "Alan",
  "Juan", "Logan", "Wayne", "Ralph", "Roy", "Eugene", "Randy", "Vincent", "Russell", "Louis",
]);

const maxNameLength = names.reduce((acc, name) => Math.max(acc, name.length), 0);

const signs = [
  ["wild hair", "raised hair", "combed hair", "long hair"],
  ["blonde", "brunette", "redhead", "gray"],
  ["smooth-faced", "mustache", "goatee", "full beard"],
  ["without glasses", "round glasses", "square glasses", "sunglasses"],
]

/** @type {Map<string,[number,number,number,number]>} */
const descriptions = new Map();

let nameIndex = 0;
for (let i = 0; i < 4; i++) {
  for (let j = i + 1; j < 4; j++) {
    for (let iv = 0; iv < 4; iv++) {
      for (let jv = 0; jv < 4; jv++) {
        const v = [-1, -1, -1, -1];
        v[i] = iv;
        v[j] = jv;
        descriptions.set(names[nameIndex++], v);
      }
    }
  }
}

for (const name of sorted(descriptions.keys())) {
  const v = descriptions.get(name);
  console.log(
    (name.padStart(maxNameLength) + " " + v.toString()).padEnd(maxNameLength + 10),
    v.map((n, i) => n === -1 ? null : signs[i][n]).filter(a => a !== null).join(", "),
  );
}
