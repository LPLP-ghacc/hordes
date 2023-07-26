using UnityEngine;
using UnityEngine.AI;

public class NetEntityController : MonoBehaviour
{
    [Range(0, 10f)]
    public float randomPointRadius = 2;
    public Vector3 randowPoint;
    public float raycastDistance = 10f;
    public float killDistance = 1f;

    public GameObject soundRun;
    public GameObject soundIdle;

    private NavMeshAgent _agent;
    private NavMeshPath meshPath;
    private Animator _animator;
    private GameObject _player;
    private bool _pathComplete;

    private void Update()
    {
        if(_player == null)
        {
            _player = CheckIfHit();
            Lurking();
            _animator.SetBool("isPlayer", false);
        }
        else
        {
            Chase();
            _animator.SetBool("isPlayer", true);
        }
    }

    private void Chase()
    {
        transform.LookAt(_player.transform.position);
    }

    private void Lurking()
    {
        if (!_pathComplete)
        {
            NavMeshHit hit;

            if (NavMesh.SamplePosition(gameObject.transform.position + Random.insideUnitSphere * randomPointRadius, out hit, randomPointRadius, NavMesh.AllAreas))
            {
                randowPoint = new Vector3(hit.position.x, 0f, hit.position.z);

                _agent.CalculatePath(randowPoint, meshPath);

                if (meshPath.status == NavMeshPathStatus.PathComplete)
                {
                    _pathComplete = true;
                }
                else if(meshPath.status == NavMeshPathStatus.PathInvalid)
                {
                    Lurking();
                }
            }
        }else
        {
            _agent.SetDestination(randowPoint);

            if (gameObject.transform.position.x == randowPoint.x && gameObject.transform.position.z == randowPoint.z)
            {
                _pathComplete = true;
            }
        }
    }

    public GameObject CheckIfHit()
    {
        GameObject player = null;

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
                    player = hit.collider.gameObject;
                    isHit = true;
                }
            }

            Debug.DrawRay(transform.position, raycastDirection * raycastDistance, isHit ? Color.red : Color.green);
        }

        return player;
    }
}
