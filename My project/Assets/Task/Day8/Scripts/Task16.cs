using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public sealed class Graph<T> : IEnumerable<GraphNode<T>>
{
    readonly Dictionary<T, GraphNode<T>> nodes = new Dictionary<T, GraphNode<T>>();
    public int Count => nodes.Count;
    
    public GraphNode<T> Add(T vertex)
    {
        GraphNode<T> graphnode = new GraphNode<T>();    //그래프 생성
        graphnode.Vertex = vertex;      //노드의 정점 저장
        nodes.Add(vertex, graphnode);   //노드 딕셔너리에 추가

        return graphnode;
    }

    public bool Contains(T vertex)
    {
        foreach(KeyValuePair<T, GraphNode<T>> i in nodes)           // TryGetValue랑 최적화 하고싶다.
        {
            if(i.Key.Equals(vertex)) return true;
        }

        return false;
    }

    public bool TryGetValue(T vertex, out GraphNode<T> result)
    {
        foreach(KeyValuePair<T, GraphNode<T>> i in nodes)
        {
            if(i.Key.Equals(vertex))
            {
                result = i.Value;
                return true;
            }
        }
        throw new Exception("값이 없습니다.");  //여기에 false넣는법
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<GraphNode<T>> GetEnumerator()
    {
        foreach(var i in nodes)
            yield return i.Value;
    }

    public void SetEdgeJOB(T a, T b, int weight)        //시작 노드와 끝 노드를 저장 후 노드에 알맞은 값 입력
    {
        GraphNode<T> FromNode = null, Endnode = null;
        foreach(KeyValuePair<T, GraphNode<T> > i in nodes)
        {
            if(i.Key.Equals(a))
                FromNode = i.Value;
            if(i.Key.Equals(b))
                Endnode = i.Value;
        }

        if(FromNode == null || Endnode == null)
            throw new Exception("버텍스의 노트를 찾을 수 없습니다.");

        GraphNode<T>.Edge edgeTmp = new GraphNode<T>.Edge();
        edgeTmp.Vertex = b;
        edgeTmp.Node = Endnode;
        edgeTmp.Weight = weight;

        FromNode.edge[FromNode.edgeCnt] = edgeTmp;
        FromNode.edgeCnt++;
    }

    public void SetEdge(T from, T to, int weight, bool isBoth)
    {
        SetEdgeJOB(from, to, weight);
        if(isBoth) SetEdgeJOB (to, from, weight);
    }

    public void SetEdge(T a, T b, int weight_ab, int weight_ba)
    {
        SetEdgeJOB(a, b, weight_ab);
        SetEdgeJOB(b, a ,weight_ba);
    }

    // public GraphPath<T> CreatePath(T start, T end)
    // {
    //     //...
    // }

    // public List<GraphPath<T>> SearchAll(T start, T end, SearchPolicy policy)
    // {
    //     var path = CreatePath(start, end);
    //     var paths = new List<GraphPath<T>>();

    //     // policy에 따라 깊이 우선 탐색으로 경로를 구하기
    //     //...

    //     return paths;
    // }

    // public bool Remove(T vertex)
    // {
    //     // 해당 정점과 연관된 간선을 정리
    //     //...
    // }

    // public void Clear()
    // {
    //     // 모든 정점, 간선을 정리
    //     //...
    // }


    public enum SearchPolicy
    {
        Visit = 0, // 이미 방문한 정점은 생략
        Pass,      // 이미 방문한 간선은 생략
    }
}




public class GraphNode<T> : IEnumerable<KeyValuePair<T, int>>
{
    public T Vertex;

    //public List<GraphNode<T>.Edge> edge = new List<GraphNode<T>.Edge>();
    public int edgeCnt = 0;
    public GraphNode<T>.Edge[] edge = new GraphNode<T>.Edge[100];   //100은 임의 설정
    

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
    {
        for(int i = 0; i< edgeCnt; i++)
            yield return new KeyValuePair<T, int>(edge[i].Vertex, edge[i].Weight);    //여기에 Vertex, Weight?
    }

    public struct Edge
    {
        public T Vertex;            //목적지의 버텍스
        public GraphNode<T> Node;   //진행할 노드
        public int Weight;          //가중치
    }
}





// public class GraphPath<T> : IEnumerable<T>
// {
//     public GraphNode<T> Start { private set; get; } = null;
//     public GraphNode<T> End { private set; get; } = null;

//     public readonly List<T> Vertexs = new List<T>();
//     public int Count => Vertexs.Count;
//     public bool IsNoWay
//     {
//         get
//         {
//             // Start에서 End까지 길이 이어져 있는지 여부
//             //...
//         }
//     }


//     public int GetTotalWeight()
//     {
//         //...
//     }

//     public bool IsVisited(T vertex)
//     {
//         // vertex의 방문 여부 확인		
//         //...
//     }

//     public bool IsPassed(T vertex)
//     {
//         // 마지막 vertex에서 인자로 넘어온 vertex로 향한 edge의 통과 여부 확인
//         //...
//     }
//     public bool IsPassed(T from, T to)
//     {
//         //...
//     }

//     public GraphPath<T> Clone()
//     {
//         //...
//     }

//     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
//     public IEnumerator<T> GetEnumerator()
//     {
//         return null;
//     }
// }



public class Task16 : StudyBase
{
    protected override void OnLog()
    {
        var graph = new Graph<string>();
        graph.Add("경기도");
        graph.Add("강원도");
        graph.Add("충청도");
        graph.Add("경상도");
        graph.Add("전라도");
        graph.Add("제주도");

        graph.SetEdge("경기도", "강원도", 7, 5);
        graph.SetEdge("경기도", "충청도", 5, true);

        graph.SetEdge("강원도", "충청도", 9, false);
        graph.SetEdge("강원도", "경상도", 13, true);

        graph.SetEdge("충청도", "경상도", 8, true);
        graph.SetEdge("충청도", "전라도", 7, true);

        graph.SetEdge("전라도", "경상도", 6, true);

        graph.SetEdge("경상도", "제주도", 14, false);
        graph.SetEdge("제주도", "경기도", 27, false);


        // // 경상도 to 충청도 short (8) 경상도 > 충청도(8)
        // // 경상도 to 충청도 long (57) 경상도 > 제주도(14) > 경기도(27) > 강원도(7) > 충청도(9)
        // _ShortLongLog(graph.SearchAll("경상도", "충청도", Graph<string>.SearchPolicy.Visit));

        // // 전라도 to 제주도 short (20) 전라도 > 경상도(6) > 제주도(14)
        // // 전라도 to 제주도 long (46) 전라도 > 충청도(7) > 경기도(5) > 강원도(7) > 경상도(13) > 제주도(14)
        // _ShortLongLog(graph.SearchAll("전라도", "제주도", Graph<string>.SearchPolicy.Visit));

        // // 경기도 to 경상도 short (13) 경기도 > 충청도(5) > 경상도(8)
        // // 경기도 to 경상도 long (48) 경기도 > 강원도(7) > 충청도(9) > 경기도(5) > 충청도(5) > 전라도(7) > 충청도(7) > 경상도(8)
        // _ShortLongLog(graph.SearchAll("경기도", "경상도", Graph<string>.SearchPolicy.Pass));

        // //강원도 to 전라도 short (16) 강원도 > 충청도(9) > 전라도(7)
        // //강원도 to 전라도 long (108) 강원도 > 경기도(5) > 강원도(7) > 충청도(9) > 경상도(8) > 강원도(13) > 경상도(13) > 제주도(14) > 경기도(27) > 충청도(5) > 전라도(7)
        // _ShortLongLog(graph.SearchAll("강원도", "전라도", Graph<string>.SearchPolicy.Pass));

        // // 충청도 to 제주도 short (22) 충청도 > 경상도(8) > 제주도(14)
        // // 충청도 to 제주도 long (96) 충청도 > 경기도(5) > 강원도(7) > 경기도(5) > 충청도(5) > 경상도(8) > 강원도(13) > 경상도(13) > 전라도(6) > 충청도(7) > 전라도(7) > 경상도(6) > 제주도(14)
        // _ShortLongLog(graph.SearchAll("충청도", "제주도", Graph<string>.SearchPolicy.Pass));


        // void _ShortLongLog<T>(List<GraphPath<T>> _paths)
        // {
        //     if (_paths == null || _paths.Count < 1)
        //         return;

        //     var _sPath = _paths[0];
        //     var _lPath = _sPath;

        //     var _sWeight = _sPath.GetTotalWeight();
        //     var _lWeight = _sWeight;

        //     for (int index = 1; index < _paths.Count; ++index)
        //     {
        //         var _path = _paths[index];
        //         var _weight = _path.GetTotalWeight();

        //         if (_weight < _sWeight)
        //         {
        //             _sPath = _path;
        //             _sWeight = _weight;
        //         }
        //         else if (_lWeight < _weight)
        //         {
        //             _lPath = _path;
        //             _lWeight = _weight;
        //         }
        //     }

        //     _PathLog($"{_sPath.Start.Vertex} to {_sPath.End.Vertex} short ", _sPath);
        //     _PathLog($"{_lPath.Start.Vertex} to {_lPath.End.Vertex} long ", _lPath);
        // }

        // void _PathLog<T>(string tag, GraphPath<T> _path)
        // {
        //     if (_path.IsNoWay)
        //     {
        //         Log($"{tag}(0) {_path.Start.Vertex} // {_path.End.Vertex}");
        //         return;
        //     }

        //     var sb = new StringBuilder();
        //     sb.Append(_path.Start.Vertex);
        //     var _curNode = _path.Start;
        //     int _total = 0;
        //     for (int index = 1; index < _path.Count; ++index)
        //     {
        //         _curNode.TryGetValue(_path.Vertexs[index], out var _edge);
        //         _curNode = _edge.Node;
        //         _total += _edge.Weight;

        //         sb.Append($" > {_curNode.Vertex}({_edge.Weight})");
        //     }

        //     Log($"{tag}({_total}) {sb}");
        // }
    }
}
