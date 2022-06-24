using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using CookbookBackend.Controllers.User;
using CookbookBackend.DataLayer;
using CookbookBE.DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CookbookBackEnd.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _db;

        public object TempData { get; private set; }

        public UserController(ApplicationContext db)
        {
            _db = db;
        }

        // TODO
        [HttpGet("DownloadProfilePicture")]
        public FileContentResult DownloadProfilePicture(int id)
        {
            SqlDataReader rdr; byte[] fileContent = null;
            string mimeType = ""; string fileName = "";
            const string connect = @"Server=.;Database=MyApplicationDatabase;Trusted_Connection=True;";

            using (var conn = new SqlConnection(connect))
            {
                var qry = "SELECT FileContent, MimeType, FileName FROM FileStore WHERE ID = @ID";
                var cmd = new SqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    fileContent = (byte[])rdr["FileContent"];
                    mimeType = rdr["MimeType"].ToString();
                    fileName = rdr["FileName"].ToString();
                }
            }
            return File(fileContent, mimeType, fileName);
        }

        // TODO
        [HttpPost("UploadProfilePicture/{id:int}")]
        public ActionResult UploadProfilePicture(int id)
        {

            try
            {
                // ...

                var picture = Request.Form.Files[0];

                // ...

                return Ok();
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // TODO
        [HttpGet("GetUserProfile/{id:int}")] // https://localhost:5000/api/user/GetUserProfile/45
        public ActionResult<UserDTO> GetUserProfile(int id)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return BadRequest();
            }
            else
            return Ok();

        }

        [HttpPut("UpdateUser/{id:int}")]
        public ActionResult<UserDTO> UpdateUser(int id, [FromBody] User payload)
        {
            

                var userToEdit = _db.Users.FirstOrDefault(x => x.Id == id);

                if (userToEdit != null)
                {
                userToEdit.FirstName = payload.FirstName; 
                userToEdit.LastName = payload.LastName;
                userToEdit.Email = payload.Email;
                 _db.SaveChanges();

                return Ok();
            }
            else
            {
                return BadRequest();
            }
                    
            
        }
    

        [HttpDelete("DeleteUser/{id:int}")]
        public ActionResult<bool> Delete(int id)
        {
            User user = _db.Users.Find(id);
            if (id == 0)
            {
                return BadRequest();
            }

            else
            {
                
                _db.Users.Remove(user);
                _db.SaveChanges();
                return Ok();
            }
            //     return RedirectToAction("https://localhost:44305/swagger/index.html");
            return Ok();
        }

        [HttpGet("AllUsers")] // https://localhost:5000/api/user/AllUsers?pageSize=20&pageNumber=3&sortType=1
        public ActionResult<GetAllUsersResponse> GetAllUsers(int pageSize, int pageNumber, Enums.SortType sortType)
        {
            var allUsersQuery = _db.Users.AsQueryable();

            switch (sortType)
            {
                case Enums.SortType.FirstNameAscending:
                    allUsersQuery = allUsersQuery.OrderBy(x => x.FirstName);
                    break;
                case Enums.SortType.FirstNameDescending:
                    allUsersQuery = allUsersQuery.OrderByDescending(x => x.FirstName);
                    break;
                case Enums.SortType.LastNameAscending:
                    allUsersQuery = allUsersQuery.OrderBy(x => x.LastName);
                    break;
                case Enums.SortType.LastNameDescending:
                    allUsersQuery = allUsersQuery.OrderByDescending(x => x.LastName);
                    break;
                default: throw new ApplicationException("Unknown sort type");
            }

            var allUsers = allUsersQuery
                .Select(u => new UserDTO
                {
                    FullName = u.FirstName + ' ' + u.LastName,
                    Email = u.Email,
                })
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            return Ok(new GetAllUsersResponse
            {
                Users = allUsers,
            });
        }

        private string GetCurrentUserEmail()
        {
            return HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?
                .Value;
        }

    }
}
