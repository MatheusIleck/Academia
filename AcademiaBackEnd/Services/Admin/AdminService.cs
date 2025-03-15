using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Request.Admin;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;
using AcademiaBackEnd.Utilities;

namespace AcademiaBackEnd.Services.Admin
{
    public class AdminService(db_AcademiaContext _db) : IAdminInterface
    {


        public async Task<Response<Usuario>> DeleteClient(RemoveClientRequest request)
        {
            try
            {
                var User = await _db.Usuarios.FindAsync(request.Id);

                if (User == null)
                {
                    return new Response<Usuario>(null, 404, "Cliente não encontrado.");

                }

                _db.Usuarios.Remove(User);
                await _db.SaveChangesAsync();

                return new Response<Usuario>(User, 200, "Cliente removido com sucesso!");


            }
            catch (Exception ex)
            {
                return new Response<Usuario>(null, 500, ex.Message);

            }
        }

        public async Task<Response<List<Usuario>>> GetAllClients()
        {
            try
            {
                var users = await _db.Usuarios.AsNoTracking().ToListAsync();

                if(users == null)
                {
                    return new Response<List<Usuario>>(null, 404, "Nenhum cliente encontrado.");
                }

                return new Response<List<Usuario>>(users, 200, "Clientes encontrados.");
            }
            catch (Exception ex)
            {
                return new Response<List<Usuario>>(null, 500, ex.Message);
            }
        }

        public async Task<Response<Usuario>> GetClientById(GetClientByIdRequest request)
        {
            try
            {
                var client = await _db.Usuarios.FirstOrDefaultAsync(x=> x.Id == request.id);

                if (client == null)
                    return new Response<Usuario>(null, 404, "Cliente não encontrado.");

                return new Response<Usuario>(client, 200, "Cliente encontrado.");
            }
            catch(Exception ex)
            {
                return new Response<Usuario>(null, 500, ex.Message);
            }
        }

        public async Task<Response<Usuario>> RegisterNewClient(RegisterNewClientRequest request)
        {
            try
            {
                var utilities = new Utilities.Utilities(_db);

                if (utilities.EmailExists(request.Email.ToLower()))
                    return new Response<Usuario>(null, 400, "Email:Este e-mail já está cadastrado. Tente fazer login ou use outro e-mail.");

                if (!utilities.IsValidEmailFormat(request.Email))
                    return new Response<Usuario>(null, 400, "Email:E-mail inválido.");

                if (!utilities.IsValidPassword(request.Password))
                    return new Response<Usuario>(null, 400, "Password:A senha é inválida!");

                var newClient = new Usuario
                {
                    Nome = request.Name,
                    Email = request.Email.ToLower(),
                    DataNascimento = request.Date,
                    Senha = request.Password,
                    Altura = request.Height,
                    Peso = request.Weight,
                    Role = "Client",
                    Img = "/imgs/default.png"
                };

                await _db.Usuarios.AddAsync(newClient);
                await _db.SaveChangesAsync();

                return new Response<Usuario>(newClient, 201, "Cliente Criado");
            }
            catch (Exception)
            {
                return new Response<Usuario>(null, 500, "Servidor:Houve um erro ao conectar com o servidor.");
            }
        }





        public async Task<Response<bool>> UpdateImageProfile(UpdateImageProfileRequest request, long clientId)

        {
            var file = request.image; 

            var user = await _db.Usuarios.FirstOrDefaultAsync(x => x.Id == clientId);

            if (file == null || file.Length == 0)
            {
                return new Response<bool>(false, 400, "Nenhuma imagem enviada.");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine("wwwroot/imgs", fileName);

            var directoryPath = Path.Combine("wwwroot", "imgs");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

          
            if (!string.IsNullOrEmpty(user.Img))
            {
                var oldImagePath = Path.Combine("wwwroot", user.Img.TrimStart('/'));
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath); 
                }
            }

      
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = "/imgs/" + fileName; 

            user.Img = imageUrl;

            _db.Update(user);
            await _db.SaveChangesAsync();

            var requestdto = new GetProfileRequest
            {
                ClientId = clientId,
            };


            return new Response<bool>(true, 200, "Imagem atualizada com sucesso");


        }

    
    }
}
