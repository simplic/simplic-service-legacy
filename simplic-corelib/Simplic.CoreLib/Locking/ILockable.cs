using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Locking
{
    /// <summary>
    /// ILockable interface used for Simplic Locking
    /// </summary>
    public interface ILockable
    {
        Guid GetPersistantKey();
    }
}
