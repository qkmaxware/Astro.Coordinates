using Qkmaxware.Astro.Coordinates.Transformations;
using Qkmaxware.Measurement;

namespace Qkmaxware.Astro.Coordinates.Frames {

/// <summary>
/// Horizontal Observer coordinate system
/// </summary>
public class HorizontalObserver {
    /// <summary>
    /// Time of the observation
    /// </summary>
    public Moment ObservationTime {get; private set;}
    /// <summary>
    /// Location on the earth where the observation is taking place
    /// </summary>
    public GeographicCoordinate ObservationLocation {get; private set;}

    public HorizontalObserver(Moment timeOfObservation, GeographicCoordinate observationLocation) {
        this.ObservationTime = timeOfObservation;
        this.ObservationLocation = observationLocation;
    }

    /// <summary>
    /// Create a transformation from equatorial to horizontal
    /// </summary>
    /// <returns>transformation</returns>
    public ITransformation<HorizontalCoordinates, EquatorialCoordinate> TransformationTo(EquatorialCoordinate frame) {
        var gmst = Angle.Hours(this.ObservationTime.GreenwichMeanSiderealTime().TotalHours());
        // local mean sidereal time = GMST + east longitude
        var lmst = gmst + this.ObservationLocation.Longitude;
        return new HorizontalToEquatorial(lmst, this.ObservationLocation.Latitude);
    }
}

}