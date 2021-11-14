using Qkmaxware.Measurement;

namespace Qkmaxware.Astro.Coordinates {

/// <summary>
/// Coordinate represented cylindrical coordinates
/// </summary>
public class CylindricalCoordinate {
    /// <summary>
    /// Distance from the origin to the coordinate projected onto the XY plane
    /// </summary>
    /// <value>length</value>
    public Length RadialDistance {get; set;}
    /// <summary>
    /// Angle measured around the Z axis starting from the X axis
    /// </summary>
    /// <value>angle</value>
    public Angle AzimuthalAngle {get; set;}
    /// <summary>
    /// Height of the coordinate above the XY plane
    /// </summary>
    /// <value>length</value>
    public Length Height {get; set;}

    public CylindricalCoordinate() {
        RadialDistance = Length.Zero;
        AzimuthalAngle = Angle.Zero;
        Height = Length.Zero;
    }

    public CylindricalCoordinate(Length r, Angle az, Length h) {
        RadialDistance = r;
        AzimuthalAngle = az;
        Height = h;    
    }

    /// <summary>
    /// Convert a cylindrical coordinate to a cartesian one
    /// </summary>
    /// <param name="coord">cartesian coodinate</param>
    public static explicit operator CartesianCoordinate(CylindricalCoordinate coord) {
        return new CartesianCoordinate (
            x: coord.RadialDistance * coord.AzimuthalAngle.Cos(),
            y: coord.RadialDistance * coord.AzimuthalAngle.Sin(),
            z: coord.Height
        );
    }

    /// <summary>
    /// Convert a cylindrical coordinate to a spherical one
    /// </summary>
    /// <param name="coord">spherical coodinate</param>
    public static explicit operator SphericalCoordinate (CylindricalCoordinate coord) {
        return (SphericalCoordinate)((CartesianCoordinate)coord);
    }
};

}