using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard {

        public List<GameObject> enemies;
        public GameObject king;

        public MouseMove moveScript;

        public int nearbySoldiers;

        public int hp = 100;
        public int baseDmg = 15;
        public float fov = 10.0f;
        public float attackRng = 2.0f;


    }
}