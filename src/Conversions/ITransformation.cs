namespace Qkmaxware.Astro.Coordinates.Transformations {

/// <summary>
/// Generic transformation between to kinds of coordiantes
/// </summary>
/// <typeparam name="A">input coodiante type</typeparam>
/// <typeparam name="B">result coordiante type</typeparam>
public interface ITransformation<A, B> {
    /// <summary>
    /// Forward transformation
    /// </summary>
    /// <param name="from">the inital coordinate</param>
    /// <returns>result coordiante</returns>
    B Transform(A from);
    /// <summary>
    /// Reverse transformation
    /// </summary>
    /// <param name="to">the result coordinate</param>
    /// <returns>initial coordinate</returns>
    A Transform(B to);
}

/// <summary>
/// Compound sequence of transformations
/// </summary>
/// <typeparam name="A">input coodiante type</typeparam>
/// <typeparam name="B">intermediate coodiante type</typeparam>
/// <typeparam name="C">result coodiante type</typeparam>
public class TransformationSequence<A, B, C> : ITransformation<A,C> {
    private ITransformation<A,B> first;
    private ITransformation<B,C> second;

    public TransformationSequence(ITransformation<A,B> first, ITransformation<B,C> second) {
        this.first = first;
        this.second = second;
    }

    public C Transform (A from) => second.Transform(first.Transform(from));
    public A Transform (C from) => first.Transform(second.Transform(from));
}

/// <summary>
/// Extension methods for easy construction of transformations
/// </summary>
public static class ITransformationExtensions {
    /// <summary>
    /// Apply one transformation and then another
    /// </summary>
    /// <param name="first">first transformation</param>
    /// <param name="second">second transformation</param>
    /// <typeparam name="A">input coodiante type</typeparam>
    /// <typeparam name="B">intermediate coodiante type</typeparam>
    /// <typeparam name="C">result coodiante type</typeparam>
    /// <returns>sequenced transformation</returns>
    public static ITransformation<A,C> Then<A,B,C>(this ITransformation<A,B> first, ITransformation<B,C> second) {
        return new TransformationSequence<A,B,C>(first, second);
    }
}

}