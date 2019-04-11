using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AIGroup : AIGroupMember
    {
        [SerializeField] protected List<AIGroupMember> _members = new List<AIGroupMember>();
        public List<AIGroupMember> Members
        {
            get { return _members; }
        }

        public virtual void AddMember(AIGroupMember newMember)
        {
            Debug.Log("adding " + newMember);
            if(!_members.Contains(this))
            {
                if (newMember.MyGroup)
                {
                    newMember.MyGroup.RemoveMember(newMember);
                }

                _members.Add(newMember);
                newMember.MyGroup = this;
            }
        }

        public virtual void RemoveMember(AIGroupMember oldMember)
        {
            _members.Remove(oldMember);
            if (oldMember.MyGroup == this)
            {
                oldMember.MyGroup = null;
            }
        }

#if UNITY_EDITOR
        private AIGroupMember[] oldMembers = { };

        protected override void OnValidate()
        {
            if(_members.Contains(this))
            {
                _members.Remove(this);
                return;
            }

            base.OnValidate();

            foreach(AIGroupMember prevMem in oldMembers)
            {
                if(!_members.Contains(prevMem) && prevMem)
                {
                    if (prevMem.MyGroup == this)
                    {
                        prevMem.MyGroup = null;
                    }
                }
            }
            foreach (AIGroupMember mem in Members)
            {
                if (mem)
                {
                    if (mem.MyGroup != this && mem.MyGroup)
                    {
                        mem.MyGroup.RemoveMember(mem);
                    }
                    mem.MyGroup = this;
                }
            }

            oldMembers = _members.ToArray();
        }
#endif
    }
}