using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class BST<T> : IEnumerable<T>
{
		public int Count { private set; get; } = 0;
		public BSTNode<T> Root { private set; get; } = null;
		public IComparer<T> Comparer { private set; get; } = null;

		public bool Contains(T value)
		{
			if(FindNode(Root, value) != null)   //FindNode해서 반환값이 null이 아니면 true
                return true;

            return false;
		}

        public BSTNode<T> FindNode(BSTNode<T> node, T value)        //노드 찾기 : 비교값이 작으면 왼쪽, 크면 오른쪽으로.
        {
            //이 부분에서 값을 잃어버림
            
            if(node == null)
                return null;
            else if((value.ToString()).CompareTo(node.Data.ToString()) < 0)
                return FindNode(node.LeftNode, value);
            else if((value.ToString()).CompareTo(node.Data.ToString()) > 0)
                return FindNode(node.RightNode, value);
            return node;
        }

		public BSTNode<T> Find(T value)
		{
            BSTNode<T> node = this.Root;
            return FindNode(node, value);
		}


        public void Display(BSTNode<T> node, List<T> list)  //DisPlay를 통해 리스트에 값을 넣고 IEnumerator로 전송
        {
            if(node == null)
                return;            

            Display(node.LeftNode, list);
            list.Add(node.Data);
            Display(node.RightNode, list);
        }

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<T> GetEnumerator()
		{   
            List<T> list = new List<T>();
            BSTNode<T> node = Root;
            Display(node, list);
            foreach(T i in list)
            {
                yield return i;
            }
            
		}

		public IEnumerator<T> GetOverlaps(T min, T max) // 재귀함수로 출력 or 리스트에 값을 받고 리턴
		{
				//...

            return null;
		}


		public BSTNode<T> Insert(T value)   //노드보다 작으면 왼쪽, 크면 오른쪽으로 이동하며 반복
		{
			BSTNode<T> node = this.Root;

            if(node == null)
            {
                this.Root = new BSTNode<T>(value);
                Count++;
                return node;
            }

            while(node != null)
            {
                //var Comp = Comparer.Compare(node.Data, value);
                var Comp = node.Data.ToString().CompareTo(value.ToString());
                switch(Comp)
                {
                    case -1:
                        if(node.LeftNode == null)
                        {
                            node.LeftNode = new BSTNode<T>(value);
                            Count++;
                            return node.LeftNode;
                        }
                        node = node.LeftNode;
                        break;

                    case 0:
                        throw new Exception("같은 숫자입니다.");

                    case 1:
                        if(node.RightNode == null)
                        {
                            node.RightNode = new BSTNode<T>(value);
                            Count++;
                            return node.RightNode;
                        }
                        node = node.RightNode;
                        break;
                }
            }
            throw new Exception("해당하는 케이스가 없습니다.");
		}

		public bool Remove(T value)
		{
            BSTNode<T> node = Find(value);
            
            // 이 아래로 작동하지 않아 주석 처리

            // if(node.LeftNode == null && node.RightNode == null)
            // {
            //     node = null;     // 이렇게 짜면 직접 정리하지 못함
            // }

            // else if(node.LeftNode != null && node.RightNode == null)
            // {
            //     node = node.LeftNode;
            // }

            // else if(node.RightNode != null && node.LeftNode == null)
            // {
            //     node = node.RightNode;
            // }

            // else
            // {
            //     BSTNode<T> P = null;        //부모 노드 저장

            //     BSTNode<T> temp = node.RightNode;   //진행 뱡항의 노드
                
            //     while(P.LeftNode != null)
            //     {
            //         P = temp;
            //         temp = temp.LeftNode;
            //     }

            //     node.Data = temp.Data;
                
            //     return true;
            // }

            return false;
		}
		public void Remove(BSTNode<T> node)
		{
				///...
		}

		public void Clear()     //후위 연산으로 데이터 확인하면서 날리기
		{
			if(Root.LeftNode == null && Root.RightNode == null)
            {
                Root = null;
                return;
            }    
            else if(Root.LeftNode == null && Root.RightNode != null)
            {
                Root = Root.RightNode;
                Clear();
            }
            else if(Root.LeftNode != null && Root.RightNode == null)
            {
                Root = Root.LeftNode;
                Clear();
            }
		}
}


public class BSTNode<T>
{
	public BSTNode<T> LeftNode {get; set;}

    public BSTNode<T> RightNode {get; set;}

    public T Data {get; set;}

    public BSTNode(T data)
    {
        this.Data = data;
    }
}

public class Task14 : StudyBase
{
    protected override void OnLog()
    {
        var bTree = new BST<int>();

        bTree.Insert(10);
        bTree.Insert(5);
        bTree.Insert(9);
        bTree.Insert(15);
        // 5 9 10 15
        bTree.LogValues();

        bTree.Insert(2);
        bTree.Remove(9);


        bTree.Insert(7);
        // 2 5 7 10 15
        bTree.LogValues();

        //     // 5 7 10
        //     bTree.GetOverlaps(5, 10).LogValues();
    }
}
