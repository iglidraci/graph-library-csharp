using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees.Trees
{
    public class VanEmdeBoasTree
    {
        public int Universe { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public VanEmdeBoasTree Summary { get; set; }
        public VanEmdeBoasTree[] Clusters { get; set; }
        public int High(int x)
        {
            double div = Math.Ceiling(Math.Sqrt(Universe));
            return (int)Math.Floor(x/div);
        }
        public int Low(int x) => (int)(x % Math.Sqrt(Universe));
        public int Index(int i, int j) => (i * (int)Math.Sqrt(Universe)) + j;
        public VanEmdeBoasTree(int universeSize)
        {
            Universe = universeSize;
            Min = -1;
            Max = -1;
            if(universeSize <= 2)
            {
                Summary = null;
                Clusters = null;
            }
            else
            {
                int nrOfClusters = (int)Math.Ceiling(Math.Sqrt(Universe));
                Summary = new VanEmdeBoasTree(nrOfClusters);
                Clusters = new VanEmdeBoasTree[nrOfClusters];
                for (int i = 0; i < nrOfClusters; i++)
                {
                    Clusters[i] = new VanEmdeBoasTree(nrOfClusters);
                }
            }
        }
        public void Insert(int x)
        {
            // if the tree is empty
            if(Min == -1)
            {
                Min = Max = x;
                return;
            }
            // if key is less than min, just swap them, we save the min only once not in clusters
            if (x < Min)
            {
                int temp = x;
                x = Min;
                Min = temp;
            }
            // max is stored also in clusters
            if (x > Max)
                Max = x;
            if(Universe == 2)
            {
                Max = x;
                return;
            }
            if (Clusters[High(x)].Min == -1)
                Summary.Insert(High(x));
            Clusters[High(x)].Insert(Low(x));
        }
        public void Delete(int x)
        {
            if (Min == -1)
                return;
            if (Min == Max)
            {
                Min = -1;
                Max = -1;
                return;
            }
            if (Universe == 2)
            {
                if (x == 1)
                {
                    if (Min == 1)
                    {
                        Min = -1;
                        Max = -1;
                    }
                    else if (Min == 0)
                        Max = 0;
                }
                else
                {
                    if (Max == 0)
                    {
                        Min = -1;
                        Max = -1;
                    }
                    else if (Max == 1)
                        Min = 1;
                }
                return;
            }
            if (x == Min)
            {
                int itempMin = Summary.Min;
                Min = Index(itempMin, Clusters[itempMin].Min);
                return;
            }
            int i = High(x);
            int j = Low(x);
            Clusters[i].Delete(j);
            if (Clusters[i].Min == -1)
                Summary.Delete(i);
            if (x == Max)
            {
                if (Summary.Min == -1)
                    Max = Min;
                else
                {
                    i = Summary.Min;
                    Max = Index(i, Clusters[i].Max);
                }
            }
        }
        public int Successor(int x)
        {
            if (x <= Min)
                return Min;
            else if (x > Max)
                return Universe;
            int i = High(x);
            int j = Low(x);
            if (j <= Clusters[i].Max)
            {
                int result = Clusters[i].Max;
                if (result >= (Universe / (int)Math.Sqrt(Universe)))
                    return Universe;
                return x - j + result;
            }
            else
            {
                int result = Clusters[Summary.Successor(i)].Min;
                if (result >= Clusters[Summary.Min].Universe)
                    return Universe;
                return Index(Summary.Successor(i), result);               
            }
        }
        

    }
}
