using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlightSim.AI
{
    public class ComplexAI : RoamingAI
    {
        PaperPlane plane;
        PaperPlane enemyTarget;
        PaperPlane allyFormationTarget;

        [SerializeField] float maximumShootingDistance = 500;
        [SerializeField] float maximumShootingAngle = 10;

        ActionNode enemySearch;
        ActionNode aimAtEnemy;
        ActionNode fireWeapons;
        ActionNode flyTowardsEnemy;
        ActionNode allySearch;
        ActionNode attemptFormation;
        Sequence enemySearchAndDestroy;
        Selector pursueEnemy;
        Sequence shootAtEnemy;
        Sequence formation;

        

        protected override void AddAdditionalNodes()
        {
            plane = GetComponent<PaperPlane>();
            
            aimAtEnemy = new ActionNode(AlignedWithEnemyTarget);
            fireWeapons = new ActionNode(FireWeapon);
            flyTowardsEnemy = new ActionNode(ChaseEnemyTarget);

            shootAtEnemy = new Sequence(new List<Node>() {aimAtEnemy, fireWeapons, flyTowardsEnemy});
            pursueEnemy = new Selector(new List<Node>() {shootAtEnemy, flyTowardsEnemy});

            enemySearch = new ActionNode(SearchForEnemies);
            enemySearchAndDestroy = new Sequence(new List<Node>() {enemySearch, pursueEnemy});
            rootChildren.Add(enemySearchAndDestroy);

            allySearch = new ActionNode(SearchForAllies);
            attemptFormation = new ActionNode(ChaseFormationTarget);
            formation = new Sequence(new List<Node>() {allySearch, attemptFormation});
            rootChildren.Add(formation);
        }


        NodeState SearchForEnemies()
        {
            if (enemyTarget != null && boundary.IsInsideBoundary(enemyTarget.transform.position))
                return NodeState.SUCCESS;
            
            PaperPlane.AllPlanes.UpdatePositions();
            PaperPlane target;
            
            target = PaperPlane.AllPlanes
                .FindAll(p => p.Squadron != plane.Squadron 
                              && !plane.Squadron.FriendlySquadrons.Contains(p.Squadron)
                              && !p.Squadron.FriendlySquadrons.Contains(plane.Squadron))
                .FindClosest(plane.transform.position);

            if (target == null || !boundary.IsInsideBoundary(target.transform.position)) return NodeState.FAILURE;

            enemyTarget = target;
            return NodeState.SUCCESS;
        }

        NodeState SearchForAllies()
        {
            if (allyFormationTarget != null && boundary.IsInsideBoundary(allyFormationTarget.transform.position))
                return NodeState.SUCCESS;
            
            PaperPlane.AllPlanes.UpdatePositions();
            PaperPlane target;
            
            target = PaperPlane.AllPlanes
                .FindAll(p => p.Squadron == plane.Squadron && p != plane && p.Rank < plane.Rank)
                .FindClosest(plane.transform.position);

            if (target == null || !boundary.IsInsideBoundary(target.transform.position)) return NodeState.FAILURE;

            allyFormationTarget = target;
            return NodeState.SUCCESS;
        }

        NodeState ChaseEnemyTarget()
        {
            SetTarget(enemyTarget.transform.position);
            return NodeState.RUNNING;
        }

        NodeState ChaseFormationTarget()
        {
            SetTarget(allyFormationTarget.transform.position - allyFormationTarget.transform.forward * 20);
            return NodeState.RUNNING;
        }
        
        NodeState FireWeapon()
        {
            Debug.Log("Pow!");
            GetComponent<AIShoot>().shoot();
            return NodeState.SUCCESS;
        }

        NodeState AlignedWithEnemyTarget()
        {
            if (Vector3.Distance(transform.position, enemyTarget.transform.position) > maximumShootingDistance)
            {
                return NodeState.FAILURE;
            }

            Vector3 directionToTarget = (enemyTarget.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            return angleToTarget < maximumShootingAngle ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}