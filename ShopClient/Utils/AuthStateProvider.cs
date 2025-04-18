using Blazored.LocalStorage; // Tarayıcının local storage alanına erişim sağlayan kütüphane
using Microsoft.AspNetCore.Components.Authorization; // Kimlik doğrulama ve yetkilendirme için temel sınıfları içerir
using System.Net.Http.Headers; // HTTP başlıkları ile çalışmak için gerekli sınıflar
using System.Security.Claims; // Kullanıcı kimliği ve talepleri (claims) ile çalışmak için gerekli sınıflar

namespace ShopClient.Utils
{
    // Bu sınıf, uygulamanın kimlik doğrulama durumunu yönetir ve Blazor'a bildirir.
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService; // Local storage'a erişim için enjekte edilmiş servis
        private readonly AuthenticationState _anonimous; // Kimliği doğrulanmamış kullanıcı durumu için statik bir nesne
        private readonly HttpClient _httpClient; // HTTP istekleri yapmak için enjekte edilmiş istemci

        // AuthStateProvider sınıfının yapıcı metodu (constructor)
        public AuthStateProvider(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            _localStorageService = localStorageService; // Local storage servisini sınıf değişkenine atar
            _anonimous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())); // Kimliği doğrulanmamış bir kullanıcı oluşturur. Token veya email olmadığında bu durum kullanılır.
            _httpClient = httpClient; // HTTP istemcisini sınıf değişkenine atar
        }

        // Uygulamanın mevcut kimlik doğrulama durumunu asenkron olarak döndüren metot.
        // Blazor altyapısı tarafından periyodik olarak veya kimlik doğrulama durumu değiştiğinde çağrılır.
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            String apiToken = await _localStorageService.GetItemAsStringAsync("token"); // Local storage'dan "token" anahtarıyla saklanan değeri asenkron olarak okur.
            String email = await _localStorageService.GetItemAsStringAsync("email"); // Local storage'dan "email" anahtarıyla saklanan değeri asenkron olarak okur.

            // Eğer local storage'da token bilgisi yoksa, anonim (giriş yapılmamış) durumu döndürür.
            if (String.IsNullOrEmpty(apiToken))
                return _anonimous;

            // Eğer token varsa, bu token ve email bilgisiyle bir ClaimsPrincipal oluşturur.
            // ClaimsPrincipal, kullanıcının kimliğini ve sahip olduğu talepleri temsil eder.
            var cp = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "jwtAuthType"));

            // Tokeni yolla En Son bu kod yazılmalı
            // HTTP istemcisinin varsayılan istek başlıklarına "Authorization" başlığını ekler.
            // Bu sayede sonraki tüm HTTP isteklerinde sunucuya Bearer tokeni otomatik olarak gönderilir.
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);

            // Oluşturulan ClaimsPrincipal ile birlikte yeni bir AuthenticationState nesnesi döndürür.
            // Bu, kullanıcının giriş yapmış ve kimliğinin doğrulanmış olduğu anlamına gelir.
            return new AuthenticationState(cp);
        }


        /// Giriş Yapılınca
        /// Kullanıcının giriş yapması durumunda çağrılan metot.
        public void NotifyUserLogin(String email)
        {
            // Giriş yapan kullanıcının email bilgisiyle bir ClaimsPrincipal oluşturur.
            var cp = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "jwtAuthType"));
            // Oluşturulan ClaimsPrincipal ile yeni bir AuthenticationState nesnesi oluşturur ve bunu bir Task<AuthenticationState> nesnesine sarar.
            var authState = Task.FromResult(new AuthenticationState(cp));
            // Blazor altyapısına kimlik doğrulama durumunun değiştiğini bildirir.
            // Bu, GetAuthenticationStateAsync() metodunun tekrar çağrılmasına ve UI'ın güncellenmesine neden olur.
            NotifyAuthenticationStateChanged(authState);
        }

        /// Çıkış Yapınca
        /// Kullanıcının çıkış yapması durumunda çağrılan metot.
        public void NotifyUserLogOut()
        {
            // Anonim (giriş yapılmamış) kullanıcı durumunu temsil eden AuthenticationState nesnesini bir Task<AuthenticationState> nesnesine sarar.
            var authState = Task.FromResult(_anonimous);
            // Blazor altyapısına kimlik doğrulama durumunun değiştiğini bildirir.
            // Bu, GetAuthenticationStateAsync() metodunun tekrar çağrılmasına ve UI'ın güncellenmesine neden olur.
            NotifyAuthenticationStateChanged(authState);
        }
    }
}