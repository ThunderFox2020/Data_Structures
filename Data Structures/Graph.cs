using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class Graph<TName, TData>
    {
        private Dictionary<TName, Vertex> vertices = new Dictionary<TName, Vertex>();
        private Dictionary<(Vertex from, Vertex to), Edge> edges = new Dictionary<(Vertex from, Vertex to), Edge>();

        public int Count { get => vertices.Count; }

        public IEnumerable<TName> GetVertices()
        {
            foreach (var item in vertices)
                yield return item.Key;
        }
        public IEnumerable<(TName from, TName to)> GetEdges()
        {
            foreach (var item in edges)
                yield return (item.Key.from.Name, item.Key.to.Name);
        }
        public void Add(TName name, TData data, Dictionary<TName, int> inConnection, Dictionary<TName, int> outConnection)
        {
            if (vertices.ContainsKey(name))
                throw new Exception("The vertex already exists");

            if (!(VerticesExists(inConnection) && VerticesExists(outConnection)))
                throw new Exception("There are no vertices to connect");

            Dictionary<Vertex, int> inConnectionConverted = GetVerticesCollection(inConnection);
            Dictionary<Vertex, int> outConnectionConverted = GetVerticesCollection(outConnection);

            Vertex newVertex = new Vertex(name, data);
            vertices.Add(newVertex.Name, newVertex);

            foreach (var item in inConnectionConverted)
                edges.Add((item.Key, newVertex), new Edge(item.Key, newVertex, item.Value));
            foreach (var item in outConnectionConverted)
                edges.Add((newVertex, item.Key), new Edge(newVertex, item.Key, item.Value));
        }
        public void Remove(TName name)
        {
            if (!vertices.ContainsKey(name))
                throw new Exception("The vertex does not exist");

            Vertex toRemove = vertices[name];

            vertices.Remove(name);
            foreach (var item in edges)
            {
                if (item.Value.From == toRemove || item.Value.To == toRemove)
                    edges.Remove(item.Key);
            }
        }
        public void Clear()
        {
            vertices.Clear();
            edges.Clear();
        }
        public TData SearchData(TName name)
        {
            if (!vertices.ContainsKey(name))
                throw new Exception("The vertex does not exist");

            Vertex required = vertices[name];

            return required.Data;
        }
        public int SearchWeight(TName name1, TName name2)
        {
            if (!vertices.ContainsKey(name1))
                throw new Exception("The vertex 'from' does not exist");
            if (!vertices.ContainsKey(name2))
                throw new Exception("The vertex 'to' does not exist");

            Vertex from = vertices[name1];
            Vertex to = vertices[name2];

            if (!edges.ContainsKey((from, to)))
                throw new Exception("The edge does not exist");

            return edges[(from, to)].Weight;
        }
        public List<TName> GetReachablePoints(TName name)
        {
            if (!vertices.ContainsKey(name))
                throw new Exception("The vertex does not exist");

            List<TName> result = new List<TName>();
            foreach (var item in vertices)
            {
                if (AnyWayExists(name, item.Key))
                    result.Add(item.Key);
            }
            return result;
        }
        public List<TName> GetNearestPoints(TName name)
        {
            if (!vertices.ContainsKey(name))
                throw new Exception("The vertex does not exist");

            List<TName> result = new List<TName>();
            foreach (var item in vertices)
            {
                if (StraightWayExists(name, item.Key))
                    result.Add(item.Key);
            }
            return result;
        }
        public bool AnyWayExists(TName name1, TName name2)
        {
            if (!vertices.ContainsKey(name1))
                throw new Exception("The vertex 'from' does not exist");
            if (!vertices.ContainsKey(name2))
                throw new Exception("The vertex 'to' does not exist");

            if (name1.Equals(name2))
                return false;

            bool result = false;

            AnyWayExists(name1, name2, ref result);
            UnVisiting();

            return result;
        }
        public bool StraightWayExists(TName name1, TName name2)
        {
            if (!vertices.ContainsKey(name1))
                throw new Exception("The vertex 'from' does not exist");
            if (!vertices.ContainsKey(name2))
                throw new Exception("The vertex 'to' does not exist");

            if (name1.Equals(name2))
                return false;

            if (!edges.ContainsKey((vertices[name1], vertices[name2])))
                return false;

            return true;
        }
        public List<TName> GetShortestWay(TName name1, TName name2)
        {
            if (!vertices.ContainsKey(name1))
                throw new Exception("The vertex 'from' does not exist");
            if (!vertices.ContainsKey(name2))
                throw new Exception("The vertex 'to' does not exist");

            List<TName> result = new List<TName>();

            if (!AnyWayExists(name1, name2))
                return result;

            Vertex from = vertices[name1];
            Vertex to = vertices[name2];

            Leveling(from);
            SearchWay(to, result);
            UnLeveling();

            return result;
        }
        
        private bool VerticesExists(Dictionary<TName, int> forChecking)
        {
            if (forChecking == null)
                return true;

            foreach (var item in forChecking)
            {
                if (!vertices.ContainsKey(item.Key))
                    return false;
            }

            return true;
        }
        private Dictionary<Vertex, int> GetVerticesCollection(Dictionary<TName, int> toConvert)
        {
            Dictionary<Vertex, int> result = new Dictionary<Vertex, int>();
            if (toConvert != null)
            {
                foreach (var item in toConvert)
                    result.Add(vertices[item.Key], item.Value);
            }
            return result;
        }
        private void AnyWayExists(TName name1, TName name2, ref bool result)
        {
            if (StraightWayExists(name1, name2))
                result = true;

            if (!result)
            {
                foreach (var item in GetNearestPoints(name1))
                {
                    if (!result && !vertices[item].Visited)
                    {
                        vertices[name1].Visited = true;
                        AnyWayExists(item, name2, ref result);
                    }
                }
            }
        }
        private void UnVisiting()
        {
            foreach (var item in vertices)
                item.Value.Visited = false;
        }
        private void Leveling(Vertex from)
        {
            Dictionary<int, List<Vertex>> result = new Dictionary<int, List<Vertex>>();

            result.Add(0, new List<Vertex>());
            from.Level = 0;
            result[0].Add(from);

            bool flag = true;
            for (int i = 1; flag; i++)
            {
                flag = false;
                result.Add(i, new List<Vertex>());
                foreach (var item in result[i - 1])
                {
                    foreach (var subItem in GetNearestPoints(item.Name))
                    {
                        if (vertices[subItem].Level == null)
                        {
                            vertices[subItem].Level = i;
                            result[i].Add(vertices[subItem]);
                            flag = true;
                        }
                    }
                }
            }
        }
        private void UnLeveling()
        {
            foreach (var item in vertices)
                item.Value.Level = default;
        }
        private void SearchWay(Vertex to, List<TName> result)
        {
            for (int? i = to.Level; i >= 0; i--)
            {
                result.Add(to.Name);
                foreach (var item in GetInConnection(to))
                {
                    if (vertices[item].Level == i - 1)
                    {
                        to = vertices[item];
                        break;
                    }
                }
            }
            result.Reverse();
        }
        private List<TName> GetInConnection(Vertex to)
        {
            List<TName> result = new List<TName>();
            foreach (var item in vertices)
            {
                if (edges.ContainsKey((item.Value, to)))
                    result.Add(item.Key);
            }
            return result;
        }

        private class Vertex
        {
            public Vertex(TName name, TData data)
            {
                Name = name;
                Data = data;
            }

            public TName Name { get; set; }
            public TData Data { get; set; }
            public bool Visited { get; set; }
            public int? Level { get; set; }
        }
        private class Edge
        {
            public Edge(Vertex from, Vertex to, int weight)
            {
                From = from;
                To = to;
                Weight = weight;
            }

            public Vertex From { get; set; }
            public Vertex To { get; set; }
            public int Weight { get; set; }
        }
    }
}