namespace GooglePlayUploader
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Google.Apis.Auth.OAuth2;
    using CommandLine;
    using Newtonsoft.Json;

    internal class Program
    {
        private static readonly string[] scopes = { "https://www.googleapis.com/auth/androidpublisher" };
        private static readonly HttpClient client = new HttpClient();
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed(options => Run(options).GetAwaiter().GetResult());
        }

        private static async Task Run(Options options)
        {
            await GoogleAuth(options.AuthFile);

            var editId = await CreateEdit(options.AppID);

            await UploadApp(options.AppID, editId, options.AppFile);

            await ConfirmEdit(options.AppID, editId);
        }

        private static async Task UploadApp(string AppID, string editId, string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException($"Cannot find AppBundle at the specified output path: {file}");

            var extension = Path.GetExtension(file);

            if (extension == ".apk")
                await UploadPackage(AppID, editId, file);

            if (extension == ".aab")
                await UploadBundle(AppID, editId, file);
        }

        private static async Task UploadPackage(string AppID, string editId, string appPackage)
        {
            var stream = new FileStream(appPackage, FileMode.Open, FileAccess.Read);

            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.android.package-archive");

            Console.WriteLine("Uploading Package");

            var response = await client.PostAsync($"https://www.googleapis.com/upload/androidpublisher/v3/applications/{AppID}/edits/{editId}/apks", content);

            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<GoogleError>(responseString).error.message;
                throw new InvalidDataException(error);
            }

            Console.WriteLine("Uploaded Package");
        }

        private static async Task UploadBundle(string AppID, string editId, string appBundle)
        {
            var stream = new FileStream(appBundle, FileMode.Open, FileAccess.Read);

            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            Console.WriteLine("Uploading Bundle");

            var response = await client.PostAsync($"https://www.googleapis.com/upload/androidpublisher/v3/applications/{AppID}/edits/{editId}/bundles", content);

            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<GoogleError>(responseString).error.message;
                throw new InvalidDataException(error);
            }

            Console.WriteLine("Uploaded Bundle");
        }

        private static async Task ConfirmEdit(string AppID, string editId)
        {
            var response = await client.PostAsync($"https://www.googleapis.com/androidpublisher/v3/applications/{AppID}/edits/{editId}:commit", null);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<GoogleError>(responseString).error.message;
                throw new InvalidDataException(error);
            }

            Console.WriteLine("Edit Success");
        }

        private static async Task<string> CreateEdit(string AppID)
        {
            var response = await client.PostAsync($"https://www.googleapis.com/androidpublisher/v3/applications/{AppID}/edits", null);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<GoogleError>(responseString).error.message;
                throw new InvalidDataException(error);
            }

            var editId = JsonConvert.DeserializeObject<Edit>(responseString).Id;

            if (editId == null)
                throw new InvalidOperationException("Failed to create new edit");

            Console.WriteLine("Created New Edit");

            return editId;
        }

        private static async Task GoogleAuth(string AuthFile)
        {
            if (!File.Exists(AuthFile))
                throw new FileNotFoundException($"Cannot find AuthFile at the specified output path: {AuthFile}");

            var stream = new FileStream(AuthFile, FileMode.Open, FileAccess.Read);

            var credential = GoogleCredential.FromStream(stream)
                            .CreateScoped(scopes)
                            .UnderlyingCredential as ServiceAccountCredential;

            var token = await credential.GetAccessTokenForRequestAsync();

            if (token == null)
                throw new InvalidDataException("Auth Failed");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Console.WriteLine("Authenticated to Google");
        }
    }
}
