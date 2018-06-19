using Simplic.Framework.UI;
using Simplic.Icon;
using System.Windows.Media;

namespace Simplic.Authorization.UI
{
    public class AccessRightItemViewModel : ExtendableViewModel
    {
        private readonly IIconService iconService;        
        private readonly AccessRightItem model;
        public AccessRightItemViewModel(AccessRightItem model, IIconService iconService)
        {
            this.model = model;
            this.iconService = iconService;
        }

        public int Id
        {
            get { return model.Id; }
        }

        public string DisplayText
        {
            get { return model.DisplayText; }
        }

        public AccessRightItemType ItemType
        {
            get { return model.ItemType; }
        }

        public AccessRightType RightType
        {
            get { return model.RightType; }
            set { model.RightType = value; }
        }

        public ImageSource DisplayImage
        {
            get
            {
                return ItemType == AccessRightItemType.Group
                    ? iconService.GetByNameAsImage("group_32x")
                    : iconService.GetByNameAsImage("user_32x");
            }
        }
    }
}
