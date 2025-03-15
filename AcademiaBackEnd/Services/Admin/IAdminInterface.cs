using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Request.Admin;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;

namespace AcademiaBackEnd.Services.Admin
{
    public interface IAdminInterface
    {
        Task<Response<Usuario>> RegisterNewClient(RegisterNewClientRequest request);

        Task<Response<Usuario>> DeleteClient(RemoveClientRequest request);
        
        Task<Response<Usuario>> GetClientById(GetClientByIdRequest request);

        Task<Response<List<Usuario>>> GetAllClients();
        Task<Response<bool>> UpdateImageProfile(UpdateImageProfileRequest request, long clientId);

    }
}
