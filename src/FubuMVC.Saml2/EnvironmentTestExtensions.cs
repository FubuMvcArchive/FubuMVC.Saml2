using System;
using Bottles.Diagnostics;
using FubuCore;

namespace FubuMVC.Saml2
{
    public static class EnvironmentTestExtensions
    {
        public static T VerifyRegistration<T>(this IServiceLocator services, IPackageLog log)
        {
            try
            {
                var service = services.GetInstance<T>();
                log.Trace("Using {0} for {1}", service.GetType().FullName, typeof(T).FullName);

                return service;
            }
            catch (Exception ex)
            {
                log.MarkFailure("Could not resolve " + typeof(T).FullName);
                log.MarkFailure(ex);

                return default(T);
            }
        }    
    }
}