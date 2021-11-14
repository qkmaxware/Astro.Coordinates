using System;
using Qkmaxware.Astro.Coordinates.Transformations;
using Qkmaxware.Measurement;

namespace Qkmaxware.Astro.Coordinates.Frames {

/// <summary>
/// Ecliptic Geocentric coordinate system
/// </summary>
public class EclipticGeocentric {
    /// <summary>
    /// The obliquity of ecliptic for this coordinate frame
    /// </summary>
    public Angle ObliquityOfEcliptic {get; private set;}
    private double ε => (double)ObliquityOfEcliptic.TotalRadians();

    /// <summary>
    /// Ecliptic geocentric coordinate frame at J2000
    /// </summary>
    public static readonly EclipticGeocentric J2000 = new EclipticGeocentric(Angle.Degrees(23.4));

    /// <summary>
    /// Create a new coordiante frame with the given obliquity of ecliptic
    /// </summary>
    /// <param name="obliquity">obliquity of ecliptic</param>
    public EclipticGeocentric(Angle obliquity) {
        this.ObliquityOfEcliptic = obliquity;
    }

    /// <summary>
    /// Create a transformation from equatorial to ecliptical
    /// </summary>
    /// <returns>transformation</returns>
    public ITransformation<EclipticCoordinate, EquatorialCoordinate> TransformationTo(EquatorialGeocentric frame) {
        return new EclipticToEquatorial(this.ObliquityOfEcliptic);
    }
}

}