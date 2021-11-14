using System;
using Qkmaxware.Measurement;
using Qkmaxware.Numbers;

namespace Qkmaxware.Astro.Coordinates {

/// <summary>
/// Coordinates and velocity of an orbiting body in keplerian motion
/// </summary>
public class KeplerElements {
    private enum AnomalyType {
        Mean, True, Eccentric 
    }

    # region Elements
    /// <summary>
    /// The sum of the periapsis and apoapsis Lengths divided by two
    /// </summary>
    public Length SemimajorAxis {get; private set;}
    /// <summary>
    /// The sum of the periapsis and apoapsis Lengths divided by two
    /// </summary>
    public Length a {get => SemimajorAxis; private set => SemimajorAxis = value;}
    /// <summary>
    /// Vertical tilt of the ellipse with respect to the reference plane
    /// </summary>
    public Angle Inclination {get; private set;}
    /// <summary>
    /// Vertical tilt of the ellipse with respect to the reference plane
    /// </summary>
    public Angle i {get => Inclination; private set => Inclination = value;}
    /// <summary>
    /// Shape of the ellipse, describing how much it is elongated compared to a circle
    /// </summary>
    public double Eccentricity {get; private set;}
    /// <summary>
    /// Shape of the ellipse, describing how much it is elongated compared to a circle
    /// </summary>
    public double e {get => Eccentricity; private set => Eccentricity = value;}
    /// <summary>
    /// Horizontally orients the ascending node of the ellipse
    /// </summary>
    public Angle LongitudeOfAscendingNode {get; private set;}
    /// <summary>
    /// Horizontally orients the ascending node of the ellipse
    /// </summary>
    public Angle @Ω {get => LongitudeOfAscendingNode; private set => LongitudeOfAscendingNode = value;}
    /// <summary>
    /// The angle between the direction of periapsis and the current position of the body
    /// </summary>
    public Angle ArgumentOfPeriapsis {get; private set;}
    /// <summary>
    /// The angle between the direction of periapsis and the current position of the body
    /// </summary>
    public Angle w {get => ArgumentOfPeriapsis; private set => ArgumentOfPeriapsis = value;}

    private AnomalyType anomalyType;    
    private Angle anomalyValue;
    # endregion


    # region Anomaly Conversion Utilities
    /// <summary>
    /// Convert mean anomaly to true anomaly
    /// </summary>
    /// <param name="e">eccentricity</param>
    /// <param name="M">mean anomaly radians</param>
    /// <returns>true anomaly radians</returns>
    private static double mean2True(double e, double M) {
        // https://en.wikipedia.org/wiki/Mean_anomaly
        // This is a fourth order approximation. It is by no means precise
        var first = M;
        var second = (2d * e - (1d/4d) * (e * e * e)) * Math.Sin(M);
        var third = (5d/4d) * (e * e) * Math.Sin(2d * M);
        var fourth = (13d/12d) * (e * e * e) * Math.Sin(3d * M);
        
        return first + second + third + fourth;
    }

    /// <summary>
    /// Convert mean anomaly to true anomaly
    /// </summary>
    /// <param name="e">eccentricity</param>
    /// <param name="M">mean anomaly radians</param>
    /// <returns>eccentric anomaly radians</returns>
    private static double mean2Eccentric (double e, double M, int decimalPlaces = 8) {
        // Find roots of f(E) = E - e*sin(E) - M
        var iterations = 30; 
        var i = 0;
        var delta = Math.Pow(10, -decimalPlaces);

        // Newton's method        
        double En  = Math.PI; // initial guess

        while (i < iterations) {
            double En1 = En - (En - e * Math.Sin(En) - M) / (1 - e * Math.Cos(En));
            if (Math.Abs(En1 - En) < delta) {
                En = En1;
                break;
            } else {
                En = En1;
                continue;
            }
        }

        return En;
    }

    /// <summary>
    /// Convert true anomaly to mean anomaly
    /// </summary>
    /// <param name="e">eccentricity</param>
    /// <param name="T">true anomaly radians</param>
    /// <returns>mean anomaly radians</returns>
    private static double true2Mean(double e, double T) {
        var E = true2Eccentric(e, T);
        return eccentricToMean(e, E);
    }

    /// <summary>
    /// Convert true anomaly to eccentric anomaly
    /// </summary>
    /// <param name="e">eccentricity</param>
    /// <param name="T">true anomaly radians</param>
    /// <returns>eccentric anomaly radians</returns>
    private static double true2Eccentric(double e, double T) {
        return Math.Atan2(
            Math.Sqrt(1 - e * e) * Math.Sin(T),
            e + Math.Cos(T)
        );
    }

    /// <summary>
    /// Convert eccentric anomaly to mean anomaly
    /// </summary>
    /// <param name="e">eccentricity</param>
    /// <param name="E">eccentric anomaly radians</param>
    /// <returns>mean anomaly radians</returns>
    private static double eccentricToMean(double e, double E) {
        return E - e * Math.Sin(E);
    }

