using System;

namespace AdvancedContentSecurity.Core.Testing
{
    public class AnonymousRepository : IAnonymousRepository
    {
        public T GetValue<T>(Func<T> func)
        {
            return func();
        }

        public TOut GetValue<TIn, TOut>(Func<TIn, TOut> func, TIn input)
        {
            return func(input);
        }

        public void Execute(Action action)
        {
            action();
        }
    }
}
