using System;
using Autofac.Builder;

namespace DemiCode.Mvvm.Autofac
{
    public static class RegistrationBuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InstancePerWindow<TLimit, TActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration)
        {
            if (registration == null)
                throw new ArgumentNullException("registration");

            return registration.InstancePerMatchingLifetimeScope(WindowLifetimeScopeProvider.WindowTag);
        }
    }
}