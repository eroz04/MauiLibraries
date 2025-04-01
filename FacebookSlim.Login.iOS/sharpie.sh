#!/bin/bash
dotnet build
sharpie bind \
  -framework ../NativeIOS/bin/Release/net8.0-ios/BindingFacebookiOS.xcframework/ios-arm64/BindingFacebook.framework \
  -namespace FacebookSlim.Login.iOS
mv ApiDefinitions.cs Apidefinition.cs
