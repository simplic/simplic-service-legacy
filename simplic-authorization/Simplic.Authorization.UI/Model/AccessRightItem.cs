namespace Simplic.Authorization.UI
{
    public class AccessRightItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public AccessRightItemType ItemType { get; set; }
        public AccessRightType RightType { get; set; }
    }   
}
