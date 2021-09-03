using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Sensor : MonoBehaviour
{
    public float distance = 10f;
    public float angle = 30;
    public Material mat;
    private Mesh _mesh;
    [Header("Scan Properties")] public LayerMask layers;
    public LayerMask occlusionLayers;
    private Collider[] _colliders;
    //
    private float _scanFrequency = 5;
    private int _count;
    private float _scanInterval;
    private float _scanTimer;

    void Awake()
    {
        //mesh generate
        GenerateMesh();
        //scan
        _scanInterval = 1.0f / _scanFrequency;
        _colliders = new Collider[2];
    }

    public GameObject ScanEnemy()
    {
        _count = Physics.OverlapSphereNonAlloc(transform.position, distance, _colliders, layers,
            QueryTriggerInteraction.Collide);
        for (int i = 0; i < _count; i++)
        {
            GameObject obj = _colliders[i].gameObject;
            if (IsInSight(obj))
            {
                return obj;
            }
        }

        return null;
    }

    private bool IsInSight(GameObject o)
    {
        Vector3 origin = transform.position;
        Vector3 dest = o.transform.position;
        Vector3 direction = dest - origin;
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }

        return true;
    }
    private void GenerateMesh()
    {
        var mr = GetComponent<MeshRenderer>();
        mr.material = mat;
        var mf = GetComponent<MeshFilter>();
        _mesh = CreateWedgeMesh();
        mf.mesh = _mesh;
        transform.localPosition = new Vector3(0, .01f, 0);
        transform.rotation = Quaternion.identity;
    }

    private Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 6;
        int numTriangles = segments;
        int numVertices = numTriangles * 3;
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 center = Vector3.zero;
        Vector3 left;
        Vector3 right;

        int vert = 0;

        Vector2[] uvs = new Vector2[numVertices];

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            left = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            right = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;
            // side
            vertices[vert++] = center;
            vertices[vert++] = left;
            vertices[vert++] = right;
            //segments angle increasing
            currentAngle += deltaAngle;
        }

        //Setting vertices to triangles array
        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        //Assigning mesh properties
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.uv = uvs;

        mesh.RecalculateNormals();

        return mesh;
    }
}