    /// <summary>
    /// Convert eccentric anomaly to true anomaly
    /// </summary>
    /// <param name="e">eccentricity</param>
    /// <param name="E">eccentric anomaly radians</param>
    /// <returns>true anomaly radians</returns>
    private static double eccentric2True(double e, double E) {
        // https://en.wikipedia.org/wiki/Eccentric_anomaly
        var sqrt = Math.Sqrt((1 + e) / (1 - e)) * Math.Tan(E / 2);
        return 2 * Math.Atan(sqrt);
    }
    #endregion

    # region Anomalies
    /// <summary>
    /// Compute the mean anomaly
    /// </summary>
    /// <returns>estimated mean anomaly if provided anomaly was not mean</returns>
    public Angle MeanAnomaly() {
        switch (anomalyType) {
            case AnomalyType.Mean:
                return  anomalyValue;
            case AnomalyType.Eccentric:
                return Angle.Radians(eccentricToMean(Eccentricity, (double)anomalyValue.TotalRadians())).Wrap();
            case AnomalyType.True:
            default:
                return Angle.Radians(true2Mean(Eccentricity, (double)anomalyValue.TotalRadians())).Wrap();
        }
    }
    /// <summary>
    /// Compute the mean anomaly (alias of MeanAnomaly())
    /// </summary>
    /// <returns>estimated mean anomaly if provided anomaly was not mean</returns>
    public Angle M() => MeanAnomaly();
    /// <summary>
    /// Compute the true anomaly
    /// </summary>
    /// <returns>estimated true anomaly if provided anomaly was not true</returns>
    public Angle TrueAnomaly() {
        switch (anomalyType) {
            case AnomalyType.Mean:
                return Angle.Radians(mean2True(Eccentricity, (double)anomalyValue.TotalRadians())).Wrap();
            case AnomalyType.Eccentric:
                return Angle.Radians(eccentric2True(Eccentricity, (double)anomalyValue.TotalRadians())).Wrap();
            case AnomalyType.True:
            default:
                return anomalyValue;
        }
    }
    /// <summary>
    /// Compute the true anomaly (alias of TrueAnomaly())
    /// </summary>
    /// <returns>estimated true anomaly if provided anomaly was not true</returns>
    public Angle ν() => TrueAnomaly();
    /// <summary>
    /// Compute the eccentric anomaly
    /// </summary>
    /// <returns>estimated eccentric anomaly if provided anomaly was not eccentric</returns>
    public Angle EccentricAnomaly() {
        switch (anomalyType) {
            case AnomalyType.Mean:
                return Angle.Radians(mean2Eccentric(Eccentricity, (double)anomalyValue.TotalRadians())).Wrap();
            case AnomalyType.Eccentric:
                return anomalyValue;
            case AnomalyType.True:
            default:
                return Angle.Radians(true2Eccentric(Eccentricity, (double)anomalyValue.TotalRadians())).Wrap();
        }
    }
    /// <summary>
    /// Compute the eccentric anomaly (alias of EccentricAnomaly())
    /// </summary>
    /// <returns>estimated eccentric anomaly if provided anomaly was not eccentric</returns>
    public Angle E() => EccentricAnomaly();
    # endregion

    # region Derived Quantities
    # endregion

    # region Constructors
    /// <summary>
    /// Create a new orbital element collection
    /// </summary>
    /// <param name="a">semimajor-axis</param>
    /// <param name="i">angle of inclination</param>
    /// <param name="e">eccentricity</param>
    /// <param name="Ω">longitude of ascending node</param>
    /// <param name="ω">argument of periapsis</param>
    /// <param name="anomalyValue">anomaly value</param>
    /// <param name="anomalyType">type of anomaly</param>
    private KeplerElements (Length a, Angle i, double e, Angle @Ω, Angle @w, AnomalyType anomalyType, Angle anomalyValue) {
        this.a = a;
        this.i = i;
        this.e = e;
        this.Ω = Ω;
        this.w = w;
        this.anomalyType = anomalyType;
        this.anomalyValue = anomalyValue.Wrap();
    }

    /// <summary>
    /// Create a new orbital element collection using a mean anomaly
    /// </summary>
    /// <param name="a">semimajor-axis</param>
    /// <param name="i">angle of inclination</param>
    /// <param name="e">eccentricity</param>
    /// <param name="Ω">longitude of ascending node</param>
    /// <param name="ω">argument of periapsis</param>
    /// <param name="M">mean anomaly</param>
    public static KeplerElements FromMean(Length a, Angle i, double e, Angle @Ω, Angle @w, Angle M) {
        return new KeplerElements(
            a: a,
            i: i,
            e: e,
            Ω: Ω,
            w: w,
            anomalyType: AnomalyType.Mean,
            anomalyValue: M
        );
    }

