using AcademiaBackEnd.Data.Models;
using System.Text.RegularExpressions;

namespace AcademiaBackEnd.Utilities
{
    public class Utilities (db_AcademiaContext _db)
    {
    

        public bool IsValidEmailFormat(string email)
        {
            bool ok = Regex.IsMatch(email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");
            if (!ok)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public bool IsValidPassword(string senha)
        {
            bool ok = Regex.IsMatch(senha, "^(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+{}\\[\\]:;<>,.?~\\\\/-]).{6,12}$");
            if (!ok)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool EmailExists( string email)
        {

            return _db.Usuarios.Any(x => x.Email == email);
        }

    }
}
