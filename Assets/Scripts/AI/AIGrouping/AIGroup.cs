using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AIGroup : IAIGroupMember
    {
        public List<IAIGroupMember> Members;

        protected AIGroup parentGroup;

        public void SetGroup(AIGroup group)
        {
            parentGroup = group;
        }

        public AIGroup GetGroup()
        {
            return parentGroup;
        }
    }
}