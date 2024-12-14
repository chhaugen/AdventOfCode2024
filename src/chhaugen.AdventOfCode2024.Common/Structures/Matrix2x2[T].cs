using System.Numerics;

namespace chhaugen.AdventOfCode2024.Common.Structures;
public readonly struct Matrix2x2<T>
    where T : INumber<T>
{
    public Matrix2x2(T a, T b, T c, T d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    public T A { get; }
    public T B { get; }
    public T C { get; }
    public T D { get; }

    public Matrix2x2<TReturn> Cast<TReturn>(Func<T, TReturn> caster)
        where TReturn : INumber<TReturn>
        => new(caster(A), caster(B), caster(C), caster(D));
}
