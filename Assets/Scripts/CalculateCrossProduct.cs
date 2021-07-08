using UnityEngine;
using Random = UnityEngine.Random;

public class CalculateCrossProduct : MonoBehaviour
{
    #region --Fields / Properties--

    /// <summary>
    /// Random negative to positive value range for each axis to generate random vectors from.
    /// </summary>
    [SerializeField]
    private float _range;
    
    /// <summary>
    /// How large to draw the quad.
    /// </summary>
    [SerializeField]
    private float _scale = 5f;
    
    /// <summary>
    /// All the vectors used to draw the quad.
    /// </summary>
    private Vector3 _vectorA;
    private Vector3 _vectorB;
    private Vector3 _aPlusB;
    private Vector3 _aPlusBToA;
    private Vector3 _aPlusBToB;
    
    /// <summary>
    /// Stores the result of the cross product between _vectorA and _vectorB.
    /// </summary>
    private Vector3 _resultant;
    
    /// <summary>
    /// Cached Transform component.
    /// </summary>
    private Transform _transform;
    
    #endregion
    
    #region --Unity Specific Methods--

    private void Start()
    {
        Init();
        DrawQuad();
    }

    private void Update()
    {
        DrawVectors();        
    }
    
    #endregion
    
    #region --Custom Methods--

    /// <summary>
    /// Initializes variables and caches components.
    /// </summary>
    private void Init()
    {
        _transform = transform;
        _vectorA = new Vector3(Random.Range(-_range, _range), Random.Range(-_range, _range), Random.Range(-_range, _range)).normalized * _scale;
        _vectorB = new Vector3(Random.Range(-_range, _range), Random.Range(-_range, _range), Random.Range(-_range, _range)).normalized * _scale;
        _aPlusB = (_vectorA + _vectorB);
        _aPlusBToA = (_vectorA - _aPlusB);
        _aPlusBToB = (_vectorB - _aPlusB);
        _resultant = CrossProductSimple(_vectorA, _vectorB);
    }

    /// <summary>
    /// Draws a primitive quad game object in between the 4 provided vectors.
    /// </summary>
    private void DrawQuad()
    {
        MeshRenderer _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter _meshFilter = gameObject.AddComponent<MeshFilter>();
        Mesh _mesh = new Mesh();

        Vector3[] _vertices =
                             {
                                 transform.position,
                                 _vectorA,
                                 _vectorB,
                                 _aPlusB
                             };
        _mesh.vertices = _vertices;
        
        int[] _triangles =
                         {
                             0, 1, 2,
                             2, 1 ,3
                             
                         };
        _mesh.triangles = _triangles;

        Vector3[] _normals = 
                            {
                                _resultant,
                                _resultant,
                                _resultant,
                                _resultant
                            };
        _mesh.normals = _normals;
        
        Vector2[] _uv = 
                       {
                           new Vector2(0, 0),
                           new Vector2(1, 0),
                           new Vector2(0, 1),
                           new Vector2(1, 1)
                       };
        _mesh.uv = _uv;
        
        _meshFilter.mesh = _mesh;
    }

    /// <summary>
    /// Calculates the cross product between the vectors provided.
    /// </summary>
    private Vector3 CrossProductSimple(Vector3 _vector1, Vector3 _vector2)
    {
        Vector3 _cross = Vector3.zero;
        _cross.x = (_vector1.y * _vector2.z) - (_vector1.z * _vector2.y);
        _cross.y = (_vector1.z * _vector2.x) - (_vector1.x * _vector2.z);
        _cross.z = (_vector1.x * _vector2.y) - (_vector1.y * _vector2.x);

        return _cross;
    }
    
    /// <summary>
    /// Draws debug rays so they can be viewed during runtime.
    /// </summary>
    private void DrawVectors()
    {
        Vector3 _position = _transform.position;
        Debug.DrawRay(_position, _vectorA, Color.cyan);
        Debug.DrawRay(_position, _vectorB, Color.yellow);
        Debug.DrawRay(_position, _resultant, Color.green);
        Debug.DrawRay(_aPlusB, _aPlusBToA, Color.blue);
        Debug.DrawRay(_aPlusB, _aPlusBToB, Color.red);
    }
    
    #endregion
    
}
