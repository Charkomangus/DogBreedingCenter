using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using LoLSDK;

namespace Assets.Scripts
{
    public class Dog : MonoBehaviour
    {
        [SerializeField]private string _dogName;     
        [SerializeField]private string _sex;
        [SerializeField]private int _size;
        [SerializeField]private int _endurance ;
        [SerializeField]private int _Scent;
        [SerializeField]private int _hearing ;
        [SerializeField]private int _demeanor;
        [SerializeField]private int _sight;
        [SerializeField] private int _Intelligence ;
        [SerializeField] private int _hairLength;
        [SerializeField] private int _strength;
        [SerializeField] private int _bark;
        [SerializeField]private List<GameObject> _siblings = new List<GameObject>(2);
        [SerializeField]private List<GameObject> _halfsiblings = new List<GameObject>(2);
        private Generation _generation;
        private readonly GameObject[] _parents = new GameObject[2];
        [SerializeField]private string[] _initialParents = new string[2];
        [SerializeField]private bool _startingGeneration;


        private readonly string[] _maleDogNames = {"Jeffrey", "Bruno", "Bob", "Max", "Slayer", "Gino", "George", "Scooter", "Spots", "Brutus", "Ziggy", "Ben", "Charlie",
                                                    "Mikey", "Samson", "Scottie", "Chris", "Shadow", "Spike", "Rusty", "Buster", "Oscar", "Yoda", "Zeus", "Winston", "Teddy", "Ace", "Buddy", "Rocky", "Jake","Jack",
                                                    "Toby", "Cody", "Buster", "Duke","Cooper", "Bear","Lucky", "Sammy", "Dean", "Oliver", "Ace","Apollo","Bailey","Bandit","Baxter","Bear","Beau","Benji","Benny","Bentley",
                                                    "Blue","Bo","Boomer","Brady","Brody","Bruno","Brutus","Bubba","Buddy","Buster","Cash","Champ","Chance","Charlie",
                                                       "Chase","Chester","Chico","Coco","Cody","Cooper","Copper","Dexter","Diesel","Duke","Elvis","Finn","Frankie","George","Gizmo",
                                                    "Gunner","Gus","Hank","Harley","Henry","Hunter","Jack","Jackson","Jake","Jasper","Jax","Joey","Kobe","Leo","Loki","Louie","Lucky","Luke","Mac","Marley","Max","Mickey","Milo",
                                                    "Moose","Murphy","Oliver","Ollie","Oreo","Oscar","Otis","Peanut","Prince","Rex","Riley","Rocco","Rocky","Romeo","Roscoe","Rudy","Rufus","Rusty","Sam","Sammy","Samson","Scooter",
                                                    "Scout","Shadow","Simba","Sparky","Spike","Tank","Teddy","Thor","Toby","Tucker","Tyson","Vader","Winston","Yoda","Zeus","Ziggy" };


        private readonly string[] _femaleDogNames = {"Abby","Allie","Angel","Annie","Athena","Baby","Bailey","Bella","Belle","Bonnie","Brandy","Cali","Callie","Casey","Charlie",
            "Chloe","Cleo","Coco","Cocoa","Cookie","Daisy","Dakota","Dixie","Ella","Ellie","Emma","Gigi","Ginger","Grace","Gracie","Hannah","Harley","Hazel","Heidi","Holly",
            "Honey","Izzy","Jasmine","Josie","Katie","Kona","Lacey","Lady","Layla","Lexi","Lexie","Lilly","Lily","Lola","Lucy","Lulu","Luna","Macy","Maddie","Madison","Maggie",
            "Marley","Maya","Mia","Millie","Mimi","Minnie","Missy","Misty","Mocha","Molly","Nala","Nikki","Olive","Peanut","Pebbles","Penny","Pepper","Phoebe","Piper","Princess",
            "Riley","Rosie","Roxie","Roxy","Ruby","Sadie","Sally","Sandy","Sasha","Sassy","Scout","Shadow","Shelby","Sierra","Sophie","Stella","Sugar","Sydney","Trixie","Willow",
            "Winnie","Zoe","Zoey", "Robin", "Eveline", "Aga",  "Liza", "Roxxie", "Charlotte", "Therese", "Bella", "Lucy", "Molly", " Daisy", "Maggie", "Sophie", "Sadie", "Chloe",
            "Bailey", "Lola", "Zoe", "Abby", "Ginger", "Roxy", "Gracie", "Coco", "Sasha", "Angel", "Princess", "Emma", "Rosie", "Ruby", "Lily", "Gosia"};
       
        // Use this for initialization
        void Start()
        {

            _generation = GetComponentInParent<Generation>();
            if (string.IsNullOrEmpty(_dogName))
            {
                SetName();
            }


            name = _dogName;
            FindSiblings();
            _initialParents[0] = _maleDogNames[Random.Range(0, _maleDogNames.Length - 1)];
            _initialParents[1] = _femaleDogNames[Random.Range(0, _femaleDogNames.Length - 1)];
            GetComponentInChildren<Text>().text = _dogName;
        }
    

