using System;
using System.Globalization;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;

using ZipPack;

using Apptest.Constants;

namespace Apptest
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            FindViewById<Button>(Resource.Id.open_document).Click += OpenDocumentClick;
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M && !this.CheckSelfPermissions())
            {
                RequestPermissions(this.GetRequestedPermissions(), AppConstants.RequestCodePermissions);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == AppConstants.RequestCodePermissions)
            {
                if (grantResults.Any(p => p == Permission.Denied))
                {
                    new AlertDialog.Builder(this)
                        .SetTitle(Android.Resource.String.DialogAlertTitle)
                        .SetMessage("Permissions are required.")
                        .SetCancelable(false)
                        .SetPositiveButton(Android.Resource.String.Ok, AlertDialogPermissionsRequiredClick)
                        .Create()
                        .Show();
                }
            }
        }

        private void AlertDialogPermissionsRequiredClick(object sender, DialogClickEventArgs e) => 
            RequestPermissions(this.GetRequestedPermissions(), AppConstants.RequestCodePermissions);

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == AppConstants.RequestCodeOpenDocument
                && resultCode == Result.Ok
                && data != null)
            {
                var fileDestination = data.ClipData == null ?
                    CreateZipFile(this, data.Data) : CreateZipFile(this, data.ClipData.ToUriArray());

                Toast.MakeText(this, fileDestination, ToastLength.Long).Show();
            }
        }

        private void OpenDocumentClick(object sender, EventArgs e) =>
            this.PerformJpgFileSearch(AppConstants.RequestCodeOpenDocument, true);

        private string CreateZipFile(Context context, params Android.Net.Uri[] files)
        {
            var fileDestinationPath = $"/sdcard/{DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)}.zip";
            var zip = new ZipFileBuilder(this, fileDestinationPath);

            foreach (var file in files)
            {
                using (var stream = context.ContentResolver.OpenInputStream(file))
                {
                    zip.AddFile($"{Guid.NewGuid().ToString("N")}.jpg", stream);
                }
            }
            zip.Build();

            return fileDestinationPath;
        }
    }
}