using System;

using Android.Content;
using Android.Media;

namespace ZipPack
{
    /// <summary>
    /// To be added.
    /// </summary>
    public class ZipFileBuilder : IDisposable
    {
        /// <summary>
        /// To be added.
        /// </summary>
        public Context Context { get; }

        /// <summary>
        /// To be added.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// To be added.
        /// </summary>
        public Java.Util.Zip.ZipOutputStream ZipStream { get; private set; }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileDestinationPath"></param>
        public ZipFileBuilder(Context context, string fileDestinationPath)
        {
            Context = context;
            FilePath = fileDestinationPath;

            var fileStream = new System.IO.FileStream(FilePath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            ZipStream = new Java.Util.Zip.ZipOutputStream(fileStream);
        }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        /// <exception cref="Java.Lang.IllegalArgumentException"/>
        public ZipFileBuilder WithComment(string comment)
        {
            ZipStream.SetComment(comment);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="Java.Lang.IllegalArgumentException"/>
        public ZipFileBuilder WithCompressionLevel(int level)
        {
            ZipStream.SetLevel(level);
            return this;
        }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        /// <exception cref="Java.Util.Zip.ZipException"/>
        /// <exception cref="Java.IO.IOException"/>
        public ZipFileBuilder AddFile(string fileName, byte[] fileData)
        {
            var entry = new Java.Util.Zip.ZipEntry(fileName);
            ZipStream.PutNextEntry(entry);
            ZipStream.Write(fileData);
            ZipStream.CloseEntry();
            return this;
        }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        /// <exception cref="Java.Util.Zip.ZipException"/>
        /// <exception cref="Java.IO.IOException"/>
        public ZipFileBuilder AddFile(string fileName, string fileData) =>
            AddFile(fileName, System.Text.Encoding.UTF8.GetBytes(fileData));

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Java.Util.Zip.ZipException"/>
        /// <exception cref="Java.IO.IOException"/>
        public ZipFileBuilder AddFile(string fileName, System.IO.Stream stream)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                stream.CopyTo(ms);
                return AddFile(fileName, ms.ToArray());
            }
        }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="scanCompletedListener"></param>
        /// <exception cref="Java.Util.Zip.ZipException"/>
        /// <exception cref="Java.IO.IOException"/>
        public void Build(MediaScannerConnection.IOnScanCompletedListener scanCompletedListener)
        {
            ZipStream.Close();
            ZipStream = null;
            MediaScannerConnection.ScanFile(Context, new string[] { FilePath }, new string[] { "application/zip" }, scanCompletedListener);
        }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <exception cref="Java.Util.Zip.ZipException"/>
        /// <exception cref="Java.IO.IOException"/>
        public void Build() => Build(null);

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) => ZipStream?.Dispose();

        /// <summary>
        /// To be added.
        /// </summary>
        public void Dispose() => Dispose(true);
    }
}