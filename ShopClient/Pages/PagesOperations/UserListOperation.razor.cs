using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ShopClient.Utils;
using ShopSharedLibrary.DataObject.ResponseModels;
using ShopSharedLibrary.DTO_Operation.DTO;
using System.Net.Http.Json;

namespace ShopClient.Pages.PagesOperations
{
    public class UserListOperation : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }
        [Inject]
        ModalManager manager { get; set; }
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
            try
            {
                //var serviceResponse = await HttpClient.GetFromJsonAsync<ServiceResponse<List<UserDTO>>>("api/User/Users");

                //if (serviceResponse.IsSuccess)
                //    userList = serviceResponse.Value;
                userList = await HttpClient.GetServiceResponseAsync<List<UserDTO>>("api/User/Users", true);

            }
            catch (ApiExeption ex)
            {
                manager.Show(ex.Message);
                throw;
            }

        }
    }
}
