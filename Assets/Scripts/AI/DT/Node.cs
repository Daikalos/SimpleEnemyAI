using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public abstract class Node
    {
        public int ID { get; }

        public Node(int id)
        {
            ID = id;
        }

        public abstract bool Evaluate();
    }
}
