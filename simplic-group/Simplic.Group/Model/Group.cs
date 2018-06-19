namespace Simplic.Group
{
    public class Group
    {
        /// <summary>
        /// Gets or sets the Id of the row. (NOTE: This not the Group ID)
        /// </summary>
        public int Ident { get; set; }

        /// <summary>
        /// Gets or sets the group name, max length: 100
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the group id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets if the group is the default group
        /// </summary>
        public bool IsDefaultGroup { get; set; }
    }
}
