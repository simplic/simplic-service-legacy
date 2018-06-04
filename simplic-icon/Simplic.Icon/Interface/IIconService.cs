using System;
using System.Collections.Generic;

namespace Simplic.Icon
{
    /// <summary>
    /// Responsible for Icon Management    
    /// </summary>
    public interface IIconService
    {
        /// <summary>
        /// Gets an icon object of a given Guid
        /// </summary>
        /// <param name="id">Icon Id</param>
        /// <returns><see cref="Icon"/></returns>
       Icon GetByIdAsIcon(Guid id);

        /// <summary>
        /// Gets a image of an icon of a given Guid
        /// </summary>
        /// <param name="id">Icon Id</param>
        /// <returns>Icon as <see cref="System.Windows.Media.Imaging.BitmapImage"/></returns>
        System.Windows.Media.Imaging.BitmapImage GetByIdAsImage(Guid id);

        /// <summary>
        /// Gets a byte array of an icon of a given Guid
        /// </summary>
        /// <param name="id">Icon Id</param>
        /// <returns>Icon as Byte[]</returns>
        byte[] GetById(Guid id);

        /// <summary>
        /// Gets a byte array of an icon of a given name
        /// </summary>
        /// <param name="name">Icon name</param>
        /// <returns>Icon as Byte[]</returns>
        byte[] GetByName(string name);

        /// <summary>
        /// Returns a <see cref="System.Windows.Media.Imaging.BitmapImage"/> by a given name of an icon
        /// </summary>
        /// <param name="name">Icon name to be searched</param>
        /// <returns><see cref="System.Windows.Media.Imaging.BitmapImage"/> by a given name of an icon</returns>
        System.Windows.Media.Imaging.BitmapImage GetByNameAsImage(string name);

        /// <summary>
        /// Gets a list of icons filtered out by their name
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        IEnumerable<Icon> Search(string searchText);

        /// <summary>
        /// Gets all icons in the database
        /// </summary>
        /// <returns>A list of <see cref="Icon"/></returns>
        IEnumerable<Icon> GetAll();

        /// <summary>
        /// Inserts or updates an icon
        /// </summary>
        /// <param name="icon">Icon object to save</param>
        /// <returns>true if successful</returns>
        bool Save(Icon icon);

        /// <summary>
        /// Deletes an icon from db, cache and file system
        /// </summary>
        /// <param name="id">Icon Id</param>
        /// <returns>true if successful</returns>
        bool Delete(Guid id);

        /// <summary>
        /// Deletes an icon from db, cache and file system
        /// </summary>
        /// <param name="id">Icon Id</param>
        /// <returns>true if successful</returns>
        bool Delete(Icon icon);

        /// <summary>
        /// Generates a checksum looping through all bytes it has given asa parameter        
        /// </summary>
        /// <param name="icons">List of bytes</param>
        /// <returns>Checksum hash</returns>
        string GenerateChecksum(IList<byte[]> icons);

        /// <summary>
        /// Saves the checksum value to the database (in the configuration table)
        /// </summary>
        /// <param name="checksum">Checksum hash to be saved</param>
        void SaveChecksumToDb(string checksum);

        /// <summary>
        /// Compares checksums between db and filesystem. If they differ, 
        /// reads all the icons from the database and writes them down 
        /// and creates a new checksum
        /// </summary>
        /// <returns>true if successful</returns>
        bool SyncFilesFromDatabase();

        /// <summary>
        /// Get system icon by file extension
        /// </summary>
        /// <param name="name">File extension</param>
        /// <param name="size">Icon size</param>
        /// <param name="linkOverlay">Using an icon overlay</param>
        /// <returns>Icon system (system drawing)</returns>
        System.Drawing.Icon GetSystemIcon(string name, SystemIconSize size = SystemIconSize.Large, bool linkOverlay = false);

        /// <summary>
        /// Get system icon by file extension or system name as image
        /// </summary>
        /// <param name="name">System icon name or file extension</param>
        /// <param name="size">Icon size</param>
        /// <param name="linkOverlay">Using an icon overlay</param>
        /// <returns>Icon as image (system drawing)</returns>
        System.Windows.Controls.Image GetSystemIconAsImage(string name, SystemIconSize size = SystemIconSize.Large, bool linkOverlay = false);
    }
}
