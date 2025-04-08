using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using TrainResApp.Models;

namespace TrainResApp.Controllers
{
    public class AuthController : ApiController
    {
        private TrainReservationDB1Entities1 db = new TrainReservationDB1Entities1();

        [HttpPost]
        [Route("api/register")]
        public IHttpActionResult Register(RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (db.Users.Any(u => u.email == dto.Email))
                return BadRequest("Email already exists.");

            var hashedPassword = HashPassword(dto.Password);

            var user = new User
            {
                name = dto.Name,
                email = dto.Email,
                phone = dto.Phone,
                password_hash = hashedPassword,
                gender = dto.Gender,
                age = dto.Age,
                address = dto.Address,
                role = dto.Role, 
                created_at = DateTime.Now
            };

            db.Users.Add(user);
            db.SaveChanges();

            return Ok("User registered successfully.");

        }

        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = db.Users.FirstOrDefault(u => u.email == dto.Email);
            if (user == null)
                return Unauthorized(); // Email not found

            var hashedInputPassword = HashPassword(dto.Password);

            if (user.password_hash != hashedInputPassword)
                return Unauthorized(); // Incorrect password

            return Ok(new
            {
                Message = "Login successful",
                User = new
                {
                    user.user_id,
                    user.name,
                    user.email,
                    user.phone,
                    user.gender,
                    user.age,
                    user.address,
                    user.role
                }
            });
        }



        private string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}