using UnityEngine;
using static UnityEngine.Vector3;
using static UnityEngine.Quaternion;

public class FABRIK : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Vector3 _inDirection = Vector3.up;
    [SerializeField]
    private Transform[] _bones;
    
    private Vector3[] _positions;
    private float[] _lengths;

    private void Start()
    {
        _positions = new Vector3[ _bones.Length ];
        _lengths = new float[ _bones.Length - 1 ];

        for ( int i = 0; i < _bones.Length - 1; i++ )
            _lengths[ i ] = Distance( _bones[ i + 1 ].position, _bones[ i ].position );
    }

    private void Update()
    {
        _positions[ 0 ]                     = _bones[ 0 ].position;
        _positions[ _positions.Length - 1 ] = _target.position;
        
        for ( int i = _positions.Length - 2; i > 0; i-- )
        _positions[ i ] = _positions[ i + 1 ] - Normalize( _positions[ i + 1 ] - _bones[ i ].position ) * _lengths[ i ];
    
        for ( int i = 0; i < _positions.Length - 1; i++ )
        {
            Vector3 from = _bones[ i ].rotation * _inDirection, to = Normalize( _positions[ i + 1 ] - _positions[ i ] );
            _positions[ i + 1 ] = _positions[ i ] + to * _lengths[ i ];
            _bones[ i ].rotation = FromToRotation( from, to ) * _bones[ i ].rotation;
        }
    }
}
