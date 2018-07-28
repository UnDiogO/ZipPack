namespace Android.Content
{
    /// <summary>
    /// To be added.
    /// </summary>
    public static class AndroidClipDataExtensions
    {
        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="clipData"></param>
        /// <returns></returns>
        public static Net.Uri[] ToUriArray(this ClipData clipData)
        {
            var array = new Net.Uri[clipData.ItemCount];
            for (var i = 0; i < clipData.ItemCount; i++)
            {
                array[i] = clipData.GetItemAt(i).Uri;
            }
            return array;
        }
    }
}