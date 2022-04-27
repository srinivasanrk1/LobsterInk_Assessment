using System;
using System.Collections;
using System.Collections.Generic;

namespace Adventure.API.System
{
    public class Node
    {
        public string label { get; set; }
        public string question { get; set; }
        public int? level { get; set; }
        public List<Node> children { get; set; } = new List<Node>();
    }

    public class AdventureGame
    {
        public string adventureName { get; set; }
        public Node node { get; set; } = new Node();
    }

}


