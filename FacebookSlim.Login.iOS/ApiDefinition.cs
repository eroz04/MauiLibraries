using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace FacebookSlim.Login.iOS;

[Static]
partial interface Constants
{
	// extern double BindingFacebookVersionNumber;
	[Field ("BindingFacebookVersionNumber", "__Internal")]
	double BindingFacebookVersionNumber { get; }

	// extern const unsigned char[] BindingFacebookVersionString;
	[Field ("BindingFacebookVersionString", "__Internal")]
	NSString BindingFacebookVersionString { get; }
}

// @interface FacebookCoreKitManager : NSObject
[BaseType (typeof(NSObject))]
interface FacebookCoreKitManager
{
	// @property (readonly, nonatomic, strong, class) FacebookCoreKitManager * _Nonnull shared;
	[Static]
	[Export ("shared", ArgumentSemantic.Strong)]
	FacebookCoreKitManager Shared { get; }

	// -(void)enableLoggingBehaviorWithApploggingBehavior:(enum LoggingBehaviorEnum)apploggingBehavior;
	[Export ("enableLoggingBehaviorWithApploggingBehavior:")]
	void EnableLoggingBehavior (LoggingBehaviorEnum apploggingBehavior);

	// @property (nonatomic) BOOL isAdvertiserTrackingEnabled;
	[Export ("isAdvertiserTrackingEnabled")]
	bool IsAdvertiserTrackingEnabled { get; set; }

	// @property (nonatomic) BOOL isAdvertiserIdCollectionEnabled;
	[Export ("isAdvertiserIdCollectionEnabled")]
	bool IsAdvertiserIdCollectionEnabled { get; set; }

	// -(void)logEventWithAppEventName:(enum AppEventNameEnum)appEventName appparameters:(NSDictionary * _Nonnull)appparameters;
	[Export ("logEventWithAppEventName:appparameters:")]
	void LogEvent (AppEventNameEnum appEventName, NSDictionary appparameters);

	// @property (copy, nonatomic) NSString * _Nullable userId;
	[NullAllowed, Export ("userId")]
	string UserId { get; set; }

	// -(void)setUserWithUserEmail:(NSString * _Nullable)userEmail firstName:(NSString * _Nullable)firstName lastName:(NSString * _Nullable)lastName phone:(NSString * _Nullable)phone dateOfBirth:(NSString * _Nullable)dateOfBirth gender:(NSString * _Nullable)gender city:(NSString * _Nullable)city state:(NSString * _Nullable)state zip:(NSString * _Nullable)zip country:(NSString * _Nullable)country;
	[Export ("setUserWithUserEmail:firstName:lastName:phone:dateOfBirth:gender:city:state:zip:country:")]
	void SetUser ([NullAllowed] string userEmail, [NullAllowed] string firstName, [NullAllowed] string lastName, [NullAllowed] string phone, [NullAllowed] string dateOfBirth, [NullAllowed] string gender, [NullAllowed] string city, [NullAllowed] string state, [NullAllowed] string zip, [NullAllowed] string country);

	// -(NSString * _Nonnull)anonymousId __attribute__((warn_unused_result("")));
	[Export ("anonymousId")]
	string AnonymousId { get; }

	// -(void)activateApp;
	[Export ("activateApp")]
	void ActivateApp ();

	// -(void)logEventCustomWithAppEventName:(NSString * _Nonnull)appEventName appparameters:(NSDictionary * _Nonnull)appparameters;
	[Export ("logEventCustomWithAppEventName:appparameters:")]
	void LogEvent (string appEventName, NSDictionary appparameters);

	// -(void)initializeSdk;
	[Export ("initializeSdk")]
	void InitializeSdk ();

	// -(BOOL)finishedLaunchingWithApp:(UIApplication * _Nonnull)app options:(NSDictionary<UIApplicationLaunchOptionsKey,id> * _Nonnull)options __attribute__((warn_unused_result("")));
	[Export ("finishedLaunchingWithApp:options:")]
	bool FinishedLaunching (UIApplication app, NSDictionary<NSString, NSObject> options);

	// -(BOOL)openUrlWithApp:(UIApplication * _Nonnull)app url:(NSURL * _Nonnull)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> * _Nonnull)options __attribute__((warn_unused_result("")));
	[Export ("openUrlWithApp:url:options:")]
	bool OpenUrl (UIApplication app, NSUrl url, NSDictionary<NSString, NSObject> options);
}

// @interface FacebookLoginManager : NSObject
[BaseType (typeof(NSObject))]
interface FacebookLoginManager
{
	// @property (readonly, nonatomic, strong, class) FacebookLoginManager * _Nonnull shared;
	[Static]
	[Export ("shared", ArgumentSemantic.Strong)]
	FacebookLoginManager Shared { get; }

	// -(void)loginWithPermissions:(NSArray<NSString *> * _Nonnull)permissions viewController:(UIViewController * _Nonnull)viewController onCompleted:(void (^ _Nonnull)(enum LoginResultEnum, LoginResult * _Nullable, NSError * _Nullable))onCompleted;
	[Export ("loginWithPermissions:viewController:onCompleted:")]
	void Login (string[] permissions, UIViewController viewController, Action<LoginResultEnum, ILoginResult, NSError> onCompleted);

	// -(void)logout;
	[Export ("logout")]
	void Logout ();
	
	// -(bool)isLogin;
	[Export ("isLogin")]
	bool IsLogin ();
}

// @interface LoginResult : NSObject
[BaseType (typeof(NSObject), Name = "_TtC12FacebookSdk11LoginResult")]
interface ILoginResult
{
	// @property (copy, nonatomic) NSString * _Nonnull token;
	[Export ("token")]
	string Token { get; set; }
	
	// @property (copy, nonatomic) NSString * _Nonnull userId;
	[Export ("userId")]
	string UserId { get; set; }

	// @property (copy, nonatomic) NSString * _Nonnull authenticationToken;
	[Export ("authenticationToken")]
	string AuthenticationToken { get; set; }

	// @property (copy, nonatomic) NSSet<NSString *> * _Nonnull grantedPermissions;
	[Export ("grantedPermissions", ArgumentSemantic.Copy)]
	NSSet<NSString> GrantedPermissions { get; set; }

	// @property (copy, nonatomic) NSSet<NSString *> * _Nonnull declinedPermissions;
	[Export ("declinedPermissions", ArgumentSemantic.Copy)]
	NSSet<NSString> DeclinedPermissions { get; set; }
}

// @interface FacebookGraphRequest : NSObject
[BaseType(typeof(NSObject))]
interface FacebookGraphRequest
{
	// @property (readonly, nonatomic, strong, class) FacebookGraphRequest * _Nonnull shared;
	[Static]
	[Export ("shared", ArgumentSemantic.Strong)]
	FacebookGraphRequest Shared { get; }

	// -(void)meRequestWithParameters:(NSArray<NSString *> * _Nonnull)parameters onCompleted:(void (^ _Nonnull)(NSDictionary * _Nullable))onCompleted;
	[Export ("meRequestWithParameters:onCompleted:")]
	void MeRequest (string[] permissions, Action<NSDictionary> onCompleted);
}