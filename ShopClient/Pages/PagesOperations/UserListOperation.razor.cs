using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ShopSharedLibrary.DataObject.ResponseModels;
using ShopSharedLibrary.DTO_Operation.DTO;
using System.Net.Http.Json;

namespace ShopClient.Pages.PagesOperations
{
    public class UserListOperation : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }
        protected List<UserDTO> userList = new List<UserDTO>();

        protected async override Task OnInitializedAsync()
        {
            await LoadList();
        }
        /// <summary>
        /// APIden Verileri Getir
        /// </summary>
        /// <returns></returns>
        protected async Task LoadList()
        {
            var serviceResponse = await HttpClient.GetFromJsonAsync<ServiceResponse<List<UserDTO>>>("api/User");
            if (serviceResponse.IsSuccess)
                userList = serviceResponse.Value;
        }
    }
}
