using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public abstract class Node
    {
        /// <summary>
        /// Return true, nothing went wrong <br/>
        /// Return false, something went wrong
        /// </summary>
        public abstract bool Evaluate();
    }
}
