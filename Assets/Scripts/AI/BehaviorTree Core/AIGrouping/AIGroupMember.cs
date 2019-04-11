using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AIGroupMember : MonoBehaviour
    {
        public Blackboard localBlackboard;
        [SerializeField]protected AIGroup _myGroup;
        public AIGroup MyGroup
        {
            get { return _myGroup; }
            set
            {
                if(value)
                {
                    if (!value.Members.Contains(this))
                    {
                        value.AddMember(this);
                    }
                }
                _myGroup = value;
            }
        }

#if UNITY_EDITOR
        private AIGroup _oldGroup;

        protected virtual void OnValidate()
        {
            if (_myGroup != _oldGroup)
            {
                if (_oldGroup)
                {
                    _oldGroup.RemoveMember(this);
                }

                if (_myGroup)
                {
                    if (!_myGroup.Members.Contains(this))
                    {
                        _myGroup.AddMember(this);
                    }
                }
            }

            _oldGroup = _myGroup;
        }
#endif
    }
}