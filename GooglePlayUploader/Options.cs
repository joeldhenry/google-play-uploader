namespace GooglePlayUploader
{
    using CommandLine;

    public class Options
    {
        [Option('a', "authFile", Required = true, HelpText = "Google Auth File")]
        public string AuthFile { get; set; }

        [Option('i', "appId", Required = true, HelpText = "App id")]
        public string AppID { get; set; }

        [Option('f', "appFile", Required = true, HelpText = "App File")]
        public string AppFile { get; set; }
    }
}
