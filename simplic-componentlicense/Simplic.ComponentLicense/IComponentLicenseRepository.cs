namespace Simplic.ComponentLicense
{
    /// <summary>
    /// Component license repository (data)
    /// </summary>
    public interface IComponentLicenseRepository
    {
        /// <summary>
        /// Get component license by name
        /// </summary>
        /// <param name="name">Component name</param>
        /// <returns>License instance</returns>
        ComponentLicense Get(string name);
    }
}