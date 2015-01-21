
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace BreadthFirst
{
    class Program
    {
        [DataContract]
        public class Person
        {
            public Person(string name)
            {
                this.name = name;
            }

            [DataMember]
            public string name { get; set; }
            public List<Person> Friends
            {
                get
                {
                    return FriendsList;
                }
            }

            public void isFriendOf(Person p)
            {
                FriendsList.Add(p);
            }

            [DataMember]
            List<Person> FriendsList = new List<Person>();

            public override string ToString()
            {
                return name;
            }
        }

        public class BreadthFirstAlgorithm
        {
            public Person BuildFriendGraph()
            {
                Person Aaron = new Person("Aaron");
                Person Betty = new Person("Betty");
                Person Brian = new Person("Brian");
                Aaron.isFriendOf(Betty);
                Aaron.isFriendOf(Brian);

                Person Catherine = new Person("Catherine");
                Person Carson = new Person("Carson");
                Person Darian = new Person("Darian");
                Person Derek = new Person("Derek");
                Betty.isFriendOf(Catherine);
                Betty.isFriendOf(Darian);
                Brian.isFriendOf(Carson);
                Brian.isFriendOf(Derek);

                return Aaron;
            }

            //http://en.wikipedia.org/wiki/Breadth-first_search#Pseudocode
            public Person Search(Person root, string nameToSearchFor)
            {
                Queue<Person> Q = new Queue<Person>();
                HashSet<Person> S = new HashSet<Person>();
                Q.Enqueue(root);
                S.Add(root);

                while (Q.Count > 0)
                {
                    Person p = Q.Dequeue();
                    if (p.name == nameToSearchFor)
                        return p;
                    foreach (Person friend in p.Friends)
                    {
                        if (!S.Contains(friend))
                        {
                            Q.Enqueue(friend);
                            S.Add(friend);
                        }
                    }
                }
                return null;
            }

            public void Traverse(Person root)
            {
                Queue<Person> traverseOrder = new Queue<Person>();

                Queue<Person> Q = new Queue<Person>();
                HashSet<Person> S = new HashSet<Person>();
                Q.Enqueue(root);
                S.Add(root);

                while (Q.Count > 0)
                {
                    Person p = Q.Dequeue();
                    traverseOrder.Enqueue(p);

                    foreach (Person friend in p.Friends)
                    {
                        if (!S.Contains(friend))
                        {
                            Q.Enqueue(friend);
                            S.Add(friend);
                        }
                    }
                }

                while (traverseOrder.Count > 0)
                {
                    Person p = traverseOrder.Dequeue();
                    Console.WriteLine(p);
                }
            }
        }

        static void Main(string[] args)
        {
            BreadthFirstAlgorithm b = new BreadthFirstAlgorithm();
            Person root = b.BuildFriendGraph();

            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Person));
            ser.WriteObject(stream1, root);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            Console.Write("JSON form of Person object: ");
            var output = sr.ReadToEnd();
            Console.WriteLine(output);

            Console.WriteLine("Traverse\n------");
            b.Traverse(root);


            Console.WriteLine("\nSearch\n------");
            Person p = b.Search(root, "Catherine");
            Console.WriteLine(p == null ? "Person not found" : p.name);
            p = b.Search(root, "Alex");
            Console.WriteLine(p == null ? "Person not found" : p.name);
            Console.ReadLine();
        }
    }
}
