using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task_mangement_System.Data;
using Task_mangement_System.Models;
using Task_mangement_System.Models.Dto;
using Task_mangement_System.Repository.IRepository;

namespace Task_mangement_System.Repository
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private string secretKey;
		private readonly IMapper _mapper;
		public ApplicationUserRepository(ApplicationDbContext db,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,IMapper mapper,IConfiguration configuration)
        {
            _db = db;
			_roleManager = roleManager;
			_userManager = userManager;
			_mapper = mapper;
			secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
        }

		public bool IsUniqueUser(string username)
		{
			var user = _db.applicationusers.FirstOrDefault(u => u.UserName == username);
			return user == null;
		}

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
		{
			var user =  _db.applicationusers.FirstOrDefault(u=>u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

			bool isvalid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
			if (user == null || !isvalid)
			{
				return new LoginResponseDto()
				{
					Token = "",
					ApplicationUser = null,
				};
			}
			var roles = await _userManager.GetRolesAsync(user);
			var tokenhandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);
			var tokendescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.UserName.ToString()),
					new Claim(ClaimTypes.Role, roles.FirstOrDefault())
				}),
				Expires = DateTime.UtcNow.AddDays(5),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenhandler.CreateToken(tokendescriptor);
			LoginResponseDto loginResponseDto = new LoginResponseDto()
			{
				Token = tokenhandler.WriteToken(token),
				ApplicationUser = _mapper.Map<ApplicationUserDto>(user)
			};
			return loginResponseDto;
		}

		public async Task<ApplicationUserDto> Register(RegisterationRequestDto registerationRequestDto)
		{
			ApplicationUser user = new ApplicationUser()
			{
				UserName = registerationRequestDto.UserName,
				Name = registerationRequestDto.Name,
				Email = registerationRequestDto.Email,
				NormalizedEmail = registerationRequestDto.Email.ToUpper(),
				Country = registerationRequestDto.Country,
			};
			try
			{
				var result = await _userManager.CreateAsync(user,registerationRequestDto.Password);
				if (result.Succeeded)
				{
					if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
					{
						await _roleManager.CreateAsync(new IdentityRole("admin"));
						await _roleManager.CreateAsync(new IdentityRole("User"));
					}
					await _userManager.AddToRoleAsync(user, registerationRequestDto.Role);
					var usertoreturn = _db.applicationusers.FirstOrDefault(u=>u.UserName == registerationRequestDto.UserName);
					return _mapper.Map<ApplicationUserDto>(usertoreturn);
				}
			}
			catch(Exception ex)
			{

			}
			return new ApplicationUserDto();
		}

		public async Task<ApplicationUser> UpdateAsync(ApplicationUser applicationUser)
        {
            _db.applicationusers.Update(applicationUser);
            await _db.SaveChangesAsync();
            return applicationUser;
        }
    }
}
