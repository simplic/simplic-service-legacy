using System;

namespace Simplic.Tracking
{
    /// <summary>
    /// Disables the Tracking for this Property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DisableTrackingAttribute : Attribute
    {
    }
}
