using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Models;

namespace TravelCompanion.Domain.Services
{
    public class AppUserService
    {
        private readonly TravelCompanionContext _context;
        private readonly IMapper _mapper;

        public AppUserService(TravelCompanionContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckUserExistsAsync(int userId)
        {
            return await _context.AppUsers.AnyAsync(u => u.AppUserId == userId);
        }

        public async Task<Guid?> GetAppUserGuid(int userId)
        {
            var user = await _context.AppUsers.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            return user.AppUserGuid;
        }

        public async Task<AppUserDto> GetByIdAsync(int userId)
        {
            var user = await _context.AppUsers.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> GetByGuidAsync(Guid guid)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.AppUserGuid == guid);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> GetByEmailAsync(string email)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> CreateUserAsync(AppUserDto appUserDto)
        {
            var user = _mapper.Map<AppUser>(appUserDto);
            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> UpdateUserAsync(AppUserDto appUserDto)
        {
            var user = await _context.AppUsers.FindAsync(appUserDto.AppUserId);
            if (user == null)
            {
                return null;
            }

            _mapper.Map(appUserDto, user);
            await _context.SaveChangesAsync();

            return _mapper.Map<AppUserDto>(user);
        }
    }
}
