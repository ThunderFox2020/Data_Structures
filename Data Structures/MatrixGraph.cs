using System;
using System.Collections.Generic;

namespace Data_Structures
{
    public class MatrixGraph<TName, TData>
    {
        private int[,] matrix = new int[10, 10];
        private Dictionary<TName, Vertex> vertices = new Dictionary<TName, Vertex>();
        private Dictionary<TName, int> vertexToIndex = new Dictionary<TName, int>();
        private Dictionary<int, TName> indexToVertex = new Dictionary<int, TName>();

        public int Count { get; private set; }

        public IEnumerable<TName> GetVertices()
        {
            foreach (var item in vertices)
                yield return item.Key;
        }
        public IEnumerable<(TName from, TName to)> GetEdges()
        {
            foreach (var from in vertices)
            {
                foreach (var to in vertices)
                {
                    if (matrix[vertexToIndex[from.Key], vertexToIndex[to.Key]] != 0)
                        yield return (from.Key, to.Key);
                }
            }
        }
        public void Add(TName name, TData data, Dictionary<TName, int> inConnection, Dictionary<TName, int> outConnection)
        {
            if (Count >= matrix.GetLength(0))
                Increase(matrix.GetLength(0) * 2);

            if (vertices.ContainsKey(name))
                throw new Exception("The vertex already exists");

            if (!(VerticesExists(inConnection) && VerticesExists(outConnection)))
                throw new Exception("There are no vertices to connect");

            if (inConnection == null)
                inConnection = new Dictionary<TName, int>();
            if (outConnection == null)
                outConnection = new Dictionary<TName, int>();

            Vertex newVertex = new Vertex(name, data);

            vertices.Add(newVertex.Name, newVertex);
            vertexToIndex.Add(newVertex.Name, Count);
            indexToVertex.Add(Count, newVertex.Name);

            foreach (var item in inConnection)
            {
                int fromIndex = vertexToIndex[item.Key];
                int toIndex = vertexToIndex[newVertex.Name];
                matrix[fromIndex, toIndex] = item.Value;
            }
            foreach (var item in outConnection)
            {
                int fromIndex = vertexToIndex[newVertex.Name];
                int toIndex = vertexToIndex[item.Key];
                matrix[fromIndex, toIndex] = item.Value;
            }

            Count++;
        }
        public void Remove(TName name)
        {
            if (!vertices.ContainsKey(name))
                throw new Exception("The vertex does not exist");

            int indexToRemove = vertexToIndex[name];

            vertices.Remove(name);
            vertexToIndex.Remove(name);
            indexToVertex.Remove(indexToRemove);

            for (int i = 0; i < matrix.GetLength(0); i++)
                matrix[i, indexToRemove] = default;
            for (int i = 0; i < matrix.GetLength(1); i++)
                matrix[indexToRemove, i] = default;

            Count--;

            if (10 < Count && Count < matrix.GetLength(0) / 2)
            {
                Defragmentation();
                Decrease(matrix.GetLength(0) / 2);
            }
        }
        public void Clear()
        {
            matrix = new int[10, 10];
            vertices.Clear();
            vertexToIndex.Clear();
            indexToVertex.Clear();
            Count = 0;
        }
        public TData SearchData(TName name)
        {
            if (!vertices.ContainsKey(name))
                throw new Exception("The vertex does not exist");

            return vertices[name].Data;
        }
        public int SearchWeight(TName name1, TName name2)
        {
            if (!vertices.ContainsKey(name1))
                throw new Exception("The vertex 'from' does not exist");
            if (!vertices.ContainsKey(name2))
                throw new Exception("The vertex 'to' does not exist");

            int fromIndex = vertexToIndex[name1];
            int toIndex = vertexToIndex[name2];

            if (matrix[fromIndex, toIndex] == 0)
                throw new Exception("The edge does not exist");

            return matrix[fromIndex, toIndex];
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

            int fromIndex = vertexToIndex[name1];
            int toIndex = vertexToIndex[name2];

            if (matrix[fromIndex, toIndex] == 0)
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
        
        private void Increase(int newLength)
        {
            int[,] newMatrix = new int[newLength, newLength];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    newMatrix[i, j] = matrix[i, j];
                }
            }
            matrix = newMatrix;
        }
        private void Decrease(int newLength)
        {
            int[,] newMatrix = new int[newLength, newLength];
            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(1); j++)
                {
                    newMatrix[i, j] = matrix[i, j];
                }
            }
            matrix = newMatrix;
        }
        private void Defragmentation()
        {
            int[,] newMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
            Dictionary<TName, int> newVertexToIndex = new Dictionary<TName, int>();
            Dictionary<int, TName> newIndexToVertex = new Dictionary<int, TName>();

            int index = 0;
            foreach (var item in vertices)
            {
                newVertexToIndex.Add(item.Key, index);
                newIndexToVertex.Add(index++, item.Key);
            }

            int oldFromIndex;
            int oldToIndex;
            int newFromIndex;
            int newToIndex;
            foreach (var from in vertices)
            {
                oldFromIndex = vertexToIndex[from.Key];
                newFromIndex = newVertexToIndex[from.Key];
                foreach (var to in vertices)
                {
                    oldToIndex = vertexToIndex[to.Key];
                    newToIndex = newVertexToIndex[to.Key];
                    newMatrix[newFromIndex, newToIndex] = matrix[oldFromIndex, oldToIndex];
                }
            }

            matrix = newMatrix;
            vertexToIndex = newVertexToIndex;
            indexToVertex = newIndexToVertex;
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
                if (StraightWayExists(item.Key, to.Name))
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
    }
}