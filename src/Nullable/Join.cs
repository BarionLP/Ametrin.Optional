namespace Ametrin.Optional.Nullable;

public static class NullableJoinExtensions
{
    extension<T1>(T1? a) where T1 : class
    {
        public (T1, T2)? Join<T2>(T2? b)
            where T2 : class
            => a is null || b is null ? null : (a, b);

        public (T1, T2)? Join<T2>(T2? b)
            where T2 : struct
            => a is null || !b.HasValue ? null : (a, b.Value);
    }

    extension<T1>(T1? a) where T1 : struct
    {
        public (T1, T2)? Join<T2>(T2? b)
            where T2 : class
            => a.HasValue && b is not null ? (a.Value, b) : null;

        public (T1, T2)? Join<T2>(T2? b)
            where T2 : struct
            => a.HasValue && b.HasValue ? (a.Value, b.Value) : null;
    }

    extension<T1, T2>((T1, T2)? a)
    {
        public (T1, T2, T3)? Join<T3>(T3? b)
            where T3 : class
            => a.HasValue && b is not null ? (a.Value.Item1, a.Value.Item2, b) : null;
        
        public (T1, T2, T3)? Join<T3>(T3? b)
            where T3 : struct
            => a.HasValue && b is not null ? (a.Value.Item1, a.Value.Item2, b.Value) : null;
    }

    extension<T1, T2, T3>((T1, T2, T3)? a)
    {
        public (T1, T2, T3, T4)? Join<T4>(T4? b)
            where T4 : class
            => a.HasValue && b is not null ? (a.Value.Item1, a.Value.Item2, a.Value.Item3, b) : null;
        
        public (T1, T2, T3, T4)? Join<T4>(T4? b)
            where T4 : struct
            => a.HasValue && b is not null ? (a.Value.Item1, a.Value.Item2, a.Value.Item3, b.Value) : null;
    }

    extension<T1, T2, T3, T4>((T1, T2, T3, T4)? a)
    {
        public (T1, T2, T3, T4, T5)? Join<T5>(T5? b)
            where T5 : class
            => a.HasValue && b is not null ? (a.Value.Item1, a.Value.Item2, a.Value.Item3, a.Value.Item4, b) : null;
        
        public (T1, T2, T3, T4, T5)? Join<T5>(T5? b)
            where T5 : struct
            => a.HasValue && b is not null ? (a.Value.Item1, a.Value.Item2, a.Value.Item3, a.Value.Item4, b.Value) : null;
    }
}
