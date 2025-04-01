//
//  FacebookProxy.swift
//  BindingFacebook
//
//  Created by Christian Kapplmüller on 25.06.24.
//  Modified by 

import Foundation
import UIKit
import FacebookLogin

@objc public enum LoginResultEnum : Int {
    case Error
    case Cancelled
    case LoggedIn
}

@objc public enum LoggingBehaviorEnum : Int {
    case   AccessTokens
    case   PerformanceCharacteristics
    case   AppEvents
    case   Informational
    case   CacheErrors
    case   UIControlErrors
    case   GraphAPIDebugWarning
    // we do not expose GraphAPIDebugInfo until https://github.com/facebook/facebook-ios-sdk/issues/2152 is resolved
    //case   GraphAPIDebugInfo
    case   NetworkRequests
    case   DeveloperErrors
}

@objc public enum AppEventNameEnum : Int {
    case   AchievedLevel
    case   AddedPaymentInfo
    case   AddedToCart
    case   AddedToWishlist
    case   CompletedRegistration
    case   CompletedTutorial
    case   InitiatedCheckout
    case   Purchased
    case   Rated
    case   Searched
    case   SpentCredits
    case   UnlockedAchievement
    case   ViewedContent
    case   Contact
    case   CustomizeProduct
    case   Donate
    case   FindLocation
    case   Schedule
    case   StartTrial
    case   SubmitApplication
    case   Subscribe
    case   AdImpression
    case   AdClick
}

@objc public class LoginResult : NSObject {
    
    @objc public var token = ""
    @objc public var userId = ""
    @objc public var authenticationToken = ""
    @objc public var grantedPermissions: Set<String> = []
    @objc public var declinedPermissions: Set<String> = []
}

@objc(FacebookLoginManager)
public class FacebookLoginManager : NSObject {
    
    @objc
    public static let shared = FacebookLoginManager()
    private let loginManager = LoginManager()
    
    @objc
    public func login(permissions: [String], viewController: UIViewController, onCompleted: @escaping  (LoginResultEnum, LoginResult?, Error?) -> Void) {
        
        FacebookLoginManager.shared.loginManager.logIn(permissions: permissions, from: viewController) { result, error in
            if let error = error {
                onCompleted(LoginResultEnum.Error, nil, error)
            } else if let result = result, result.isCancelled {
                onCompleted(LoginResultEnum.Cancelled, nil, nil)
            } else {
                let loginResult = LoginResult()
                
                loginResult.token = result?.token?.tokenString ?? ""
                loginResult.userId = result?.token?.userID ?? ""
                loginResult.authenticationToken = result?.authenticationToken?.tokenString ?? ""
                loginResult.grantedPermissions = result?.grantedPermissions ?? []
                loginResult.declinedPermissions = result?.declinedPermissions ?? []
                
                onCompleted(LoginResultEnum.LoggedIn, loginResult, nil)
            }
        }
    }
    
    @objc
    public func logout() -> Void {
        FacebookLoginManager.shared.loginManager.logOut();
    }
    
    @objc
    public func isLogin() -> Bool {
        return AccessToken.isCurrentAccessTokenActive;
    }
}


@objc(FacebookCoreKitManager)
public class FacebookCoreKitManager : NSObject {
    
    @objc
    public static let shared = FacebookCoreKitManager()
    private static let settings = FBSDKCoreKit.Settings()
    
    @objc
    public func enableLoggingBehavior(apploggingBehavior: LoggingBehaviorEnum) -> Void {
        
        var loggingBehavior = LoggingBehavior.appEvents
        
        switch apploggingBehavior {
        case   LoggingBehaviorEnum.AccessTokens:
            loggingBehavior = LoggingBehavior.accessTokens
        case   LoggingBehaviorEnum.PerformanceCharacteristics:
            loggingBehavior = LoggingBehavior.performanceCharacteristics
        case   LoggingBehaviorEnum.AppEvents:
            loggingBehavior = LoggingBehavior.appEvents
        case   LoggingBehaviorEnum.Informational:
            loggingBehavior = LoggingBehavior.informational
        case   LoggingBehaviorEnum.CacheErrors:
            loggingBehavior = LoggingBehavior.cacheErrors
        case   LoggingBehaviorEnum.UIControlErrors:
            loggingBehavior = LoggingBehavior.uiControlErrors
        case   LoggingBehaviorEnum.GraphAPIDebugWarning:
            loggingBehavior = LoggingBehavior.graphAPIDebugWarning
        // we do not expose GraphAPIDebugInfo until https://github.com/facebook/facebook-ios-sdk/issues/2152 is resolved
        //case   LoggingBehaviorEnum.GraphAPIDebugInfo:
        //    loggingBehavior = LoggingBehavior.graphAPIDebugInfo
        case     LoggingBehaviorEnum.NetworkRequests:
            loggingBehavior = LoggingBehavior.networkRequests
        case   LoggingBehaviorEnum.DeveloperErrors:
            loggingBehavior = LoggingBehavior.developerErrors
        }
        
        FacebookCoreKitManager.settings.enableLoggingBehavior(loggingBehavior)
    }
    
    @objc
    public var isAdvertiserTrackingEnabled: Bool {
        get { FacebookCoreKitManager.settings.isAdvertiserTrackingEnabled }
        set(enabled) { FacebookCoreKitManager.settings.isAdvertiserTrackingEnabled = enabled }
    }
    
