using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Models
{
    public class AppPermissions
    {
        private readonly List<PermissionType> _permission = new List<PermissionType>();

        private AppPermissions()
        {

        }

        public static AppPermissions Instance { get; private set; } = new AppPermissions();

        public enum PermissionType
        {
            CourseLocation,
            FineLocation,
            Bluetooth,
            NFC,
            Camera,
        }

        public void SetGranted(PermissionType permission)
        {
            lock (_permission)
            {
                if (!_permission.Contains(permission))
                {
                    _permission.Add(permission);
                }
            }
        }

        public void SetRevoked(PermissionType permission)
        {
            lock (_permission)
            {
                if (_permission.Contains(permission))
                {
                    _permission.Remove(permission);
                }
            }
        }

        public bool HasPermission(PermissionType permission)
        {
            return _permission.Contains(permission);
        }
    }
}
