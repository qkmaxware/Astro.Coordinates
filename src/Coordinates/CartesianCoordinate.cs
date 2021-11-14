using Qkmaxware.Measurement;
using Qkmaxware.Numbers;

namespace Qkmaxware.Astro.Coordinates {

/// <summary>
/// Coordinate represented in cartesian coordinates
/// </summary>
public class CartesianCoordinate : Vec3<Length> {
    public CartesianCoordinate() : base(Length.Zero, Length.Zero, Length.Zero){}

    public CartesianCoordinate(Length x, Length y, Length z) : base(x,y,z) {}

    public CartesianCoordinate(Vec3<Length> position): base(position.X, position.Y, position.Z) {}

    /// <summary>
    /// Convert a cartesian coordinate to a cylindrical one
    /// </summary>
    /// <param name="coord">cylindrical coodinate</param>
    public static explicit operator CylindricalCoordinate (CartesianCoordinate coord) {
        return new CylindricalCoordinate {
                            // X^2 + Y^2
            RadialDistance = coord.X.MultiplyBy(coord.X) + coord.Y.MultiplyBy(coord.Y),
            AzimuthalAngle = Angle.Radians(System.Math.Atan2((double)coord.Y.TotalKilometres(), (double)coord.X.TotalKilometres())),
            Height = coord.Z,
        };
    }

    /// <summary>
    /// Convert a cartesian coordinate to a spherical one
    /// </summary>
    /// <param name="coord">spherical coodinate</param>
    public static explicit operator SphericalCoordinate (CartesianCoordinate coord) {
        var r = (coord.X.MultiplyBy(coord.X) + coord.Y.MultiplyBy(coord.Y) + coord.Z.MultiplyBy(coord.Z)).Sqrt();
        return new SphericalCoordinate {
            ZenithAngle = Angle.Acos((double)coord.Z.DivideBy(r).TotalKilometres()),
            AzimuthalAngle = Angle.Radians(System.Math.Atan2((double)coord.Y.TotalKilometres(), (double)coord.X.TotalKilometres())),
            Radius = r,
        };
    }
};

}