    @objc
    public var isAdvertiserIdCollectionEnabled : Bool {
        get { FacebookCoreKitManager.settings.isAdvertiserIDCollectionEnabled }
        set(enabled) { FacebookCoreKitManager.settings.isAdvertiserIDCollectionEnabled = enabled }
    }
    
    @objc
    public func logEvent(appEventName : AppEventNameEnum, appparameters : NSDictionary) -> Void {
        
        var appEventsName = AppEvents.Name("default")
        
        switch appEventName {
        case AppEventNameEnum.AchievedLevel:
            appEventsName = AppEvents.Name.achievedLevel
        case AppEventNameEnum.AddedPaymentInfo:
            appEventsName = AppEvents.Name.addedPaymentInfo
        case AppEventNameEnum.AddedToCart:
            appEventsName = AppEvents.Name.addedToCart
        case AppEventNameEnum.AddedToWishlist:
            appEventsName = AppEvents.Name.addedToWishlist
        case AppEventNameEnum.CompletedRegistration:
            appEventsName = AppEvents.Name.completedRegistration
        case AppEventNameEnum.CompletedTutorial:
            appEventsName = AppEvents.Name.completedTutorial
        case AppEventNameEnum.InitiatedCheckout:
            appEventsName = AppEvents.Name.initiatedCheckout
        case AppEventNameEnum.Purchased:
            appEventsName = AppEvents.Name.purchased
        case AppEventNameEnum.Rated:
            appEventsName = AppEvents.Name.rated
        case AppEventNameEnum.Searched:
            appEventsName = AppEvents.Name.searched
        case AppEventNameEnum.SpentCredits:
            appEventsName = AppEvents.Name.spentCredits
        case AppEventNameEnum.UnlockedAchievement:
            appEventsName = AppEvents.Name.unlockedAchievement
        case AppEventNameEnum.ViewedContent:
            appEventsName = AppEvents.Name.viewedContent
        case AppEventNameEnum.Contact:
            appEventsName = AppEvents.Name.contact
        case AppEventNameEnum.CustomizeProduct:
            appEventsName = AppEvents.Name.customizeProduct
        case AppEventNameEnum.Donate:
            appEventsName = AppEvents.Name.donate
        case AppEventNameEnum.FindLocation:
            appEventsName = AppEvents.Name.findLocation
        case AppEventNameEnum.Schedule:
            appEventsName = AppEvents.Name.schedule
        case AppEventNameEnum.StartTrial:
            appEventsName = AppEvents.Name.startTrial
        case AppEventNameEnum.SubmitApplication:
            appEventsName = AppEvents.Name.submitApplication
        case AppEventNameEnum.Subscribe:
            appEventsName = AppEvents.Name.subscribe
        case AppEventNameEnum.AdImpression:
            appEventsName = AppEvents.Name.adImpression
        case AppEventNameEnum.AdClick:
            appEventsName = AppEvents.Name.adClick
        }
        
        FBSDKCoreKit.AppEvents.shared.logEvent(appEventsName)
    }
    
    @objc
    public var userId : String? {
        get { FBSDKCoreKit.AppEvents.shared.userID }
        set(userId) { FBSDKCoreKit.AppEvents.shared.userID = userId }
    }
    
    @objc
    public func setUser(userEmail : String?, firstName : String?, lastName : String?, phone : String?, dateOfBirth : String?, gender : String?, city : String?, state : String?, zip : String?, country : String?) -> Void {
        FBSDKCoreKit.AppEvents.shared.setUser(
            email: userEmail,
            firstName: firstName,
            lastName: lastName,
            phone: phone,
            dateOfBirth: dateOfBirth,
            gender: gender,
            city: city,
            state: state,
            zip: zip,
            country: country)
    }
    
    @objc
    public func anonymousId() -> String {
        return FBSDKCoreKit.AppEvents.shared.anonymousID
    }
    
    @objc
    public func activateApp() -> Void {
        FBSDKCoreKit.AppEvents.shared.activateApp()
    }
    
    @objc
    public func logEventCustom(appEventName : String, appparameters : NSDictionary) -> Void {
        
        let appEventsName = AppEvents.Name(appEventName)
        FBSDKCoreKit.AppEvents.shared.logEvent(appEventsName)
    }
    
    @objc
    public func initializeSdk() -> Void {
        FBSDKCoreKit.ApplicationDelegate.shared.initializeSDK()
    }
    
    @objc
    public func finishedLaunching(app : UIApplication, options : [UIApplication.LaunchOptionsKey : Any]) -> Bool {
        return FBSDKCoreKit.ApplicationDelegate.shared.application(app, didFinishLaunchingWithOptions: options)
    }
    
    @objc
    public func openUrl(app : UIApplication, url : URL, options : [UIApplication.OpenURLOptionsKey : Any]) -> Bool {
        return FBSDKCoreKit.ApplicationDelegate.shared.application(app,open:url,options:options)
    }
}

//@objc(FacebookGraphRequest)
public class FacebookGraphRequest : NSObject {

    @objc
    public static let shared = FacebookGraphRequest()
    
    @objc
    public func MeRequest(parameters: [String : Any], onCompleted: @escaping (Any?) -> Void) {
        
        let graphRequest = GraphRequest(graphPath: "me", parameters: parameters)
        graphRequest.start() { connection, result, error in
            if let resultobj = result, error == nil {
                onCompleted(resultobj)
                return
            }
            onCompleted(nil)
        }
    }
}
