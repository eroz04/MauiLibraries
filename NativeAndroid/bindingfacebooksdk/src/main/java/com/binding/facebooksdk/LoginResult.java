package com.binding.facebooksdk;

public interface LoginResult {
    AccessToken getAccessToken();
    String[] getRecentlyGrantedPermissions();
    String[] getRecentlyDeniedPermissions();
}
