using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Networking_EnemyAIController : MonoBehaviour
{
    [Range(0, 10f)]
    public float randomPointRadius = 2;
    public Vector3 randowPoint;
    private NavMeshAgent _agent;
    private NavMeshPath meshPath;

    private bool isComplete = true;

    public bool isStalking = false;

    private GameObject _currentPlayer;
    private Animator _animator;

    public GameObject soundRun;
    public GameObject soundIdle;

    public float killDistance = 1f;

    private void Start()
    {
        meshPath = new NavMeshPath();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = 6.9f;
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.GetComponent<NetworkingPlayerController>())
    //    {
    //        _currentPlayer = other.gameObject;
    //
    //        _animator.SetBool("isPlayer", true);
    //    }
    //}
    //
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.GetComponent<NetworkingPlayerController>())
    //    {
    //        _currentPlayer = null;
    //
    //        _animator.SetBool("isPlayer", false);
    //    }
    //}
    //
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.GetComponent<NetworkingPlayerController>())
    //    {
    //        isStalking = true;
    //    }
    //    else
    //    {
    //        isStalking = false;
    //
    //        _animator.SetBool("isPlayer", false);
    //    }
    //}

    public float raycastDistance = 10f;
    public float rotationSpeed = 10f;

    public bool CheckIfHit()
    {
        bool isHit = false;

        Vector3[] raycastDirections = { transform.forward, -transform.forward, transform.right, -transform.right };

        for (int i = 0; i < raycastDirections.Length; i++)
        {
            Vector3 raycastDirection = raycastDirections[i];
            RaycastHit hit;

            if (Physics.Raycast(transform.position, raycastDirection, out hit, raycastDistance))
            {
                NetworkingPlayerController hitController = hit.collider.gameObject.GetComponent<NetworkingPlayerController>();

                if (hitController != null)
                {
                    isHit = true;
                }
            }

            Debug.DrawRay(transform.position, raycastDirection * raycastDistance, isHit ? Color.red : Color.green);
        }

        return isHit;
    }

    void FixedUpdate()
    {
        CheckIfHit();

        //if (_currentPlayer)
        //{
        //    StalkingLOL();
        //        
        //    soundIdle.SetActive(false);
        //    soundRun.SetActive(true);
        //}
        //else
        //{
        //    soundIdle.SetActive(true);
        //    soundRun.SetActive(false);
        //
        //    Patrolling();
        //}
    }

    private void StalkingLOL()
    {
        transform.LookAt(_currentPlayer.transform.position);

        _agent.SetDestination(_currentPlayer.transform.position);

        if (Vector3.Distance(this.gameObject.transform.position, _currentPlayer.transform.position) <= killDistance)
            _currentPlayer.GetComponent<AnimateController>().Kill();
    }

    private void Patrolling()
    {
        if(isComplete)
        {
            NavMeshHit hit;

            if (NavMesh.SamplePosition(gameObject.transform.position + Random.insideUnitSphere * randomPointRadius, out hit, randomPointRadius, NavMesh.AllAreas))
            {
                randowPoint = new Vector3(hit.position.x, 0f, hit.position.z);

                _agent.CalculatePath(randowPoint, meshPath);

                if(meshPath.status == NavMeshPathStatus.PathComplete)
                    isComplete = false;

                if(meshPath.status == NavMeshPathStatus.PathInvalid)
                {
                    isComplete = true;
                    Patrolling();
                }
            }
        }else
        {
            _agent.SetDestination(randowPoint);

            if (gameObject.transform.position.x == randowPoint.x && gameObject.transform.position.z == randowPoint.z)
            {
                isComplete = true;
            }
        }
    }
}
