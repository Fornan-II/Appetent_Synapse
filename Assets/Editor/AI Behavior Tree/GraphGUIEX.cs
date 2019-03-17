using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphs;

namespace BehaviourTreeUI
{
    public class GraphGUIEX : GraphGUI
    {

        public new virtual Vector2 GetCenterPosition()
        {
            return base.GetCenterPosition();
        }

        public override void OnGraphGUI()
        {
            base.OnGraphGUI();

            if(graph is TreeGraph)
            {
                TreeGraph t = graph as TreeGraph;
                t.Validate();
            }
        }
    }
}