using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private List<Transform> points;
    public GameObject pointObject;
    private int _destPoint = 0;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Sensor _sensor;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        points =pointObject.GetComponentsInChildren<Transform>().ToList();
        points.RemoveAt(0);
        _agent.autoBraking = false;

        _sensor = GetComponentInChildren<Sensor>();

        _animator = GetComponent<Animator>();
        _animator.SetBool("run",true);
        GotoNextPoint();
    }
    void Update()
    {
        if (GameManager.Instance.currentState == GameStates.Failed ||
            GameManager.Instance.currentState == GameStates.Success)
        {
            _agent.isStopped = true;
            _animator.SetBool("run",false);
        }
        if (!_agent.pathPending && _agent.remainingDistance < 0.8f)
            GotoNextPoint();

        if (_sensor.ScanEnemy() != null)
        {
            GameManager.Instance.Failed();
        }
    }
    
    void GotoNextPoint() {
        if (points.Count == 0)
            return;
        _agent.destination = points[_destPoint].position;
        _destPoint = (_destPoint + 1) % points.Count;
    }
}
