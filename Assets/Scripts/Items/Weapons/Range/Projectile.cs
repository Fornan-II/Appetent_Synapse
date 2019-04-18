using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public const float DefaultMaxTravelDistance = 128.0f;

    public float AfterCollisionLifeTime = -1.0f;
    public bool UseGravity = false;
    public bool FreezePosition = false;
 
    protected Vector3 _velocity;
    protected DamagePacket _damage;
    protected LayerMask _hittable;
    protected Collider _col;
    protected Pawn _source;
    protected float _maxTravelDistance = DefaultMaxTravelDistance;
    protected float _distanceTravelled = 0.0f;
    
    protected virtual void Start()
    {
        _col = GetComponent<Collider>();
    }

    public virtual void Initialize(Vector3 initVelocity, DamagePacket dmg, LayerMask hittable, Pawn source, float maxDistance = DefaultMaxTravelDistance)
    {
        _velocity = initVelocity;
        _damage = dmg;
        _hittable = hittable;
        _source = source;
        _maxTravelDistance = maxDistance;
    }

    protected virtual void FixedUpdate()
    {
        if(FreezePosition)
        {
            return;
        }

        if(_distanceTravelled > _maxTravelDistance)
        {
            Destroy(gameObject);
            return;
        }

        if (UseGravity)
        {
            _velocity += Physics.gravity * Time.fixedDeltaTime;
        }
        Vector3 UpdateVelocity = _velocity * Time.fixedDeltaTime;

        transform.forward = UpdateVelocity;

        float UpdateDistance = UpdateVelocity.magnitude;
        _distanceTravelled += UpdateDistance;

        RaycastHit hit;

        if(_col)
        {
            if(_col is SphereCollider)
            {
                float r = (_col as SphereCollider).radius;
                if(Physics.SphereCast(transform.position, r, UpdateVelocity, out hit, UpdateDistance, _hittable, QueryTriggerInteraction.Ignore))
                {
                    CollideWith(hit);
                    transform.position = hit.point;
                    return;
                }
            }
            else if(_col is BoxCollider)
            {
                Vector3 halfExtents = (_col as BoxCollider).size * 0.5f;
                if(Physics.BoxCast(transform.position, halfExtents, UpdateVelocity, out hit, transform.rotation, UpdateDistance, _hittable, QueryTriggerInteraction.Ignore))
                {
                    CollideWith(hit);
                    transform.position = hit.point;
                    return;
                }
            }
            else if(_col is CapsuleCollider)
            {
                CapsuleCollider capsule = _col as CapsuleCollider;
                //Long, painful, and arduous journey to find point1 and point2 of a capsule.
                //
                float centerToPointsDistance = (capsule.height * 0.5f) - capsule.radius;
                Vector3 point1 = transform.position + transform.right * capsule.center.x + transform.up * capsule.center.y + transform.forward * capsule.center.z;
                Vector3 point2 = transform.position + transform.right * capsule.center.x + transform.up * capsule.center.y + transform.forward * capsule.center.z;
                //X = 0, Y = 1, Z = 2
                switch (capsule.direction)
                {
                    case 0:
                        {
                            Vector3 offset = transform.right * centerToPointsDistance;
                            point1 += offset;
                            point2 -= offset;
                            break;
                        }
                    case 1:
                        {
                            Vector3 offset = transform.up * centerToPointsDistance;
                            point1 += offset;
                            point2 -= offset;
                            break;
                        }
                    case 2:
                        {
                            Vector3 offset = transform.forward * centerToPointsDistance;
                            point1 += offset;
                            point2 -= offset;
                            break;
                        }
                }
                //

                if (Physics.CapsuleCast(point1, point2, capsule.radius, UpdateVelocity, out hit, UpdateDistance, _hittable, QueryTriggerInteraction.Ignore))
                {
                    CollideWith(hit);
                    transform.position = hit.point;
                    return;
                }
            }
        }

        //Else
        if (Physics.Raycast(transform.position, UpdateVelocity, out hit, UpdateDistance, _hittable, QueryTriggerInteraction.Ignore))
        {
            CollideWith(hit);
            transform.position = hit.point;
        }
        else
        {
            transform.position += UpdateVelocity;
        }
    }

    protected virtual void CollideWith(RaycastHit hitInfo)
    {
        DamageReciever.DealDamageToTarget(hitInfo.transform.gameObject, _damage, _source);
        if(AfterCollisionLifeTime >= 0)
        {
            Destroy(gameObject, AfterCollisionLifeTime);
        }
        FreezePosition = true;
        transform.parent = hitInfo.transform;
    }
}
