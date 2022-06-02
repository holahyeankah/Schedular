using ChurchAdmin.Context;
using ChurchAdmin.Models;
using Customer.Models;
using Customer.Models.DatabaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using Schedular.Request;
using SjxLogistics.Components;
using SjxLogistics.Controllers.AuthenticationComponent;
using SjxLogistics.Models.Request;
using SjxLogistics.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SjxLogistics.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IpasswordHasher _ipasswordHasher;
        private readonly CustomerDbContext _context;
        private readonly AccessToken _accessTokkenGenerator;

        public CustomerController(IpasswordHasher ipasswordHasher, CustomerDbContext context, AccessToken accessToken)
        {
            _context = context;
            _ipasswordHasher = ipasswordHasher;
            _accessTokkenGenerator = accessToken;

        }
        private static string value = "";
        private static Random random = new Random();
        private static char[] charList = new char[]

        {  '1', '2', '3', '4', '5', '6', '7', '8', '9','0'};

        private dynamic GenerateOtp(int max)
        {
            while (value.Length < max)
            {
                value += charList[random.Next(0, charList.Count())];
            }
            return value;
        }


        [HttpPost("sendOtp")]
        public async Task<IActionResult> SendOtp([FromBody] RegisterRequest request)
        {
            string otp = GenerateOtp(6);
            string mobileNo = request.PhoneNumber;

            var response = SendSMS(mobileNo, otp);

            if (response != "Fail")
            {
                await AddData(otp);
            }
            return Ok(response);


        }
        [HttpPost("saveOtp")]
        public async Task<IActionResult> AddData(string otp)
        {

            var response = new ServiceResponses<User>();

            var text = _context.Customers.FirstOrDefault(i => i.Otp == otp);
            if (text == null)
            {

                User requestOtp = new()
                {
                    Otp = otp,
                };
                await _context.Customers.AddAsync(requestOtp);
                await _context.SaveChangesAsync();
                response.Messages = "Successful";
                response.StatusCode = 200;
                response.Success = true;
                response.Data = requestOtp;

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




        string SendSMS(string phoneNumber, string otp)
        {
            string Username = "youremail@domain.com";
            string APIKey = "MyApiKey";
            string SenderName = "MyName";
            string Number = phoneNumber;
            string Message = "Your OTP code is - " + otp;
            string URL = "http://api.urlname.in/send/?username=" + Username +
                "&hash=" + APIKey + "&sender=" + SenderName + "&numbers=" + Number + "&message=" + Message;
            string strResponce = GetResponse(URL);
            string msg = "";
            if (strResponce.Equals("Fail"))
            {
                msg = "Fail";
            }
            else
            {
                msg = strResponce;
            }
            return msg;
        }

         string GetResponse(string URL)
        {
            try
            {
                WebClient objWebClient = new WebClient();
                System.IO.StreamReader reader = new System.IO.StreamReader(objWebClient.OpenRead(URL));
                string ResultHTML = reader.ReadToEnd();
                return ResultHTML;
            }
            catch (Exception)
            {
                return "Fail";
            }
        }


        //Get List of States

        [HttpGet("state")]

        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {

            var states = _context.State;

            return states;
        }



       // Get List of Lga that has same stateId

       [HttpGet("lga/{StateId}")]
        public async Task<ActionResult<Lga>> GetLga(int StateId)
        {
            var response = new ServiceResponses<IQueryable<Lga>>();
            var lga = _context.Lga.Where(i => i.StateId == StateId);

            if (lga != null)
            {

                response.Messages = "Successful";
                response.StatusCode = 200;
                response.Data = lga;
                response.Success = true;
                return Ok(response);
            }
            else
            {
                response.Messages = "Failed";
                response.Data = lga;
                response.Success = false;
                return BadRequest(response);
            }


        }

        // Create customer after Otp Verification
        [HttpPost("customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] RegisterRequest request)
        {

            var response = new ServiceResponses<User>();
            User usersEmail = await _context.Customers.FirstOrDefaultAsync(i => i.Email == request.Email);
            if (usersEmail != null)
            {
                response.Messages = "This user already exist";
                response.StatusCode = 409;
                response.Success = false;
                response.Data = null;

                return Conflict(response);
            }

            var otp = await _context.Customers.FirstOrDefaultAsync(i => i.Otp == request.Otp);

            try
            {


                if (usersEmail == null && otp != null)
                {
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                    User requestUser = new()
                    {
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                        Password = passwordHash,
                        State = request.StateId,
                        Lga = request.LgaId

                    };
                    await _context.Customers.AddAsync(requestUser);
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








        //Api that return existing banks
        [HttpGet("bankList")]
        public async Task<RestResponse> Index()
        {
            var client = new RestClient("https://wema-alatdev-apimgt.azure-api.net/alat-test/api/Shared/GetAllBanks");
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("User-Agent", "Thunder Client (https://www.thunderclient.com)");
            request.AddHeader("Accept", "*/*");
            var response = await client.GetAsync(request);
            return response;

        }

        // Get all existing customers
        //Get Api:https://localhost:44362/api/customer/getAllCustomers
        [HttpGet("getAllCustomers")]
        public async Task<IActionResult> GetAllCustomerss()
        {
            var response = new ServiceResponses<DbSet<User>>(); ;
            var user = _context.Customers;

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

        // Customer Login Endpoint

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
        {
            var response = new ServiceResponses<User>();

            if (!ModelState.IsValid)
            {
                response.Messages = "One or more field is empty";
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }

            User Info = await _context.Customers.FirstOrDefaultAsync(i => i.Email == request.Email);
            if (Info == null)
            {
                response.Messages = "User does not exist";
                response.StatusCode = 401;
                response.Success = false;
                response.Data = null;

                return Unauthorized(response);
            }
            bool isCorrect = _ipasswordHasher.VerifyPassword(request.Password, Info.Password);
            if (!isCorrect)
            {
                response.Messages = "User password is Incorrect";
                response.StatusCode = 401;
                response.Success = false;
                response.Data = null;

                return Unauthorized(response);
            }
            string token = _accessTokkenGenerator.GenerateToken(Info);


            response.Messages = "Successful";
            response.StatusCode = 200;
            response.Success = true;
            response.Data = Info;
            response.Tokken = token;
            return Ok(response);

        }
    };
}



  



