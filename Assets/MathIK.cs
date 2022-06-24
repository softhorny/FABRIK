using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class MathfIK
{
    private const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

    [MethodImpl( INLINE )] 
    public static Vector3 Normalize( Vector3 value ) => 
        value * ( 1f / (float)System.Math.Sqrt( value.sqrMagnitude ) );

    [MethodImpl( INLINE )] 
    public static Vector3 Normalize( Vector3 value, out float magnitude ) => 
        value * ( 1f / ( magnitude = (float)System.Math.Sqrt( value.sqrMagnitude ) ) );
    
    [MethodImpl( INLINE )]
    public static Quaternion ShortestArcRotation( Vector3 from, Vector3 to )
    {
        Vector3 axis = Vector3.Cross( from, to );
        float w = Vector3.Dot( from, to ), m = axis.sqrMagnitude;
        w += (float)Math.Sqrt( m + w * w );

        if( w <= float.Epsilon )
        {
            if( ( from.z * from.z ) > ( from.x * from.x ) )
                return new Quaternion( 0f, from.z, -from.y, w );
            
            return new Quaternion( from.y, -from.x, 0f, w );
        }

        m = 1f / (float)Math.Sqrt( m + w * w );

        return new Quaternion( axis.x * m, axis.y * m, axis.z * m, w * m );
    }
}
