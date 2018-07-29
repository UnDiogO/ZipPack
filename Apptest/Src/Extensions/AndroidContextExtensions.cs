using System.Linq;

using Android.Content.PM;

namespace Android.Content
{
    /// <summary>
    /// To be added.
    /// </summary>
    public static class AndroidContextExtensions
    {
        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string[] GetRequestedPermissions(this Context context) =>
            context
            .PackageManager
            .GetPackageInfo(context.PackageName, PackageInfoFlags.Permissions)
            .RequestedPermissions?
            .ToArray();

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static bool CheckSelfPermissions(this Context context, params string[] permissions) =>
            !permissions
            .Select(s => context.CheckSelfPermission(s))
            .Any(p => p == Permission.Denied);

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool CheckSelfPermissions(this Context context) =>
            CheckSelfPermissions(context, GetRequestedPermissions(context));
    }
}