        public void InitialiseDogs(string gender)
        {
            _sex = gender;
            _dogName = _sex == "Male" ? _maleDogNames[Random.Range(0, _maleDogNames.Length - 1)] : _femaleDogNames[Random.Range(0, _femaleDogNames.Length - 1)];
            _initialParents[0] = _maleDogNames[Random.Range(0, _maleDogNames.Length - 1)];
            _initialParents[1] = _femaleDogNames[Random.Range(0, _femaleDogNames.Length - 1)];

            _size = Random.Range(0, 100);
            _endurance = Random.Range(0, 100);
            _Scent = Random.Range(0, 100);
            _hearing = Random.Range(0, 100);
            _demeanor = Random.Range(0, 100);
            _sight = Random.Range(0, 100);
            _Intelligence  = Random.Range(0, 100);
            _hairLength = Random.Range(0, 100);
            _strength = Random.Range(0, 100);
            _bark = Random.Range(0, 100);
            _startingGeneration = true;

        }


        
        //Siblings
        public List<GameObject> ReturnSiblings()
        {
            return _siblings;
        }

        //Siblings
        public void SetSiblings(List<GameObject> siblings)
        {
            _siblings = siblings;
        }

        public GameObject ReturnSiblings(int index)
        {
            int counter = 0;
            for (int i = 0; i < _siblings.Count; i++)
            {
                if (counter == index)
                    return _siblings[i];
                counter++;
            }
            return null;
        }
        //Make a nice list of the sibling names and show them
        public StringBuilder ReturnSiblingNames()
        {
            System.Text.StringBuilder familyBuilder = new System.Text.StringBuilder();
            bool first = true;
            for (int i = 0; i < _siblings.Count; i++)
            {
                if (!first)
                    familyBuilder.Append(", ");
                first = false;
                familyBuilder.Append(_siblings[i].name);

            }
            return familyBuilder;
        }


        //Half-siblings
        public List<GameObject> ReturnHalfSiblings()
        {
            return _halfsiblings;
        }

        //Siblings
        public void SetHalfSiblings(List<GameObject> halfSiblings)
        {
            _siblings = halfSiblings;
        }

        //Make a nice list of the sibling names and show them
        public GameObject ReturnHalfSiblings(int index)
        {
            int counter = 0;
            for (int i = 0; i < _halfsiblings.Count; i++)
            {
                if (counter == index)
                    return _halfsiblings[i];
                counter++;
            }
            return null;

        }

        //Make a nice list of the half - sibling names and show them
        public StringBuilder ReturnHalfSiblingNames()
        {
            System.Text.StringBuilder familyBuilder = new StringBuilder();
            bool first = true;
            for (int i = 0; i < _halfsiblings.Count; i++)
            {
                if (!first)
                    familyBuilder.Append(", ");
                first = false;
                familyBuilder.Append(_halfsiblings[i].name);
            }
            return familyBuilder;
        }



        //Parents
        public GameObject[] ReturnParents()
        {
            return _parents;
        }

        public void SetGeneration(Generation generation)
        {
            _generation = generation;
        }

        public void FindSiblings()
        {
        
            if (_startingGeneration || _generation == null) return;
            _siblings.Clear();
            _halfsiblings.Clear();
            for (int i = 0; i < _generation._generationDogs.Length; i++)
            {
                Dog dog = _generation._generationDogs[i];
                //Skip this dog, it cant be its own sibling!
                if (dog == this || dog.transform.parent.tag =="PuppySlots") continue;
               
           
                int counter = 0;

                if (dog.ReturnParents()[0] == _parents[0] || dog.ReturnParents()[0] == _parents[1])
                {
                    counter++;
                }
                if (dog.ReturnParents()[1] == _parents[0] || dog.ReturnParents()[1] == _parents[1])
                {
                    counter++;

                }
                switch (counter)
                {
                    case 1:
                        if(_halfsiblings.Contains(dog.gameObject)) return;
                        _halfsiblings.Add(dog.gameObject);
                        break;
                    case 2:
                        if (_siblings.Contains(dog.gameObject)) return;
                        _siblings.Add(dog.gameObject);
                        break;
                    default:
                        continue;
                }
            }
        }


        //Return parents names
        public string ReturnParentNames()
        {
            if (_parents[0] == null || _parents[1] == null)
                return _initialParents[0] + " and " + _initialParents[1];
            return _parents[0].GetComponent<Dog>().ReturnName() + " and " + _parents[1].GetComponent<Dog>().ReturnName();
        }



        public void SetParents(string parent1, string parent2)
        {
            _initialParents[0] = parent1;
            _initialParents[1] = parent2;
        }

        public void SetParents(GameObject parent1, GameObject parent2)
        {
            _parents[0] = parent1;
            _parents[1] = parent2;
        }





        //Name
        public string ReturnName()
        {
            return _dogName;
        }

        public void SetName()
        {
            _dogName = _sex == "Male" ? _maleDogNames[Random.Range(0, _maleDogNames.Length - 1)] : _femaleDogNames[Random.Range(0, _femaleDogNames.Length - 1)];
        }

