using System;

namespace Overlogger
{
    /// <summary>
    /// NotImplementedInReferenceAssemblyException
    /// Taken from Xamarin.Essentials
    /// </summary>
    public class NotImplementedInReferenceAssemblyException : NotImplementedException
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public NotImplementedInReferenceAssemblyException()
            : base("This functionality is not implemented in the portable version of this assembly. You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.")
        {
        }
    }
}