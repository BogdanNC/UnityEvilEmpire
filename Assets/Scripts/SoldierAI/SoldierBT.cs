using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class SoldierBT : BehaviourTree.Tree
{
    private Transform king;

    protected override Node SetupTree()
    {
        king = GameObject.Find("King").transform;

        Node root = new TaskMoveTo(transform, king);

        return root;
    }
}