        public void SetName(string dogName)
        {
         
            _dogName = dogName;
        }

        //Sex
        public string ReturnSex()
        {
            return _sex;
        }

        public void SetSex(string sex)
        {
            _sex = sex;
        }

        public void SetSex()
        {
            int temp = Random.Range(0, 2);
            _sex = temp == 0 ? "Male" : "Female";
        }
        

        
    

        //Size Small - Medium - Large
        public int ReturnSize()
        {
            return _size;
        }

        public void SetSize(int size)
        {
            _size = size;
        }

        public string ReturnSizeDescription()
        {
            if(_size <= 30)
                return "Small";
            if (_size <= 60)
                return "Medium";
            if (_size <= 90)
                return "Large";
            return "Giant";
        }

        //Endurance Weak - Average - Strong
        public int ReturnEndurance()
        {
            return _endurance;
        }

        public void SetEndurance(int endurance)
        {
            _endurance = endurance;
        }

        public string ReturnEnduranceDescription()
        {
            if (_endurance <= 30)
                return "Weak";
            if (_endurance <= 60)
                return "Average";
            if (_endurance <= 90)
                return "Strong";
            return "Unstoppable";
           
        }

        //Strength Weak - Average - Strong
        public int ReturnStrength()
        {
            return _strength;
        }

        public void SetStrength(int strenth)
        {
            _strength = strenth;
        }

        public string ReturnStrengthDescription()
        {
            if (_strength <= 30)
                return "Weak";
            if (_strength <= 60)
                return "Average";
            if (_strength <= 90)
                return "Strong";
            return "Hercules";
        }

        //Scent Weak - Average - Strong
        public int ReturnScent()
        {
            return _Scent;
        }

        public void SetScent(int Scent)
        {
            _Scent = Scent;
        }

        public string ReturnScentDescription()
        {
            if (_Scent <= 30)
                return "Weak";
            if (_Scent <= 60)
                return "Average";
            if (_Scent <= 90)
                return "Strong";
            return "Powerful";

          
        }

        //Hearing Weak - Average - Strong
        public int ReturnHearing()
        {
            return _hearing;
        }

        public void SetHearing(int hearing)
        {
            _hearing = hearing;
        }

        public string ReturnHearingDescription()
        {
            if (_hearing <= 30)
                return "Weak";
            if (_hearing <= 60)
                return "Average";
            if (_hearing <= 90)
                return "Strong";
            return "Sonar";
        }

        //Sight Weak - Average - Strong
        public int ReturnSight()
        {
            return _sight;
        }

        public void SetSight(int sight)
        {
            _sight = sight;
        }

        public string ReturnSightDescription()
        {
            if (_sight <= 30)
                return "Weak";
            if (_sight <= 60)
                return "Average";
            if (_sight <= 90)
                return "Strong";
            return "Hawk";
        }
        
        //Demeanour Calm - Normal - Energetic
        public int ReturnDemeanor()
        {
            return _demeanor;
        }

        public void SetDemeanor(int demeanor)
        {
            _demeanor = demeanor;
        }

        public string ReturnDemeanorDescription()
        {
            if (_demeanor <= 30)
                return "Wild";
            if (_demeanor <= 60)
                return "Normal";
            if(_demeanor <= 90)
                return "Focused";
           return "Exceptional";
        
        }

        //Bark Low - Infrequent - Often
        public int ReturnBark()
        {
            return _bark;
        }

        public void SetBark(int bark)
        {
            _bark = bark;
        }

        public string ReturnBarkDescription()
        {
            if (_bark <= 30)
                return "Quiet";
            if(_bark <= 60)
                return "Infrequent";
            if(_bark <= 90)
                return "Often";
           return "Constantly";
       
        }

        //HairLength Short - Medium - Long
        public int ReturnHair()
        {
            return _hairLength;
        }

        public void SetHairLength(int hairLength)
        {
            _hairLength = hairLength;
        }

        public string ReturnHairLengthDescription()
        {
            if (_hairLength <= 33)
                return "Short";
            return _hairLength <= 66 ? "Medium" : "Long";
        }

        //Intelligence  Low - Medium - High
        public int ReturnIntelligence()
        {
            return _Intelligence ;
        }

        public void SetIntelligence(int Intelligence)
        {
            _Intelligence  = Intelligence ;
        }

        public string ReturnIntelligenceDescription()
        {
            if (_Intelligence  <= 30)
                return "Low";
            if (_Intelligence  <= 60)
                return "Medium";
            if (_Intelligence  <= 90)
                return "High";
            return "Genius";
        }

        public bool StartingGeneration()
        {
            return _startingGeneration;
        }

        //Clamp Value between two values
        public static int Clamp(int value, int min, int max)
        {
            return (value <= min) ? min : (value >= max) ? max : value;
        }

        public float CheckDiversity()
        {
             return (float)(_halfsiblings.Count*2.5)/2 + (_siblings.Count*5)/2f;
        }
    }
}
