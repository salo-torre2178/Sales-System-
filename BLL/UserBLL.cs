using SalesSystem.DAL;
using SalesSystem.DTOs.User;
using SalesSystem.Entities;
using SalesSystem.UI;
using SalesSystem.UI.Seller;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SalesSystem.BLL
{
    public class UserBLL
    {
        private readonly UserDAL userDAL;
        private readonly UserBLL userBLL;

        public UserBLL()
        {
            userDAL = new UserDAL();
        }

        public UserLoginResponseDTO Login(UserLoginDTO userLogin)
        {
            if (userLogin == null || string.IsNullOrWhiteSpace(userLogin.Email) || string.IsNullOrWhiteSpace(userLogin.Password))
            {
                throw new Exception("Debe completar todos los campos.");
            }

            try
            {
                UserLoginResponseDTO user = userDAL.Login(userLogin);

                if (user == null)
                {
                    throw new Exception("Credenciales inválidas o rol no autorizado.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar iniciar sesión.", ex);
            }
        }

        public string GetWelcomeMessage(UserLoginResponseDTO user)
        {
            if (user == null)
            {
                throw new Exception("El usuario es nulo");
            }

            return $"¡Bienvenido {user.Name}! Tu rol es {user.Role}.";
        }

        public List<User> GetAll()
        {
            return userDAL.GetAll();
        }

        public int DeleteUser(DeleteUserDTO userID)
        {
            int filasAfectadas = userDAL.Delete(userID);
            return filasAfectadas;
        }

        public void Add(AddUserDTO user)
        {
            userDAL.Add(user);
        }

        public EditUserResponseDTO GetById(int id)
        {
            return userDAL.GetById(id);
        }

        public void UpdateUser(UpdateUserDTO userDto)
        {
            if (userDto == null) throw new ArgumentNullException(nameof(userDto));

            // Aquí puedes poner validaciones de negocio
            if (string.IsNullOrWhiteSpace(userDto.FullName))
                throw new Exception("El nombre no puede estar vacío.");

            // Llamada al DAL
            var userDAL = new UserDAL();
            userDAL.Update(userDto);
        }

    }
}
