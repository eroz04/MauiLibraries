namespace Firebase.Auth;

public interface IFirebaseUser : Plugin.Firebase.Auth.IFirebaseUser
{
    bool IsNewUser { get; }
    string? FirstName { get; }
    string? LastName { get; }
}
