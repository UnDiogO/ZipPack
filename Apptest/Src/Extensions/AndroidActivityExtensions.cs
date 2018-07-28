using Android.Content;

namespace Android.App
{
    /// <summary>
    /// To be added.
    /// </summary>
    public static class AndroidActivityExtensions
    {
        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="mimeDataType"></param>
        /// <param name="requestCode"></param>
        /// <param name="allowMultiple"></param>
        public static void PerformJpgFileSearch(this Activity activity, string mimeDataType, int requestCode, bool allowMultiple = false)
        {
            var intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.SetType(mimeDataType);
            intent.PutExtra(Intent.ExtraAllowMultiple, allowMultiple);
            activity.StartActivityForResult(intent, requestCode);
        }
    }
}