package com.binding.facebooksdk;

import android.app.Activity;
import android.content.Intent;

import androidx.annotation.NonNull;

import com.facebook.AccessToken;
import com.facebook.CallbackManager;
import com.facebook.FacebookCallback;
import com.facebook.FacebookException;

import java.util.Arrays;
import java.util.Objects;

class LoginManagerImpl implements LoginManager {

    private final CallbackManager callbackManager = CallbackManager.Factory.create();
    private LoginCallback loginCallback;
    private AccessToken accessToken;

    public LoginManagerImpl() {
        com.facebook.login.LoginManager.getInstance().registerCallback(callbackManager, new FacebookCallback<com.facebook.login.LoginResult>() {
            @Override
            public void onSuccess(com.facebook.login.LoginResult loginResult) {
                if (loginCallback == null) {
                    return;
                }

                accessToken = loginResult.getAccessToken();
                LoginResult facebookLoginResult = new LoginResultImpl(
                        new AccessTokenImpl(
                                accessToken.getExpires(),
                                accessToken.getPermissions().toArray(new String[0]),
                                accessToken.getDeclinedPermissions().toArray(new String[0]),
                                accessToken.getExpiredPermissions().toArray(new String[0]),
                                accessToken.getToken(),
                                accessToken.getLastRefresh(),
                                accessToken.getApplicationId(),
                                accessToken.getUserId(),
                                accessToken.getDataAccessExpirationTime(),
                                accessToken.getGraphDomain()
                        ),
                        loginResult.getRecentlyGrantedPermissions().toArray(new String[0]),
                        loginResult.getRecentlyDeniedPermissions().toArray(new String[0]));
            }

            @Override
            public void onCancel() {
                if (loginCallback == null) {
                    return;
                }
                loginCallback.onCanceled();
            }

            @Override
            public void onError(@NonNull FacebookException e) {
                if (loginCallback == null) {
                    return;
                }
                loginCallback.onError(e);
            }
        });
        com.facebook.login.LoginManager.getInstance().setLoginBehavior(com.facebook.login.LoginBehavior.NATIVE_WITH_FALLBACK);
    }

    @Override
    public void login(Activity activity, String[] permissions, LoginCallback loginCallback) {
        this.loginCallback = loginCallback;
        com.facebook.login.LoginManager.getInstance().logInWithReadPermissions(activity, Arrays.asList(permissions));
    }

    @Override
    public void logout() {
        com.facebook.login.LoginManager.getInstance().logOut();
    }

    @Override
    public boolean onActivityResult(int requestCode, int resultCode, Intent data) {
        return callbackManager.onActivityResult(requestCode, resultCode, data);
    }

    @Override
    public void setLoginBehavior(String behavior) {
        if(Objects.equals(behavior, "NATIVE_WITH_FALLBACK"))
            com.facebook.login.LoginManager.getInstance().setLoginBehavior(com.facebook.login.LoginBehavior.NATIVE_WITH_FALLBACK);
        else if(Objects.equals(behavior, "WEB_ONLY")) {
            com.facebook.login.LoginManager.getInstance().setLoginBehavior(com.facebook.login.LoginBehavior.WEB_ONLY);
        }
        else if(Objects.equals(behavior, "NATIVE_ONLY")) {
            com.facebook.login.LoginManager.getInstance().setLoginBehavior(com.facebook.login.LoginBehavior.NATIVE_ONLY);
        }
        else {
            com.facebook.login.LoginManager.getInstance().setLoginBehavior(com.facebook.login.LoginBehavior.NATIVE_WITH_FALLBACK);
        }
    }

    @Override
    public boolean isLogin() {
        return AccessToken.isCurrentAccessTokenActive();
    }
}
