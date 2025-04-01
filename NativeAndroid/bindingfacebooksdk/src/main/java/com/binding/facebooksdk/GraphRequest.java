package com.binding.facebooksdk;

import android.os.Bundle;

import androidx.annotation.Nullable;

import org.json.JSONObject;

public class GraphRequest {
    public static void NewMeRequest(GraphJSONObjectCallback graphJSONObjectCallback, Bundle parameters)
    {
        if(graphJSONObjectCallback == null) return;
        if(parameters == null) return;

        com.facebook.GraphRequest graphRequest = com.facebook.GraphRequest.newMeRequest(com.facebook.AccessToken.getCurrentAccessToken(), new com.facebook.GraphRequest.GraphJSONObjectCallback() {
            @Override
            public void onCompleted(@Nullable JSONObject jsonObject, @Nullable com.facebook.GraphResponse graphResponse) {
                graphJSONObjectCallback.onCompleted(jsonObject);
            }
        });
        graphRequest.setParameters(parameters);
        graphRequest.executeAsync();
    }
}