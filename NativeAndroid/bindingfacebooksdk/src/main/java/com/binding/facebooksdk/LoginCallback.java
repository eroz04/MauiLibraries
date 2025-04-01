package com.binding.facebooksdk;

public interface LoginCallback {
    void onSuccess(LoginResult loginResult);
    void onCanceled();
    void onError(Exception exception);
}
