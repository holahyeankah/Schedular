using ChurchAdmin.Context;
using ChurchAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schedular.Interface;
using Schedular.Request;
using Schedular.Static_classes;
using SjxLogistics.Components;
using SjxLogistics.Controllers.AuthenticationComponent;
using SjxLogistics.Models.Request;
using SjxLogistics.Models.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IpasswordHasher _ipasswordHasher;
        private readonly SchedularDbContext _context;
        private readonly AccessToken _accessTokkenGenerator;
        private readonly IMaillingService _mailservice;


        public AdminController(IpasswordHasher ipasswordHasher, SchedularDbContext context, AccessToken accessToken, IMaillingService mailservice)
        {
            _context = context;
            _ipasswordHasher = ipasswordHasher;
            _accessTokkenGenerator = accessToken;
            _mailservice = mailservice;
        }
        private static string value = "";
        private static Random random = new Random();
        private static char[] charList = new char[]

        {   'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p',
            'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z',
            '@', '#', '$', '%', '&',
            '0', '1', '2','3', '4', '5', '6', '7', '8', '9',
            'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P',
            'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z',
        };

        private static char[] List = new char[]

     {  '-', 'q', 'w', 'e', 'r', '-', 't', 'y', '-', 'u', 'i', 'o', 'p',
          '-',  'a', 's', '-', 'd', 'f', 'g', '-', '-', 'h', 'j', '-', 'k', 'l', 'z',
           
     };

        private dynamic GenerateString(int max)
        {
            while (value.Length < max)
            {
                value += List[random.Next(0, List.Count())];
            }
            return value;
        }
        private dynamic GeneratePassword(int max)
        {
            while (value.Length < max)
            {
                value += charList[random.Next(0, charList.Count())];
            }
            return value;
        }
       

        [HttpPost("admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterRequest request)
        {

            var response = new ServiceResponses<Users>();
            //string randomPassword = GeneratePassword(10);
            string tag = GetnewId();
            Users usersEmail = _context.User.FirstOrDefault(i => i.Email == request.Email);
            if (usersEmail != null)
            {
                response.Messages = "This user already exist";
                response.StatusCode = 409;
                response.Success = false;
                response.Data = null;

                return Conflict(response);
            }
            try
            {
                if (usersEmail == null)
                {
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    if(request.Appearance == true)
                    {
                        string generateString = GenerateString(10);
                        //request.virtualCodes = generateString;
                        request.virtualCodes = $"meet.google.com/{generateString}";
                    }
                 
                    Users requestUser = new()
                    {
                        Email = request.Email,
                        FirstName = request.FirstName,
                        MiddleName = request.MiddleName,
                        SurName = request.SurName,
                        PhoneNumber = request.PhoneNumber,
                        Address = request.Address,
                        Gender = request.Gender,
                        Title = request.Title,
                        Province = request.Province,
                        Parish = request.Parish,
                        ProgramType = request.ProgramType,
                        Password = passwordHash,
                        Appearance= request.Appearance,
                        Role = Role.ChurchAdmin,
                        tagCode = tag,
                        virtualCode = request.virtualCodes


                    };
                    await _context.User.AddAsync(requestUser);
                    await _context.SaveChangesAsync();
                  
                    string token = _accessTokkenGenerator.GenerateToken(requestUser);

                    response.Messages = "Successful";
                    response.StatusCode = 200;
                    response.Success = true;
                    response.Data = requestUser;
                    response.Tokken = token;
                    return Ok(response);
                }
                else
                {
                    response.Messages = "Registration Failed ";
                    response.StatusCode = 400;
                    response.Success = false;
                    response.Data = null;
                    return BadRequest(response);

                }
            }
            catch (Exception ex)
            {
                response.Messages = "Registration Failed " + ex;
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }

        }

       string GenerateVirtualCode()
        {
            string codes = Generate();
            if (_context.User.FirstOrDefault(i => i.virtualCode == codes) != null)
            {
                return GenerateVirtualCode();
            }

            return codes;
        }


        public string Generate()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(100000000, 999999999).ToString();
        }

        [HttpPost("Usher")]
        public async Task<IActionResult> CreateUsher([FromBody] RegisterRequest request)
        {

            var response = new ServiceResponses<Users>();
            string randomPassword = GeneratePassword(10);
            Users usersEmail = _context.User.FirstOrDefault(i => i.Email == request.Email);
            if (usersEmail != null)
            {
                response.Messages = "This user already exist";
                response.StatusCode = 409;
                response.Success = false;
                response.Data = null;

                return Conflict(response);
            }
            try
            {
                if (usersEmail == null)
                {
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword);
                    string role = GetRoleById(request.RoleId);
                    Users requestUser = new()
                    {
                        Email = request.Email,
                        FirstName = request.FirstName,
                        MiddleName = request.MiddleName,
                        SurName = request.SurName,
                        PhoneNumber = request.PhoneNumber,
                        Address = request.Address,
                        Gender = request.Gender,
                        Title = request.Title,
                        Province = request.Province,
                        Parish = request.Parish,
                        ProgramType = request.ProgramType,
                        Password = passwordHash,
                        Role = Role.Usher


                    };
                    await _context.User.AddAsync(requestUser);
                    await _context.SaveChangesAsync();
                    string token = _accessTokkenGenerator.GenerateToken(requestUser);

                    response.Messages = "Successful";
                    response.StatusCode = 200;
                    response.Success = true;
                    response.Data = requestUser;
                    response.Tokken = token;

                    return Ok(response);
                }
                else
                {
                    response.Messages = "Registration Failed ";
                    response.StatusCode = 400;
                    response.Success = false;
                    response.Data = null;
                    return BadRequest(response);

                }
            }
            catch (Exception ex)
            {
                response.Messages = "Registration Failed " + ex;
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }

        }



        [HttpPost("Member")]
        public async Task<IActionResult> CreateWorker([FromBody] RegisterRequest request)
        {

            var response = new ServiceResponses<Users>();
            string randomPassword = GeneratePassword(10);
            Users usersEmail = _context.User.FirstOrDefault(i => i.Email == request.Email);
            if (usersEmail != null)
            {
                response.Messages = "This user already exist";
                response.StatusCode = 409;
                response.Success = false;
                response.Data = null;

                return Conflict(response);
            }
            try
            {
                if (usersEmail == null)
                {
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword);
                    string role = GetRoleById(request.RoleId);
                    Users requestUser = new()
                    {
                        Email = request.Email,
                        FirstName = request.FirstName,
                        MiddleName = request.MiddleName,
                        SurName = request.SurName,
                        PhoneNumber = request.PhoneNumber,
                        Address = request.Address,
                        Gender = request.Gender,
                        Title = request.Title,
                        Province = request.Province,
                        Parish = request.Parish,
                        ProgramType = request.ProgramType,
                        Appearance = request.Appearance,
                        Password = passwordHash,
                        Role =Role.Member

                    };
                    await _context.User.AddAsync(requestUser);
                    await _context.SaveChangesAsync();
                    string token = _accessTokkenGenerator.GenerateToken(requestUser);

                    response.Messages = "Successful";
                    response.StatusCode = 200;
                    response.Success = true;
                    response.Data = requestUser;
                    response.Tokken = token;

                    return Ok(response);
                }
                else
                {
                    response.Messages = "Registration Failed ";
                    response.StatusCode = 400;
                    response.Success = false;
                    response.Data = null;
                    return BadRequest(response);

                }
            }
            catch (Exception ex)
            {
                response.Messages = "Registration Failed " + ex;
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
        {
            var response = new ServiceResponses<Users>();

            if (!ModelState.IsValid)
            {
                response.Messages = "One or more field is empty";
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }

            Users userInfo = await _context.User.FirstOrDefaultAsync(i => i.Email == request.Email);
            if (userInfo == null)
            {
                response.Messages = "User does not exist";
                response.StatusCode = 401;
                response.Success = false;
                response.Data = null;

                return Unauthorized(response);
            }
            bool isCorrect = _ipasswordHasher.VerifyPassword(request.Password, userInfo.Password);
            if (!isCorrect)
            {
                response.Messages = "User password is Incorrect";
                response.StatusCode = 401;
                response.Success = false;
                response.Data = null;

                return Unauthorized(response);
            }
            string token = _accessTokkenGenerator.GenerateToken(userInfo);


            response.Messages = "Successful";
            response.StatusCode = 200;
            response.Success = true;
            response.Data = userInfo;
            response.Tokken = token;
            return Ok(response);

        }


        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = new ServiceResponses<DbSet<Users>>(); ;
            var user = _context.User;

            if (user != null)
            {
                response.Success = true;
                response.Messages = "Data fetched Successfully";
                response.Data = user;
                return Ok(response);

            }
            else
            {
                response.Success = false;
                response.Messages = "No Record Found";
                response.Data = null;
                return BadRequest(response);
            }



        }

        static string GetRoleById(int RoleId)
        {
            return RoleId switch
            {
                1 => Role.ChurchAdmin,
                2 => Role.Usher,
                _ => Role.Member,
            };

        }


         string GetnewId()
        {
            string sum =null;
            int number = 1+_context.User.Count();

            var s = "LLC/2022/";

            if (number < 10)
            {
                sum = "000" + number.ToString();
            }
            else if (number < 100)
            {
                sum = "00" + number.ToString();
            }

      
       
            return s + sum;
        }
    }
}



