using ChurchAdmin.Models;
using SjxLogistics.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace SjxLogistics.Components
{
    public class AccessToken
    {
        private readonly AccessConfig _accessConfig;
        private readonly TokkenGeneratorClass _tokenGen;
        public AccessToken(AccessConfig accessConfig, TokkenGeneratorClass tokkenGeneration)
        {
            _accessConfig = accessConfig;
            _tokenGen = tokkenGeneration;
        }
        public string GenerateToken(User users)
        {
            List<Claim> claims = new() {
                new Claim(ClaimTypes.Name, users.Id.ToString()),
                new Claim(ClaimTypes.Email, users.Email),
              
            };
            return _tokenGen.TokenGenerator(
                _accessConfig.AccessKey,
                _accessConfig.Audiance,
                _accessConfig.Issuer,
                _accessConfig.Expiration,
                claims);
        }

       
        
    }
}
