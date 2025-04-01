package com.binding.facebooksdk;

class LoginResultImpl implements LoginResult {

    private final AccessToken accessToken;
    private final String[] recentlyGrantedPermissions;
    private final String[] recentlyDeniedPermissions;

    public LoginResultImpl(AccessToken accessToken, String[] recentlyGrantedPermissions, String[] recentlyDeniedPermissions) {
        this.accessToken = accessToken;
        this.recentlyGrantedPermissions = recentlyGrantedPermissions;
        this.recentlyDeniedPermissions = recentlyDeniedPermissions;
    }

    @Override
    public AccessToken getAccessToken() {
        return accessToken;
    }

    @Override
    public String[] getRecentlyGrantedPermissions() {
        return recentlyGrantedPermissions;
    }

    @Override
    public String[] getRecentlyDeniedPermissions() {
        return recentlyDeniedPermissions;
    }

}
