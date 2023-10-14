using System;
using System.Collections;
using System.Collections.Generic;

namespace Test2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Hand h = new Hand()
            {
                AllThrowable = new IThrowable[]
                {
                    new Darts(),
                    new Grenade()
                }
            };
            //foreach (var item in h)
            //{
            //    Console.WriteLine(item);
            //}

            IEnumerator enumerator = h.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current);
            }

            Grenade[] grenades = new Grenade[]
            {
                new Grenade() { Atk = 5, AtkDistance = 10 },
                new Grenade() { Atk = 10, AtkDistance = 5 },
                new Grenade() { Atk = 8, AtkDistance = 8 },
            };
            Array.Sort(grenades);
            Array.Sort(grenades, new GrenadeAtkDistanceCompare());
            MySort(grenades, (a, b) => a.Atk < b.Atk);
        }
        public static void MySort<T>(T[] arrays, Func<T, T, bool> handler)
        {
            for (int i = 0; i < arrays.Length - 1; i++)
            {
                for (int j = i + 1; j < arrays.Length; j++)
                {
                    if (handler(arrays[i], arrays[j]))
                    {
                        T temp = arrays[i];
                        arrays[i] = arrays[j];
                        arrays[j] = temp;
                    }
                }
            }
        }
    }

    public interface IThrowable
    {
        void Fly();
    }

    public class Darts : IThrowable
    {
        public void Fly()
        {
            Console.WriteLine("扔飞镖");
        }
    }

    public class Grenade : IThrowable, IComparable
    {
        public int Atk { get; set; }
        public int AtkDistance { get; set; }

        public void Fly()
        {
            Console.WriteLine("扔手雷");
        }

        public int CompareTo(object obj)
        {
            Grenade parmas = obj as Grenade;
            return this.Atk.CompareTo(parmas.Atk);
        }
    }

    public class GrenadeAtkCompare : IComparer<Grenade>
    {
        public int Compare(Grenade x, Grenade y)
        {
            return x.Atk.CompareTo(y.Atk);
        }
    }

    public class GrenadeAtkDistanceCompare : IComparer<Grenade>
    {
        public int Compare(Grenade x, Grenade y)
        {
            return x.AtkDistance.CompareTo(y.AtkDistance);
        }
    }

    public class Hand: IEnumerable
    {
        public IThrowable[] AllThrowable { get; set; }

        public void Throw()
        {
            foreach (var variable in AllThrowable)
            {
                variable.Fly();
            }
        }


        //public IEnumerator GetEnumerator()
        //{
        //    return new HandEnumerator() { target = AllThrowable };
        //}

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < AllThrowable.Length; i++)
            {
                yield return AllThrowable[i];
            }
        }
    }

    public class HandEnumerator: IEnumerator
    {
        private int index = -1;
        public IThrowable[] target { get; set; }
        public bool MoveNext()
        {
            index++;
            return index < target.Length;
        }

        public void Reset()
        {
           
        }

        public object Current
        {
            get { return target[index]; }
        }
    }
}