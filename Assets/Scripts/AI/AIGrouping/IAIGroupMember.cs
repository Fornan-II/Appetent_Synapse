using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public interface IAIGroupMember
    {
        void SetGroup(AIGroup group);
        AIGroup GetGroup();
    }
}