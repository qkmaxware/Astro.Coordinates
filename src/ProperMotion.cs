using Qkmaxware.Measurement;

namespace Qkmaxware.Astro.Coordinates {

/// <summary>
/// Coordinate represented celestial coordinates
/// </summary>
public class ProperMotion {
    /// <summary>
    /// Change in right ascension angle
    /// </summary>
    /// <value>angle</value>
    public Angle DeltaRightAscension {get; set;}
    /// <summary>
    /// Change in angle of declination
    /// </summary>
    /// <value>angle</value>
    public Angle DeltaDeclination {get; set;}
    /// <summary>
    /// Duration over which change occurs
    /// </summary>
    public Duration Duration {get; set;}

    public ProperMotion() {
        this.DeltaRightAscension = Angle.Zero;
        this.DeltaDeclination = Angle.Zero;
        this.Duration = Duration.Seconds(1);
    }

    public ProperMotion(Angle dra, Angle ddec, Duration duration) {
        this.DeltaRightAscension = dra;
        this.DeltaDeclination = ddec;
        this.Duration = duration;
    }
}

}