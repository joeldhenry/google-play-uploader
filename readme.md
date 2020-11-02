# Google Play Uploader

.Net Application to Upload APK or AAB to Google Play Console.

- Note: in order to use this application, you must [create a service acount](#create-google-service-account--auth-file)

### Build

- dotnet run

### Build

- dotnet build GooglePlayUploader -c Release

## Publish

- dotnet publish --runtime osx.10.14-x64
- mv bin/Debug/netcoreapp2.2/osx.10.14-x64/publish/GooglePlayUploader ./GooglePlayUploader

## Usage

Example: `GooglePlayUploader --authFile serviceAccount.json --appId au.com.google.google -appFile package.apk`

```
  -a, --authFile    Required. Google Auth File
  -i, --appId       Required. App id
  -f, --appFile     Required. App File
  --help            Display this help screen.
  --version         Display version information.
```

## Create Google Service Account & Auth File

1. Create a service account and get access keys json file
2. Open Google Play console. Go to “Settings” > “Developer account” > “API access”.
3. In “Service Accounts” section, choose “Create Service Account”.
4. You will be sent to “Google Cloud Plaform” console. Continue creating account there.
5. Give it a chosen name and ID.
6. Set “Service Account User” role.
7. Create private key and download it as a json file & rename. This will be our authFile.
8. No need to grant any additional access here.
9. Copy e-mail of a created user. Should look like some_id@api-12345–12345.iam.gserviceaccount.com
10. Return to “Service Accounts” in Google Play. Created account should be visible there. If not, invite it using saved e-mail.
11. Press “Grant Access” button next to your service account and allow it to upload apks. When finished choosing permissions, press “add”.