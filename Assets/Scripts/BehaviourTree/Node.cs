using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        //Stores important data to be used by leaf nodes
        //Data is shared between branches
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        //Constructor for Empty node (or root)
        public Node()
        {
            parent = null;
        }

        //Constructor with children
        public Node(List<Node> children)
        {
            foreach(Node child in children)
            {
                Attach(child);
            }
        }
        private void Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        //As data is shared we need to search the whole branch
        //Returns null if no data was found, and the value otherwise
        public object GetData(string key)
        {
            object value = null;

            //Check if the data is in this node
            if(_dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = parent;

            //Search up the branch
            while(node != null)
            {
                value = node.GetData(key);

                if(value != null)
                {
                    return value;
                }
                node = node.parent;
            }

            //No data with this key found
            return null;
        }

        //As data is shared we need to search the whole branch
        //Returns false if no data was found, and true otherwise
        public bool ClearData(string key)
        {
            //If data is in this node
            if (_dataContext.ContainsKey(key))
            {
                //Remove and return
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;

            //Check recursively up the branch
            while (node != null)
            {
                bool cleared = node.ClearData(key);

                if (cleared)
                {
                    return true;
                }

                node = node.parent;
            }

            //No data with this key was found
            return false;
        }

        //Eval function can be ovewritten (virtual) to fit the necessity
        public virtual NodeState Evaluate() => NodeState.FAILURE;
    }

}

