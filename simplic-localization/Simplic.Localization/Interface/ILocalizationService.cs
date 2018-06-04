using System.Collections.Generic;
using System.Globalization;

namespace Simplic.Localization
{
    /// <summary>
    /// Localization service returns strings based on language keys.
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Translates a key to a language
        /// </summary>
        /// <param name="key">Key to translate</param>
        /// <returns>Translated text</returns>
        string Translate(string key);

        /// <summary>
        /// Changes the current language
        /// </summary>
        /// <param name="language">Language to change</param>
        void ChangeLanguage(CultureInfo language);

        /// <summary>
        /// Returns a list of available languages
        /// </summary>
        /// <returns>a list of available languages</returns>
        IList<CultureInfo> GetAvailableLanguages();

        /// <summary>
        /// Gets the current language
        /// </summary>
        /// <returns><see cref="CultureInfo"/> Current language</returns>        
        CultureInfo CurrentLanguage { get; }

        /// <summary>
        /// Searches the key list and returns the matching keys
        /// </summary>
        /// <param name="searchKey">Search text</param>
        /// <returns>Result</returns>
        IDictionary<string, string> Search(string searchKey);

        /// <summary>
        /// Load localization from the database
        /// </summary>
        void LoadDatabaseLocalization();
    }
}
