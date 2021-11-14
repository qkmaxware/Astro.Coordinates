using Qkmaxware.Measurement;

namespace Qkmaxware.Astro.Coordinates {

/// <summary>
/// Coordinate represented spherical coordinates
/// </summary>
public class SphericalCoordinate {
    /// <summary>
    /// Angle measured from the vertical axis downwards
    /// </summary>
    /// <value>angle</value>
    public Angle ZenithAngle {get; set;}
    /// <summary>
    /// Angle measured around the Z axis starting from the X axis
    /// </summary>
    /// <value>angle</value>
    public Angle AzimuthalAngle {get; set;}
    /// <summary>
    /// Distance from the origin
    /// </summary>
    /// <value>length</value>
    public Length Radius {get; set;}

    public SphericalCoordinate() {
        ZenithAngle = Angle.Zero;
        AzimuthalAngle = Angle.Zero;
        Radius = Length.Zero;
    }

    public SphericalCoordinate(Angle zen, Angle az, Length r) {
        ZenithAngle = zen;
        AzimuthalAngle = az;
        Radius = r;    
    }

    /// <summary>
    /// Convert a spherical coordinate to a cartesian one
    /// </summary>
    /// <param name="coord">spherical coodinate</param>
    public static explicit operator CartesianCoordinate(SphericalCoordinate coord) {
        return new CartesianCoordinate (
            x: coord.Radius * (coord.ZenithAngle.Sin() * coord.AzimuthalAngle.Cos()),
            y: coord.Radius * (coord.ZenithAngle.Sin() * coord.AzimuthalAngle.Sin()),
            z: coord.Radius * (coord.ZenithAngle.Cos())
        );
    }   

    /// <summary>
    /// Convert a spherical coordinate to a cylindrical one
    /// </summary>
    /// <param name="coord">spherical coodinate</param>
    public static explicit operator CylindricalCoordinate (SphericalCoordinate coord) {
        return (CylindricalCoordinate)((CartesianCoordinate)coord);
    }
};

}