    /// <summary>
    /// Create a new orbital element collection using a true anomaly
    /// </summary>
    /// <param name="a">semimajor-axis</param>
    /// <param name="i">angle of inclination</param>
    /// <param name="e">eccentricity</param>
    /// <param name="Ω">longitude of ascending node</param>
    /// <param name="ω">argument of periapsis</param>
    /// <param name="v">true anomaly</param>
    public static KeplerElements FromTrue(Length a, Angle i, double e, Angle @Ω, Angle @w, Angle v) {
        return new KeplerElements(
            a: a,
            i: i,
            e: e,
            Ω: Ω,
            w: w,
            anomalyType: AnomalyType.True,
            anomalyValue: v
        );
    }

    /// <summary>
    /// Create a new orbital element collection using a eccentric anomaly
    /// </summary>
    /// <param name="a">semimajor-axis</param>
    /// <param name="i">angle of inclination</param>
    /// <param name="e">eccentricity</param>
    /// <param name="Ω">longitude of ascending node</param>
    /// <param name="ω">argument of periapsis</param>
    /// <param name="E">eccentric anomaly</param>
    public static KeplerElements FromEccentric(Length a, Angle i, double e, Angle @Ω, Angle @w, Angle E) {
        return new KeplerElements(
            a: a,
            i: i,
            e: e,
            Ω: Ω,
            w: w,
            anomalyType: AnomalyType.Eccentric,
            anomalyValue: E
        );
    }

    private static readonly Scientific G = new Scientific(6.67384, -11);
    private static Scientific μ(Mass mass) {
        return G * mass.TotalKilograms();
    }

    /// <summary>
    /// Create orbital elements from cartesian state vector
    /// </summary>
    /// <param name="parent">mass of body being orbited</param>
    /// <param name="positionVector">position relative to the parent object</param>
    /// <param name="velocityVector">velocity relative to the parent object</param>    
    public static KeplerElements FromStateVector (Mass parent, Vec3<Length> positionVector, Vec3<Speed> velocityVector) {
        var position = positionVector.Map(x => (Scientific)x.TotalMetres());
        var velocity = velocityVector.Map(x => (Scientific)x.TotalMetresPerSecond());
        var distance = position.Length;
        var speed = velocity.Length;
        var M = parent;

        // Cartesian state vector to orbital element conversion
        var H = Vec3<Scientific>.Cross(position, velocity);
        var h = H.Length;
        var up = new Vec3<Scientific>(0,0,1);
        var N = Vec3<Scientific>.Cross(up, H);
        var n = N.Length;

        var E = (Vec3<Scientific>.Cross(velocity,H) / μ(M)) - (position / distance);
        var e = (double)E.Length;

        var energy = (speed * speed)/2 - μ(M)/distance;

        Scientific a; Scientific p;
        if (Math.Abs(e - 1.0) > double.Epsilon) {
            a = -μ(M) / (2 * energy);
            p = a * (1 - e * e);
        } else {
            p = (h * h) / μ(M);
            a = double.PositiveInfinity;
        }

        double i = Math.Acos((double)(H.Z / h));

        double eps = double.Epsilon;
        double Omega; double w;
        if (Math.Abs(i) < eps) {
            Omega = 0; // For non-inclined orbits, this is undefined, set to 0 by convention
            if (Math.Abs(e) < eps) {
                w = 0; // For circular orbits, place periapsis at ascending node by convention
            }
            else {
                w = Math.Acos((double)(E.X / e)); 
            }
        } else {
            Omega = Math.Acos((double)(N.X / n));
            if (N.Y < 0) {
                Omega = (2 * Math.PI) - Omega;
            }

            w = Math.Acos((double)(Vec3<Scientific>.Dot(N, E) / (n * e)));
        }

        double nu;
        if (Math.Abs(e) < eps) {
            if (Math.Abs(i) < eps) {
                nu = Math.Acos((double)(position.X / distance));
                if (velocity.X > 0) {
                    nu = (2 * Math.PI) - nu;
                }
            } else {
                nu = Math.Acos((double)(Vec3<Scientific>.Dot(N,position) / (n * distance)));
                if (Vec3<Scientific>.Dot(N,velocity) > 0) {
                    nu = (2 * Math.PI) - nu;
                }
            }
        } else {
            if (E.Z < 0) {
                w = (2 * Math.PI) - w;
            }

            nu = Math.Acos((double)(Vec3<Scientific>.Dot(E, position) / (e * distance)));
            if (Vec3<Scientific>.Dot(position,velocity) < 0) {
                nu = (2 * Math.PI) - nu;
            }
        }

        return new KeplerElements(
            a: Length.Metres(a),
            i: Angle.Radians(i),
            e: e,
            Ω: Angle.Radians(Omega),
            w: Angle.Radians(w),
            anomalyType: AnomalyType.True,
            anomalyValue: Angle.Radians(nu)
        );
    } 

    # endregion
}

}