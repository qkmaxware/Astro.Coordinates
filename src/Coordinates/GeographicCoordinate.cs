using Qkmaxware.Measurement;

namespace Qkmaxware.Astro.Coordinates {

/// <summary>
/// Coordiante system for geographic coordinates
/// </summary>
public class GeographicCoordinate {
    /// <summary>
    /// Angle of longitude
    /// </summary>
    public Angle Longitude {get; private set;}
    /// <summary>
    /// Angle of latitude
    /// </summary>
    public Angle Latitude {get; private set;}
    /// <summary>
    /// Altitude
    /// </summary>
    public Length Altitude {get; private set;}

    /// <summary>
    /// Create a given geographic coordinate with the given latitude and longitude
    /// </summary>
    /// <param name="lat">latitude</param>
    /// <param name="lon">longitude</param>
    /// <returns>coordinate</returns>
    public GeographicCoordinate(Angle lat, Angle lon) : this(lat, lon, Length.Zero) {}

    /// <summary>
    /// Create a given geographic coordinate with the given latitude, longitude, and altitude
    /// </summary>
    /// <param name="lat">latitude</param>
    /// <param name="lon">longitude</param>
    /// <param name="alt">altitude</param>
    /// <returns>coordinate</returns>
    public GeographicCoordinate(Angle lat, Angle lon, Length alt) {
        this.Latitude = lat;
        this.Longitude = lon;
        this.Altitude = alt;
    } 
}

}