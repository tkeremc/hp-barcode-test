using Microsoft.AspNetCore.Mvc;
using HPBarcodeTest.Services;
using HPBarcodeTest.Helpers;
using System.Threading.Tasks;
using HPBarcodeTest.Interfaces;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Kullanıcı kayıt işlemi. MongoDB'ye kaydedilir.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // Aynı email ile kayıtlı kullanıcı var mı kontrol et
            var existingUser = await _userService.GetUserByEmail(model.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists with this email");
            }

            // Yeni kullanıcıyı kaydet
            var user = await _userService.Register(model.Email, model.Password);
            return Ok(new { Message = "User registered successfully", UserId = user.Id });
        }

        /// <summary>
        /// Kullanıcı giriş işlemi. JWT Token döner.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Kullanıcı bilgilerini kontrol et
            var user = await _userService.Login(model.Email, model.Password);
            if (user == null)
                return Unauthorized("Invalid email or password");

            // Kullanıcıyı doğruladıysak JWT Token oluştur
            var token = JwtHelper.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// Firebase üzerinden kimlik doğrulama işlemi.
        /// Firebase'den alınan token'ı doğrular.
        /// </summary>
        [HttpPost("firebase-auth")]
        public async Task<IActionResult> FirebaseAuth([FromBody] FirebaseAuthModel model)
        {
            try
            {
                // Firebase token'ını doğrula
                var decodedToken = await FirebaseHelper.ValidateFirebaseToken(model.FirebaseToken);

                // Token doğrulandıysa başarı mesajı dönelim (Firebase doğrulaması geçerli)
                return Ok(new { Message = "Firebase authentication successful", Uid = decodedToken.Uid });
            }
            catch
            {
                // Token doğrulanamazsa hata dön
                return Unauthorized("Invalid Firebase token");
            }
        }
    }

    /// <summary>
    /// Kullanıcı kayıt modeli
    /// </summary>
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Kullanıcı giriş modeli
    /// </summary>
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Firebase kimlik doğrulama modeli
    /// </summary>
    public class FirebaseAuthModel
    {
        public string FirebaseToken { get; set; }
    }
}
