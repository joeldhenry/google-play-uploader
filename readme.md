# Google Play Uploader

.Net Application to Upload APK or AAB to Google Play Console

### Build

- dotnet run

### Build

- dotnet build GooglePlayUploader -c Release

## Publish

- dotnet publish --runtime osx.10.14-x64
- mv bin/Debug/netcoreapp2.2/osx.10.14-x64/publish/GooglePlayUploader ./GooglePlayUploader

## Usage

```
  -a, --authFile    Required. Google Auth File
  -i, --appId       Required. App id
  -f, --appFile     Required. App File
  --help            Display this help screen.
  --version         Display version information.
```