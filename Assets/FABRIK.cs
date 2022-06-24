using UnityEngine;
using static MathfIK;

public class FABRIK : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Vector3 _inDirection = Vector3.up;
    [SerializeField]
    private bool _isFixed = true;
    [SerializeField]
    private Transform[] _bones;
    
    private Vector3[] _positions;
    private float[] _lengths;
    private float _completeLength;

    private void Start()
    {
        _positions = new Vector3[ _bones.Length ];
        _lengths = new float[ _bones.Length - 1 ];
        _completeLength = 0f;

        _positions[ 0 ] = _bones[ 0 ].position;
        for ( int i = 0; i < _bones.Length - 1; i++ )
        {
            _positions[ i + 1 ] = _bones[ i + 1 ].position;
            _lengths[ i ] = Vector3.Distance( _positions[ i + 1 ], _positions[ i ] );
            _completeLength += _lengths[ i ];
        }
    }

    private void Update()
    {
        if ( _isFixed )
        {
            Vector3 direction = Normalize( _target.position - _positions[ 0 ], out float distance );

            if ( distance >= _completeLength )
            {
                for ( int i = 1; i < _positions.Length; i++ )
                    _positions[ i ] = _positions[ i - 1 ] + direction * _lengths[ i - 1 ];
            }
            else 
            {
                _positions[ _positions.Length - 1 ] = _target.position;
                for ( int i = _positions.Length - 2; i > 0; i-- )
                    _positions[ i ] = _positions [ i + 1 ] - Normalize( _positions[ i + 1 ] - _positions[ i ] ) * _lengths[ i ];
                
                for ( int i = 1; i < _positions.Length; i++ )
                    _positions[ i ] = _positions [ i - 1 ] - Normalize( _positions[ i - 1 ] - _positions[ i ] ) * _lengths[ i - 1 ];
            }
        }
        else
        {
            _positions[ _positions.Length - 1 ] = _target.position;
            for ( int i = _positions.Length - 2; i >= 0; i-- )
                _positions[ i ] = _positions [ i + 1 ] - Normalize( _positions[ i + 1 ] - _positions[ i ] ) * _lengths[ i ];
        }

        _bones[ 0 ].position = _positions[ 0 ];
        for (int i = 0; i < _positions.Length - 1; i++)
        {
            Quaternion rotation = _bones[ i ].rotation;
            Vector3 from = rotation * _inDirection, to = Normalize( _positions[ i + 1 ] - _positions[ i ] );
            _bones[ i ].rotation = ShortestArcRotation( from, to ) * rotation;
        }
    }
}