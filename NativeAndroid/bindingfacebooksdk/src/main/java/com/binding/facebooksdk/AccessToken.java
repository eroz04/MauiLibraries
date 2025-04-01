package com.binding.facebooksdk;

import java.util.Date;

public interface AccessToken {
    Date getExpires();
    String[] getPermissions();
    String[] getDeclinedPermissions();
    String[] getExpiredPermissions();
    String getToken();
    Date getLastRefresh();
    String getApplicationId();
    String getUserId();
    Date getDataAccessExpirationTime();
    String getGraphDomain();
}
