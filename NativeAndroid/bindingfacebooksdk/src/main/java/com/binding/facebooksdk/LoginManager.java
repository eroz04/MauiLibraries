package com.binding.facebooksdk;

import android.app.Activity;
import android.content.Intent;

public interface LoginManager {
    
    public static LoginManager createInstance(){
        return new LoginManagerImpl();
    }

    void login(Activity activity, String[] permissions, LoginCallback loginCallback);

    void logout();

    boolean onActivityResult(int requestCode, int resultCode, Intent data);

    void setLoginBehavior(String behavior);

    boolean isLogin();
}
