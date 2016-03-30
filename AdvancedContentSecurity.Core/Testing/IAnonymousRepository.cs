using System;

namespace AdvancedContentSecurity.Core.Testing
{
    public interface IAnonymousRepository
    {
        T GetValue<T>(Func<T> func);

        TOut GetValue<TIn, TOut>(Func<TIn, TOut> func, TIn input);
        void Execute(Action action);
    }
}