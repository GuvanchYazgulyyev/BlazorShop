using Microsoft.AspNetCore.Components;
using ShopSharedLibrary.DataObject.ResponseModels;
using ShopSharedLibrary.DTO_Operation.DTO;
using System.Net.Http.Json;

namespace ShopClient.Pages.PagesOperations
{
    public class LoginOperation : ComponentBase
    {
        [Inject]
        HttpClient client { get; set; }

        [Inject]
        ModalManager modalManager { get; set; }
          [Inject]
        NavigationManager navigation { get; set; }

        private UserLoginRequestDTO userLoginRequest = new UserLoginRequestDTO();

        private async Task loginProcess()
        {
            var httpRequest = await client.PostAsJsonAsync("api/User/Login", userLoginRequest);
            if (httpRequest.IsSuccessStatusCode)
            {
                var ress=await httpRequest.Content.ReadFromJsonAsync<ServiceResponse<UserLoginResponseDTO>>();
                if (ress.IsSuccess)
                    navigation.NavigateTo("/");
                else
                     modalManager.Show(ress.Message);
            }

        }
    }
}
