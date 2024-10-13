using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace HPBarcodeTest.Helpers;

public static class FirebaseHelper
{
    private static FirebaseApp _firebaseApp;

    static FirebaseHelper()
    {
        _firebaseApp = FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile("hpbarcodetest-firebase-adminsdk.json")
        });
    }

    public static async Task<FirebaseToken> ValidateFirebaseToken(string token)
    {
        return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
    }
}