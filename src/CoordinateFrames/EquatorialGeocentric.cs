using System;
using Qkmaxware.Astro.Coordinates.Transformations;
using Qkmaxware.Measurement;

namespace Qkmaxware.Astro.Coordinates.Frames {

/// <summary>
/// Equatorial Geocentric coordinate system
/// </summary>
public class EquatorialGeocentric {

    /// <summary>
    /// Create a new coordiante frame with the given obliquity of ecliptic
    /// </summary>
    /// <param name="obliquity">obliquity of ecliptic</param>
    public EquatorialGeocentric() {}
    
    /// <summary>
    /// Create a transformation from equatorial to ecliptical
    /// </summary>
    /// <returns>transformation</returns>
    public ITransformation<EquatorialCoordinate, EclipticCoordinate> TransformationTo(EclipticGeocentric frame) {
        return new EquatorialToEcliptic(frame.ObliquityOfEcliptic);
    }

    /// <summary>
    /// Create a transformation from equatorial to horizontal
    /// </summary>
    /// <returns>transformation</returns>
    public ITransformation<EquatorialCoordinate, HorizontalCoordinates> TransformationTo(HorizontalObserver frame) {
        var gmst = Angle.Hours(frame.ObservationTime.GreenwichMeanSiderealTime().TotalHours);
        // local mean sidereal time = GMST + east longitude
        var lmst = gmst + frame.ObservationLocation.Longitude;
        return new EquatorialToHorizontal(lmst, frame.ObservationLocation.Latitude);
    }
}

}